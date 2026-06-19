using EventSystem.DTO;
using EventSystem_ClassLibrary.Models;
using EventSystem_ClassLibrary.Repository;

namespace EventSystem.Services
{
    public class SessionService
    {
        private readonly SessionRepository _sessionRepository;
        private readonly EventRepository _eventRepository;
        private readonly SessionRegistrationRepository _sessionRegistrationRepository;
        private readonly UserRepository _userRepository;

        public SessionService(SessionRepository sessionRepository, EventRepository eventRepository, SessionRegistrationRepository sessionRegistrationRepository, UserRepository userRepository)
        {
            _sessionRepository = sessionRepository;
            _eventRepository = eventRepository;
            _sessionRegistrationRepository = sessionRegistrationRepository;
            _userRepository = userRepository;
        }

        public string CreateSession(int id, SessionDTO sessionDto)
        {
            Event ev = _eventRepository.GetEventById(id);
            if (ev == null)
            {
                return $"Event {id} not found";
            }

            if (sessionDto.Session_Start_Time >= sessionDto.Session_End_Time)
            {
                return "Session_Start_Time >= Session_End_Time";
            }

            if (sessionDto.Session_Start_Time < ev.StartDate || sessionDto.Session_End_Time > ev.EndDate)
            {
                return "Session_Start_Time < ev.StartDate || Session_End_Time > ev.EndDate";
            }

            Session newSession = new Session();
            newSession.EventId = ev.Id;
            newSession.Title = sessionDto.Session_Title;
            newSession.Description = sessionDto.Session_Description;
            newSession.SpeakerName = sessionDto.Session_Speaker_Name;
            newSession.StartTime = sessionDto.Session_Start_Time;
            newSession.EndTime = sessionDto.Session_End_Time;
            newSession.RoomName = sessionDto.Session_Room_Name;

            _sessionRepository.InsertSession(newSession);
            sessionDto.Session_Id = newSession.Id;
            return "Success";
        }

        public string RegisterUser(int userId, int sessionId)
        {
            if (_sessionRepository.GetSessionById(sessionId) == null)
                return $"Session {sessionId} not found";

            if (!_userRepository.IsUserInSystem(userId))
                return $"User {userId} not found";

            if (_sessionRegistrationRepository.IsUserRegisteredToSession(userId, sessionId))
                return "User is already registered for this session or not found";

            if (_sessionRegistrationRepository.IsUserRegisteredSessionsOverlap(userId, sessionId))
                return "Schedule overlap";

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
                UserDTO dto = new UserDTO(); //ask only for name and email
                dto.User_Full_Name = u.FullName;
                dto.User_Email = u.Email;

                dtoList.Add(dto);
            }

            return dtoList;
        }

        public string UpdateSession(int id, SessionDTO sessionDto)
        {
            if (sessionDto == null)
                return "Session data is missing";

            Session existingSession = _sessionRepository.GetSessionById(id);
            if (existingSession == null)
                return $"Session {id} not found";

            Event ev = _eventRepository.GetEventById(existingSession.EventId);
            if (ev == null)
                return "Error: The specified event was not found.";

            if (sessionDto.Session_Start_Time >= sessionDto.Session_End_Time)
                return "Error: Session end time must be after the start time.";

            if (sessionDto.Session_Start_Time < ev.StartDate || sessionDto.Session_End_Time > ev.EndDate)
                return "Error: Session times must be strictly within the main event's timeframe.";

            Session s = new Session();
            existingSession.Title = sessionDto.Session_Title;
            existingSession.Description = sessionDto.Session_Description;
            existingSession.SpeakerName = sessionDto.Session_Speaker_Name;
            existingSession.StartTime = sessionDto.Session_Start_Time;
            existingSession.EndTime = sessionDto.Session_End_Time;
            existingSession.RoomName = sessionDto.Session_Room_Name;

            RemoveConflictingRegistrations(id, sessionDto.Session_Start_Time, sessionDto.Session_End_Time);
            _sessionRepository.UpdateSession(existingSession);

            return "Success";
        }

        private void RemoveConflictingRegistrations(int currentSessionId, DateTime newStartTime, DateTime newEndTime)
        {
            List<SessionRegistration> currentRegistrations = _sessionRegistrationRepository.GetRegistrationsBySessionId(currentSessionId);

            List<SessionRegistration> registrationsToRemove = new List<SessionRegistration>();

            for (int i = 0; i < currentRegistrations.Count; i++)
            {
                int userId = currentRegistrations[i].UserId;

                List<SessionRegistration> userOtherRegistrations = _sessionRegistrationRepository.GetRegistrationsByUserId(userId);

                bool hasConflict = false;

                for (int j = 0; j < userOtherRegistrations.Count; j++)
                {
                    int otherSessionId = userOtherRegistrations[j].SessionId;

                    if (otherSessionId == currentSessionId)
                    {
                        continue;
                    }

                    Session otherSession = _sessionRepository.GetSessionById(otherSessionId);

                    if (otherSession != null)
                    {
                        if (newStartTime < otherSession.EndTime && newEndTime > otherSession.StartTime)
                        {
                            hasConflict = true;
                            break;
                        }
                    }
                }

                if (hasConflict)
                {
                    registrationsToRemove.Add(currentRegistrations[i]);
                }
            }

            for (int i = 0; i < registrationsToRemove.Count; i++)
            {
                _sessionRegistrationRepository.DeleteRegistration(registrationsToRemove[i]);
            }
        }

        public string DeleteSession(int id)
        {
            return _sessionRepository.DeleteSession(id) ? "Success" : "Session not found";
        }
    }
}
