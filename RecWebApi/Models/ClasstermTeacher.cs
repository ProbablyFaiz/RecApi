using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class ClassTermTeacher
    {
        public int ClassTermId { get; set; }
        public int TeacherId { get; set; }

        public ClassTerm ClassTerm { get; set; }
        public Teacher Teacher { get; set; }
    }
}
