using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTO
{
    public class RefreshTokenDTO
    {
        [Required]
        public string RefreshToken { get; set; }

    }
}
