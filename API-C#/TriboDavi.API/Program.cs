using Common.Functions;
using Hangfire;
using Hangfire.PostgreSql;
using TriboDavi.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain.Enum;
using TriboDavi.Domain.Identity;
using TriboDavi.Persistence;
using TriboDavi.Service;
using TriboDavi.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace TriboDavi.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            string databaseTriboDavi = Environment.GetEnvironmentVariable("DatabaseConnection") ?? configuration.GetConnectionString("DatabaseConnection")!;

            Console.WriteLine("Início dos parâmetros da aplicação \n");
            Console.WriteLine($"(DatabaseConnection) String de conexao com banco de dados para TriboDavi: \n{databaseTriboDavi} \n");
            Console.WriteLine("Fim dos parâmetros da aplicação \n");

            builder.Services.AddDbContext<TriboDaviContext>(x =>
            {
                x.UseNpgsql(databaseTriboDavi);
                if (builder.Environment.IsDevelopment())
                {
                    x.EnableSensitiveDataLogging();
                    x.EnableDetailedErrors();
                }
            });

            builder.Services.AddIdentity<User, Role>()
                            .AddEntityFrameworkStores<TriboDaviContext>()
                            .AddDefaultTokenProviders();

            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAmbulanceService, AmbulanceService>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAmbulanceRepository, AmbulanceRepository>();

            builder.Services.AddScoped<RoleManager<Role>>();
            builder.Services.AddScoped<UserManager<User>>();

            using (var serviceProvider = builder.Services.BuildServiceProvider())
            {
                var dbContext = serviceProvider.GetService<TriboDaviContext>();
                dbContext.Database.Migrate();
                SeedRoles(serviceProvider).Wait();
                SeedAdminUser(serviceProvider).Wait();
            }

            builder.Services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<Role>()
            .AddRoleManager<RoleManager<Role>>()
            .AddSignInManager<SignInManager<User>>()
            .AddRoleValidator<RoleValidator<Role>>()
            .AddEntityFrameworkStores<TriboDaviContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("TokenKey")!)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddControllers()
                    .AddJsonOptions(options =>
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
                    )
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    );

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "TriboDavi.API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header usando Bearer.
                                Entre com 'Bearer ' [espaço] então coloque seu token.
                                Exemplo: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.AddCors();

            builder.Services.AddHangfire(x =>
            {
                x.UsePostgreSqlStorage(databaseTriboDavi);
            });
            builder.Services.AddHangfireServer(x => x.WorkerCount = 1);

            builder.Services.AddMvc();
            builder.Services.AddRouting();

            builder.Services.AddHealthChecks();

            var app = builder.Build();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() },
            });

            app.UseCors(builder =>
            {
                builder.AllowAnyMethod()
                       .AllowAnyOrigin()
                       .AllowAnyHeader();
            });

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapHealthChecks("/health");

            app.Run();
        }

        private static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var roles = new List<string>() { RoleName.Student.ToString(), RoleName.Teacher.ToString(), RoleName.AssistantTeacher.ToString(), RoleName.Admin.ToString() };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new Role { Name = role });
                }
            }
        }

        private static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var adminEmail = "admin@admin.com";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            var user = new User { Name = "Admin", Email = adminEmail, UserName = "admin" };
            if (adminUser == null)
            {
                await userManager.CreateAsync(user, "Admin@123");
            }
            if (!await userManager.IsInRoleAsync(adminUser ?? user, RoleName.Admin.ToString()))
                await userManager.AddToRoleAsync(adminUser ?? user, RoleName.Admin.ToString());
        }
    }
}