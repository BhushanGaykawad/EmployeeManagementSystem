using EmployeeManagementSystem.CustomizedExceptions;
using EmployeeManagementSystem.Model;
using EmployeeManagementSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartmentController:ControllerBase
    {

        private readonly IDeparment _departmentRepository;
        private readonly ILogger<Department> _logger;

        public DepartmentController(IDeparment departmentRepository, ILogger<Department> logger)
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartment()
        {
            try
            {
                _logger.LogInformation("Fetching All Departments");
                var department = await _departmentRepository.GetAllDeparments();
                if(department == null)
                {
                    _logger.LogInformation("Records fetched are all null.");
                    return NotFound();
                }
                return Ok(department);
            }catch(Exception ex){
                _logger.LogError("Error While Fetching Records from Department Table");
                throw new CustomizedException("Error Occured while fetching records", ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartmentById(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching Department by department Id:{id}");
                var department = await _departmentRepository.GetDeparmentsById(id);

                if (department == null)
                {
                    return NotFound();
                }
                return Ok(department);

            } catch (Exception ex)
            {
                _logger.LogError("Record with Given ID not Found");
                throw new CustomizedException("Error Occured while fetching the department", ex);

            }

        }
        [HttpPost]
        public async Task<ActionResult<Department>> CreateDepartment([FromBody ]Department department)
        {
            try
            {
                if(department == null)
                {
                    _logger.LogWarning("department provided in null");
                    return BadRequest(); 
                }
                _logger.LogInformation("Creating new Department");
                var createdDepartment = await _departmentRepository.CreateDepartment(department);   
                return CreatedAtAction(nameof(CreateDepartment), new {id=createdDepartment.DepartmentId},createdDepartment);

            }catch(Exception ex)
            {
                _logger.LogError(ex, "erro while creating new role");
                throw new CustomizedException("Error occured while creating department");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Department>> UpdateDepartment(int id,[FromBody]Department department)
        {
            try
            {
                if (department == null)
                {
                    _logger.LogInformation("Either department details provided are null or ID mismatched");
                    return BadRequest("Either department details provided are null or ID mismatched");
                }
                _logger.LogInformation("Updating the record using department ID" + id);
                var updatedDepartment = await _departmentRepository.UpdateDepartment(department);
                if(updatedDepartment == null)
                {
                    _logger.LogWarning($"Department with Id{id} not found for updation.");
                    return NotFound();
                }
                return Ok(updatedDepartment);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured while updating department");
                throw new CustomizedException($"Failed to update department");

            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult>DeleteDepartment(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting record with deparment id {id}");
                var result=await _departmentRepository.DeleteDepartment(id);
                return Ok(result);
            }catch (Exception ex)
            {
                _logger.LogError("Error while deleting department");
                throw new CustomizedException($"Error while deleting department");
            }
        }



    }
}
