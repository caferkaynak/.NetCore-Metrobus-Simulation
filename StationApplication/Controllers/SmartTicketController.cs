using Microsoft.AspNetCore.Mvc;
using StationApplication.Common;
using StationApplication.Common.SmartTicketTypeView;
using StationApplication.Common.StationSmartTicketView;
using StationApplication.Entity.Entities;
using Microsoft.Extensions.Logging;

namespace StationApplication.Web.Controllers
{
    public class SmartTicketController : BaseController
    {
        public IActionResult Index() => View(SmartTicketTypeService.SmartTicketTypeList());

        public IActionResult SmartTicketAdd(ServiceResult serviceResult)
        {
            if (serviceResult.Result==true)
                return View(serviceResult);
            else
                return RedirectToAction("Index", "SmartTicket");
        }
        [HttpPost]
        public IActionResult SmartTicketAdd(SmartTicketTypeView smartTicketTypeView)
        {
            ServiceResult serviceResult = new ServiceResult();
            if (ModelState.IsValid)
            {
                SmartTicket smartTicket = new SmartTicket
                {
                    SmartTicketTypeId = smartTicketTypeView.SmartTicketType.Id,
                    Arrears = 0
                };
                serviceResult = SmartTicketService.Add(smartTicket);
                Logger.LogError("New Smart Ticket, Id : " + serviceResult.Id.ToString());
                return View(serviceResult);
            }
            return RedirectToAction("Index","SmartTicket");
        }
        public IActionResult SingInId()
        {   
            return View();
        }
        [HttpPost]
        public IActionResult SingInId(SmartTicket smartTicket)
        {
            smartTicket = SmartTicketService.List(smartTicket);
            if (smartTicket != null)
            {
                Logger.LogError("Sing In, Smart Ticket Id : " + smartTicket.Id);
                return RedirectToAction("Index", "StationSmartTicket", smartTicket);
            }
            else
            {
                Logger.LogError("Smart Ticket Id Null");
                ModelState.AddModelError("Error", "Kimlik Doğrulama Başarısız.");
                return View();
            }
        }
        public IActionResult UpdateArrear(StationSmartTicketView stationSmartTicketView)
        {
            return View(stationSmartTicketView.SmartTicket);
        }
        [HttpPost]
        public IActionResult UpdateArrear(SmartTicket smartTicket)
        {
            if (smartTicket.Arrears>0)
            {
                SmartTicketService.Update(smartTicket);
                Logger.LogError("Smart Ticket Id : " + smartTicket.Id + " Yükleme Tutarı : " + smartTicket.Arrears);
            }
            else
            {
                Logger.LogError("Smart Ticket Id : " + smartTicket.Id + " Yükleme Başarısız");
            }
            return RedirectToAction("Index", "StationSmartTicket", smartTicket);
        }
    }
}