/*
using BuisnessLayer;
using DataAccessLayer.Entities;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FundooNotesApp.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IuserBL _userBL;
       // private readonly ILogger<UserController> _logger;

        public UserController(IuserBL userBL) //ILogger<UserController> logger)
        {
            _userBL = userBL;
           // _logger = logger;
        }



        [HttpPost("register")]
        public IActionResult Register([FromBody] UserModel userModel)
        {
            try
            {
                _userBL.RegisterUser(userModel);
                return Ok(new { message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }




        
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel userLoginModel)
        {
            var token = _userBL.ValidateUser(userLoginModel);
            if (token == null)
            {
                return Unauthorized(new { Error = "Unauthorized:Invalid email or password." });
            }

            return Ok(new { Token = token });
        }

        [HttpGet("{id}")]
        [Authorize] // Ensures only authorized users can access this endpoint
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _userBL.GetUserById(id);

                if (user == null)
                    return NotFound(new { message = "User not found." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
      

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                return BadRequest("Email is required.");
            }

            _userBL.SendResetPasswordEmail(model.Email);
            return Ok("Password reset email sent.");
        }





        // Reset Password - Use Token to Set New Password
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
        {
            try
            {
                _userBL.ResetPassword(model);
                return Ok("Password reset successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        

    }
}
*/


using BuisnessLayer;
using DataAccessLayer.Entities;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FundooNotesApp.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IuserBL _userBL;
        private readonly ILogger<UserController> _logger;

        public UserController(IuserBL userBL, ILogger<UserController> logger)
        {
            _userBL = userBL;
            _logger = logger;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserModel userModel)
        {
            try
            {
                _logger.LogInformation("Register request initiated for email: {Email}", userModel.Email);
                _userBL.RegisterUser(userModel);
                _logger.LogInformation("User registered successfully: {Email}", userModel.Email);

                return Ok(new { message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user: {Email}", userModel.Email);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel userLoginModel)
        {
            try
            {
                _logger.LogInformation("Login attempt for email: {Email}", userLoginModel.Email);
                var token = _userBL.ValidateUser(userLoginModel);

                if (token == null)
                {
                    _logger.LogWarning("Unauthorized login attempt for email: {Email}", userLoginModel.Email);
                    return Unauthorized(new { Error = "Unauthorized: Invalid email or password." });
                }

                _logger.LogInformation("User logged in successfully: {Email}", userLoginModel.Email);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login for email: {Email}", userLoginModel.Email);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetUserById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching user with ID: {Id}", id);
                var user = _userBL.GetUserById(id);

                if (user == null)
                {
                    _logger.LogWarning("User not found with ID: {Id}", id);
                    return NotFound(new { message = "User not found." });
                }

                _logger.LogInformation("User retrieved successfully with ID: {Id}", id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user with ID: {Id}", id);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                _logger.LogWarning("Forgot password request failed due to missing email.");
                return BadRequest("Email is required.");
            }

            try
            {
                _logger.LogInformation("Forgot password request initiated for email: {Email}", model.Email);
                _userBL.SendResetPasswordEmail(model.Email);
                _logger.LogInformation("Password reset email sent successfully for email: {Email}", model.Email);

                return Ok("Password reset email sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending password reset email for email: {Email}", model.Email);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
        {
            try
            {
                _logger.LogInformation("Password reset attempt for email: {Email}", model);
                _userBL.ResetPassword(model);
                _logger.LogInformation("Password reset successfully for email: {Email}", model);

                return Ok("Password reset successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during password reset for email: {Email}", model);
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
