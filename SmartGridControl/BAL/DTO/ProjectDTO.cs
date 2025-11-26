using System.ComponentModel.DataAnnotations;

namespace BAL.DTO
{
    public class ProjectDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Project name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(150, ErrorMessage = "Location cannot exceed 150 characters")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Time zone is required")]
        [RegularExpression(@"^(UTC|GMT)[+-]\d{1,2}$",
            ErrorMessage = "Time zone must be in format UTC+2 or GMT-5")]
        public string TimeZone { get; set; }

        [Required(ErrorMessage = "Unit must be selected")]
        public int UnitId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
