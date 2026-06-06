using EventSystem.DTO;
using EventSystem_ClassLibrary.Models;
using EventSystem_ClassLibrary.Repository;
using Microsoft.AspNetCore.Http;

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

        public List<UserDTO> GetAllUser()
        {
            List<User> users = _userRepository.GetAllUser();
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
