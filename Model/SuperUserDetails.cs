using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Model
{
    public class SuperUserDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SuperUserId{get;set;}
        
        [Column("Username")]
        public String UserName { get;set;}


        [Column("Password")]
        public String UserPassword { get;set;}
    }
}
