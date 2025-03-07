using DataAccessLayer.Entities;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
   public interface IUserRepository
    {

        bool UserExists(string email);
        void RegisterUser(UserModel userModel);
        string ValidateUser(UserLoginModel userLoginModel);
        public UserModel GetUserById(int id);
        public void ResetPassword(ResetPasswordModel model);
        public void SendResetPasswordEmail(string email);
    }
}
