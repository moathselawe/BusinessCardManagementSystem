using System.Reflection;

namespace HireMind.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<BusinessCard> BusinessCards { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Lookup> Lookups { get; set; }
    public DbSet<AnalyzeCv> AnalyzeCvs { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<HiringStage> HiringStages { get; set; }
    public DbSet<ApplicationStage> ApplicationStages { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ignore Questions property in Job
        modelBuilder.Entity<Job>()
            .Ignore(j => j.Questions);

        // Ignore Questions property in Job
        modelBuilder.Entity<HiringStage>()
            .Ignore(s => s.InterviewQuestions)
            .Ignore(s => s.ExamQuestions);

        // Apply configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // ===== GENERIC: Fix multiple cascade path issues =====
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var foreignKey in entityType.GetForeignKeys())
            {
                // Optional: keep cascade only for AnalyzeCv -> Job
                if (foreignKey.PrincipalEntityType.ClrType == typeof(Job) &&
                    foreignKey.DeclaringEntityType.ClrType == typeof(AnalyzeCv))
                {
                    foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
                }
                else
                {
                    // All other FKs use Restrict to prevent multiple cascade paths
                    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }
        }



        // JobApplication -> ApplicationStages
        modelBuilder.Entity<ApplicationStage>()
            .HasOne(a => a.JobApplication)
            .WithMany(j => j.ApplicationStages)
            .HasForeignKey(a => a.JobApplicationId)
            .OnDelete(DeleteBehavior.Restrict);

        // HiringStage -> ApplicationStages
        modelBuilder.Entity<ApplicationStage>()
            .HasOne(a => a.HiringStage)
            .WithMany(h => h.ApplicationStages)
            .HasForeignKey(a => a.HiringStageId)
            .OnDelete(DeleteBehavior.Restrict);

        // CurrentStage relation
        modelBuilder.Entity<JobApplication>()
            .HasOne(j => j.CurrentStage)
            .WithMany()
            .HasForeignKey(j => j.CurrentStageId)
            .OnDelete(DeleteBehavior.Restrict);

        // FinalStage relation
        //modelBuilder.Entity<JobApplication>()
        //    .HasOne(j => j.FinalStage)
        //    .WithMany()
        //    .HasForeignKey(j => j.FinalStageId)
        //    .OnDelete(DeleteBehavior.Restrict);

        // Configure PersonalInfo as owned type
        modelBuilder.Entity<JobApplication>()
            .OwnsOne(j => j.PersonalInfo, pi =>
            {
                pi.Property(p => p.FullName).HasMaxLength(200);
                pi.Property(p => p.MobileNumber).HasMaxLength(20);
                pi.Property(p => p.EmailAddress).HasMaxLength(200);

                // For CountryCode relation
                pi.HasOne(p => p.CountryCode)
                  .WithMany()
                  .HasForeignKey(p => p.CountryCodeId)
                  .OnDelete(DeleteBehavior.Restrict);
            });

    }
}