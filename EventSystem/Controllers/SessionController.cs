using EventSystem.DTO;
using EventSystem.Services;
using EventSystem_ClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly SessionService _sessionService;

        public SessionController(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost] // POST: /api/session/{sessionId}/register
        [Route("{sessionId}/register")]
        public ActionResult RegisterUser([FromBody] int userId, int sessionId)
        {
            string result = _sessionService.RegisterUser(userId, sessionId);
            return result == "Success" ? Ok($"user (id:{userId}) register successfully to session(id:{sessionId})") : BadRequest(result);
        }

        [HttpGet] // GET: /api/session/{sessionId}/user
        [Route("{sessionId}/user")]
        public ActionResult<List<UserDTO>> GetUsersBySession(int sessionId)
        { 
            List<UserDTO> users = _sessionService.GetUsersBySessionId(sessionId);
            if (users == null || !users.Any()) return NotFound("Session not found or no users registered.");
            return Ok(users);
        }

        //for the session feture:
        //for feature number 2 (sessions) - my logic: canot operate if the new time start>end or before or after the event time.
        //also delete user the register to this event if this is in the same time with other session
        [HttpPut] // PUT: /api/session/{sessionId}
        [Route("{sessionId}")] 
        public ActionResult UpdateSession(int sessionId, [FromBody] SessionDTO sessionDto)
        {
            string result = _sessionService.UpdateSession(sessionId, sessionDto);
            return result == "Success" ? Ok($"Session {sessionId} updated successfully") : BadRequest(result);
        }

        //for feature number 2 (sessions)
        //use the on delete cascade to delete all related rows from other table with this session ID
        [HttpDelete] // DELETE: /api/session/{sessionId}
        [Route("{sessionId}")]
        public ActionResult DeleteSession(int sessionId)
        {
            string result = _sessionService.DeleteSession(sessionId);
            return result == "Success" ? Ok($"Session {sessionId} deleted successfully") : NotFound(result);
        }
    }
}
