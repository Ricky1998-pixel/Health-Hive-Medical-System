using Health_Hive_Project_2024.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Health_Hive_Project_2024.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Health_Hive_Project_2024.Models.ActiveIngredientRecords> ActiveIngredientRecords { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.Allergies> Allergies { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.AnaesthesiaOrder> AnaesthesiaOrder { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.ConditionDiagnosisRecords> ConditionDiagnosisRecords { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.ContraIndicationsRecords> ContraIndicationsRecords { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.DailyStockReport> DailyStockReports { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.DayHospital> DayHospitals { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.DispensingInformation> DispensingInformation { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.MedicationAdministration> MedicationAdministrations { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.MedicationInteractionRecords> MedicationInteractionRecords { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.MedicationRecords> MedicationRecords { get; set; }

        public DbSet<Health_Hive_Project_2024.Models.OperatingTheatreRecords> OperatingTheatreRecords { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.Patient> Patients { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.PatientAdmission> PatientAdmissions { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.PatientCondition> PatientCondition { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.PatientMedication> PatientMedications { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.PatientVitals> PatientVitals { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.PrescriptionRecords> PrescriptionRecords { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.TreatmentRecords> TreatmentRecords { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.VitalType> VitalType { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.WardRecords> WardRecords { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.SurgeryBooking> SurgeryBooking { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.MedicalProfessionalRecords> MedicalProfessionalRecords { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.OrderItem> OrderItem { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.StockRequest> StockRequests { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.BedRecords> BedRecords { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.DosageForm> DosageForm { get; set;}
        public DbSet<Health_Hive_Project_2024.Models.Condition> Condition { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.MedicationAlert> MedicationAlerts { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.MedicationIngredient> MedicationIngredients { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.PatientAssesment> PatientAssesments { get; set; }
        public DbSet<Health_Hive_Project_2024.Models.MedicalRecords> MedicalRecords { get; set; }
        public DbSet<DispensedPrescription> DispensedPrescriptions { get; set; }
        public DbSet<RejectedPrescription> RejectedPrescriptions { get; set; }
        public DbSet<OrderMedication> OrderMedications { get; set; }
        public DbSet<MedicationOrder> MedicationOrders { get; set; }
        public DbSet<VitalPatient> VitalPatients { get; set; }
        public DbSet<PatientV> PatientVs { get; set; }
        public DbSet<IdentityRole> IdentityRole { get; set; }
        public DbSet<OrderMedications> GetMedicationOrder { get; set; }
        public DbSet<MedicationDataStore> GetMedicationData { get; set; }
        public DbSet<NormalRange> GetNormalRanges { get; set; }
        public DbSet<VitalModel> GetVitalModels { get; set; }
        public DbSet<NortificationModel> GetNortifications { get; set; }

        //new line of code
        public DbSet<MedicalHistoryAllergy> MedicalHistoryAllergies { get; set; }
        public DbSet<MedicalHistoryMedication> MedicalHistoryMedications { get; set; }

        public DbSet<MedicalHistoryCondition> MedicalHistoryCondition { get; set; }

        public DbSet<PrescriptionMedications> PrescriptionMedications { get; set; }

        //Complete surgeries to be stored
        public DbSet<CompleteSurgery> CompleteSurgeries { get; set; }

        //new lines
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
        public DbSet<Allergy> Allergy { get; set; }
        public DbSet<SurgeryBookingTreatment> SurgeryBookingTreatments { get; set; }

        //new dbset for medication records & active ingredients
        public DbSet<MedicationActiveIngredient> MedicationActiveIngredients { get; set; }

        //new code
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the SurgeryBookingTreatment entity
            modelBuilder.Entity<SurgeryBookingTreatment>()
                .HasKey(sbt => sbt.Id); // Primary Key

            //Model Builder for SurgeryBookingTreatments
            modelBuilder.Entity<SurgeryBookingTreatment>()
            .HasOne(sbt => sbt.SurgeryBooking)
            .WithMany(sb => sb.SurgeryBookingTreatments)
            .HasForeignKey(sbt => sbt.SurgeryID);

            modelBuilder.Entity<SurgeryBookingTreatment>()
                .HasOne(sbt => sbt.TreatmentRecords)
                .WithMany(tr => tr.SurgeryBookingTreatments)
                .HasForeignKey(sbt => sbt.TreatmentID);

            //Model Builder for above New Code
            //new
            modelBuilder.Entity<MedicalHistoryCondition>()
           .HasKey(mhc => mhc.Id);

            modelBuilder.Entity<MedicalHistoryCondition>()
                .HasOne(mhc => mhc.MedicalHistory)
                .WithMany(mh => mh.MedicalHistoryCondition)
                .HasForeignKey(mhc => mhc.MedicalHistoryID);

            modelBuilder.Entity<MedicalHistoryCondition>()
                .HasOne(mhc => mhc.Condition)
                .WithMany()
                .HasForeignKey(mhc => mhc.ConditionID); ;


            modelBuilder.Entity<OrderMedications>()
           .HasMany(o => o.GetMedicationData)  // OrderMedications has many MedicationDataStores
           .WithOne(m => m.GetOrder)  // MedicationDataStore has one OrderMedication
           .HasForeignKey(m => m.OrderMedicationID)  // Foreign key in MedicationDataStore
           .OnDelete(DeleteBehavior.Restrict);



            // Configure foreign key relationships for PatientAdmission
            modelBuilder.Entity<PatientAdmission>()
                .HasOne(p => p.Patient)
                .WithMany()
                .HasForeignKey(p => p.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PatientAdmission>()
                .HasOne(p => p.Nurse)
                .WithMany()
                .HasForeignKey(p => p.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PatientAdmission>()
                .HasOne(p => p.Ward)
                .WithMany()
                .HasForeignKey(p => p.WardID)
                .OnDelete(DeleteBehavior.Restrict);






            // Configure other entity relationships and model configurations here
            // Configure MedicationIngredient relationships
            //modelBuilder.Entity<MedicationIngredient>()
            //    .HasKey(mi => mi.MedicationIngredientID);

            //modelBuilder.Entity<MedicationIngredient>()
            //    .HasOne(mi => mi.Medication)
            //    .WithMany(m => m.MedicationIngredients)
            //    .HasForeignKey(mi => mi.MedicationID)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<MedicationIngredient>()
            //    .HasOne(mi => mi.Ingredient)
            //    .WithMany()
            //    .HasForeignKey(mi => mi.IngredientID)
            //    .OnDelete(DeleteBehavior.Restrict);

            // Configure other relationships here if needed...

            modelBuilder.Entity<MedicationRecords>().ToTable("MedicationRecords");
            modelBuilder.Entity<MedicationIngredient>().ToTable("MedicationIngredients");
            modelBuilder.Entity<ActiveIngredientRecords>().ToTable("ActiveIngredientRecords");

            // Configure foreign key relationships for PatientCondition
            modelBuilder.Entity<PatientCondition>()
                .HasOne(pc => pc.Patient)
                .WithMany()
                .HasForeignKey(pc => pc.PatientID)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict instead of Cascade for Patients

            modelBuilder.Entity<PatientCondition>()
                .HasOne(pc => pc.Condition)
                .WithMany()
                .HasForeignKey(pc => pc.ConditionID)
                .OnDelete(DeleteBehavior.Cascade); // Keep Cascade for Condition if necessary

            // Configure other entity relationships and model configurations here

            // Configure foreign key relationships for PatientAssesments
            // Configure foreign key relationships for PatientAssesments
            modelBuilder.Entity<PatientAssesment>()
                .HasOne(pa => pa.Patient)
                .WithMany()
                .HasForeignKey(pa => pa.PatientID)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict instead of Cascade

            modelBuilder.Entity<PatientAssesment>()
                .HasOne(pa => pa.Anaesthesiologist)
                .WithMany()
                .HasForeignKey(pa => pa.Id)
                .OnDelete(DeleteBehavior.Cascade); // Keep Cascade if needed

            modelBuilder.Entity<PatientAssesment>()
                .HasOne(pa => pa.MedicalRecord)
                .WithMany()
                .HasForeignKey(pa => pa.RecordsID)
                .OnDelete(DeleteBehavior.Cascade); // Keep Cascade if needed

            modelBuilder.Entity<OrderMedications>()
                .HasOne(pc => pc.GetAnaesthesiologist)
                .WithMany()
                .HasForeignKey(pc => pc.Id)
                .OnDelete(DeleteBehavior.Cascade);
            // Other configurations...

            // Specify the table name for PatientAssesments explicitly
            modelBuilder.Entity<PatientAssesment>().ToTable("PatientAssesments");
        }
        public DbSet<Health_Hive_Project_2024.Models.Vitals> Vitals { get; set; } = default!;

    }

}
