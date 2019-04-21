using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Http;

namespace RecWebApi
{
    public class DbActions
    {
        public static List<CSession> GetSessionList(int teacherId, DateTime date)
        {
            try
            {
                using (var db = new RecDbContext())
                {
                    var query = from session in db.Session.Where(item => item.Date.Month == date.Month && item.Date.Year == date.Year)
                                select new
                                {
                                    session.SessionId,
                                    session.Date,
                                    session.ClassTerm.ClassId,
                                    session.ClassTerm.Class.Description,
                                    session.ClassTerm.Class.Name
                                };
                    var restrictToTeachersClasses = db.ClassTeacher.Where(ct => ct.TeacherId == teacherId).Select(ct => ct.ClassId).ToArray(); //ClassId Array
                    var restrictToTeachersClassTerms = db.ClassTermTeacher.Where(ctt => ctt.TeacherId == teacherId).Select(ctt => ctt.ClassTermId).ToArray(); //ClassTermId Array
                    var restrictToTeachersSessions = db.SessionTeacher.Where(st => st.TeacherId == teacherId).Select(ctt => ctt.SessionId).ToArray(); //Session Array

                    var list = new List<CSession>();
                    foreach (var item in query)
                    {
                        var session = new CSession
                        {
                            SessionId = item.SessionId,
                            Date = item.Date
                        };
                        session.ClassInfo = new CClass
                        {
                            ClassId = item.ClassId,
                            ClassName = item.Name,
                            ClassDescription = item.Description
                        };
                        list.Add(session);
                    }

                    List<CSession> sessionsOfTeacher = new List<CSession>();
                    foreach (var session in list)
                    {
                        if (restrictToTeachersClasses.Contains(session.ClassInfo.ClassId) || restrictToTeachersClassTerms.Contains(session.ClassTermId) || restrictToTeachersSessions.Contains(session.SessionId))
                            sessionsOfTeacher.Add(session);
                    }
                    return list;
                }

            }
            catch (Exception e)
            {
                throw new ServerException(e.Message, e, true);
            }
        }

        public static List<CClass> GetClasses(int teacherId) //Update to use class teacher
        {
            try
            {
                using (var db = new RecDbContext())
                {
                    return (from ct in db.ClassTeacher.Where(item => item.TeacherId == teacherId)
                            join cl in db.Class on ct.ClassId equals cl.ClassId
                            join tc in db.Teacher on ct.TeacherId equals tc.TeacherId
                            select new CClass
                            {
                                ClassId = ct.ClassId,
                                PrimaryTeacherName = tc.FirstName + " " + tc.LastName,
                                ClassDescription = cl.Description,
                                ClassName = cl.Name,
                            }).ToList();
                }

            }
            catch (Exception e)
            {
                throw new ServerException(e.Message, e, true);
            }
        }

        public static CSession GetSession(int sessionId)
        {
            try
            {
                using (var db = new RecDbContext())
                {
                    CSession session = LoadSession(sessionId, db);
                    if (session != null)
                        LoadStudents(db, session);

                    return session;
                }
            }
            catch (Exception e)
            {
                throw new ServerException(e.Message, e, true);
            }
        }

        private static void LoadStudents(RecDbContext db, CSession session)
        {

            var students = from cts in db.ClassTermStudent
                           join stu in db.Student on cts.StudentId equals stu.StudentId
                           join ct in db.ClassTerm on cts.ClassTermId equals ct.ClassTermId
                           join s in db.Session.Where(item => item.SessionId == session.SessionId) on ct.ClassTermId equals s.ClassTermId
                           join att in db.Attendance
                           on new { K1 = cts.StudentId, K2 = s.SessionId } equals new { K1 = att.StudentId, K2 = att.SessionId } into att1
                           from temp in att1.DefaultIfEmpty()
                           select new CAttendance
                           {
                               StudentId = cts.StudentId,
                               StudentName = stu.FirstName + " " + stu.LastName,
                               AttendanceStatusId = temp == null ? 0 : temp.AttendanceStatusId,
                               ReasonId = temp == null ? 0 : temp.ReasonId.GetValueOrDefault()
                           };

            foreach (var student in students)
            {
                var ca = new CAttendance
                {
                    StudentId = student.StudentId,
                    StudentName = student.StudentName,
                    AttendanceStatusId = student.AttendanceStatusId == null ? 0 : student.AttendanceStatusId,
                    ReasonId = student.ReasonId == null ? 0 : student.ReasonId
                };
                session.Students.Add(ca);
            }
        }

