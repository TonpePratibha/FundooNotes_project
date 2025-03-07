
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
      
