using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TriboDavi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_AssistantTeacherId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "AssistantTeacherId",
                table: "AspNetUsers",
                newName: "MainTeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_AssistantTeacherId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_MainTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_MainTeacherId",
                table: "AspNetUsers",
                column: "MainTeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_MainTeacherId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "MainTeacherId",
                table: "AspNetUsers",
                newName: "AssistantTeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_MainTeacherId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_AssistantTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_AssistantTeacherId",
                table: "AspNetUsers",
                column: "AssistantTeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
