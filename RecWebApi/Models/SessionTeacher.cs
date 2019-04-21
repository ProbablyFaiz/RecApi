using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class SessionTeacher
    {
        public int SessionTeacherId { get; set; }
        public int SessionId { get; set; }
        public int TeacherId { get; set; }
        public bool? IsPrimaryTeacher { get; set; }

        public Session Session { get; set; }
        public Teacher Teacher { get; set; }
    }
}
