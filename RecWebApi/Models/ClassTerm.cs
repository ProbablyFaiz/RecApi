using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class ClassTerm
    {
        public ClassTerm()
        {
            ClassTermStudent = new HashSet<ClassTermStudent>();
            ClassTermTeacher = new HashSet<ClassTermTeacher>();
            Session = new HashSet<Session>();
        }

        public int ClassTermId { get; set; }
        public int ClassId { get; set; }
        public int TermId { get; set; }

        public Class Class { get; set; }
        public ICollection<ClassTermStudent> ClassTermStudent { get; set; }
        public ICollection<ClassTermTeacher> ClassTermTeacher { get; set; }
        public ICollection<Session> Session { get; set; }
    }
}
