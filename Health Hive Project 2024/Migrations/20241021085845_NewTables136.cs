using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Health_Hive_Project_2024.Migrations
{
    /// <inheritdoc />
    public partial class NewTables136 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedications_AspNetUsers_NurseID",
                table: "PrescriptionMedications");

            migrationBuilder.DropIndex(
                name: "IX_PrescriptionMedications_NurseID",
                table: "PrescriptionMedications");

            migrationBuilder.DropColumn(
                name: "NurseID",
                table: "PrescriptionMedications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NurseID",
                table: "PrescriptionMedications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedications_NurseID",
                table: "PrescriptionMedications",
                column: "NurseID");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionMedications_AspNetUsers_NurseID",
                table: "PrescriptionMedications",
                column: "NurseID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
