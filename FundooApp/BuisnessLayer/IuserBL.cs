using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer
{
    public interface IuserBL
    {
        void RegisterUser(UserModel userModel);
        string ValidateUser(UserLoginModel userLoginModel);
        public UserModel GetUserById(int id);
        public void SendResetPasswordEmail(string email);
        public void ResetPassword(ResetPasswordModel model);
    }
}
