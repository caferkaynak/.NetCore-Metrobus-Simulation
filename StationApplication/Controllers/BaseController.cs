using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StationApplication.Entity.Entities;
using StationApplication.Service;

namespace StationApplication.Web.Controllers
{
    public class BaseController : Controller
    {
        private ISmartTicketTypeService _SmartTicketTypeService;
        private ISmartTicketService _SmartTicketService;
        private IStationSmartTicketService _StationSmartTicketService;
        private ILoggerFactory _DepLoggerFactory; 
        private ILogger _Logger;
        private IRepostService _ReportService;
        private IUserService _UserService;
        private SignInManager<User> _SingInManager;
        private UserManager<User> _UserManager;
        public ISmartTicketTypeService SmartTicketTypeService => _SmartTicketTypeService ?? (_SmartTicketTypeService = HttpContext?.RequestServices.GetService<ISmartTicketTypeService>());
        public ISmartTicketService SmartTicketService => _SmartTicketService ?? (_SmartTicketService = HttpContext?.RequestServices.GetService<ISmartTicketService>());
        public IStationSmartTicketService StationSmartTicketService => _StationSmartTicketService ?? (_StationSmartTicketService = HttpContext?.RequestServices.GetService<IStationSmartTicketService>());
        public ILoggerFactory DepLoggerFactory => _DepLoggerFactory ?? (_DepLoggerFactory = HttpContext?.RequestServices.GetService<ILoggerFactory>());
        public IRepostService ReportService => _ReportService ?? (_ReportService = HttpContext?.RequestServices.GetService<IRepostService>());  
        public ILogger Logger => _Logger ?? (_Logger = DepLoggerFactory.CreateLogger(""));
        public IUserService UserService => _UserService ?? (_UserService = HttpContext?.RequestServices.GetService<IUserService>());
        public SignInManager<User> SingInManager => _SingInManager ?? (_SingInManager = HttpContext?.RequestServices.GetService<SignInManager<User>>());
        public UserManager<User> UserManager => _UserManager ?? (_UserManager = HttpContext?.RequestServices.GetService<UserManager<User>>());

    }
}