using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Health_Hive_Project_2024.Migrations
{
    /// <inheritdoc />
    public partial class Kgaugelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxValue2",
                table: "Vitals");

            migrationBuilder.DropColumn(
                name: "MinimumValue2",
                table: "Vitals");

            migrationBuilder.AlterColumn<string>(
                name: "VitalName2",
                table: "Vitals",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "VitalName",
                table: "Vitals",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "NurseID",
                table: "PrescriptionRecords",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuantityAdministered",
                table: "PrescriptionItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Diagnosis",
                table: "Condition",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CODE",
                table: "Condition",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionRecords_NurseID",
                table: "PrescriptionRecords",
                column: "NurseID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAdmissions_SurgeryBID",
                table: "PatientAdmissions",
                column: "SurgeryBID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAdmissions_SurgeryBooking_SurgeryBID",
                table: "PatientAdmissions",
                column: "SurgeryBID",
                principalTable: "SurgeryBooking",
                principalColumn: "SurgeryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionRecords_AspNetUsers_NurseID",
                table: "PrescriptionRecords",
                column: "NurseID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAdmissions_SurgeryBooking_SurgeryBID",
                table: "PatientAdmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionRecords_AspNetUsers_NurseID",
                table: "PrescriptionRecords");

            migrationBuilder.DropIndex(
                name: "IX_PrescriptionRecords_NurseID",
                table: "PrescriptionRecords");

            migrationBuilder.DropIndex(
                name: "IX_PatientAdmissions_SurgeryBID",
                table: "PatientAdmissions");

            migrationBuilder.DropColumn(
                name: "NurseID",
                table: "PrescriptionRecords");

            migrationBuilder.AlterColumn<string>(
                name: "VitalName2",
                table: "Vitals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VitalName",
                table: "Vitals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxValue2",
                table: "Vitals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumValue2",
                table: "Vitals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "QuantityAdministered",
                table: "PrescriptionItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Diagnosis",
                table: "Condition",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CODE",
                table: "Condition",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
