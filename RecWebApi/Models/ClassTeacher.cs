using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class ClassTeacher
    {
        public int ClassTeacherId { get; set; }
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public DateTime? DisableDate { get; set; }

        public Class Class { get; set; }
        public Teacher Teacher { get; set; }
    }
}
