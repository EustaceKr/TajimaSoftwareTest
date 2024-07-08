﻿using Data.Context.Entities;
using Microsoft.EntityFrameworkCore;

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
    }
}