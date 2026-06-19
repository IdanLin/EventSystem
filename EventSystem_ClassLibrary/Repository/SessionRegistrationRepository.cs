using EventSystem_ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem_ClassLibrary.Repository
{
    public class SessionRegistrationRepository
    {
        EventSystemContext db = new EventSystemContext();

        //for the update of sessions:
        public List<SessionRegistration> GetRegistrationsBySessionId(int sessionId)
        {
            return db.SessionRegistrations.Where(x => x.SessionId == sessionId).ToList();
        }

        public List<SessionRegistration> GetRegistrationsByUserId(int userId)
        {
            return db.SessionRegistrations.Where(x => x.UserId == userId).ToList();
        }

        public void DeleteRegistration(SessionRegistration registration)
        {
            db.SessionRegistrations.Remove(registration);
            db.SaveChanges();
        }

        public bool IsUserRegisteredToSession(int userId, int sessionId)
        {
            return db.SessionRegistrations.Any(r => r.UserId == userId && r.SessionId == sessionId);
        }
        public bool IsUserRegisteredSessionsOverlap(int userId, int sessionId)
        {
            Session s = db.Sessions.SingleOrDefault(s => s.Id == sessionId);

            if (s == null)
                return false;

            return db.SessionRegistrations
                .Where(reg => reg.UserId == userId)
                .Select(reg => reg.Session)
                .Any(existingSession =>s.StartTime < existingSession.EndTime &&s.EndTime > existingSession.StartTime);
        }
        public void AddRegistration(SessionRegistration registration)
        {
            db.SessionRegistrations.Add(registration);
            db.SaveChanges();
        }

        public List<User> GetUsersBySessionId(int sessionId)
        {
            return db.SessionRegistrations.Where(sr => sr.SessionId == sessionId).Select(sr => sr.User).ToList();
        }
    }
}
