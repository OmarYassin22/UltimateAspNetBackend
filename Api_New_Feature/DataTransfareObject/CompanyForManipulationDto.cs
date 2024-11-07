using System.ComponentModel.DataAnnotations;

namespace Api_New_Feature.DataTransfareObject
{
    public abstract class CompanyForManipulationDto
    {
        [Required(ErrorMessage = "Company name is required")]
        [MaxLength(60, ErrorMessage = "Max Length For Cmapany Name is 60 chars")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Company address is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Address is 60 characters")]
        public string Address { get; set; }
        public string Country { get; set; }
        public IEnumerable<EmployeeForCreationDTO>? employees { get; set; }
    }
}
