using EducationProject.Core.DAL.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.EFCore
{
    public class EducationProjectDbContext: DbContext
    {
        private string connectionString;

        public EducationProjectDbContext()
        {
            connectionString = @"Data Source=ADMINPC;Initial Catalog=EducationProjectDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        public EducationProjectDbContext(DbContextOptions options)
            :base(options)
        {
            
        }

        public EducationProjectDbContext(string dbConnectionString)
            :base()
        {
            connectionString = dbConnectionString;
        }

        public DbSet<AccountDBO> Accounts { get; set; }

        public DbSet<CourseDBO> Courses { get; set; }

        public DbSet<SkillDBO> Skills { get; set; }

        public DbSet<BaseMaterialDBO> Materials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AccountDBO>(entity =>
            {
                entity.ToTable("Accounts");
                entity.HasKey(a => a.Id);
                entity.HasAlternateKey(a => a.Email);
                entity.HasIndex(a => a.PhoneNumber)
                .IsUnique(true);
            });

            modelBuilder.Entity<AccountCourseDBO>(entity =>
            {
                entity.ToTable("AccountCourses");
                entity.HasKey(p => new { p.AccountId, p.CourseId });
                
                entity.HasOne(ac => ac.Account)
                .WithMany(a => a.AccountCourses)
                .HasForeignKey(ac => ac.AccountId);
                entity.HasOne(ac => ac.Course)
                .WithMany(c => c.AccountCourses)
                .HasForeignKey(ac => ac.CourseId);
            });

            modelBuilder.Entity<CourseDBO>(entity =>
            {
                entity.ToTable("Courses");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.CreatorId)
                .IsRequired(false);

                entity.HasOne(c => c.CreatorAccount)
                .WithMany(a => a.CreatedCourses)
                .HasForeignKey(c => c.CreatorId)
                .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<CourseSkillDBO>(entity =>
            {
                entity.ToTable("CourseSkills");
                entity.HasKey(cs => new { cs.CourseId, cs.SkillId });
                
                entity.HasOne(cs => cs.Skill)
                .WithMany(s => s.CourseSkills)
                .HasForeignKey(cs => cs.SkillId);
                entity.HasOne(cs => cs.Course)
                .WithMany(c => c.CourseSkills)
                .HasForeignKey(cs => cs.CourseId);
                
            });

            modelBuilder.Entity<SkillDBO>(entity =>
            {
                entity.ToTable("Skills");
                entity.HasKey(s => s.Id);
            });

            modelBuilder.Entity<CourseMaterialDBO>(entity =>
            {
                entity.ToTable("CourseMaterials");
                entity.HasKey(cm => new { cm.CourseId, cm.MaterialId });
                
                entity.HasOne(cm => cm.Course)
                .WithMany(c => c.CourseMaterials)
                .HasForeignKey(cm => cm.CourseId);
                entity.HasOne(cm => cm.Material)
                .WithMany(m => m.CourseMaterials)
                .HasForeignKey(cm => cm.MaterialId);
                
            });

            modelBuilder.Entity<AccountMaterialDBO>(entity =>
            {
                entity.ToTable("AccountMaterials");
                entity.HasKey(am => new { am.AccountId, am.MaterialId });
                
                entity.HasOne(am => am.Account)
                .WithMany(a => a.AccountMaterials)
                .HasForeignKey(am => am.AccountId);
                entity.HasOne(am => am.Material)
                .WithMany(m => m.AccountMaterials)
                .HasForeignKey(am => am.MaterialId);
                
            });

            modelBuilder.Entity<BaseMaterialDBO>(entity =>
            {
                entity.ToTable("BaseMaterials");
                entity.HasKey(bm => bm.Id);
            });

            modelBuilder.Entity<BookMaterialDBO>(entity =>
            {
                entity.ToTable("BookMaterials");
            });

            modelBuilder.Entity<ArticleMaterialDBO>(entity =>
            {
                entity.ToTable("ArticleMaterials");                
            });

            modelBuilder.Entity<VideoMaterialDBO>(entity =>
            {
                entity.ToTable("VideoMaterials");
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
