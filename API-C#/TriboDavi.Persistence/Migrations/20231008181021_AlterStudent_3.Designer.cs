﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TriboDavi.Persistence;

#nullable disable

namespace TriboDavi.Persistence.Migrations
{
    [DbContext(typeof(TriboDaviContext))]
    [Migration("20231008181021_AlterStudent_3")]
    partial class AlterStudent_3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TriboDavi.Domain.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Neighborhood")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StreetNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("StreetNumber", "StreetName", "Neighborhood")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("TriboDavi.Domain.FieldOperation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AddressId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("FieldOperations");
                });

            modelBuilder.Entity("TriboDavi.Domain.FieldOperationStudent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean");

                    b.Property<int>("FieldOperationTeacherId")
                        .HasColumnType("integer");

                    b.Property<int>("StudentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("FieldOperationTeacherId");

                    b.HasIndex("StudentId");

                    b.ToTable("FieldOperationStudents");
                });

            modelBuilder.Entity("TriboDavi.Domain.FieldOperationTeacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean");

                    b.Property<int>("FieldOperationId")
                        .HasColumnType("integer");

                    b.Property<int>("TeacherId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("FieldOperationId");

                    b.HasIndex("TeacherId");

                    b.ToTable("FieldOperationTeachers");
                });

            modelBuilder.Entity("TriboDavi.Domain.Graduation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("GraduationType")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("Position")
                        .IsUnique();

                    b.ToTable("Graduations");
                });

            modelBuilder.Entity("TriboDavi.Domain.Identity.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("TriboDavi.Domain.Identity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<int>("GraduationId")
                        .HasColumnType("integer");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("RG")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("GraduationId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("TriboDavi.Domain.Identity.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("TriboDavi.Domain.LegalParent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RG")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Relationship")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CPF")
                        .IsUnique();

                    b.ToTable("LegalParents");
                });

            modelBuilder.Entity("TriboDavi.Domain.RollCall", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("FieldOperationStudentId")
                        .HasColumnType("integer");

                    b.Property<bool>("Presence")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("FieldOperationStudentId", "Date")
                        .IsUnique();

                    b.ToTable("RollCalls");
                });

            modelBuilder.Entity("TriboDavi.Domain.Student", b =>
                {
                    b.HasBaseType("TriboDavi.Domain.Identity.User");

                    b.Property<int?>("AddressId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("EmergencyNumber")
                        .HasColumnType("text");

                    b.Property<decimal>("Height")
                        .HasColumnType("numeric");

                    b.Property<int?>("LegalParentId")
                        .HasColumnType("integer");

                    b.Property<int?>("SchoolGrade")
                        .HasColumnType("integer");

                    b.Property<string>("SchoolName")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Weight")
                        .HasColumnType("numeric");

                    b.HasIndex("AddressId");

                    b.HasIndex("LegalParentId");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("TriboDavi.Domain.Teacher", b =>
                {
                    b.HasBaseType("TriboDavi.Domain.Identity.User");

                    b.Property<int?>("MainTeacherId")
                        .HasColumnType("integer");

                    b.HasIndex("MainTeacherId");

                    b.HasDiscriminator().HasValue("Teacher");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("TriboDavi.Domain.Identity.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("TriboDavi.Domain.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("TriboDavi.Domain.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("TriboDavi.Domain.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TriboDavi.Domain.FieldOperation", b =>
                {
                    b.HasOne("TriboDavi.Domain.Address", "Address")
                        .WithMany("FieldOperations")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("TriboDavi.Domain.FieldOperationStudent", b =>
                {
                    b.HasOne("TriboDavi.Domain.FieldOperationTeacher", "FieldOperationTeacher")
                        .WithMany()
                        .HasForeignKey("FieldOperationTeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TriboDavi.Domain.Student", "Student")
                        .WithMany("FieldOperationStudents")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FieldOperationTeacher");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("TriboDavi.Domain.FieldOperationTeacher", b =>
                {
                    b.HasOne("TriboDavi.Domain.FieldOperation", "FieldOperation")
                        .WithMany()
                        .HasForeignKey("FieldOperationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TriboDavi.Domain.Teacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FieldOperation");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("TriboDavi.Domain.Identity.User", b =>
                {
                    b.HasOne("TriboDavi.Domain.Graduation", "Graduation")
                        .WithMany()
                        .HasForeignKey("GraduationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Graduation");
                });

            modelBuilder.Entity("TriboDavi.Domain.Identity.UserRole", b =>
                {
                    b.HasOne("TriboDavi.Domain.Identity.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TriboDavi.Domain.Identity.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TriboDavi.Domain.RollCall", b =>
                {
                    b.HasOne("TriboDavi.Domain.FieldOperationStudent", "FieldOperationStudent")
                        .WithMany()
                        .HasForeignKey("FieldOperationStudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FieldOperationStudent");
                });

            modelBuilder.Entity("TriboDavi.Domain.Student", b =>
                {
                    b.HasOne("TriboDavi.Domain.Address", "Address")
                        .WithMany("Students")
                        .HasForeignKey("AddressId");

                    b.HasOne("TriboDavi.Domain.LegalParent", "LegalParent")
                        .WithMany("Students")
                        .HasForeignKey("LegalParentId");

                    b.Navigation("Address");

                    b.Navigation("LegalParent");
                });

            modelBuilder.Entity("TriboDavi.Domain.Teacher", b =>
                {
                    b.HasOne("TriboDavi.Domain.Teacher", "MainTeacher")
                        .WithMany("AssistantTeachers")
                        .HasForeignKey("MainTeacherId");

                    b.Navigation("MainTeacher");
                });

            modelBuilder.Entity("TriboDavi.Domain.Address", b =>
                {
                    b.Navigation("FieldOperations");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("TriboDavi.Domain.Identity.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("TriboDavi.Domain.Identity.User", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("TriboDavi.Domain.LegalParent", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("TriboDavi.Domain.Student", b =>
                {
                    b.Navigation("FieldOperationStudents");
                });

            modelBuilder.Entity("TriboDavi.Domain.Teacher", b =>
                {
                    b.Navigation("AssistantTeachers");
                });
#pragma warning restore 612, 618
        }
    }
}