using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class Session
    {
        public Session()
        {
            Attendance = new HashSet<Attendance>();
            SessionTeacher = new HashSet<SessionTeacher>();
        }

        public int SessionId { get; set; }
        public int ClassTermId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DisableDate { get; set; }

        public ClassTerm ClassTerm { get; set; }
        public ICollection<Attendance> Attendance { get; set; }
        public ICollection<SessionTeacher> SessionTeacher { get; set; }
    }
}
