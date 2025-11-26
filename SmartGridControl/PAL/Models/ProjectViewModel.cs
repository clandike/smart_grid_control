namespace PAL.Models
{
    public class ProjectViewModel
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string TimeZone { get; set; }

        public int UnitId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UnitName { get; set; }
    }
}
