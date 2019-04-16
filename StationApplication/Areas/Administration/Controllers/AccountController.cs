using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StationApplication.Common;
using StationApplication.Common.UserView;
using StationApplication.Web.Controllers;
using Microsoft.Extensions.Logging;

namespace StationApplication.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class AccountController : BaseController
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated == true)
                return RedirectToAction("Index", "Report");
            return RedirectToAction("Login", "Account");
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated == true)
                return RedirectToAction("Index", "Report");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserView user)
        {
            if (ModelState.IsValid)
            {
                ServiceResult serviceResult = new ServiceResult();
                serviceResult = await UserService.Login(user);
                if (serviceResult.Result == true)
                {
                    Logger.LogError(user.UserName + " : Yönetici girişi yaptı.");
                    return RedirectToAction("Index", "Report");
                }
                else
                {
                    Logger.LogError(user.UserName + " : Yönetici girişi denedi başarısız oldu.");
                    ModelState.AddModelError("", "Kullanıcı Adı veya Şifre Yanlış");
                }
            }
            return View();
        }
        [Authorize]
        public IActionResult LogOut()
        {
            Logger.LogError(User.Identity.Name + " : Yönetici panelinden çıkış yaptı.");
            UserService.LogOut();
            return RedirectToAction("Index", "Account");
        }
    }
}