using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RecWebApi.Models
{
    public class CStudent
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
    }

    public class CAttendance
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int? AttendanceStatusId { get; set; }
        public int? ReasonId { get; set; }
    }

    public class CSession
    {
        public int SessionId { get; set; }
        public CClass ClassInfo { get; set; }
        public DateTime Date { get; set; }
        public List<CAttendance> Students = new List<CAttendance>();
        public int ClassTermId { get; set; }
    }

    public class CClass
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public string RecName { get; set; }
        public string PrimaryTeacherName { get; set; }
    }

    public class CTeacher
    {
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public List<CClassTerm> ClassesTaught { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsAdministrator { get; set; }
        public string Operation { get; set; }
    }

    public class CClassTerm
    {
        public int ClassTermId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Operation { get; set; }
    }

    public class CUser
    {
        public string EmailAddress { get; set; }
        public Boolean ExistsOnServer { get; set; }

        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Operation
    {
        public const string New = nameof(New);
        public const string Updated = nameof(Updated);
        public const string Deleted = nameof(Deleted);
    }

    public class CGoogleInfo
    {
        public List<CGoogleInfoEmail> Emails { get; set; }
        public CGoogleInfoName Name { get; set; }
    }

    public class CGoogleInfoEmail
    {
        public String Value { get; set; }
    }

    public class CGoogleInfoName
    {
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        public string Formatted { get; set; }
    }
}