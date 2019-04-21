using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class RecTeacher
    {
        public int RecTeacherId { get; set; }
        public int RecId { get; set; }
        public int TeacherId { get; set; }
        public bool IsPrimaryRec { get; set; }

        public Rec Rec { get; set; }
        public Teacher Teacher { get; set; }
    }
}
