using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class Enrollment
    {
        public int StudentId { get; set; }
        public int ClassId { get; set; }

        public Class Class { get; set; }
        public Student Student { get; set; }
    }
}
