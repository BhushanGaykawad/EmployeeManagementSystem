namespace EmployeeManagementSystem.DTO
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeePhoneNumber { get; set; }
        public int EmployeeDepartmentId { get; set; }
        public string DepartmentName { get; set; } // New property
        public int EmployeeRoleId { get; set; }
        public string RoleType { get; set; } // New property
        public DateTime DateOfJoining { get; set; }
        public float EmployeeSalary { get; set; }
    }
}
