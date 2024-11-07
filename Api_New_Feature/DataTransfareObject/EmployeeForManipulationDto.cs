using System.ComponentModel.DataAnnotations;

namespace Api_New_Feature.DataTransfareObject
{
    public abstract class EmployeeForManipulationDto
    {
        [Required(ErrorMessage = "Employee name is required")]
        [MaxLength(60, ErrorMessage = "Max Length For Employee Name is 60 chars")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Employee age is required")]
        [Range(18, 60)]
        public int Age { get; set; }
        [Required(ErrorMessage = "Employee position is required")]
        [MaxLength(60, ErrorMessage = "Max Length For Employee Position is 60 chars")]
        public string Position { get; set; }
    }
}
