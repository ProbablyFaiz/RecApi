using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RecWebApi.Models
{
    public partial class RecContext : DbContext
    {
        public RecContext()
        {
        }

        public RecContext(DbContextOptions<RecContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attendance> Attendance { get; set; }
        public virtual DbSet<AttendanceStatus> AttendanceStatus { get; set; }
        public virtual DbSet<Class> Class { get; set; }
        public virtual DbSet<ClassTeacher> ClassTeacher { get; set; }
        public virtual DbSet<ClassTerm> ClassTerm { get; set; }
        public virtual DbSet<ClassTermStudent> ClassTermStudent { get; set; }
        public virtual DbSet<ClassTermTeacher> ClassTermTeacher { get; set; }
        public virtual DbSet<ClassType> ClassType { get; set; }
        public virtual DbSet<Enrollment> Enrollment { get; set; }
        public virtual DbSet<Reason> Reason { get; set; }
        public virtual DbSet<Rec> Rec { get; set; }
        public virtual DbSet<RecTeacher> RecTeacher { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<SessionTeacher> SessionTeacher { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Teacher> Teacher { get; set; }
        public virtual DbSet<Term> Term { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=xxx,1433;Initial Catalog=Rec;Persist Security Info=False;User ID=xxx;Password=xxx;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasKey(e => new { e.SessionId, e.StudentId });

                entity.Property(e => e.CreationDtTm).HasColumnType("datetime");

                entity.HasOne(d => d.AttendanceStatus)
                    .WithMany(p => p.Attendance)
                    .HasForeignKey(d => d.AttendanceStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Attendance_AttendanceStatus");

                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.Attendance)
                    .HasForeignKey(d => d.ReasonId)
                    .HasConstraintName("FK_Attendance_Reason");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Attendance)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Attendance_Session");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Attendance)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Attendance_Student");
            });

            modelBuilder.Entity<AttendanceStatus>(entity =>
            {
                entity.Property(e => e.AttendanceStatusId).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Rec)
                    .WithMany(p => p.Class)
                    .HasForeignKey(d => d.RecId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Class_Rec");
            });

            modelBuilder.Entity<ClassTeacher>(entity =>
            {
                entity.Property(e => e.DisableDate).HasColumnType("date");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassTeacher)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClassTeacher_Class");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.ClassTeacher)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClassTeacher_Teacher");
            });

            modelBuilder.Entity<ClassTerm>(entity =>
            {
                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassTerm)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClassTerm_Class");
            });

            modelBuilder.Entity<ClassTermStudent>(entity =>
            {
                entity.HasKey(e => new { e.ClassTermId, e.StudentId });

                entity.HasOne(d => d.ClassTerm)
                    .WithMany(p => p.ClassTermStudent)
                    .HasForeignKey(d => d.ClassTermId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClasstermStudent_ClassTerm");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.ClassTermStudent)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClasstermStudent_Student");
            });

            modelBuilder.Entity<ClassTermTeacher>(entity =>
            {
                entity.HasKey(e => new { e.ClassTermId, e.TeacherId });

                entity.HasOne(d => d.ClassTerm)
                    .WithMany(p => p.ClassTermTeacher)
                    .HasForeignKey(d => d.ClassTermId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClasstermTeacher_ClassTerm");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.ClassTermTeacher)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClasstermTeacher_Teacher");
            });

            modelBuilder.Entity<ClassType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.ClassId });

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Enrollment)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollment_Class");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Enrollment)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollment_Student");
            });

            modelBuilder.Entity<Reason>(entity =>
            {
                entity.Property(e => e.ReasonId).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Rec>(entity =>
            {
                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PostalCode).HasMaxLength(20);

                entity.Property(e => e.Street).HasMaxLength(50);
            });

            modelBuilder.Entity<RecTeacher>(entity =>
            {
                entity.HasKey(e => new { e.RecId, e.TeacherId });

                entity.Property(e => e.DisableDate).HasColumnType("date");

                entity.HasOne(d => d.Rec)
                    .WithMany(p => p.RecTeacher)
                    .HasForeignKey(d => d.RecId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecTeacher_Rec");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.RecTeacher)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecTeacher_Teacher");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DisableDate).HasColumnType("date");

                entity.HasOne(d => d.ClassTerm)
                    .WithMany(p => p.Session)
                    .HasForeignKey(d => d.ClassTermId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Session_ClassTerm");
            });

            modelBuilder.Entity<SessionTeacher>(entity =>
            {
                entity.HasOne(d => d.Session)
                    .WithMany(p => p.SessionTeacher)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SessionTeacher_Session");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.SessionTeacher)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SessionTeacher_Teacher");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.DisableDate).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.Property(e => e.TeacherId).HasColumnName("TeacherID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Term>(entity =>
            {
                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("date");
            });
        }
    }
}
