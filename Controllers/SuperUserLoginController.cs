using EmployeeManagementSystem.DTO;
using EmployeeManagementSystem.Repository;
using EmployeeManagementSystem.RepositoryImpl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuperUserLoginController:ControllerBase
    {
        private readonly ISuperUser _SuperUserrepository;
        private readonly ILogger<SuperUserLoginController> _logger; 

        public SuperUserLoginController(ISuperUser SuperUserrepository, ILogger<SuperUserLoginController> logger)
        {
            _SuperUserrepository = SuperUserrepository;
            _logger = logger;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SuperUserLoginDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password))
            {
                _logger.LogInformation("Provided credentials are either empty or not matched");
                return BadRequest("Username and password are required.");
            }

            var token=await _SuperUserrepository.Login(loginDto.Username, loginDto.Password);
            if(token == null)
            {
                _logger.LogInformation("Invalid username and Password.");
                return Unauthorized("Invalid username or password.");
            }

            return Ok(token);

            /*
            var superAdmin = _SuperUserrepository.Login(loginDto.Username, loginDto.Password);
            if (superAdmin == null)
            {
                _logger.LogInformation("Invalid username and Password.");
                return Unauthorized("Invalid username or password.");
            }
            return Ok(new { Message = "Login successful!" });
            */

        }


        [HttpPost("logout")]
        public async Task<ActionResult> LogOut([FromBody] LogOutDTO logoutdto)
        {
            //var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer","");
            if(string.IsNullOrEmpty(logoutdto.Token))
            {
                return BadRequest("Token Not Provided");
            }
            var result=await _SuperUserrepository.Logout(logoutdto.Token);
            return Ok(result);
        }


    }
}
