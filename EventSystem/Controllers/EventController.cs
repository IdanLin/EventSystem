using EventSystem.DTO;
using EventSystem.Services;
using EventSystem_ClassLibrary.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;

namespace EventSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;
        private readonly SessionService _sessionService;

        public EventController(EventService eventService, SessionService sessionService)
        {
            _eventService = eventService;
            _sessionService = sessionService;
        }

        [HttpPost] // POST: /api/event/
        public ActionResult<EventDTO> CreateEvent([FromBody] EventDTO e)
        {
            string result = _eventService.CreateEvent(e);
            return result == "Success" ? Ok(e) : BadRequest(result);
        }

        [HttpGet] // GET: /api/event/{eventId}
        [Route("{eventId}")]
        public ActionResult<EventDTO> GetEventById(int eventId)
        {
            EventDTO e = _eventService.GetEventById(eventId);
            return e != null ? Ok(e) : NotFound($"Event with ID {eventId} not found.");
        }

        [HttpPut] // PUT: /api/event/{eventId}
        [Route("{eventId}")]
        public ActionResult UpdateEvent(int eventId, [FromBody] EventDTO e) //for feature number 2 (events) - my logic: delete session that start before the new start time or end after the new end time
        {
            string result = _eventService.UpdateEvent(eventId, e);
            return result == "Success" ? Ok($"Event with ID {eventId} updated successfully") : BadRequest(result);
        }

        [HttpDelete] // DELETE: /api/event/{eventId}
        [Route("{eventId}")]
        public ActionResult DeleteEvent(int eventId)
        {
            string result = _eventService.DeleteEvent(eventId);
            return result == "Success" ? Ok($"Event with ID {eventId} deleted successfully") : NotFound(result);
        }

        [HttpGet] // GET: /api/event/schedule
        [Route("schedule")]
        public ActionResult<List<EventDTO>> GetAllEvents()
        {
            List<EventDTO> events = _eventService.GetAllEvents();
            if (events == null || !events.Any()) return NotFound("No events found.");
            return Ok(events);
        }

        [HttpPost] // POST: /api/event/{eventId}/session
        [Route("{eventId}/session")]
        public ActionResult<SessionDTO> CreateSessionInEvent(int eventId, [FromBody] SessionDTO s)
        {
            string result = _sessionService.CreateSession(eventId, s);
            return result == "Success" ? Ok(s) : BadRequest(result);
        }

        [HttpGet] // GET: /api/event/{eventId}/weather
        [Route("{eventId}/weather")]
        public async Task<IActionResult> GetWeatherForEvent(int eventId)
        {
            var weather = await _eventService.GetWeatherForEvent(eventId);
            if (weather.ValueKind == JsonValueKind.Undefined) // cheack if returned default
                return NotFound($"Event {eventId} not found or Weather data is unavailable.");

            return Ok(weather);
        }
    }
}
