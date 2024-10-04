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
        private readonly ILogger<EmployeeController> _logger;
        
        public EmployeeController(IEmployee employeeRepository, ILogger<EmployeeController> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
        {
            try
            {
                _logger.LogInformation("Fetching All Employees");
                var employees=await _employeeRepository.GetAllEmployee();
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
        public async Task<ActionResult<Employee>>GetEmployeeById(int eid)
        {
            try
            {
                _logger.LogInformation($"Fetching employees with id :{eid}");
                var employee= _employeeRepository.GetEmployeebyId(eid); 
                if(employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }catch (Exception ex) 
            {
                _logger.LogError("Error While fetching employee");
                throw new CustomizedException("Error While fetching employee", ex);
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
        public async Task<ActionResult<bool>> DeleteEmployee(int eid)
        {
            try
            {
                _logger.LogInformation($"Deleting Employee from Employee entity with id:{eid}");
                var result= await _employeeRepository.DeleteEmployee(eid);
                return Ok(result);
            }
            catch ( Exception ex)
            {
                _logger.LogInformation($"Error while deleting employee with id:{eid}");
                throw new CustomizedException("Error while deleting employee",ex);
            }
        }

    }
}
