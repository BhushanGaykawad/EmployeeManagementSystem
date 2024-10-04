using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Model
{
    public class BlackListedJWTToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column]
        public string Token { get; set; }

        [Column]
        public DateTime CreatedAt { get; set; }

        [Column]
        public DateTime BlackListedAt { get; set; }= DateTime.UtcNow;
    }
}
