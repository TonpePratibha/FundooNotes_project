/*
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DataAccessLayer.Models;

namespace BuisnessLayer
{
   public class UserBL:IuserBL
    {
        private readonly IUserRepository _userRepository;

       

        public UserBL(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void RegisterUser(UserModel userModel)
        {
            _userRepository.RegisterUser(userModel);
        }

        public string ValidateUser(UserLoginModel userLoginModel)
        {
            return _userRepository.ValidateUser(userLoginModel);
        }
        public UserModel GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }


        public void SendResetPasswordEmail(string email)
        {
            _userRepository.SendResetPasswordEmail(email);
        }

        // Call the repository method to reset the user's password
        public void ResetPassword(ResetPasswordModel model)
        {
            _userRepository.ResetPassword(model);
        }



    }
}
      
*/



using BuisnessLayer;
using DataAccessLayer.Entities;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.Logging;

namespace BuisnessLayer
{
    public class UserBL : IuserBL
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserBL> _logger;

        public UserBL(IUserRepository userRepository, ILogger<UserBL> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public void RegisterUser(UserModel userModel)
        {
            try
            {
                _logger.LogInformation("Registering user: {Email}", userModel.Email);
                _userRepository.RegisterUser(userModel);
                _logger.LogInformation("User registered successfully: {Email}", userModel.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user: {Email}", userModel.Email);
                throw;
            }
        }

        public string ValidateUser(UserLoginModel userLoginModel)
        {
            try
            {
                _logger.LogInformation("Validating user: {Email}", userLoginModel.Email);
                var token = _userRepository.ValidateUser(userLoginModel);

                if (token == null)
                {
                    _logger.LogWarning("Invalid credentials for user: {Email}", userLoginModel.Email);
                }
                else
                {
                    _logger.LogInformation("User validated successfully: {Email}", userLoginModel.Email);
                }

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while validating user: {Email}", userLoginModel.Email);
                throw;
            }
        }

        public UserModel GetUserById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching user with ID: {Id}", id);
                var user = _userRepository.GetUserById(id);

                if (user == null)
                {
                    _logger.LogWarning("User not found with ID: {Id}", id);
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user with ID: {Id}", id);
                throw;
            }
        }

        public void SendResetPasswordEmail(string email)
        {
            try
            {
                _logger.LogInformation("Sending password reset email to: {Email}", email);
                _userRepository.SendResetPasswordEmail(email);
                _logger.LogInformation("Password reset email sent successfully to: {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending reset password email to: {Email}", email);
                throw;
            }
        }

        public void ResetPassword(ResetPasswordModel model)
        {
            try
            {
                _logger.LogInformation("Resetting password for user with token.");
                _userRepository.ResetPassword(model);
                _logger.LogInformation("Password reset successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during password reset.");
                throw;
            }
        }
    }
}
