using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class AttendanceType
    {
        public AttendanceType()
        {
            Attendance = new HashSet<Attendance>();
        }

        public int AttendanceTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Attendance> Attendance { get; set; }
    }
}
