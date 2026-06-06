using EventSystem.DTO;
using EventSystem.Services;
using EventSystem_ClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        [HttpGet]
        [Route("schedule/{userId}")]
        public ActionResult<List<SessionDTO>> GetUserSchedule(int userId)
        {
            return Ok(_userService.GetUserSchedule(userId));
        }
    }
}
