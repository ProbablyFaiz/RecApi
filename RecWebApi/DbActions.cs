using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RecWebApi
{
    public class DbActions
    {
        static ILogger Logger { get; } =
            ApplicationLogging.CreateLogger<Program>();

        public static List<CSession> GetSessionList(int teacherId, DateTime? date)
        {
            try
            {
                using (var db = new RecContext())
                {
                    var query = from session in db.Session
                                select new
                                {
                                    session.SessionId,
                                    session.Date,
                                    session.ClassTerm.ClassId,
                                    session.ClassTerm.Class.Description,
                                    session.ClassTerm.Class.Name
                                };
                    if (date != null)
                    {
                        var definiteDate = (DateTime)date;
                        query = query.Where(item => item.Date.Month == definiteDate.Month && item.Date.Year == definiteDate.Year);
                    }
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
                var sException = new ServerException(e.Message, e, true);
                LogServerException(sException);
            }
            return new List<CSession>();
        }

        public static int GetClosestSessionId(int teacherId)
        {
            try
            {
                var listOfSessions = GetSessionList(teacherId, null);
                TimeSpan minTimeSpan = TimeSpan.MaxValue;
                CSession closestSession = new CSession();
                foreach (CSession session in listOfSessions)
                {
                    var timeUntilOrAfterPresent = session.Date.Subtract(DateTime.Now).Duration();
                    if (timeUntilOrAfterPresent < minTimeSpan)
                    {
                        minTimeSpan = timeUntilOrAfterPresent;
                        closestSession = session;
                    }
                }
                return closestSession.SessionId;
            }
            catch (Exception e)
            {
                var sException = new ServerException(e.Message, e, true);
                LogServerException(sException);
            }
            return 0;
        }

        public static List<CClass> GetClasses(int teacherId)
        {
            try
            {
                using (var db = new RecContext())
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
                var sException = new ServerException(e.Message, e, true);
                LogServerException(sException);
            }
            return new List<CClass>();
        }

        public static CSession GetSession(int sessionId)
        {
            try
            {
                using (var db = new RecContext())
                {
                    CSession session = LoadSession(sessionId, db);
                    if (session != null)
                        LoadStudents(db, session);

                    return session;
                }
            }
            catch (Exception e)
            {
                var sException = new ServerException(e.Message, e, true);
                LogServerException(sException);
            }
            return new CSession();
        }

        private static void LoadStudents(RecContext db, CSession session)
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

        private static CSession LoadSession(int sessionId, RecContext db)
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
                using (var db = new RecContext())
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
                    db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                var sException = new ServerException(e.Message, e, false);
                LogServerException(sException);
            }
        }

        public static void ValidateUser(CUser user, string authToken)
        {
            try
            {
                using (var db = new RecContext())
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
            catch (Exception e)
            {
                var sException = new ServerException(e.Message, e, true);
                LogServerException(sException);
            }
        }

        public static void SignOut(string authToken)
        {
            try
            {
                using (var db = new RecContext())
                {
                    var teacher = db.Teacher.Where(t => t.AuthorizationToken == authToken).FirstOrDefault();
                    if (teacher == null)
                        return;
                    db.Update(teacher);
                    teacher.AuthorizationToken = null;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                var sException = new ServerException(e.Message, e, true);
                LogServerException(sException);
            }
        }

        public static bool ValidateRequest(HttpRequest request)
        {
            string authToken = request.Headers["Authorization"].ToString();
            try
            {
                using (var db = new RecContext())
                {
                    var myToken = db.Teacher.Where(t => t.TeacherId == 2).Select(t => t.AuthorizationToken).FirstOrDefault();
                    if (db.Teacher.Any(t => t.AuthorizationToken == authToken))
                        return true;
                }
            }
            catch (Exception e)
            {
                var sException = new ServerException(e.Message, e, false);
                LogServerException(sException);
            }
            return false;
        }

        public static List<CTeacher> GetTeacherList(int teacherId)
        {
            var teacherList = new List<CTeacher>();
            try
            {
                using (var db = new RecContext())
                {
                    var userTeacher = db.Teacher.Where(t => t.TeacherId == teacherId);
                    var recIdForAdmin = db.RecTeacher.Where(rt => rt.TeacherId == teacherId && rt.IsAdministrator == true).Select(rt => rt.RecId).FirstOrDefault();
                    var teachersForRec = db.RecTeacher.Where(rt => rt.RecId == recIdForAdmin).Select(rt => rt.Teacher).ToArray();
                    foreach (Teacher teacher in teachersForRec)
                    {
                        teacherList.Add(new CTeacher
                        {
                            TeacherId = teacher.TeacherId,
                            FirstName = teacher.FirstName,
                            LastName = teacher.LastName,
                            EmailAddress = teacher.Email,
                        });

                    }
                }
            }
            catch (Exception e)
            {
                var sException = new ServerException(e.Message, e, true);
                LogServerException(sException);
            }
            return teacherList;
        }

        public static CTeacher GetTeacher(int teacherId, int userId)
        {
            var teacher = new CTeacher();
            teacher.ClassesTaught = new List<CClassTerm>();
            try
            {
                using (var db = new RecContext())
                {
                    int recIdForAdmin = GetRecIdForUser(userId, db);
                    var dbTeacher = db.Teacher.Where(t => t.TeacherId == teacherId).FirstOrDefault();
                    teacher.TeacherId = dbTeacher.TeacherId;
                    teacher.FirstName = dbTeacher.FirstName;
                    teacher.LastName = dbTeacher.LastName;
                    teacher.EmailAddress = dbTeacher.Email;
                    var disableDate = db.RecTeacher.Where(rt => rt.TeacherId == teacher.TeacherId).Select(rt => rt.DisableDate).FirstOrDefault();
                    if (disableDate == null)
                        teacher.IsDisabled = false;
                    else
                        teacher.IsDisabled = true;
                    teacher.IsAdministrator = db.RecTeacher.Where(rt => rt.TeacherId == teacher.TeacherId && rt.RecId == recIdForAdmin).Select(rt => rt.IsAdministrator).FirstOrDefault();

                    var classTermsForTeacher = db.ClassTermTeacher.Where(ctt => ctt.TeacherId == teacher.TeacherId && ctt.ClassTerm.Class.RecId == recIdForAdmin).Select(ctt => ctt.ClassTerm).ToArray();
                    foreach (ClassTerm classTerm in classTermsForTeacher)
                    {
                        var classInfo = db.Class.Where(c => c.ClassId == classTerm.ClassId).FirstOrDefault();
                        var termInfo = db.Term.Where(t => t.TermId == classTerm.TermId).FirstOrDefault();
                        teacher.ClassesTaught.Add(new CClassTerm
                        {
                            ClassTermId = classTerm.ClassTermId,
                            Name = classInfo.Name + " (" + termInfo.Name + ")",
                            Description = classInfo.Description
                        });
                    }
                }
            }

            catch (Exception e)
            {
                var sException = new ServerException(e.Message, e, true);
                LogServerException(sException);
            }
            return teacher;
        }

        private static int GetRecIdForUser(int userId, RecContext db)
        {
            return db.RecTeacher.Where(rt => rt.TeacherId == userId && rt.IsAdministrator == true).Select(rt => rt.RecId).FirstOrDefault();
        }

        public static void AddNewTeacher(CTeacher teacher, int userId)
        {
            try
            {
                using (var db = new RecContext())
                {
                    var dbTeacher = new Teacher
                    {
                        FirstName = teacher.FirstName,
                        LastName = teacher.LastName,
                        Email = teacher.EmailAddress
                    };
                    db.Add(dbTeacher);
                    db.SaveChanges();
                    var teacherId = db.Teacher.Where(t => t.Email == teacher.EmailAddress).Select(t => t.TeacherId).First();
                    var recTeacher = new RecTeacher
                    {
                        TeacherId = teacherId,
                        RecId = GetRecIdForUser(userId, db),
                        IsAdministrator = teacher.IsAdministrator,
                    };
                    if (teacher.IsDisabled)
                        recTeacher.DisableDate = DateTime.Now;
                    db.Add(recTeacher);                                  
                    foreach (CClassTerm classTerm in teacher.ClassesTaught)
                    {
                        db.Add(new ClassTermTeacher
                        {
                            ClassTermId = classTerm.ClassTermId,
                            TeacherId = teacherId
                        });
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                var sException = new ServerException(e.Message, e, false);
                LogServerException(sException);
            }
        }

        public static void UpdateExistingTeacher(CTeacher teacher, int userId)
        {
            try
            {
                using (var db = new RecContext())
                {
                    var dbTeacher = db.Teacher.Where(t => t.TeacherId == teacher.TeacherId).First();
                    db.Update(dbTeacher);
                    dbTeacher.FirstName = teacher.FirstName;
                    dbTeacher.LastName = teacher.LastName;
                    dbTeacher.Email = teacher.EmailAddress;
                    var recId = GetRecIdForUser(userId, db);
                    var recTeacher = db.RecTeacher.Where(rt => rt.TeacherId == teacher.TeacherId && rt.RecId == recId).FirstOrDefault();
                    db.Update(recTeacher);
                    recTeacher.IsAdministrator = teacher.IsAdministrator;
                    if (teacher.IsDisabled)
                        recTeacher.DisableDate = DateTime.Now;
                    
                    foreach (CClassTerm classTerm in teacher.ClassesTaught)
                    {
                        var dbClassTerm = db.ClassTermTeacher.Where(ctt => ctt.TeacherId == teacher.TeacherId && ctt.ClassTermId == classTerm.ClassTermId).FirstOrDefault();
                        if (dbClassTerm == null)
                        {
                            db.Add(new ClassTermTeacher
                            {
                                ClassTermId = classTerm.ClassTermId,
                                TeacherId = teacher.TeacherId
                            });
                        }
                        else if (classTerm.Operation == Operation.Deleted)
                        {
                            db.Remove(dbClassTerm);
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                var sException = new ServerException(e.Message, e, false);
                LogServerException(sException);
            }
        }

        public static List<CClassTerm> GetClassTermsForRec(int teacherId)
        {
            var classTermList = new List<CClassTerm>();
            try
            {
                using (var db = new RecContext())
                {
                    var recIdForAdmin = db.RecTeacher.Where(rt => rt.TeacherId == teacherId && rt.IsAdministrator == true).Select(rt => rt.RecId).FirstOrDefault();
                    var classTermsForRecId = db.ClassTerm.Where(ct => ct.Class.RecId == recIdForAdmin).ToList();
                    foreach (ClassTerm dbClassTerm in classTermsForRecId)
                    {
                        var className = db.Class.Where(c => c.ClassId == dbClassTerm.ClassId).Select(c => c.Name).FirstOrDefault();
                        var term = db.Term.Where(t => t.TermId == dbClassTerm.TermId).FirstOrDefault();
                        if (term.EndDate >= DateTime.Now)
                        {
                            classTermList.Add(new CClassTerm
                            {
                                ClassTermId = dbClassTerm.ClassTermId,
                                Name = className + " (" + term.Name + ")"
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var sException = new ServerException(e.Message, e, true);
                LogServerException(sException);
            }
            return classTermList;
        }

        public static bool CheckIfEmailIsDuplicate(string emailAddress)
        {
            try
            {
                using (var db = new RecContext())
                {
                    return db.Teacher.Where(t => t.Email == emailAddress).FirstOrDefault() != null;
                }
            }
            catch (Exception e)
            {
                var sException = new ServerException(e.Message, e, true);
                LogServerException(sException);
            }
            return false; //This is bad, fix to return error instead
        }

        private static void LogServerException(ServerException ex)
        {
            ApplicationLogging.LoggerFactory.AddConsole(true);
            Logger.LogError(ex.Message);
            Logger.LogInformation(ex.StackTrace);
        }
    }

    [TestClass]
    public class DbTests
    {
        [TestMethod]
        public void UpdateTeacherTest()
        {
            var classTerms = DbActions.GetClassTermsForRec(2);
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
