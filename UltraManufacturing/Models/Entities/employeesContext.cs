using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace UltraManufacturing.Models.Entities
{
    public partial class employeesContext : DbContext
    {
        public virtual DbSet<Cmpg323Project2Dataset> Cmpg323Project2Dataset { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserCredential> UserCredential { get; set; }
        public virtual DbSet<UserPermission> UserPermission { get; set; }

        
        public employeesContext(DbContextOptions<employeesContext> options)
            : base(options)
        {
        }

        public employeesContext()
        {
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cmpg323Project2Dataset>(entity =>
            {
                entity.HasKey(e => e.EmployeeNumber);

                entity.ToTable("CMPG323 project 2 dataset");

                entity.Property(e => e.EmployeeNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Age)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Attrition)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessTravel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DailyRate)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Department)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DistanceFromHome)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Education)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EducationField)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeCount)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EnvironmentSatisfaction)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HourlyRate)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.JobInvolvement)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.JobLevel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.JobRole)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.JobSatisfaction)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MaritalStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MonthlyIncome)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MonthlyRate)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumCompaniesWorked)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Over18)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OverTime)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PercentSalaryHike)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PerformanceRating)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RelationshipSatisfaction)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StandardHours)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StockOptionLevel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TotalWorkingYears)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TrainingTimesLastYear)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WorkLifeBalance)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.YearsAtCompany)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.YearsInCurrentRole)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.YearsSinceLastPromotion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.YearsWithCurrManager)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserCredential>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.HashedPassword)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.UserCredential)
                    .HasForeignKey<UserCredential>(d => d.Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_UserCredential_User");
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.PermissionId })
                    .HasName("PK_UserPermissions");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.UserPermission)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_UserPermissions_Permissions");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPermission)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_UserPermissions_Users");
            });
        }
    }
}
