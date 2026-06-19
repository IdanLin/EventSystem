using EventSystem_ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem_ClassLibrary.Repository
{
    public class UserRepository
    {
        EventSystemContext db = new EventSystemContext();
        public List<User> GetAllUser()
        {
            return db.Users.ToList();
        }

        public bool IsUserInSystem(int userId)
        {
            return db.Users.Any(u => u.Id == userId);
        }

        public List<Session> GetUserSchedule(int userId)
        {
            return db.SessionRegistrations.Where(sr => sr.UserId == userId).Select(sr => sr.Session).ToList();
        }
    }
}
