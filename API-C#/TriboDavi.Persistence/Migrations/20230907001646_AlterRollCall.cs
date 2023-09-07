using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TriboDavi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterRollCall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RollCalls_FieldOperationStudentId",
                table: "RollCalls");

            migrationBuilder.CreateIndex(
                name: "IX_RollCalls_FieldOperationStudentId_Date",
                table: "RollCalls",
                columns: new[] { "FieldOperationStudentId", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RollCalls_FieldOperationStudentId_Date",
                table: "RollCalls");

            migrationBuilder.CreateIndex(
                name: "IX_RollCalls_FieldOperationStudentId",
                table: "RollCalls",
                column: "FieldOperationStudentId");
        }
    }
}
