using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class Rec
    {
        public Rec()
        {
            Class = new HashSet<Class>();
            RecTeacher = new HashSet<RecTeacher>();
            Teacher = new HashSet<Teacher>();
        }

        public int RecId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public ICollection<Class> Class { get; set; }
        public ICollection<RecTeacher> RecTeacher { get; set; }
        public ICollection<Teacher> Teacher { get; set; }
    }
}
