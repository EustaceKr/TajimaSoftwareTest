using Data.Context.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : base(opt){    }
        public DbSet<Design> Designs { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<TemplateDesign> TemplateDesigns { get; set; }
        public DbSet<LogEntry> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()")
                        .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                    modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime?>("UpdatedDate")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("GETDATE()");
                }
            }

            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship
            modelBuilder.Entity<TemplateDesign>()
                .HasKey(td => new { td.TemplateId, td.DesignId });

            modelBuilder.Entity<TemplateDesign>()
                .HasOne(td => td.Template)
                .WithMany(t => t.TemplateDesigns)
                .HasForeignKey(td => td.TemplateId);

            modelBuilder.Entity<TemplateDesign>()
                .HasOne(td => td.Design)
                .WithMany(d => d.TemplateDesigns)
                .HasForeignKey(td => td.DesignId);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property("CreatedDate").IsModified = false;
                    ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}
