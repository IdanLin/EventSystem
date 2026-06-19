namespace EventSystem.DTO
{
    public class EventDTO
    {
        public int Event_Id { get; set; }

        public string Event_Title { get; set; }

        public string Event_Description { get; set; }

        public DateTime Event_Start_Date { get; set; }

        public DateTime Event_End_Date { get; set; }

        public string Event_Location { get; set; }

        public string Event_City { get; set; }

        public string Event_Type { get; set; }

        public List<SessionDTO> Event_Sessions { get; set; } = new List<SessionDTO>();
    }
}
