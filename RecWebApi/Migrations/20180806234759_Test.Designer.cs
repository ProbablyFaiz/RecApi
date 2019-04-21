﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RecWebApi.Models;

namespace RecWebApi.Migrations
{
    [DbContext(typeof(RecContext))]
    [Migration("20180806234759_Test")]
    partial class Test
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RecWebApi.Models.Attendance", b =>
                {
                    b.Property<int>("AttendanceId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AttendanceTypeId");

                    b.Property<DateTime?>("CreationDtTm")
                        .HasColumnType("datetime");

                    b.Property<int?>("ReasonId");

                    b.Property<int>("SessionId");

                    b.Property<int>("StudentId");

                    b.HasKey("AttendanceId");

                    b.HasIndex("AttendanceTypeId");

                    b.HasIndex("ReasonId");

                    b.HasIndex("SessionId");

                    b.HasIndex("StudentId");

                    b.ToTable("Attendance");
                });

            modelBuilder.Entity("RecWebApi.Models.AttendanceType", b =>
                {
                    b.Property<int>("AttendanceTypeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("AttendanceTypeId");

                    b.ToTable("AttendanceType");
                });

            modelBuilder.Entity("RecWebApi.Models.Class", b =>
                {
                    b.Property<int>("ClassId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassTypeId");

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<int?>("PrimaryTeacher2Id");

                    b.Property<int>("PrimaryTeacherId");

                    b.Property<int>("RecId");

                    b.Property<int>("TermId");

                    b.HasKey("ClassId");

                    b.HasIndex("TermId");

                    b.ToTable("Class");
                });

            modelBuilder.Entity("RecWebApi.Models.ClassType", b =>
                {
                    b.Property<int>("ClassTypeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ClassTypeId");

                    b.ToTable("ClassType");
                });

            modelBuilder.Entity("RecWebApi.Models.Enrollment", b =>
                {
                    b.Property<int>("EnrollmentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassId");

                    b.Property<int>("StudentId");

                    b.HasKey("EnrollmentId");

                    b.HasIndex("ClassId");

                    b.HasIndex("StudentId");

                    b.ToTable("Enrollment");
                });

            modelBuilder.Entity("RecWebApi.Models.Reason", b =>
                {
                    b.Property<int>("ReasonId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ReasonId");

                    b.ToTable("Reason");
                });

            modelBuilder.Entity("RecWebApi.Models.Rec", b =>
                {
                    b.Property<int>("RecId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasMaxLength(50);

                    b.Property<string>("Country")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("PostalCode")
                        .HasMaxLength(20);

                    b.Property<string>("Street")
                        .HasMaxLength(50);

                    b.HasKey("RecId");

                    b.ToTable("Rec");
                });

            modelBuilder.Entity("RecWebApi.Models.Session", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassId");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<TimeSpan?>("EndTime");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time(0)");

                    b.Property<int?>("Teacher1Id");

                    b.Property<int>("Teacher2Id");

                    b.HasKey("SessionId");

                    b.HasIndex("ClassId");

                    b.HasIndex("Teacher1Id");

                    b.HasIndex("Teacher2Id");

                    b.ToTable("Session");
                });

            modelBuilder.Entity("RecWebApi.Models.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasMaxLength(50);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50);

                    b.HasKey("StudentId");

                    b.ToTable("Student");
                });

            modelBuilder.Entity("RecWebApi.Models.Teacher", b =>
                {
                    b.Property<int>("TeacherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("TeacherID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50);

                    b.Property<int?>("PrimaryRecId")
                        .HasColumnName("PrimaryRecID");

                    b.HasKey("TeacherId");

                    b.HasIndex("PrimaryRecId");

                    b.ToTable("Teacher");
                });

            modelBuilder.Entity("RecWebApi.Models.Term", b =>
                {
                    b.Property<int>("TermId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("TermId");

                    b.ToTable("Term");
                });

            modelBuilder.Entity("RecWebApi.Models.Attendance", b =>
                {
                    b.HasOne("RecWebApi.Models.AttendanceType", "AttendanceType")
                        .WithMany("Attendance")
                        .HasForeignKey("AttendanceTypeId")
                        .HasConstraintName("FK_Attendance_AttendanceType");

                    b.HasOne("RecWebApi.Models.Reason", "Reason")
                        .WithMany("Attendance")
                        .HasForeignKey("ReasonId")
                        .HasConstraintName("FK_Attendance_Reason");

                    b.HasOne("RecWebApi.Models.Session", "Session")
                        .WithMany("Attendance")
                        .HasForeignKey("SessionId")
                        .HasConstraintName("FK_Attendance_Session");

                    b.HasOne("RecWebApi.Models.Student", "Student")
                        .WithMany("Attendance")
                        .HasForeignKey("StudentId")
                        .HasConstraintName("FK_Attendance_Student");
                });

            modelBuilder.Entity("RecWebApi.Models.Class", b =>
                {
                    b.HasOne("RecWebApi.Models.Term", "Term")
                        .WithMany("Class")
                        .HasForeignKey("TermId")
                        .HasConstraintName("FK_Class_Term");
                });

            modelBuilder.Entity("RecWebApi.Models.Enrollment", b =>
                {
                    b.HasOne("RecWebApi.Models.Class", "Class")
                        .WithMany("Enrollment")
                        .HasForeignKey("ClassId")
                        .HasConstraintName("FK_Enrollment_Class");

                    b.HasOne("RecWebApi.Models.Student", "Student")
                        .WithMany("Enrollment")
                        .HasForeignKey("StudentId")
                        .HasConstraintName("FK_Enrollment_Student");
                });

            modelBuilder.Entity("RecWebApi.Models.Session", b =>
                {
                    b.HasOne("RecWebApi.Models.Class", "Class")
                        .WithMany("Session")
                        .HasForeignKey("ClassId")
                        .HasConstraintName("FK_Session_Class");

                    b.HasOne("RecWebApi.Models.Teacher", "Teacher1")
                        .WithMany("SessionTeacher1")
                        .HasForeignKey("Teacher1Id")
                        .HasConstraintName("FK_Session_Teacher1");

                    b.HasOne("RecWebApi.Models.Teacher", "Teacher2")
                        .WithMany("SessionTeacher2")
                        .HasForeignKey("Teacher2Id")
                        .HasConstraintName("FK_Session_Teacher2");
                });

            modelBuilder.Entity("RecWebApi.Models.Teacher", b =>
                {
                    b.HasOne("RecWebApi.Models.Rec", "PrimaryRec")
                        .WithMany("Teacher")
                        .HasForeignKey("PrimaryRecId")
                        .HasConstraintName("FK_Teacher_Rec");
                });
#pragma warning restore 612, 618
        }
    }
}
