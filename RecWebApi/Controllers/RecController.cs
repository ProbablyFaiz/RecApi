using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecWebApi.Models;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.Logging;

namespace RecWebApi.Controllers
{

    [Route("api/rec")]
    [ApiController]
    public class RecController : ControllerBase
    {
        public readonly ILogger _logger;

        public RecController(ILogger<RecController> logger)
        {
            _logger = logger;
        }

        [HttpGet("session/{sessionId}", Name = "GetSession")]
        public ActionResult<CSession> GetAttendance(int sessionId)
        {
            if (DbActions.ValidateRequest(Request))
                return DbActions.GetSession(sessionId);
            return StatusCode(401);
        }

        [HttpGet("session/nearestId", Name = "GetNearestToDateSession")]
        public ActionResult<int> GetIdOfNearestSession(int teacherId)
        {
            if (DbActions.ValidateRequest(Request))
                return DbActions.GetClosestSessionId(teacherId);
            return StatusCode(401);
        }

        [HttpPut("session/{sessionId}")]
        public IActionResult Put(CSession attendance)
        {
            if (DbActions.ValidateRequest(Request))
                DbActions.UpdateAttendance(attendance);
            else
                return StatusCode(401);
            return StatusCode(200);
        }

        [HttpPost("session/{sessionId}")]
        public IActionResult Post(CSession attendance)
        {
            if (DbActions.ValidateRequest(Request))
                DbActions.UpdateAttendance(attendance);
            else
                return StatusCode(401);
            return StatusCode(200);
        }

        [HttpGet("sessionList/{teacherId}&month={monthYear}", Name = "GetSessionList")]
        public ActionResult<List<CSession>> GetSessionList(int teacherId, String monthYear)
        {
            DateTime parsedMonth = DateTime.ParseExact(monthYear, "MM-yyyy", null);
            if (DbActions.ValidateRequest(Request))
                return DbActions.GetSessionList(teacherId, parsedMonth);
            return StatusCode(401);
        }

        [HttpGet("teacherList/{teacherId}", Name = "GetTeacherList")]
        public ActionResult<List<CTeacher>> GetTeacherList(int teacherId)
        {
            if (DbActions.ValidateRequest(Request))
                return DbActions.GetTeacherList(teacherId);
            return StatusCode(401);
        }

        [HttpGet("teacher/{teacherId}&user={userId}", Name = "GetTeacher")]
        public ActionResult<CTeacher> GetTeacher(int teacherId, int userId)
        {
            if (DbActions.ValidateRequest(Request))
                return DbActions.GetTeacher(teacherId, userId);
            return StatusCode(401);
        }
        
        [HttpPut("teacher/{userId}/new", Name = "AddNewTeacher")]
        public IActionResult AddNewTeacher(CTeacher newTeacher, int userId)
        {
            if (DbActions.ValidateRequest(Request))
            {
                DbActions.AddNewTeacher(newTeacher, userId);
                return StatusCode(200);
            }
            return StatusCode(401);
        }

        [HttpPost("teacher/{userId}/update", Name = "UpdateExistingTeacher")]
        public IActionResult UpdateTeacher(CTeacher teacherToUpdate, int userId)
        {
            if (DbActions.ValidateRequest(Request))
            {
                DbActions.UpdateExistingTeacher(teacherToUpdate, userId);
                return StatusCode(200);
            }
            return StatusCode(401);
        }

        [HttpGet("classTermList/{teacherId}", Name = "GetClassTermListForAdmin")]
        public ActionResult<List<CClassTerm>> GetClassTermList(int teacherId)
        {
            if (DbActions.ValidateRequest(Request))
            {
                return DbActions.GetClassTermsForRec(teacherId);
            }
            return StatusCode(401);
        }

        [HttpGet("emailIsDuplicate/{emailAddress}", Name = "CheckIfEmailIsDuplicate")]
        public ActionResult<bool> CheckDuplicateEmail(String emailAddress)
        {
            if (DbActions.ValidateRequest(Request))
            {
                return DbActions.CheckIfEmailIsDuplicate(emailAddress);
            }
            return StatusCode(401);
        }

        [HttpGet("auth", Name = "authenticateUser")]
        public ActionResult<CUser> AuthenticateUser()
        {
            var user = new CUser();
            var authToken = Request.Headers["Authorization"].ToString();
            GetGoogleInfo(user, authToken);
            DbActions.ValidateUser(user, authToken);
            Request.HttpContext.Response.Headers.Add("Bearer-Token", authToken);
            return user;
        }

        //For future reference: NEVER, EVER try to implement the .NET Google API, it will only bring you great sorrow. Stick to the REST service call.
        public void GetGoogleInfo(CUser user, string authToken)
        {
            var url = "https://www.googleapis.com/plus/v1/people/me?fields=emails%2Fvalue%2Cname(familyName%2Cformatted%2CgivenName%2CmiddleName)";
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + authToken);

            try
            {
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string json = reader.ReadToEnd();

                var googleInfo = JsonConvert.DeserializeObject<CGoogleInfo>(json);
                user.EmailAddress = googleInfo.Emails[0].Value;
                user.FirstName = googleInfo.Name.GivenName ?? "";
                user.LastName = googleInfo.Name.FamilyName ?? "";
            }
            catch (Exception e)
            {
                throw new ServerException(e.Message, e, true);
            }
        }

    }

}
