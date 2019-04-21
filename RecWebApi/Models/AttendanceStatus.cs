using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class AttendanceStatus
    {
        public AttendanceStatus()
        {
            Attendance = new HashSet<Attendance>();
        }

        public int AttendanceStatusId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Attendance> Attendance { get; set; }
    }
}
