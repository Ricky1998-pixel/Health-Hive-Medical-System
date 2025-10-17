using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Health_Hive_Project_2024.Migrations
{
    /// <inheritdoc />
    public partial class PublishingProject2024October : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActiveIngredientRecords",
                columns: table => new
                {
                    IngredientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IngredientName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveIngredientRecords", x => x.IngredientID);
                });

            migrationBuilder.CreateTable(
                name: "Allergy",
                columns: table => new
                {
                    AllergyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergy", x => x.AllergyID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specialization = table.Column<int>(type: "int", nullable: true),
                    HealthCouncilRegistrationNumber = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BedRecords",
                columns: table => new
                {
                    BedID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BedNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BedRecords", x => x.BedID);
                });

            migrationBuilder.CreateTable(
                name: "Condition",
                columns: table => new
                {
                    ConditionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condition", x => x.ConditionID);
                });

            migrationBuilder.CreateTable(
                name: "DayHospitals",
                columns: table => new
                {
                    HospitalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HospitalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Suburb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HospitalContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HospitalEmaiAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PracticeManage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseManagerEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayHospitals", x => x.HospitalID);
                });

            migrationBuilder.CreateTable(
                name: "DosageForm",
                columns: table => new
                {
                    DosageFormID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosageForm", x => x.DosageFormID);
                });

            migrationBuilder.CreateTable(
                name: "GetNormalRanges",
                columns: table => new
                {
                    NormalRangeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VitalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinValue = table.Column<int>(type: "int", nullable: false),
                    MaxValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetNormalRanges", x => x.NormalRangeId);
                });

            migrationBuilder.CreateTable(
                name: "OperatingTheatreRecords",
                columns: table => new
                {
                    TheatreID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TheatreName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatingTheatreRecords", x => x.TheatreID);
                });

            migrationBuilder.CreateTable(
                name: "OrderMedications",
                columns: table => new
                {
                    OrderMedicationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnaesthesiologistName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderUrgency = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderMedications", x => x.OrderMedicationID);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientIDNumber = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientID);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentRecords",
                columns: table => new
                {
                    TreatmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreatmentCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentRecords", x => x.TreatmentID);
                });

            migrationBuilder.CreateTable(
                name: "VitalPatients",
                columns: table => new
                {
                    VitalPatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VitalPatients", x => x.VitalPatientId);
                });

            migrationBuilder.CreateTable(
                name: "Vitals",
                columns: table => new
                {
                    VitalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VitalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VitalName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinimumValue = table.Column<int>(type: "int", nullable: false),
                    MaxValue = table.Column<int>(type: "int", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vitals", x => x.VitalID);
                });

            migrationBuilder.CreateTable(
                name: "VitalType",
                columns: table => new
                {
                    VitalTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value1 = table.Column<int>(type: "int", nullable: false),
                    Value2 = table.Column<int>(type: "int", nullable: true),
                    Value3 = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VitalType", x => x.VitalTypeID);
                });

            migrationBuilder.CreateTable(
                name: "WardRecords",
                columns: table => new
                {
                    WardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WardName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WardType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WardRecords", x => x.WardID);
                });

            migrationBuilder.CreateTable(
                name: "MedicationAlerts",
                columns: table => new
                {
                    MedicationAlertID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IngredientID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationAlerts", x => x.MedicationAlertID);
                    table.ForeignKey(
                        name: "FK_MedicationAlerts_ActiveIngredientRecords_IngredientID",
                        column: x => x.IngredientID,
                        principalTable: "ActiveIngredientRecords",
                        principalColumn: "IngredientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GetNortifications",
                columns: table => new
                {
                    NotifyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GetAnaesthesiologistId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetNortifications", x => x.NotifyId);
                    table.ForeignKey(
                        name: "FK_GetNortifications_AspNetUsers_GetAnaesthesiologistId",
                        column: x => x.GetAnaesthesiologistId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContraIndicationsRecords",
                columns: table => new
                {
                    ContraIndicationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConditionID = table.Column<int>(type: "int", nullable: false),
                    IngredientID = table.Column<int>(type: "int", nullable: false),
                    AlertMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlertType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContraIndicationsRecords", x => x.ContraIndicationID);
                    table.ForeignKey(
                        name: "FK_ContraIndicationsRecords_ActiveIngredientRecords_IngredientID",
                        column: x => x.IngredientID,
                        principalTable: "ActiveIngredientRecords",
                        principalColumn: "IngredientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContraIndicationsRecords_Condition_ConditionID",
                        column: x => x.ConditionID,
                        principalTable: "Condition",
                        principalColumn: "ConditionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicationRecords",
                columns: table => new
                {
                    MedicationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DosageFormID = table.Column<int>(type: "int", nullable: false),
                    Schedule = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityOnHand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReOrderLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConditionID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationRecords", x => x.MedicationID);
                    table.ForeignKey(
                        name: "FK_MedicationRecords_Condition_ConditionID",
                        column: x => x.ConditionID,
                        principalTable: "Condition",
                        principalColumn: "ConditionID");
                    table.ForeignKey(
                        name: "FK_MedicationRecords_DosageForm_DosageFormID",
                        column: x => x.DosageFormID,
                        principalTable: "DosageForm",
                        principalColumn: "DosageFormID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicationOrders",
                columns: table => new
                {
                    MedicationOrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderMedicationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationOrders", x => x.MedicationOrderID);
                    table.ForeignKey(
                        name: "FK_MedicationOrders_OrderMedications_OrderMedicationID",
                        column: x => x.OrderMedicationID,
                        principalTable: "OrderMedications",
                        principalColumn: "OrderMedicationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Allergies",
                columns: table => new
                {
                    AllergyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    IngredientID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergies", x => x.AllergyID);
                    table.ForeignKey(
                        name: "FK_Allergies_ActiveIngredientRecords_IngredientID",
                        column: x => x.IngredientID,
                        principalTable: "ActiveIngredientRecords",
                        principalColumn: "IngredientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Allergies_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnaesthesiaOrder",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MedicationtoOrder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderUrgency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnaesthesiaOrder", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_AnaesthesiaOrder_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnaesthesiaOrder_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompleteSurgeries",
                columns: table => new
                {
                    CompleteSurgeryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurgeryID = table.Column<int>(type: "int", nullable: false),
                    SurgeonID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PatientID = table.Column<int>(type: "int", nullable: true),
                    AnaesthesiologistID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TheatreID = table.Column<int>(type: "int", nullable: true),
                    SurgeryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Session = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompleteSurgeries", x => x.CompleteSurgeryID);
                    table.ForeignKey(
                        name: "FK_CompleteSurgeries_AspNetUsers_AnaesthesiologistID",
                        column: x => x.AnaesthesiologistID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompleteSurgeries_AspNetUsers_SurgeonID",
                        column: x => x.SurgeonID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompleteSurgeries_OperatingTheatreRecords_TheatreID",
                        column: x => x.TheatreID,
                        principalTable: "OperatingTheatreRecords",
                        principalColumn: "TheatreID");
                    table.ForeignKey(
                        name: "FK_CompleteSurgeries_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID");
                });

            migrationBuilder.CreateTable(
                name: "ConditionDiagnosisRecords",
                columns: table => new
                {
                    DiagnosisID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    ConditionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConditionDiagnosisRecords", x => x.DiagnosisID);
                    table.ForeignKey(
                        name: "FK_ConditionDiagnosisRecords_Condition_ConditionID",
                        column: x => x.ConditionID,
                        principalTable: "Condition",
                        principalColumn: "ConditionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConditionDiagnosisRecords_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GetMedicationOrder",
                columns: table => new
                {
                    OrderMedicationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PrescriptionOrder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsReceived = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PharmacistID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetMedicationOrder", x => x.OrderMedicationID);
                    table.ForeignKey(
                        name: "FK_GetMedicationOrder_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GetMedicationOrder_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GetVitalModels",
                columns: table => new
                {
                    VitalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReadingValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalRangeId = table.Column<int>(type: "int", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetVitalModels", x => x.VitalID);
                    table.ForeignKey(
                        name: "FK_GetVitalModels_GetNormalRanges_NormalRangeId",
                        column: x => x.NormalRangeId,
                        principalTable: "GetNormalRanges",
                        principalColumn: "NormalRangeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GetVitalModels_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalHistories",
                columns: table => new
                {
                    MedicalHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    DateRecorded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalHistories", x => x.MedicalHistoryID);
                    table.ForeignKey(
                        name: "FK_MedicalHistories_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientCondition",
                columns: table => new
                {
                    PatientConditionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    ConditionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientCondition", x => x.PatientConditionID);
                    table.ForeignKey(
                        name: "FK_PatientCondition_Condition_ConditionID",
                        column: x => x.ConditionID,
                        principalTable: "Condition",
                        principalColumn: "ConditionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientCondition_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionRecords",
                columns: table => new
                {
                    PrescriptionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurgeonID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NurseID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrescriptionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PharmacistID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionRecords", x => x.PrescriptionID);
                    table.ForeignKey(
                        name: "FK_PrescriptionRecords_AspNetUsers_NurseID",
                        column: x => x.NurseID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrescriptionRecords_AspNetUsers_PharmacistID",
                        column: x => x.PharmacistID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrescriptionRecords_AspNetUsers_SurgeonID",
                        column: x => x.SurgeonID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrescriptionRecords_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurgeryBooking",
                columns: table => new
                {
                    SurgeryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurgeonID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PatientID = table.Column<int>(type: "int", nullable: true),
                    SurgeryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Session = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnaesthesiologistID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TheatreID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurgeryBooking", x => x.SurgeryID);
                    table.ForeignKey(
                        name: "FK_SurgeryBooking_AspNetUsers_AnaesthesiologistID",
                        column: x => x.AnaesthesiologistID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SurgeryBooking_AspNetUsers_SurgeonID",
                        column: x => x.SurgeonID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SurgeryBooking_OperatingTheatreRecords_TheatreID",
                        column: x => x.TheatreID,
                        principalTable: "OperatingTheatreRecords",
                        principalColumn: "TheatreID");
                    table.ForeignKey(
                        name: "FK_SurgeryBooking_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID");
                });

            migrationBuilder.CreateTable(
                name: "PatientVs",
                columns: table => new
                {
                    PatientVId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VitalName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MinValue = table.Column<float>(type: "real", nullable: false),
                    MaxValue = table.Column<float>(type: "real", nullable: false),
                    CurrentValue = table.Column<float>(type: "real", nullable: false),
                    DateRecorded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VitalPatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientVs", x => x.PatientVId);
                    table.ForeignKey(
                        name: "FK_PatientVs_VitalPatients_VitalPatientId",
                        column: x => x.VitalPatientId,
                        principalTable: "VitalPatients",
                        principalColumn: "VitalPatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientVitals",
                columns: table => new
                {
                    PatientVitalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    VitalID = table.Column<int>(type: "int", nullable: false),
                    Value1 = table.Column<int>(type: "int", nullable: false),
                    Value2 = table.Column<int>(type: "int", nullable: true),
                    Value3 = table.Column<int>(type: "int", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientVitals", x => x.PatientVitalID);
                    table.ForeignKey(
                        name: "FK_PatientVitals_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientVitals_Vitals_VitalID",
                        column: x => x.VitalID,
                        principalTable: "Vitals",
                        principalColumn: "VitalID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyStockReports",
                columns: table => new
                {
                    ReportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationID = table.Column<int>(type: "int", nullable: false),
                    QuantityUsed = table.Column<int>(type: "int", nullable: false),
                    DateUsed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyStockReports", x => x.ReportID);
                    table.ForeignKey(
                        name: "FK_DailyStockReports_MedicationRecords_MedicationID",
                        column: x => x.MedicationID,
                        principalTable: "MedicationRecords",
                        principalColumn: "MedicationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DispensedPrescriptions",
                columns: table => new
                {
                    DispensedPrescriptionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionID = table.Column<int>(type: "int", nullable: false),
                    SurgeonID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrescriptionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PharmacistID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MedicationID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispensedPrescriptions", x => x.DispensedPrescriptionID);
                    table.ForeignKey(
                        name: "FK_DispensedPrescriptions_AspNetUsers_PharmacistID",
                        column: x => x.PharmacistID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DispensedPrescriptions_AspNetUsers_SurgeonID",
                        column: x => x.SurgeonID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DispensedPrescriptions_MedicationRecords_MedicationID",
                        column: x => x.MedicationID,
                        principalTable: "MedicationRecords",
                        principalColumn: "MedicationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DispensedPrescriptions_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicationActiveIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationID = table.Column<int>(type: "int", nullable: false),
                    IngredientID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationActiveIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicationActiveIngredients_ActiveIngredientRecords_IngredientID",
                        column: x => x.IngredientID,
                        principalTable: "ActiveIngredientRecords",
                        principalColumn: "IngredientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicationActiveIngredients_MedicationRecords_MedicationID",
                        column: x => x.MedicationID,
                        principalTable: "MedicationRecords",
                        principalColumn: "MedicationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicationAdministrations",
                columns: table => new
                {
                    AdministrationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationID = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PatientID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationAdministrations", x => x.AdministrationID);
                    table.ForeignKey(
                        name: "FK_MedicationAdministrations_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicationAdministrations_MedicationRecords_MedicationID",
                        column: x => x.MedicationID,
                        principalTable: "MedicationRecords",
                        principalColumn: "MedicationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicationAdministrations_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicationIngredients",
                columns: table => new
                {
                    MedicationIngredientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationID = table.Column<int>(type: "int", nullable: false),
                    IngredientID = table.Column<int>(type: "int", nullable: false),
                    ActiveIngredientStrength = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationIngredients", x => x.MedicationIngredientID);
                    table.ForeignKey(
                        name: "FK_MedicationIngredients_ActiveIngredientRecords_IngredientID",
                        column: x => x.IngredientID,
                        principalTable: "ActiveIngredientRecords",
                        principalColumn: "IngredientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicationIngredients_MedicationRecords_MedicationID",
                        column: x => x.MedicationID,
                        principalTable: "MedicationRecords",
                        principalColumn: "MedicationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicationInteractionRecords",
                columns: table => new
                {
                    InteractionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationID = table.Column<int>(type: "int", nullable: false),
                    InteractionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationInteractionRecords", x => x.InteractionID);
                    table.ForeignKey(
                        name: "FK_MedicationInteractionRecords_AspNetUsers_AdminID",
                        column: x => x.AdminID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicationInteractionRecords_MedicationRecords_MedicationID",
                        column: x => x.MedicationID,
                        principalTable: "MedicationRecords",
                        principalColumn: "MedicationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    ItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.ItemID);
                    table.ForeignKey(
                        name: "FK_OrderItem_MedicationRecords_MedicationID",
                        column: x => x.MedicationID,
                        principalTable: "MedicationRecords",
                        principalColumn: "MedicationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientMedications",
                columns: table => new
                {
                    PatientMedicationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    MedicationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientMedications", x => x.PatientMedicationID);
                    table.ForeignKey(
                        name: "FK_PatientMedications_MedicationRecords_MedicationID",
                        column: x => x.MedicationID,
                        principalTable: "MedicationRecords",
                        principalColumn: "MedicationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientMedications_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RejectedPrescriptions",
                columns: table => new
                {
                    RejectedPrescriptionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionID = table.Column<int>(type: "int", nullable: false),
                    SurgeonID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrescriptionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PharmacistID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MedicationID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RejectedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RejectedPrescriptions", x => x.RejectedPrescriptionID);
                    table.ForeignKey(
                        name: "FK_RejectedPrescriptions_AspNetUsers_PharmacistID",
                        column: x => x.PharmacistID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RejectedPrescriptions_AspNetUsers_SurgeonID",
                        column: x => x.SurgeonID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RejectedPrescriptions_MedicationRecords_MedicationID",
                        column: x => x.MedicationID,
                        principalTable: "MedicationRecords",
                        principalColumn: "MedicationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RejectedPrescriptions_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockRequests",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PharmacistID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicationID = table.Column<int>(type: "int", nullable: false),
                    FulfillmentDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockRequests", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK_StockRequests_AspNetUsers_PharmacistID",
                        column: x => x.PharmacistID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockRequests_MedicationRecords_MedicationID",
                        column: x => x.MedicationID,
                        principalTable: "MedicationRecords",
                        principalColumn: "MedicationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalRecords",
                columns: table => new
                {
                    RecordsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllergyID = table.Column<int>(type: "int", nullable: false),
                    CurrentMedication = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRecords", x => x.RecordsID);
                    table.ForeignKey(
                        name: "FK_MedicalRecords_Allergies_AllergyID",
                        column: x => x.AllergyID,
                        principalTable: "Allergies",
                        principalColumn: "AllergyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GetMedicationData",
                columns: table => new
                {
                    MedicineStoreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderMedicationID = table.Column<int>(type: "int", nullable: false),
                    MedicationID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetMedicationData", x => x.MedicineStoreId);
                    table.ForeignKey(
                        name: "FK_GetMedicationData_GetMedicationOrder_OrderMedicationID",
                        column: x => x.OrderMedicationID,
                        principalTable: "GetMedicationOrder",
                        principalColumn: "OrderMedicationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GetMedicationData_MedicationRecords_MedicationID",
                        column: x => x.MedicationID,
                        principalTable: "MedicationRecords",
                        principalColumn: "MedicationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalHistoryAllergies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalHistoryID = table.Column<int>(type: "int", nullable: false),
                    IngredientID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalHistoryAllergies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalHistoryAllergies_ActiveIngredientRecords_IngredientID",
                        column: x => x.IngredientID,
                        principalTable: "ActiveIngredientRecords",
                        principalColumn: "IngredientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalHistoryAllergies_MedicalHistories_MedicalHistoryID",
                        column: x => x.MedicalHistoryID,
                        principalTable: "MedicalHistories",
                        principalColumn: "MedicalHistoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalHistoryCondition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalHistoryID = table.Column<int>(type: "int", nullable: false),
                    ConditionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalHistoryCondition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalHistoryCondition_Condition_ConditionID",
                        column: x => x.ConditionID,
                        principalTable: "Condition",
                        principalColumn: "ConditionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalHistoryCondition_MedicalHistories_MedicalHistoryID",
                        column: x => x.MedicalHistoryID,
                        principalTable: "MedicalHistories",
                        principalColumn: "MedicalHistoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalHistoryMedications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalHistoryID = table.Column<int>(type: "int", nullable: false),
                    MedicationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalHistoryMedications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalHistoryMedications_MedicalHistories_MedicalHistoryID",
                        column: x => x.MedicalHistoryID,
                        principalTable: "MedicalHistories",
                        principalColumn: "MedicalHistoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalHistoryMedications_MedicationRecords_MedicationID",
                        column: x => x.MedicationID,
                        principalTable: "MedicationRecords",
                        principalColumn: "MedicationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DispensingInformation",
                columns: table => new
                {
                    DispensingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionID = table.Column<int>(type: "int", nullable: false),
                    DispensingPharmacist = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DispensingDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispensingInformation", x => x.DispensingID);
                    table.ForeignKey(
                        name: "FK_DispensingInformation_PrescriptionRecords_PrescriptionID",
                        column: x => x.PrescriptionID,
                        principalTable: "PrescriptionRecords",
                        principalColumn: "PrescriptionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionItems",
                columns: table => new
                {
                    PrescriptionItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionID = table.Column<int>(type: "int", nullable: false),
                    MedicationID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityAdministered = table.Column<int>(type: "int", nullable: false),
                    AdministeredDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionItems", x => x.PrescriptionItemID);
                    table.ForeignKey(
                        name: "FK_PrescriptionItems_MedicationRecords_MedicationID",
                        column: x => x.MedicationID,
                        principalTable: "MedicationRecords",
                        principalColumn: "MedicationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrescriptionItems_PrescriptionRecords_PrescriptionID",
                        column: x => x.PrescriptionID,
                        principalTable: "PrescriptionRecords",
                        principalColumn: "PrescriptionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionMedications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionID = table.Column<int>(type: "int", nullable: false),
                    MedicationID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityAdministered = table.Column<int>(type: "int", nullable: false),
                    AdministeredDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionMedications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrescriptionMedications_MedicationRecords_MedicationID",
                        column: x => x.MedicationID,
                        principalTable: "MedicationRecords",
                        principalColumn: "MedicationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrescriptionMedications_PrescriptionRecords_PrescriptionID",
                        column: x => x.PrescriptionID,
                        principalTable: "PrescriptionRecords",
                        principalColumn: "PrescriptionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientAdmissions",
                columns: table => new
                {
                    PatientAdmissionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    AdmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DischargeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WardID = table.Column<int>(type: "int", nullable: false),
                    BedID = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    SurgeryBID = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    BMI = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAdmissions", x => x.PatientAdmissionID);
                    table.ForeignKey(
                        name: "FK_PatientAdmissions_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientAdmissions_BedRecords_BedID",
                        column: x => x.BedID,
                        principalTable: "BedRecords",
                        principalColumn: "BedID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAdmissions_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientAdmissions_SurgeryBooking_SurgeryBID",
                        column: x => x.SurgeryBID,
                        principalTable: "SurgeryBooking",
                        principalColumn: "SurgeryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAdmissions_WardRecords_WardID",
                        column: x => x.WardID,
                        principalTable: "WardRecords",
                        principalColumn: "WardID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SurgeryBookingTreatments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurgeryID = table.Column<int>(type: "int", nullable: false),
                    TreatmentID = table.Column<int>(type: "int", nullable: false),
                    CompleteSurgeryID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurgeryBookingTreatments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurgeryBookingTreatments_CompleteSurgeries_CompleteSurgeryID",
                        column: x => x.CompleteSurgeryID,
                        principalTable: "CompleteSurgeries",
                        principalColumn: "CompleteSurgeryID");
                    table.ForeignKey(
                        name: "FK_SurgeryBookingTreatments_SurgeryBooking_SurgeryID",
                        column: x => x.SurgeryID,
                        principalTable: "SurgeryBooking",
                        principalColumn: "SurgeryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurgeryBookingTreatments_TreatmentRecords_TreatmentID",
                        column: x => x.TreatmentID,
                        principalTable: "TreatmentRecords",
                        principalColumn: "TreatmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientAssesments",
                columns: table => new
                {
                    PatientAssesmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    RecordsID = table.Column<int>(type: "int", nullable: false),
                    AssasmentDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAssesments", x => x.PatientAssesmentID);
                    table.ForeignKey(
                        name: "FK_PatientAssesments_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAssesments_MedicalRecords_RecordsID",
                        column: x => x.RecordsID,
                        principalTable: "MedicalRecords",
                        principalColumn: "RecordsID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAssesments_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Allergies_IngredientID",
                table: "Allergies",
                column: "IngredientID");

            migrationBuilder.CreateIndex(
                name: "IX_Allergies_PatientID",
                table: "Allergies",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_AnaesthesiaOrder_Id",
                table: "AnaesthesiaOrder",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AnaesthesiaOrder_PatientId",
                table: "AnaesthesiaOrder",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompleteSurgeries_AnaesthesiologistID",
                table: "CompleteSurgeries",
                column: "AnaesthesiologistID");

            migrationBuilder.CreateIndex(
                name: "IX_CompleteSurgeries_PatientID",
                table: "CompleteSurgeries",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_CompleteSurgeries_SurgeonID",
                table: "CompleteSurgeries",
                column: "SurgeonID");

            migrationBuilder.CreateIndex(
                name: "IX_CompleteSurgeries_TheatreID",
                table: "CompleteSurgeries",
                column: "TheatreID");

            migrationBuilder.CreateIndex(
                name: "IX_ConditionDiagnosisRecords_ConditionID",
                table: "ConditionDiagnosisRecords",
                column: "ConditionID");

            migrationBuilder.CreateIndex(
                name: "IX_ConditionDiagnosisRecords_PatientID",
                table: "ConditionDiagnosisRecords",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_ContraIndicationsRecords_ConditionID",
                table: "ContraIndicationsRecords",
                column: "ConditionID");

            migrationBuilder.CreateIndex(
                name: "IX_ContraIndicationsRecords_IngredientID",
                table: "ContraIndicationsRecords",
                column: "IngredientID");

            migrationBuilder.CreateIndex(
                name: "IX_DailyStockReports_MedicationID",
                table: "DailyStockReports",
                column: "MedicationID");

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

            migrationBuilder.CreateIndex(
                name: "IX_DispensingInformation_PrescriptionID",
                table: "DispensingInformation",
                column: "PrescriptionID");

            migrationBuilder.CreateIndex(
                name: "IX_GetMedicationData_MedicationID",
                table: "GetMedicationData",
                column: "MedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_GetMedicationData_OrderMedicationID",
                table: "GetMedicationData",
                column: "OrderMedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_GetMedicationOrder_Id",
                table: "GetMedicationOrder",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GetMedicationOrder_PatientID",
                table: "GetMedicationOrder",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_GetNortifications_GetAnaesthesiologistId",
                table: "GetNortifications",
                column: "GetAnaesthesiologistId");

            migrationBuilder.CreateIndex(
                name: "IX_GetVitalModels_NormalRangeId",
                table: "GetVitalModels",
                column: "NormalRangeId");

            migrationBuilder.CreateIndex(
                name: "IX_GetVitalModels_PatientID",
                table: "GetVitalModels",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_PatientID",
                table: "MedicalHistories",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistoryAllergies_IngredientID",
                table: "MedicalHistoryAllergies",
                column: "IngredientID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistoryAllergies_MedicalHistoryID",
                table: "MedicalHistoryAllergies",
                column: "MedicalHistoryID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistoryCondition_ConditionID",
                table: "MedicalHistoryCondition",
                column: "ConditionID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistoryCondition_MedicalHistoryID",
                table: "MedicalHistoryCondition",
                column: "MedicalHistoryID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistoryMedications_MedicalHistoryID",
                table: "MedicalHistoryMedications",
                column: "MedicalHistoryID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistoryMedications_MedicationID",
                table: "MedicalHistoryMedications",
                column: "MedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_AllergyID",
                table: "MedicalRecords",
                column: "AllergyID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationActiveIngredients_IngredientID",
                table: "MedicationActiveIngredients",
                column: "IngredientID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationActiveIngredients_MedicationID",
                table: "MedicationActiveIngredients",
                column: "MedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationAdministrations_Id",
                table: "MedicationAdministrations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationAdministrations_MedicationID",
                table: "MedicationAdministrations",
                column: "MedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationAdministrations_PatientID",
                table: "MedicationAdministrations",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationAlerts_IngredientID",
                table: "MedicationAlerts",
                column: "IngredientID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationIngredients_IngredientID",
                table: "MedicationIngredients",
                column: "IngredientID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationIngredients_MedicationID",
                table: "MedicationIngredients",
                column: "MedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationInteractionRecords_AdminID",
                table: "MedicationInteractionRecords",
                column: "AdminID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationInteractionRecords_MedicationID",
                table: "MedicationInteractionRecords",
                column: "MedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationOrders_OrderMedicationID",
                table: "MedicationOrders",
                column: "OrderMedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationRecords_ConditionID",
                table: "MedicationRecords",
                column: "ConditionID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationRecords_DosageFormID",
                table: "MedicationRecords",
                column: "DosageFormID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_MedicationID",
                table: "OrderItem",
                column: "MedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAdmissions_BedID",
                table: "PatientAdmissions",
                column: "BedID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAdmissions_Id",
                table: "PatientAdmissions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAdmissions_PatientID",
                table: "PatientAdmissions",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAdmissions_SurgeryBID",
                table: "PatientAdmissions",
                column: "SurgeryBID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAdmissions_WardID",
                table: "PatientAdmissions",
                column: "WardID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAssesments_Id",
                table: "PatientAssesments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAssesments_PatientID",
                table: "PatientAssesments",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAssesments_RecordsID",
                table: "PatientAssesments",
                column: "RecordsID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientCondition_ConditionID",
                table: "PatientCondition",
                column: "ConditionID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientCondition_PatientID",
                table: "PatientCondition",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedications_MedicationID",
                table: "PatientMedications",
                column: "MedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedications_PatientID",
                table: "PatientMedications",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVitals_PatientID",
                table: "PatientVitals",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVitals_VitalID",
                table: "PatientVitals",
                column: "VitalID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVs_VitalPatientId",
                table: "PatientVs",
                column: "VitalPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionItems_MedicationID",
                table: "PrescriptionItems",
                column: "MedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionItems_PrescriptionID",
                table: "PrescriptionItems",
                column: "PrescriptionID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedications_MedicationID",
                table: "PrescriptionMedications",
                column: "MedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedications_PrescriptionID",
                table: "PrescriptionMedications",
                column: "PrescriptionID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionRecords_NurseID",
                table: "PrescriptionRecords",
                column: "NurseID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionRecords_PatientID",
                table: "PrescriptionRecords",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionRecords_PharmacistID",
                table: "PrescriptionRecords",
                column: "PharmacistID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionRecords_SurgeonID",
                table: "PrescriptionRecords",
                column: "SurgeonID");

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
                name: "IX_StockRequests_MedicationID",
                table: "StockRequests",
                column: "MedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_StockRequests_PharmacistID",
                table: "StockRequests",
                column: "PharmacistID");

            migrationBuilder.CreateIndex(
                name: "IX_SurgeryBooking_AnaesthesiologistID",
                table: "SurgeryBooking",
                column: "AnaesthesiologistID");

            migrationBuilder.CreateIndex(
                name: "IX_SurgeryBooking_PatientID",
                table: "SurgeryBooking",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_SurgeryBooking_SurgeonID",
                table: "SurgeryBooking",
                column: "SurgeonID");

            migrationBuilder.CreateIndex(
                name: "IX_SurgeryBooking_TheatreID",
                table: "SurgeryBooking",
                column: "TheatreID");

            migrationBuilder.CreateIndex(
                name: "IX_SurgeryBookingTreatments_CompleteSurgeryID",
                table: "SurgeryBookingTreatments",
                column: "CompleteSurgeryID");

            migrationBuilder.CreateIndex(
                name: "IX_SurgeryBookingTreatments_SurgeryID",
                table: "SurgeryBookingTreatments",
                column: "SurgeryID");

            migrationBuilder.CreateIndex(
                name: "IX_SurgeryBookingTreatments_TreatmentID",
                table: "SurgeryBookingTreatments",
                column: "TreatmentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allergy");

            migrationBuilder.DropTable(
                name: "AnaesthesiaOrder");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ConditionDiagnosisRecords");

            migrationBuilder.DropTable(
                name: "ContraIndicationsRecords");

            migrationBuilder.DropTable(
                name: "DailyStockReports");

            migrationBuilder.DropTable(
                name: "DayHospitals");

            migrationBuilder.DropTable(
                name: "DispensedPrescriptions");

            migrationBuilder.DropTable(
                name: "DispensingInformation");

            migrationBuilder.DropTable(
                name: "GetMedicationData");

            migrationBuilder.DropTable(
                name: "GetNortifications");

            migrationBuilder.DropTable(
                name: "GetVitalModels");

            migrationBuilder.DropTable(
                name: "MedicalHistoryAllergies");

            migrationBuilder.DropTable(
                name: "MedicalHistoryCondition");

            migrationBuilder.DropTable(
                name: "MedicalHistoryMedications");

            migrationBuilder.DropTable(
                name: "MedicationActiveIngredients");

            migrationBuilder.DropTable(
                name: "MedicationAdministrations");

            migrationBuilder.DropTable(
                name: "MedicationAlerts");

            migrationBuilder.DropTable(
                name: "MedicationIngredients");

            migrationBuilder.DropTable(
                name: "MedicationInteractionRecords");

            migrationBuilder.DropTable(
                name: "MedicationOrders");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "PatientAdmissions");

            migrationBuilder.DropTable(
                name: "PatientAssesments");

            migrationBuilder.DropTable(
                name: "PatientCondition");

            migrationBuilder.DropTable(
                name: "PatientMedications");

            migrationBuilder.DropTable(
                name: "PatientVitals");

            migrationBuilder.DropTable(
                name: "PatientVs");

            migrationBuilder.DropTable(
                name: "PrescriptionItems");

            migrationBuilder.DropTable(
                name: "PrescriptionMedications");

            migrationBuilder.DropTable(
                name: "RejectedPrescriptions");

            migrationBuilder.DropTable(
                name: "StockRequests");

            migrationBuilder.DropTable(
                name: "SurgeryBookingTreatments");

            migrationBuilder.DropTable(
                name: "VitalType");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "GetMedicationOrder");

            migrationBuilder.DropTable(
                name: "GetNormalRanges");

            migrationBuilder.DropTable(
                name: "MedicalHistories");

            migrationBuilder.DropTable(
                name: "OrderMedications");

            migrationBuilder.DropTable(
                name: "BedRecords");

            migrationBuilder.DropTable(
                name: "WardRecords");

            migrationBuilder.DropTable(
                name: "MedicalRecords");

            migrationBuilder.DropTable(
                name: "Vitals");

            migrationBuilder.DropTable(
                name: "VitalPatients");

            migrationBuilder.DropTable(
                name: "PrescriptionRecords");

            migrationBuilder.DropTable(
                name: "MedicationRecords");

            migrationBuilder.DropTable(
                name: "CompleteSurgeries");

            migrationBuilder.DropTable(
                name: "SurgeryBooking");

            migrationBuilder.DropTable(
                name: "TreatmentRecords");

            migrationBuilder.DropTable(
                name: "Allergies");

            migrationBuilder.DropTable(
                name: "Condition");

            migrationBuilder.DropTable(
                name: "DosageForm");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "OperatingTheatreRecords");

            migrationBuilder.DropTable(
                name: "ActiveIngredientRecords");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
