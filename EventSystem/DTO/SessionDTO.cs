namespace EventSystem.DTO
{
    public class SessionDTO
    {
        public int Session_Id { get; set; }

        public int Event_Id { get; set; }

        public string Session_Title { get; set; }

        public string Session_Description { get; set; }

        public string Session_Speaker_Name { get; set; }

        public DateTime Session_Start_Time { get; set; }

        public DateTime Session_End_Time { get; set; }

        public string Session_Room_Name { get; set; }
    }
}
