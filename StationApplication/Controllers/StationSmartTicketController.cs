using Microsoft.AspNetCore.Mvc;
using StationApplication.Common.StationSmartTicketView;
using StationApplication.Entity.Entities;
using Microsoft.Extensions.Logging;

namespace StationApplication.Web.Controllers
{
    public class StationSmartTicketController : BaseController
    {
        public IActionResult Index(SmartTicket smartTicket)
        {
            if (smartTicket.Id != 0 && smartTicket.UniqueCode != null)
            {
                StationSmartTicketView stationSmartTicketView = new StationSmartTicketView();
                stationSmartTicketView = StationSmartTicketService.List(smartTicket);
                if (TempData["Message"] != null)
                {
                    Logger.LogError("Smart Ticket Id: "+smartTicket.Id +" : "+ TempData["Message"].ToString());
                    ModelState.AddModelError("Error", TempData["Message"].ToString());
                }
                return View(stationSmartTicketView);
            }
            else
            {
                Logger.LogError("Smart Ticket Id: " + smartTicket.Id + " : Kimlik Doğrulama Başarısız");
                return RedirectToAction("SingInId", "SmartTicket");
            }
        }
        [HttpPost]
        public IActionResult StartStation(StationSmartTicketView stationSmartTicketView)
        {
            if (ModelState.IsValid)
            {
                stationSmartTicketView.ServiceResult = SmartTicketService.UpdateForTrip(stationSmartTicketView);
                TempData["Message"] = stationSmartTicketView.ServiceResult.Message;
                if (stationSmartTicketView.ServiceResult.Result == true)
                {
                    Logger.LogError("SmartTicket Id : " + stationSmartTicketView.SmartTicket.Id + " Biniş Yapılan Durak Id :" + stationSmartTicketView.Station.Id);
                    StationSmartTicketService.Add(stationSmartTicketView);
                }
                    
            }
            else
                TempData["Message"] = "Lütfen Durak Seçiniz";
            return RedirectToAction("Index", "StationSmartTicket", stationSmartTicketView.SmartTicket);
        }
        public IActionResult FinishStation(StationSmartTicketView stationSmartTicketView)
        {
            if (ModelState.IsValid)
            {
                stationSmartTicketView.ServiceResult = SmartTicketService.UpdateForRefund(stationSmartTicketView);
                    TempData["Message"] = stationSmartTicketView.ServiceResult.Message;
                if (stationSmartTicketView.ServiceResult.Result == true)
                {
                    Logger.LogError("SmartTicket Id :" + stationSmartTicketView.ServiceResult.Id + "İade Alınan Durak Id :" + stationSmartTicketView.Station.Id);
                    StationSmartTicketService.Update(stationSmartTicketView);
                }
                    
            }
            else
                TempData["Message"] = "Lütfen Durak Seçiniz";
            return RedirectToAction("Index", "StationSmartTicket", stationSmartTicketView.SmartTicket);
        }
    }
}