using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Health_Hive_Project_2024.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNurse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionRecords_AspNetUsers_NurseID",
                table: "PrescriptionRecords");

            migrationBuilder.DropIndex(
                name: "IX_PrescriptionRecords_NurseID",
                table: "PrescriptionRecords");

            migrationBuilder.DropColumn(
                name: "NurseID",
                table: "PrescriptionRecords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NurseID",
                table: "PrescriptionRecords",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionRecords_NurseID",
                table: "PrescriptionRecords",
                column: "NurseID");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionRecords_AspNetUsers_NurseID",
                table: "PrescriptionRecords",
                column: "NurseID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
