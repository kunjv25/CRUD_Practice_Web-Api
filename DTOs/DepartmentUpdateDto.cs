using System.ComponentModel.DataAnnotations;

namespace CRUD_Practice_Web_Api.DTOs
{
    public class DepartmentUpdateDto
    {
        [Required]
        public string DepartmentName { get; set; } = string.Empty;
    }
}
