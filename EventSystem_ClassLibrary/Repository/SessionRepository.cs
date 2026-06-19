using EventSystem_ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem_ClassLibrary.Repository
{
    public class SessionRepository
    {
        EventSystemContext db = new EventSystemContext();
        public void InsertSession(Session newSession)
        {
            db.Sessions.Add(newSession);
            db.SaveChanges();
        }

        public Session GetSessionById(int sessionId)
        {
            return db.Sessions.SingleOrDefault(s => s.Id == sessionId);
        }

        public void UpdateSession(Session sessionObj)
        {
            db.SaveChanges();
        }

        public bool DeleteSession(int id)
        {
            Session existingSession = db.Sessions.SingleOrDefault(s => s.Id == id);
            if (existingSession == null)
                return false;
            
            db.Sessions.Remove(existingSession);
            db.SaveChanges();
            return true;
        }
    }
}
