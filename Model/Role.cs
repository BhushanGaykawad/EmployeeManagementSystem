using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Model
{
    [Table("Role")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("RoleId")]
        public int RoleID { get; set; }


        [Column("Role")]
        [Required(ErrorMessage ="Role is mandatory Field.")]
        public String RoleType { get; set; }



    }
}
