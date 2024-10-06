using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        public DateTime DateOfJoining {  get; set; }
        
        [Column("EmployeeSalary")]
        public float EmployeeSalary { get; set; }

        /*
        [ForeignKey("EmployeeDepartmentId")]
        public Department Department { get; set; }

        [ForeignKey("EmployeeRoleId")]
        public Role Role { get; set; }
        */

        [JsonIgnore]
        public virtual Department? Department { get; set; }
        [JsonIgnore]
        public virtual Role? Role { get; set; }
    }
}
