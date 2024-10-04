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
    public class RoleController :ControllerBase
    {
        private readonly IRole _roleRepository;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRole roleRepository, ILogger<RoleController> logger)
        {
            _roleRepository = roleRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetAllRole()
        {
            try
            {
                _logger.LogInformation("Fetching All Roles");
                var roles= await _roleRepository.GetAllRoles();
                if(roles == null)
                {
                    _logger.LogInformation("Records fetched are all null.");
                    return NotFound();
                }
                return Ok(roles);
            }catch (Exception ex)
            {
                _logger.LogError("Error While Fetching Records from Role Table");
                throw new CustomizedException("Error Occured while fetching records.",ex);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRoleById(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching Roles by ID:{id}" );
                var role= await _roleRepository.GetRoleById(id);
                if(role==null)
                {
                    _logger.LogInformation($"Record for role id :{id} are null.");
                    return  NotFound();
                }
                return Ok(role);
            }catch(Exception ex)
            {
                _logger.LogError("Record with given Id not Found");
                throw new CustomizedException($"Record for role id :{id} are not found",ex);

            }
        }
        
        [HttpPost]
        public async Task<ActionResult<Role>> CreateRoll([FromBody] Role role)
        {
            try
            {
                if (role == null)
                {
                    _logger.LogWarning("Provided data for role is empty.");
                    return BadRequest("Provided data for role is empty");
                }

                _logger.LogInformation("Creating a new role with provided data");
                var createdRole = await _roleRepository.CreateRole(role);  // Use await here
                return CreatedAtAction(nameof(CreateRoll), new { id = createdRole.RoleID }, createdRole);  // Return CreatedAtAction
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating new role");
                throw new CustomizedException("Error occurred while creating new role", ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Role>> UpdateRole(int id, [FromBody]Role role)
        {
            try
            {
                if (role == null || id != role.RoleID)
                {
                    _logger.LogInformation("Either Role is null or ID Mismatch");
                    return BadRequest("Either Role is null or ID Mismatch");
                }
                _logger.LogInformation("Updating the Record Using provided Id" + id);
                var updatedRole = await _roleRepository.UpdateRole(role);
                if(updatedRole == null)
                {
                    _logger.LogWarning($"Role with ID {id} not found for update.");
                    return NotFound();
                }
                return Ok(updatedRole);

            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to update role: {id}");
                throw new CustomizedException($"Failed to update role: {id}",ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult>DeleteRole(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting role record with Id:{id}");
                var result=await _roleRepository.DeleteRole(id);
                return Ok(result);  

            }catch(Exception ex)
            {
                _logger.LogError($"Error while deleting record with id:{id}");
                throw new CustomizedException($"Error while deleting record with id:{id}");

            }
        }
    }
}
