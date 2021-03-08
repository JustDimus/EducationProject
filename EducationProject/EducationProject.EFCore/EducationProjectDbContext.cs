using EducationProject.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationProject.EFCore
{
    public class EducationProjectDbContext: DbContext
    {        
        public EducationProjectDbContext(DbContextOptions options)
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Accounts");
                entity.HasKey(a => a.Id);
                entity.HasAlternateKey(a => a.Email);
                entity.HasIndex(a => a.PhoneNumber)
                .IsUnique(true);
            });

            modelBuilder.Entity<AccountCourse>(entity =>
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

            modelBuilder.Entity<Course>(entity =>
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

            modelBuilder.Entity<CourseSkill>(entity =>
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

            modelBuilder.Entity<AccountSkill>(entity =>
            {
                entity.ToTable("AccountSkills");
                entity.HasKey(acs => new { acs.AccountId, acs.SkillId });

                entity.HasOne(cs => cs.Skill)
                .WithMany(s => s.AccountSkills)
                .HasForeignKey(cs => cs.SkillId);
                entity.HasOne(cs => cs.Account)
                .WithMany(a => a.AccountSkills)
                .HasForeignKey(cs => cs.AccountId);
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.ToTable("Skills");
                entity.HasKey(s => s.Id);
            });

            modelBuilder.Entity<CourseMaterial>(entity =>
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

            modelBuilder.Entity<AccountMaterial>(entity =>
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

            modelBuilder.Entity<BaseMaterial>(entity =>
            {
                entity.ToTable("BaseMaterials");
                entity.HasKey(bm => bm.Id);
            });

            modelBuilder.Entity<BookMaterial>(entity =>
            {
                entity.ToTable("BookMaterials");
            });

            modelBuilder.Entity<ArticleMaterial>(entity =>
            {
                entity.ToTable("ArticleMaterials");
            });

            modelBuilder.Entity<VideoMaterial>(entity =>
            {
                entity.ToTable("VideoMaterials");
            });
        }
    }
}
