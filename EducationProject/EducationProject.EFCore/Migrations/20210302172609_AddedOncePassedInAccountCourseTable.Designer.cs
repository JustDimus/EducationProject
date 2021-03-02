﻿// <auto-generated />
using System;
using EducationProject.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EducationProject.EFCore.Migrations
{
    [DbContext(typeof(EducationProjectDbContext))]
    [Migration("20210302172609_AddedOncePassedInAccountCourseTable")]
    partial class AddedOncePassedInAccountCourseTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EducationProject.Core.DAL.EF.AccountCourseDBO", b =>
                {
                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<bool>("OncePassed")
                        .HasColumnType("bit");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("AccountId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("AccountCourses");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.AccountDBO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecondName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Email");

                    b.HasIndex("PhoneNumber")
                        .IsUnique()
                        .HasFilter("[PhoneNumber] IS NOT NULL");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.AccountMaterialDBO", b =>
                {
                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("MaterialId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("AccountId", "MaterialId");

                    b.HasIndex("MaterialId");

                    b.ToTable("AccountMaterials");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.AccountSkillDBO", b =>
                {
                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.Property<int>("CurrentResult")
                        .HasColumnType("int");

                    b.HasKey("AccountId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("AccountSkills");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.BaseMaterialDBO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("BaseMaterials");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.CourseDBO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.CourseMaterialDBO", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("MaterialId")
                        .HasColumnType("int");

                    b.HasKey("CourseId", "MaterialId");

                    b.HasIndex("MaterialId");

                    b.ToTable("CourseMaterials");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.CourseSkillDBO", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.Property<int>("Change")
                        .HasColumnType("int");

                    b.HasKey("CourseId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("CourseSkills");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.SkillDBO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxValue")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.ArticleMaterialDBO", b =>
                {
                    b.HasBaseType("EducationProject.Core.DAL.EF.BaseMaterialDBO");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("URI")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("ArticleMaterials");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.BookMaterialDBO", b =>
                {
                    b.HasBaseType("EducationProject.Core.DAL.EF.BaseMaterialDBO");

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Pages")
                        .HasColumnType("int");

                    b.ToTable("BookMaterials");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.VideoMaterialDBO", b =>
                {
                    b.HasBaseType("EducationProject.Core.DAL.EF.BaseMaterialDBO");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<int>("Quality")
                        .HasColumnType("int");

                    b.Property<string>("URI")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("VideoMaterials");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.AccountCourseDBO", b =>
                {
                    b.HasOne("EducationProject.Core.DAL.EF.AccountDBO", "Account")
                        .WithMany("AccountCourses")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationProject.Core.DAL.EF.CourseDBO", "Course")
                        .WithMany("AccountCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.AccountMaterialDBO", b =>
                {
                    b.HasOne("EducationProject.Core.DAL.EF.AccountDBO", "Account")
                        .WithMany("AccountMaterials")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationProject.Core.DAL.EF.BaseMaterialDBO", "Material")
                        .WithMany("AccountMaterials")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Material");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.AccountSkillDBO", b =>
                {
                    b.HasOne("EducationProject.Core.DAL.EF.AccountDBO", "Account")
                        .WithMany("AccountSkills")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationProject.Core.DAL.EF.SkillDBO", "Skill")
                        .WithMany("AccountSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.CourseDBO", b =>
                {
                    b.HasOne("EducationProject.Core.DAL.EF.AccountDBO", "CreatorAccount")
                        .WithMany("CreatedCourses")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("CreatorAccount");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.CourseMaterialDBO", b =>
                {
                    b.HasOne("EducationProject.Core.DAL.EF.CourseDBO", "Course")
                        .WithMany("CourseMaterials")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationProject.Core.DAL.EF.BaseMaterialDBO", "Material")
                        .WithMany("CourseMaterials")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Material");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.CourseSkillDBO", b =>
                {
                    b.HasOne("EducationProject.Core.DAL.EF.CourseDBO", "Course")
                        .WithMany("CourseSkills")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationProject.Core.DAL.EF.SkillDBO", "Skill")
                        .WithMany("CourseSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.ArticleMaterialDBO", b =>
                {
                    b.HasOne("EducationProject.Core.DAL.EF.BaseMaterialDBO", null)
                        .WithOne()
                        .HasForeignKey("EducationProject.Core.DAL.EF.ArticleMaterialDBO", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.BookMaterialDBO", b =>
                {
                    b.HasOne("EducationProject.Core.DAL.EF.BaseMaterialDBO", null)
                        .WithOne()
                        .HasForeignKey("EducationProject.Core.DAL.EF.BookMaterialDBO", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.VideoMaterialDBO", b =>
                {
                    b.HasOne("EducationProject.Core.DAL.EF.BaseMaterialDBO", null)
                        .WithOne()
                        .HasForeignKey("EducationProject.Core.DAL.EF.VideoMaterialDBO", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.AccountDBO", b =>
                {
                    b.Navigation("AccountCourses");

                    b.Navigation("AccountMaterials");

                    b.Navigation("AccountSkills");

                    b.Navigation("CreatedCourses");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.BaseMaterialDBO", b =>
                {
                    b.Navigation("AccountMaterials");

                    b.Navigation("CourseMaterials");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.CourseDBO", b =>
                {
                    b.Navigation("AccountCourses");

                    b.Navigation("CourseMaterials");

                    b.Navigation("CourseSkills");
                });

            modelBuilder.Entity("EducationProject.Core.DAL.EF.SkillDBO", b =>
                {
                    b.Navigation("AccountSkills");

                    b.Navigation("CourseSkills");
                });
#pragma warning restore 612, 618
        }
    }
}
