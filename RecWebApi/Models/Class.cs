using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class Class
    {
        public Class()
        {
            ClassTeacher = new HashSet<ClassTeacher>();
            ClassTerm = new HashSet<ClassTerm>();
            Enrollment = new HashSet<Enrollment>();
        }

        public int ClassId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RecId { get; set; }
        public int ClassTypeId { get; set; }

        public Rec Rec { get; set; }
        public ICollection<ClassTeacher> ClassTeacher { get; set; }
        public ICollection<ClassTerm> ClassTerm { get; set; }
        public ICollection<Enrollment> Enrollment { get; set; }
    }
}
