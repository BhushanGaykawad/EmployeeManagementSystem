using EmployeeManagementSystem.CustomizedExceptions;
using EmployeeManagementSystem.DTO;
using EmployeeManagementSystem.Model;
using EmployeeManagementSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController:ControllerBase
    {
        private readonly IEmployee _employeeRepository;
        private readonly IDeparment _departmentRepository;
        private readonly IRole _roleRepository;
        private readonly ILogger<EmployeeController> _logger;
        
        public EmployeeController(IEmployee employeeRepository, ILogger<EmployeeController> logger,IDeparment deparment,IRole role)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _departmentRepository = deparment;
            _roleRepository = role;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
        {
            try
            {
                _logger.LogInformation("Fetching All Employees");
                var employees=await _employeeRepository.GetAllEmployeeWithDetails();
                if(employees == null)
                {
                    _logger.LogInformation("Employees fetched are null or no Employees found");
                    return NotFound();
                }
                return Ok(employees);
            }catch (Exception ex)
            {
                _logger.LogError("Error While fetching employees");
                throw new CustomizedException("Error While fetching employees",ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeById(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching employee with id: {id}");
                var employee = await _employeeRepository.GetEmployeebyId(id);

                if (employee == null)
                {
                    return NotFound();
                }
                var deptId=employee.EmployeeDepartmentId;
                _logger.LogInformation($"*****************{deptId}*************************");
                var department = await _departmentRepository.GetDeparmentsById(employee.EmployeeDepartmentId);
                var role = await _roleRepository.GetRoleById(employee.EmployeeRoleId);

                var employeeDTO = new EmployeeDTO
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = employee.EmployeeName,
                    EmployeeEmail = employee.EmployeeEmail,
                    EmployeePhoneNumber = employee.EmployeePhoneNumber,
                    EmployeeDepartmentId = employee.EmployeeDepartmentId,
                    DepartmentName = department?.DepartmentName,  
                    EmployeeRoleId = employee.EmployeeRoleId,
                    RoleType = role?.RoleType,  
                    DateOfJoining = employee.DateOfJoining,
                    EmployeeSalary = employee.EmployeeSalary
                };

                return Ok(employeeDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while fetching employee: {0}", ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }


    [HttpPost]
        public async Task<ActionResult<Employee>> CreateNewEmployee([FromBody] Employee emp)
        {
            if(emp == null)
            {
                _logger.LogInformation("Employee provided is null");
                return BadRequest("Employee Provided details are null");
            }
            _logger.LogInformation("Creation new Employee");
            var createdEmployee= await _employeeRepository.CreateEmployee(emp);
            return CreatedAtAction(nameof(CreateNewEmployee), new {id=createdEmployee.EmployeeId }, createdEmployee);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, [FromBody] EmployeeUpdateDTO employeeUpdateDto)
        {
            
            try
            {
                if (employeeUpdateDto == null)
                {
                    _logger.LogWarning("Provided data for employee is empty.");
                    return BadRequest("Provided data for employee is empty.");
                }

                var updatedEmployee = await _employeeRepository.UpdateEmployee(id, employeeUpdateDto);
                if (updatedEmployee == null)
                {
                    _logger.LogWarning($"Employee with ID {id} not found.");
                    return NotFound();
                }

                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update employee: {id}");
                throw new CustomizedException($"Failed to update employee: {id}", ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteEmployee(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting Employee from Employee entity with id:{id}");
                var result= await _employeeRepository.DeleteEmployee(id);
                return Ok(result);
            }
            catch ( Exception ex)
            {
                _logger.LogInformation($"Error while deleting employee with id:{id}");
                throw new CustomizedException("Error while deleting employee",ex);
            }
        }

    }
}
