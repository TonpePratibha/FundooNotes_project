
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

        public UserController(IuserBL userBL)
        {
            _userBL = userBL;
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
        /*

                [HttpPost("forgot-password")]
                public IActionResult ForgotPassword([FromBody] string email)
                {
                    try
                    {
                        _userBL.SendResetPasswordEmail(email);
                        return Ok("Password reset link sent to your email.");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
                */

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
