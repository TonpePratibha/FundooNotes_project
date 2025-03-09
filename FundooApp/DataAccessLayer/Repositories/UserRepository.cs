
/*
using DataAccessLayer.DataContext;
using DataAccessLayer.Entities;
using DataAccessLayer.JWT;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.JWT;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace DataAccessLayer.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _config;
        public UserRepository(ApplicationDbContext context, JwtHelper jwtHelper,IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _passwordHasher = new PasswordHasher<User>();
            _jwtHelper = jwtHelper ?? throw new ArgumentNullException(nameof(jwtHelper));
            _config = configuration ?? throw new ArgumentNullException(nameof(_config));

        }
   

        public bool UserExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public void RegisterUser(UserModel userModel)
        {
            if (UserExists(userModel.Email))
                throw new Exception("User already exists with this email.");

            var user = new User
            {
                Name = userModel.Name,
                Email = userModel.Email,
                City = userModel.City,
                phone = userModel.phone
            };

            user.Password = _passwordHasher.HashPassword(user, userModel.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public string ValidateUser(UserLoginModel userLoginModel)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userLoginModel.Email);
            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, userLoginModel.Password);
            if (result != PasswordVerificationResult.Success) return null;

            return _jwtHelper.GenerateToken(user);
        }

        public UserModel GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return null;

            return new UserModel
            {
               Id=user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }


        // 1. Send Password Reset Email
        public void SendResetPasswordEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            // Generate token
            string token = _jwtHelper.GenerateResetToken(user);
            string resetLink = $"https://yourapp.com/reset-password?token={token}";

            SendEmail(user.Email, "Password Reset", $"Click here to reset your password: {resetLink}");
        }


      



        // 2. Reset Password Using Token
        public void ResetPassword(ResetPasswordModel model)
        {
            int userId = _jwtHelper.ExtractUserIdFromJwt(model.Token);

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new InvalidOperationException("Invalid token.");

            // Hash and update the password
            user.Password = _passwordHasher.HashPassword(user, model.NewPassword);
            _context.SaveChanges();
        }
        

        // Helper method to send emails
        
        public void SendEmail(string toEmail, string subject, string body)
        {
            var fromEmail = _config["EmailSettings:FromEmail"];
            var password = _config["EmailSettings:Password"];
            var smtpServer = _config["EmailSettings:SmtpServer"];
            var port = int.Parse(_config["EmailSettings:Port"]);

            using (var smtpClient = new SmtpClient(smtpServer, port))
            {
                smtpClient.Credentials = new NetworkCredential(fromEmail, password);
                smtpClient.EnableSsl = true; // Ensure SSL is enabled

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);

                smtpClient.Send(mailMessage);
            }
        }





    }
}
*/



using DataAccessLayer.DataContext;
using DataAccessLayer.Entities;
using DataAccessLayer.JWT;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _config;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ApplicationDbContext context, JwtHelper jwtHelper, IConfiguration configuration, ILogger<UserRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _passwordHasher = new PasswordHasher<User>();
            _jwtHelper = jwtHelper ?? throw new ArgumentNullException(nameof(jwtHelper));
            _config = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool UserExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public void RegisterUser(UserModel userModel)
        {
            if (UserExists(userModel.Email))
            {
                _logger.LogWarning("Registration failed. User already exists: {Email}", userModel.Email);
                throw new Exception("User already exists with this email.");
            }

            var user = new User
            {
                Name = userModel.Name,
                Email = userModel.Email,
                City = userModel.City,
                phone = userModel.phone
            };

            user.Password = _passwordHasher.HashPassword(user, userModel.Password);
            _context.Users.Add(user);
            _context.SaveChanges();

            _logger.LogInformation("User registered successfully: {Email}", userModel.Email);
        }

        public string ValidateUser(UserLoginModel userLoginModel)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userLoginModel.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed. User not found: {Email}", userLoginModel.Email);
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, userLoginModel.Password);
            if (result != PasswordVerificationResult.Success)
            {
                _logger.LogWarning("Invalid password for user: {Email}", userLoginModel.Email);
                return null;
            }

            _logger.LogInformation("User authenticated successfully: {Email}", userLoginModel.Email);
            return _jwtHelper.GenerateToken(user);
        }

        public UserModel GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {Id}", id);
                return null;
            }

            _logger.LogInformation("User retrieved successfully with ID: {Id}", id);
            return new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        public void SendResetPasswordEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                _logger.LogWarning("Password reset failed. User not found: {Email}", email);
                throw new InvalidOperationException("User not found.");
            }

            string token = _jwtHelper.GenerateResetToken(user);
            string resetLink = $"https://yourapp.com/reset-password?token={token}";

            SendEmail(user.Email, "Password Reset", $"Click here to reset your password: {resetLink}");
            _logger.LogInformation("Password reset email sent to: {Email}", email);
        }

        public void ResetPassword(ResetPasswordModel model)
        {
            int userId = _jwtHelper.ExtractUserIdFromJwt(model.Token);

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                _logger.LogWarning("Password reset failed. Invalid token.");
                throw new InvalidOperationException("Invalid token.");
            }

            user.Password = _passwordHasher.HashPassword(user, model.NewPassword);
            _context.SaveChanges();

            _logger.LogInformation("Password reset successfully for user with ID: {Id}", userId);
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var fromEmail = _config["EmailSettings:FromEmail"];
                var password = _config["EmailSettings:Password"];
                var smtpServer = _config["EmailSettings:SmtpServer"];
                var port = int.Parse(_config["EmailSettings:Port"]);

                using (var smtpClient = new SmtpClient(smtpServer, port))
                {
                    smtpClient.Credentials = new NetworkCredential(fromEmail, password);
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage(fromEmail, toEmail, subject, body) { IsBodyHtml = true };
                    smtpClient.Send(mailMessage);
                }

                _logger.LogInformation("Email sent to: {Email}", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to: {Email}", toEmail);
                throw;
            }
        }
    }
}
