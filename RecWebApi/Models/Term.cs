using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class Term
    {
        public int TermId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
