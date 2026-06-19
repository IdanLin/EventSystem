using EventSystem.DTO;
using EventSystem.Services;
using EventSystem_ClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet] // GET: /api/user/{userId}/schedule
        [Route("{userId}/schedule")]
        public ActionResult<List<SessionDTO>> GetUserSchedule(int userId)
        {
            List<SessionDTO> schedule = _userService.GetUserSchedule(userId);
            if (schedule == null || !schedule.Any()) return NotFound("User not found or schedule is empty.");
            return Ok(schedule);
        }

        // add for the register button
        [HttpGet] // GET: /api/user
        public ActionResult<List<UserDTO>> GetAllUser()
        {
            var users = _userService.GetAllUser();
            if (users == null || !users.Any()) return NotFound("No users found."); //Any == not empty
            return Ok(users);
        }
    }
}
