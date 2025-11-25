namespace DAL.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string TimeZone { get; set; }

        public int UnitId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
