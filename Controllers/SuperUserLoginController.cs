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
        public async Task<ActionResult> Login([FromBody] SuperUserLoginDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password))
            {
                _logger.LogInformation("Provided credentials are either empty or not matched");
                return BadRequest("Username and password are required.");
            }

            // Call the repository's Login method
            var (accessToken, refreshToken) = await _SuperUserrepository.LogIn(loginDto.Username, loginDto.Password);

            if (accessToken == null)
            {
                _logger.LogInformation("Invalid username or password.");
                return Unauthorized("Invalid username or password.");
            }

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        


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
        /*
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDTO refreshTokenDto)
        {
            // Extract the JWT from the Authorization header
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            try
            {
                var (newAccessToken, newRefreshToken) = await _SuperUserrepository.RefreshToken(token, refreshTokenDto.RefreshToken);
                return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while refreshing the token: " + ex.Message);
            }
        }
        */
        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh([FromBody] RefreshTokenDTO refreshTokenDto)
        {
           // _logger.LogInformation($"Refresh token retrieved is: {refreshTokenDto.RefreshToken}");

            if (refreshTokenDto == null || string.IsNullOrEmpty(refreshTokenDto.RefreshToken))
            {
                _logger.LogInformation($"Refresh token retrieved is {refreshTokenDto.RefreshToken}");
                return BadRequest("RefreshToken is required.");
            }

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            _logger.LogInformation($"----------------------------++Access token retrived from header is {token} and Refresh Token received from request body is{refreshTokenDto.RefreshToken}");
            try
            {

                var (newAccessToken, newRefreshToken) = await _SuperUserrepository.RefreshToken(token, refreshTokenDto.RefreshToken);
                return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while refreshing the token: " + ex.Message);
            }
        }




    }
}
