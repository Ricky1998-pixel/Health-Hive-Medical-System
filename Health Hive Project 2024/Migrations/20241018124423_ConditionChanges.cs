using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Health_Hive_Project_2024.Migrations
{
    /// <inheritdoc />
    public partial class ConditionChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SurgeonID",
                table: "RejectedPrescriptions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PharmacistID",
                table: "RejectedPrescriptions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConditionID",
                table: "MedicationRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SurgeonID",
                table: "DispensedPrescriptions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PharmacistID",
                table: "DispensedPrescriptions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RejectedPrescriptions_MedicationID",
                table: "RejectedPrescriptions",
                column: "MedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_RejectedPrescriptions_PatientID",
                table: "RejectedPrescriptions",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_RejectedPrescriptions_PharmacistID",
                table: "RejectedPrescriptions",
                column: "PharmacistID");

            migrationBuilder.CreateIndex(
                name: "IX_RejectedPrescriptions_SurgeonID",
                table: "RejectedPrescriptions",
                column: "SurgeonID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationRecords_ConditionID",
                table: "MedicationRecords",
                column: "ConditionID");

            migrationBuilder.CreateIndex(
                name: "IX_DispensedPrescriptions_MedicationID",
                table: "DispensedPrescriptions",
                column: "MedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_DispensedPrescriptions_PatientID",
                table: "DispensedPrescriptions",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_DispensedPrescriptions_PharmacistID",
                table: "DispensedPrescriptions",
                column: "PharmacistID");

            migrationBuilder.CreateIndex(
                name: "IX_DispensedPrescriptions_SurgeonID",
                table: "DispensedPrescriptions",
                column: "SurgeonID");

            migrationBuilder.AddForeignKey(
                name: "FK_DispensedPrescriptions_AspNetUsers_PharmacistID",
                table: "DispensedPrescriptions",
                column: "PharmacistID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DispensedPrescriptions_AspNetUsers_SurgeonID",
                table: "DispensedPrescriptions",
                column: "SurgeonID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DispensedPrescriptions_MedicationRecords_MedicationID",
                table: "DispensedPrescriptions",
                column: "MedicationID",
                principalTable: "MedicationRecords",
                principalColumn: "MedicationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DispensedPrescriptions_Patients_PatientID",
                table: "DispensedPrescriptions",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationRecords_Condition_ConditionID",
                table: "MedicationRecords",
                column: "ConditionID",
                principalTable: "Condition",
                principalColumn: "ConditionID");

            migrationBuilder.AddForeignKey(
                name: "FK_RejectedPrescriptions_AspNetUsers_PharmacistID",
                table: "RejectedPrescriptions",
                column: "PharmacistID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RejectedPrescriptions_AspNetUsers_SurgeonID",
                table: "RejectedPrescriptions",
                column: "SurgeonID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RejectedPrescriptions_MedicationRecords_MedicationID",
                table: "RejectedPrescriptions",
                column: "MedicationID",
                principalTable: "MedicationRecords",
                principalColumn: "MedicationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RejectedPrescriptions_Patients_PatientID",
                table: "RejectedPrescriptions",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DispensedPrescriptions_AspNetUsers_PharmacistID",
                table: "DispensedPrescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_DispensedPrescriptions_AspNetUsers_SurgeonID",
                table: "DispensedPrescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_DispensedPrescriptions_MedicationRecords_MedicationID",
                table: "DispensedPrescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_DispensedPrescriptions_Patients_PatientID",
                table: "DispensedPrescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicationRecords_Condition_ConditionID",
                table: "MedicationRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_RejectedPrescriptions_AspNetUsers_PharmacistID",
                table: "RejectedPrescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_RejectedPrescriptions_AspNetUsers_SurgeonID",
                table: "RejectedPrescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_RejectedPrescriptions_MedicationRecords_MedicationID",
                table: "RejectedPrescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_RejectedPrescriptions_Patients_PatientID",
                table: "RejectedPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_RejectedPrescriptions_MedicationID",
                table: "RejectedPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_RejectedPrescriptions_PatientID",
                table: "RejectedPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_RejectedPrescriptions_PharmacistID",
                table: "RejectedPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_RejectedPrescriptions_SurgeonID",
                table: "RejectedPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_MedicationRecords_ConditionID",
                table: "MedicationRecords");

            migrationBuilder.DropIndex(
                name: "IX_DispensedPrescriptions_MedicationID",
                table: "DispensedPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_DispensedPrescriptions_PatientID",
                table: "DispensedPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_DispensedPrescriptions_PharmacistID",
                table: "DispensedPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_DispensedPrescriptions_SurgeonID",
                table: "DispensedPrescriptions");

            migrationBuilder.DropColumn(
                name: "ConditionID",
                table: "MedicationRecords");

            migrationBuilder.AlterColumn<string>(
                name: "SurgeonID",
                table: "RejectedPrescriptions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "PharmacistID",
                table: "RejectedPrescriptions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SurgeonID",
                table: "DispensedPrescriptions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "PharmacistID",
                table: "DispensedPrescriptions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