        private static CSession LoadSession(int sessionId, RecDbContext db)
        {
            return (from qsession in db.Session
                    where qsession.SessionId == sessionId
                    select new CSession
                    {
                        Date = qsession.Date,
                        SessionId = qsession.SessionId,
                        ClassTermId = qsession.ClassTermId,
                        ClassInfo = new CClass
                        {
                            ClassId = qsession.ClassTerm.ClassId,
                            ClassName = qsession.ClassTerm.Class.Name,
                            PrimaryTeacherName = "Faiz Surani",
                            ClassDescription = qsession.ClassTerm.Class.Description,
                            RecName = "LAHQ"
                        }
                    }).FirstOrDefault();
        }

        public static void UpdateAttendance(CSession sessionAttendance)
        {
            List<Attendance> attendanceList = new List<Attendance>();
            try
            {
                using (var db = new RecDbContext())
                {
                    foreach (CAttendance student in sessionAttendance.Students)
                    {
                        var dbObject = db.Attendance.Where(a => a.SessionId == sessionAttendance.SessionId && a.StudentId == student.StudentId).FirstOrDefault();
                        bool doesNotExist = dbObject == null;
                        if (doesNotExist)
                        {
                            db.Add(new Attendance
                            {
                                AttendanceStatusId = (int)student.AttendanceStatusId,
                                ReasonId = student.ReasonId,
                                SessionId = sessionAttendance.SessionId,
                                StudentId = student.StudentId,
                                CreationDtTm = DateTime.Now
                            });
                        }
                        else
                        {
                            db.Update(dbObject);
                            dbObject.AttendanceStatusId = (int)student.AttendanceStatusId;
                            dbObject.ReasonId = student.ReasonId;
                            dbObject.CreationDtTm = DateTime.Now;
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new ServerException(e.Message, e, false);
            }
        }

        public static void ValidateUser(CUser user, string authToken)
        {
            using (var db = new RecDbContext())
            {
                var teacher = db.Teacher.Where(t => t.Email == user.EmailAddress).FirstOrDefault();
                if (teacher == null)
                {
                    user.ExistsOnServer = false;
                    return;
                }
                user.ExistsOnServer = true;
                user.TeacherId = teacher.TeacherId;
                db.Update(teacher);
                teacher.AuthorizationToken = authToken;
                db.SaveChanges();
            }
        }

        public static void SignOut(String emailAddress)
        {
            try
            {
                using (var db = new RecDbContext())
                {
                    var teacher = db.Teacher.Where(t => t.Email == emailAddress).FirstOrDefault();
                    if (teacher == null)
                        return;
                    db.Update(teacher);
                    teacher.AuthorizationToken = null;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new ServerException(e.Message, e, true);
            }
        }

        public static bool ValidateRequest(HttpRequest request)
        {
            var authToken = request.Headers["Authorization"].ToString();
            try
            {
                using (var db = new RecDbContext())
                {
                    var myToken = db.Teacher.Where(t => t.TeacherId == 2).Select(t => t.AuthorizationToken).FirstOrDefault();
                    if (db.Teacher.Any(t => t.AuthorizationToken == authToken))
                        return true;
                }
            }
            catch (Exception e)
            {
                throw new ServerException(e.Message, e, false);
            }
            return false;
        }

    }

    [TestClass]
    public class DbTests
    {
        [TestMethod]
        public void SessionTest()
        {
            var session = DbActions.GetSession(9);
            DbActions.UpdateAttendance(session);
        }
    }

    public class ServerException : Exception
    {
        public ServerException(string message, Exception innerException = null, bool isDbRead = false) : base(message, innerException)
        {
            if (isDbRead)
            {
                _Message = $"Error reading from database. {message}";
            }
            else
            {
                _Message = $"Error writing to database. {message}";
            }
        }
        private string _Message;
        public override string Message => _Message ?? base.Message;
    }
}
