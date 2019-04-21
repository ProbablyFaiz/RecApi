using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecWebApi.Models;
using Newtonsoft.Json;
using System.IO;

namespace RecWebApi.Controllers
{

    [Route("api/rec")]
    [ApiController]
    public class RecController : ControllerBase
    {
        [HttpGet("session/{sessionId}", Name = "GetSession")]
        public ActionResult<CSession> GetAttendance(int sessionId)
        {
            if (DbActions.ValidateRequest(Request))
                return DbActions.GetSession(sessionId);
            return StatusCode(401);
        }

        [HttpPut("session/{sessionId}")]
        public IActionResult Put(CSession attendance)
        {
            if (DbActions.ValidateRequest(Request))
                DbActions.UpdateAttendance(attendance);
            else
                return StatusCode(401);
            return NoContent();
        }

        [HttpPost("session/{sessionId}")]
        public IActionResult Post(CSession attendance)
        {
            if (DbActions.ValidateRequest(Request))
                DbActions.UpdateAttendance(attendance);
            else
                return StatusCode(401);
            return NoContent();
        }

        [HttpGet("sessionList/{teacherId}&month={monthYear}", Name = "getSessionList")]
        public ActionResult<List<CSession>> GetSessionList(int teacherId, String monthYear)
        {
            DateTime parsedMonth = DateTime.ParseExact(monthYear, "MM-yyyy", null);
            if (DbActions.ValidateRequest(Request))
                return DbActions.GetSessionList(teacherId, parsedMonth);
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
                user.MiddleName = googleInfo.Name.MiddleName ?? "";
                user.LastName = googleInfo.Name.FamilyName ?? "";
            }
            catch (Exception e)
            {
                
            }
        }

    }

}
