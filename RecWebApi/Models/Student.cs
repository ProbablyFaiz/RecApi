using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class Student
    {
        public Student()
        {
            Attendance = new HashSet<Attendance>();
            ClassTermStudent = new HashSet<ClassTermStudent>();
            Enrollment = new HashSet<Enrollment>();
        }

        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public DateTime? DisableDate { get; set; }

        public ICollection<Attendance> Attendance { get; set; }
        public ICollection<ClassTermStudent> ClassTermStudent { get; set; }
        public ICollection<Enrollment> Enrollment { get; set; }
    }
}
