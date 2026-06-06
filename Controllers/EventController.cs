using EventSystem.DTO;
using EventSystem.Services;
using EventSystem_ClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        [Route("getall")]
        public ActionResult<List<EventDTO>> GetAllEvents()
        {
            return Ok(_eventService.GetAllEvents());
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public ActionResult<EventDTO> GetEventById(int id)
        {
            var e = _eventService.GetEventById(id);
            if (e == null) return NotFound();
            return Ok(e);
        }

        [HttpPost]
        [Route("insert")]
        public ActionResult CreateEvent(EventDTO e)
        {
            _eventService.CreateEvent(e);
            return Ok("Event created successfully");
        }

        [HttpPost]
        [Route("update/{id}")]
        public ActionResult UpdateEvent(int id, EventDTO e)
        {
            _eventService.UpdateEvent(id, e);
            return Ok("Event updated successfully");
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public ActionResult DeleteEvent(int id)
        {
            _eventService.DeleteEvent(id);
            return Ok("Event deleted");
        }

        [HttpGet]
        [Route("{id}/weather")]
        public async Task<IActionResult> GetWeatherForEvent(int id)
        {
            var weather = await _eventService.GetWeatherForEvent(id);
            return Ok(weather);
        }
    }
}
