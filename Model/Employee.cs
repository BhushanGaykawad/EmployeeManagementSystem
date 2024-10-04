using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Model
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("EmployeeId")]
        public int EmployeeId { get; set; }


        [Column("EmployeeName")]
        public String EmployeeName {  get; set; }

        [Column("EmployeeEmail")]
        public String EmployeeEmail{  get; set; }

        [Column("EmployeePhoneNumber")]
        public String EmployeePhoneNumber {  get; set; }

        [ForeignKey("Department")]
        [Column("Department")]
        public int EmployeeDepartmentId {  get; set; }

        [ForeignKey("Role")]
        [Column("Role")]
        public int EmployeeRoleId { get; set; }
        
        [Column("DateOfJoining")]
        public DateTime DateOfJoininig {  get; set; }
        
        [Column("EmployeeSalary")]
        public float EmployeeSalary { get; set; }


        public Department Department { get; set; }

        public Role Role { get; set; }
    }
}
