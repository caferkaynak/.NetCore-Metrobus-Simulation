using Microsoft.AspNetCore.Identity;
using StationApplication.Common;
using StationApplication.Common.UserView;
using StationApplication.Entity.Entities;
using System.Threading.Tasks;

namespace StationApplication.Service
{
    public interface IUserService
    {
        Task<ServiceResult> Login(UserView model);
        void LogOut();
    }
    public class UserService : IUserService
    {
        private UserManager<User> _UserManager;
        private SignInManager<User> _SingInManager;
        public UserService(SignInManager<User> SingInManager, UserManager<User> userManager)
        {
            _SingInManager = SingInManager;
            _UserManager = userManager;
        }
        public async Task<ServiceResult> Login(UserView model)
        {
            ServiceResult serviceResult = new ServiceResult();
            var user = await _UserManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result = await _SingInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    serviceResult.Result = true;
                }
            }
            return serviceResult;
        }
        public void LogOut()
        {
            _SingInManager.SignOutAsync();
        }
    }
}
