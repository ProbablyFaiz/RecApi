using System;
using System.Collections.Generic;

namespace RecWebApi.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            ClassTeacher = new HashSet<ClassTeacher>();
            ClassTermTeacher = new HashSet<ClassTermTeacher>();
            RecTeacher = new HashSet<RecTeacher>();
            SessionTeacher = new HashSet<SessionTeacher>();
        }

        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AuthorizationToken { get; set; }

        public ICollection<ClassTeacher> ClassTeacher { get; set; }
        public ICollection<ClassTermTeacher> ClassTermTeacher { get; set; }
        public ICollection<RecTeacher> RecTeacher { get; set; }
        public ICollection<SessionTeacher> SessionTeacher { get; set; }
    }
}
