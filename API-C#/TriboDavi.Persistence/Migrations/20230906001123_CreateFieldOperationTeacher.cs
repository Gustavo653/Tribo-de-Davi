using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TriboDavi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateFieldOperationTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldOperationTeachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TeacherId = table.Column<int>(type: "integer", nullable: false),
                    FieldOperationId = table.Column<int>(type: "integer", nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldOperationTeachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldOperationTeachers_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldOperationTeachers_FieldOperations_FieldOperationId",
                        column: x => x.FieldOperationId,
                        principalTable: "FieldOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldOperations_Name",
                table: "FieldOperations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FieldOperationTeachers_FieldOperationId",
                table: "FieldOperationTeachers",
                column: "FieldOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldOperationTeachers_TeacherId",
                table: "FieldOperationTeachers",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldOperationTeachers");

            migrationBuilder.DropIndex(
                name: "IX_FieldOperations_Name",
                table: "FieldOperations");
        }
    }
}
