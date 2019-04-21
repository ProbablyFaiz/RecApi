using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class Reason
    {
        public Reason()
        {
            Attendance = new HashSet<Attendance>();
        }

        public int ReasonId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Attendance> Attendance { get; set; }
    }
}
