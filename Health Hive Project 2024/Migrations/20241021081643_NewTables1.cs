using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Health_Hive_Project_2024.Migrations
{
    /// <inheritdoc />
    public partial class NewTables1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Condition_Patients_PatientID",
                table: "Condition");

            migrationBuilder.DropIndex(
                name: "IX_Condition_PatientID",
                table: "Condition");

            migrationBuilder.DropColumn(
                name: "PatientID",
                table: "Condition");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientID",
                table: "Condition",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Condition_PatientID",
                table: "Condition",
                column: "PatientID");

            migrationBuilder.AddForeignKey(
                name: "FK_Condition_Patients_PatientID",
                table: "Condition",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
