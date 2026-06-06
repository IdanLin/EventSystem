using EventSystem.DTO;
using EventSystem_ClassLibrary.Models;
using EventSystem_ClassLibrary.Repository;

namespace EventSystem.Services
{
    public class SessionService
    {
        private readonly SessionRepository _sessionRepository;
        private readonly EventRepository _eventRepository;

        public SessionService(SessionRepository sessionRepository, EventRepository eventRepository)
        {
            _sessionRepository = sessionRepository;
            _eventRepository = eventRepository;
        }

        public bool CreateSession(SessionDTO sessionDto)
        {
            Event ev = _eventRepository.GetEventById(sessionDto.Event_Id);
            if (ev == null)
            {
                return false;
            }

            if (sessionDto.Session_Start_Time >= sessionDto.Session_End_Time)
            {
                return false;
            }

            if (sessionDto.Session_Start_Time < ev.StartDate || sessionDto.Session_End_Time > ev.EndDate)
            {
                return false;
            }

            Session newSession = new Session();
            newSession.EventId = sessionDto.Event_Id;
            newSession.Title = sessionDto.Session_Title;
            newSession.Description = sessionDto.Session_Description;
            newSession.SpeakerName = sessionDto.Session_Speaker_Name;
            newSession.StartTime = sessionDto.Session_Start_Time;
            newSession.EndTime = sessionDto.Session_End_Time;
            newSession.RoomName = sessionDto.Session_Room_Name;

            _sessionRepository.InsertSession(newSession);
            return true;
        }

        public string RegisterUser(int userId, int sessionId)
        {
            Session session = _sessionRepository.GetSessionById(sessionId);
            if (session == null)
            {
                return "Session not found";
            }

            List<Session> userSessions = _sessionRepository.GetUserRegisteredSessions(userId);

            foreach (Session s in userSessions)
            {
                if (s.Id == sessionId)
                {
                    return "User is already registered for this session";
                }
            }

            foreach (Session existingSession in userSessions)
            {
                if (session.StartTime < existingSession.EndTime && session.EndTime > existingSession.StartTime)
                {
                    return "Schedule overlap";
                }
            }

            SessionRegistration reg = new SessionRegistration();
            reg.SessionId = sessionId;
            reg.UserId = userId;
            reg.RegistrationDate = DateTime.Now;

            _sessionRepository.AddRegistration(reg);
            return "Success";
        }

        public List<UserDTO> GetUsersBySessionId(int sessionId)
        {
            List<User> users = _sessionRepository.GetUsersBySessionId(sessionId);
            List<UserDTO> dtoList = new List<UserDTO>();

            foreach (User u in users)
            {
                UserDTO dto = new UserDTO();
                dto.User_Id = u.Id;
                dto.User_Full_Name = u.FullName;
                dto.User_Email = u.Email;

                dtoList.Add(dto);
            }

            return dtoList;
        }
    }
}
