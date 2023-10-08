using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TriboDavi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterStudent_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_LegalParents_LegalParentId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "EmergencyNumber",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_LegalParents_LegalParentId",
                table: "AspNetUsers",
                column: "LegalParentId",
                principalTable: "LegalParents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_LegalParents_LegalParentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmergencyNumber",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_LegalParents_LegalParentId",
                table: "AspNetUsers",
                column: "LegalParentId",
                principalTable: "LegalParents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
