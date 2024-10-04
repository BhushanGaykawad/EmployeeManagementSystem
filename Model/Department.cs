using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Model
{
    [Table("Department")]
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("DepartmentId")]
        public int DepartmentId { get;set; }

        [Column("DepartmentName")]
        [Required(ErrorMessage ="Department Name is mandatory field")]
        public String DepartmentName { get; set; }

       
    }
}
