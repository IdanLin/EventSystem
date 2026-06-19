using EventSystem_ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem_ClassLibrary.Repository
{
    public class EventRepository
    {
        EventSystemContext db = new EventSystemContext();
        public List<Event> GetAllEvents()
        {
            return db.Events.OrderBy(e => e.StartDate).ToList();
        }

        public Event GetEventById(int id)
        {
            Event e = db.Events.SingleOrDefault(x => x.Id == id);

            if (e != null)
            {
                List<Session> sessions = db.Sessions.Where(s => s.EventId == id).ToList();
                e.Sessions = sessions;
            }

            return e;
        }

        public void InsertEvent(Event newEvent)
        {
            db.Events.Add(newEvent);
            db.SaveChanges();
        }

        public void UpdateEvent(Event updatedEvent)
        {
            Event e = db.Events.SingleOrDefault(x => x.Id == updatedEvent.Id);
            if (e != null)
            {
                e.Title = updatedEvent.Title;
                e.Description = updatedEvent.Description;
                e.StartDate = updatedEvent.StartDate;
                e.EndDate = updatedEvent.EndDate;
                e.Location = updatedEvent.Location;
                e.EventType = updatedEvent.EventType;

                List<Session> sessions = db.Sessions.Where(x => x.EventId == e.Id).ToList();
                for (int i = 0; i < sessions.Count; i++)
                {
                    if (sessions[i].StartTime < e.StartDate || sessions[i].EndTime > e.EndDate)
                    {
                        db.Sessions.Remove(sessions[i]);
                    }
                }
                db.SaveChanges();
            }
        }

        public bool DeleteEvent(int id)
        {
            Event e = db.Events.SingleOrDefault(x => x.Id == id);
            if (e != null)
            {
                db.Events.Remove(e);
                db.SaveChanges(); //I have in DB on delete cascade so it will delete session and registers
                return true;
            }
            return false;
        }
    }
}
