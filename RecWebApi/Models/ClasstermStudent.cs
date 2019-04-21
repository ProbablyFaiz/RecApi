using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class ClassTermStudent
    {
        public int ClassTermStudentId { get; set; }
        public int ClassTermId { get; set; }
        public int StudentId { get; set; }

        public ClassTerm ClassTerm { get; set; }
        public Student Student { get; set; }
    }
}
