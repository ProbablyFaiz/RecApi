using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class Attendance
    {
        public int SessionId { get; set; }
        public int StudentId { get; set; }
        public int AttendanceStatusId { get; set; }
        public int? ReasonId { get; set; }
        public DateTime? CreationDtTm { get; set; }

        public AttendanceStatus AttendanceStatus { get; set; }
        public Reason Reason { get; set; }
        public Session Session { get; set; }
        public Student Student { get; set; }
    }
}
