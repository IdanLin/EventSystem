using EventSystem.DTO;
using EventSystem_ClassLibrary.Models;
using EventSystem_ClassLibrary.Repository;

namespace EventSystem.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<SessionDTO> GetUserSchedule(int userId)
        {
            List<Session> sessions = _userRepository.GetUserSchedule(userId);
            List<SessionDTO> dtoList = new List<SessionDTO>();

            foreach (Session s in sessions)
            {
                SessionDTO dto = new SessionDTO();
                dto.Session_Id = s.Id;
                dto.Event_Id = s.EventId;
                dto.Session_Title = s.Title;
                dto.Session_Description = s.Description;
                dto.Session_Speaker_Name = s.SpeakerName;
                dto.Session_Start_Time = s.StartTime;
                dto.Session_End_Time = s.EndTime;
                dto.Session_Room_Name = s.RoomName;

                dtoList.Add(dto);
            }

            return dtoList;
        }
    }
}
