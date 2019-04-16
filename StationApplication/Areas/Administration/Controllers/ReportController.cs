using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StationApplication.Common.ReportView;
using StationApplication.Web.Controllers;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace StationApplication.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize]
    public class ReportController : BaseController
    {
        public IActionResult Index(ReportView reportView)
        {
            DateTime newdateTime = DateTime.Now.AddDays(-30);
            DateTime now = DateTime.Now;
            DateTime firstTime;
            DateTime lastTime;
            if (reportView.FirstTime == default(DateTime) || reportView.LastTime == default(DateTime))
            {
                firstTime = newdateTime;
                lastTime = now;
            }
            else
            {
                firstTime = reportView.FirstTime;
                lastTime = reportView.LastTime;
            }
            reportView = ReportService.TrafficTime(firstTime, lastTime);
            reportView = ReportService.RebateForRate(firstTime, lastTime);
            reportView = ReportService.MaxStation(firstTime, lastTime);
            reportView.FirstTime = firstTime;
            reportView.LastTime = lastTime;
            TempData["Time"] = firstTime.ToString() + " - " + lastTime.ToString();
            Logger.LogError("Kullanıcı : " + User.Identity.Name + " Rapor Filtrelendi : Filtre Tarihi :" + firstTime + " - " + lastTime);
            return View(reportView);
        }
        [HttpPost]
        public IActionResult Filter(ReportView reportView)
        {
            if (reportView.FirstTime > reportView.LastTime)
            {
                Logger.LogError("Kullanıcı : " + User.Identity.Name + "Filtreleme sırasında hata: FirstTime > LastTime; Report/Index'e yönlendirildi");
                return RedirectToAction("Index", "Report");

            }
            return RedirectToAction("Index", "Report", reportView);
        }
        public IActionResult DataList(ReportView reportView)
        {
            DateTime newdateTime = DateTime.Now.AddDays(-30);
            DateTime now = DateTime.Now;
            DateTime firstTime;
            DateTime lastTime;
            int take;
            if (reportView.FirstTime == default(DateTime)
                || reportView.LastTime == default(DateTime)
                || reportView.DataList_Take == default(int))
            {
                firstTime = newdateTime;
                lastTime = now;
                take = 10;
            }
            else
            {
                firstTime = reportView.FirstTime;
                lastTime = reportView.LastTime;
                take = reportView.DataList_Take;
            }
            reportView.FirstTime = firstTime;
            reportView.LastTime = lastTime;
            reportView = ReportService.DataList(firstTime, lastTime, take);
            TempData["DataListTime"] = firstTime.ToString() + " - " + lastTime.ToString();
            Logger.LogError("Kullanıcı : " + User.Identity.Name + "Veri Filtrelendi : Filtre Tarihi :" + firstTime + " - " + lastTime + " Listelenen veri sayısı : " + take);
            return View(reportView);
        }
        [HttpPost]
        public IActionResult DataListPost(ReportView reportView)
        {
            if (reportView.FirstTime > reportView.LastTime)
            {
                Logger.LogError("Kullanıcı : " + User.Identity.Name + "Filtreleme sırasında hata: FirstTime > LastTime; Report/DataList'e yönlendirildi");
                return RedirectToAction("DataList", "Report");
            }
            return RedirectToAction("DataList", "Report", reportView);
        }
        [HttpPost]
        public async Task<IActionResult> ExportExcel(CancellationToken cancellationToken, ReportView reportView)
        {
            DateTime newdateTime = DateTime.Now.AddDays(-30);
            DateTime now = DateTime.Now;
            DateTime firstTime;
            DateTime lastTime;
            int take;
            if (reportView.FirstTime == default(DateTime)
               || reportView.LastTime == default(DateTime)
               || reportView.DataList_Take == default(int))
            {
                firstTime = newdateTime;
                lastTime = now;
                take = 10;
            }
            else
            {
                firstTime = reportView.FirstTime;
                lastTime = reportView.LastTime;
                take = reportView.DataList_Take;
            }
            reportView.FirstTime = firstTime;
            reportView.LastTime = lastTime;
            reportView = ReportService.ExportExcel(firstTime, lastTime, take);
            await Task.Yield();
            var stream = new MemoryStream();
            using (var package = new OfficeOpenXml.ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("ExportExcel");
                workSheet.Cells.LoadFromCollection(reportView.StationSmartTicketLists, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"DataList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            Logger.LogError("Kullanıcı : " + User.Identity.Name + "Veri Excele Aktarıldı : Filtre Tarihi :" + firstTime + " - " + lastTime + " Listelenen veri sayısı : " + take);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}