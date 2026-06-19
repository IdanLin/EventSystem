using EventSystem.DTO;
using EventSystem_ClassLibrary.Models;
using EventSystem_ClassLibrary.Repository;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace EventSystem.Services
{
    public class EventService
    {
        private readonly EventRepository _eventRepository;
        private readonly IMemoryCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;

        private const string CacheKey = "weather_cache";
        public EventService(EventRepository eventRepository, IMemoryCache cache, IHttpClientFactory httpClientFactory)
        {
            _eventRepository = eventRepository;
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

        public List<EventDTO> GetAllEvents()
        {
            List<Event> events = _eventRepository.GetAllEvents();
            List<EventDTO> dtoList = new List<EventDTO>();

            foreach (Event e in events)
            {
                EventDTO dto = new EventDTO();
                dto.Event_Id = e.Id;
                dto.Event_Title = e.Title;
                dto.Event_Description = e.Description;
                dto.Event_Start_Date = e.StartDate;
                dto.Event_End_Date = e.EndDate;
                dto.Event_Location = e.Location;
                dto.Event_City = e.City;
                dto.Event_Type = e.EventType;

                dtoList.Add(dto);
            }

            return dtoList;
        }

        public EventDTO GetEventById(int id)
        {
            Event e = _eventRepository.GetEventById(id);
            if (e == null)
            {
                return null;
            }

            EventDTO dto = new EventDTO();
            dto.Event_Id = e.Id;
            dto.Event_Title = e.Title;
            dto.Event_Description = e.Description;
            dto.Event_Start_Date = e.StartDate;
            dto.Event_End_Date = e.EndDate;
            dto.Event_Location = e.Location;
            dto.Event_City = e.City;
            dto.Event_Type = e.EventType;
            dto.Event_Sessions = new List<SessionDTO>();

            if (e.Sessions != null)
            {
                foreach (Session s in e.Sessions)
                {
                    SessionDTO sessionDto = new SessionDTO();
                    sessionDto.Session_Id = s.Id;
                    sessionDto.Event_Id = s.EventId;
                    sessionDto.Session_Title = s.Title;
                    sessionDto.Session_Description = s.Description;
                    sessionDto.Session_Speaker_Name = s.SpeakerName;
                    sessionDto.Session_Start_Time = s.StartTime;
                    sessionDto.Session_End_Time = s.EndTime;
                    sessionDto.Session_Room_Name = s.RoomName;

                    dto.Event_Sessions.Add(sessionDto);
                }
            }

            return dto;
        }

        public string CreateEvent(EventDTO newEventDto)
        {
            if (newEventDto == null)
                return "Event data is missing";
            if (newEventDto.Event_End_Date < newEventDto.Event_Start_Date)
                return "Event_End_Date < Event_Start_Date";

            Event e = new Event();
            e.Title = newEventDto.Event_Title;
            e.Description = newEventDto.Event_Description;
            e.StartDate = newEventDto.Event_Start_Date;
            e.EndDate = newEventDto.Event_End_Date;
            e.Location = newEventDto.Event_Location;
            e.City = newEventDto.Event_City;
            e.EventType = newEventDto.Event_Type;

            _eventRepository.InsertEvent(e);
            newEventDto.Event_Id = e.Id;
            return "Success";
        }

        public string UpdateEvent(int id, EventDTO updatedEventDto)
        {
            if (updatedEventDto == null)
                return "Event data is missing";
            if (updatedEventDto.Event_End_Date < updatedEventDto.Event_Start_Date)
                return "Event_End_Date < Event_Start_Date";

            Event e = _eventRepository.GetEventById(id);
            if (e == null)
            {
                return $"Event {id} not found";
            }

            e.Title = updatedEventDto.Event_Title;
            e.Description = updatedEventDto.Event_Description;
            e.StartDate = updatedEventDto.Event_Start_Date;
            e.EndDate = updatedEventDto.Event_End_Date;
            e.Location = updatedEventDto.Event_Location;
            e.City = updatedEventDto.Event_City;
            e.EventType = updatedEventDto.Event_Type;

            _eventRepository.UpdateEvent(e);
            return "Success";
        }

        public string DeleteEvent(int id)
        {
            return _eventRepository.DeleteEvent(id) ? "Success" : $"Event {id} not found";
        }

        public async Task<JsonElement> GetWeatherForEvent(int eventId)
        {
            string cacheKeyID = $"{CacheKey}_{eventId}";

            if (_cache.TryGetValue(cacheKeyID, out JsonElement weatherData))
            {
                return weatherData;
            }

            Event e = _eventRepository.GetEventById(eventId);
            if (e == null)
            {
                return default; //no null return option in JSON
            }
            var client = _httpClientFactory.CreateClient();

            string dateForUrl = e.StartDate.ToString("yyyy-MM-dd");

            string locationForUrl = Uri.EscapeDataString(e.City.ToString()); //no spaces
            string url;
            if (e.StartDate > DateTime.Now)
            {
                url = $"https://api.weatherapi.com/v1/forecast.json?key=f37d73e810d744b9834181309260606&q={locationForUrl}&dt={dateForUrl}";
            }
            else
            {
                url = $"https://api.weatherapi.com/v1/history.json?key=f37d73e810d744b9834181309260606&q={locationForUrl}&dt={dateForUrl}";
            }
            var data = await client.GetFromJsonAsync<JsonElement>(url);
            _cache.Set(cacheKeyID, data, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            return data;
        }
    }
}
