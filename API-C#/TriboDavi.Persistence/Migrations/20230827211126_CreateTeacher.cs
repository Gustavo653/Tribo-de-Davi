using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TriboDavi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssistantTeacherId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Teacher_CPF",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Teacher_Graduation",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Teacher_RG",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AssistantTeacherId",
                table: "AspNetUsers",
                column: "AssistantTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_AssistantTeacherId",
                table: "AspNetUsers",
                column: "AssistantTeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_AssistantTeacherId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AssistantTeacherId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AssistantTeacherId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Teacher_CPF",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Teacher_Graduation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Teacher_RG",
                table: "AspNetUsers");
        }
    }
}
