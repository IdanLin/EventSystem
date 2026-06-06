using EventSystem.DTO;
using EventSystem.Services;
using EventSystem_ClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        [HttpPost]
        [Route("insert")]
        public ActionResult CreateSession(SessionDTO s)
        {
            bool success = _sessionService.CreateSession(s);
            if (!success) return BadRequest("Invalid session data");
            return Ok("Session created successfully");
        }

        [HttpPost]
        [Route("register")]
        public ActionResult RegisterUser(int userId, int sessionId)
        {
            string result = _sessionService.RegisterUser(userId, sessionId);
            if (result != "Success") return BadRequest(result);
            return Ok(result);
        }

        [HttpGet]
        [Route("getusers/{sessionId}")]
        public ActionResult<List<UserDTO>> GetUsersBySession(int sessionId)
        {
            return Ok(_sessionService.GetUsersBySessionId(sessionId));
        }
    }
}
