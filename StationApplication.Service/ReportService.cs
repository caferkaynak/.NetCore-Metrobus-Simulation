using Microsoft.EntityFrameworkCore;
using StationApplication.Common.ReportView;
using StationApplication.Data;
using StationApplication.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StationApplication.Service
{
    public interface IRepostService
    {

        ReportView RebateForRate(DateTime firstTime, DateTime lastTime);
        ReportView TrafficTime(DateTime firstTime, DateTime lastTime);
        ReportView MaxStation(DateTime firstTime, DateTime lastTime);
        ReportView DataList(DateTime firstTime, DateTime lastTime, int take);
        ReportView ExportExcel(DateTime firstTime, DateTime lastTime, int take);
    }
    public class ReportService : IRepostService
    {
        private IRepository<StationSmartTicket> _StationSmartTicket;
        ReportView reportView = new ReportView();
        private List<MaxStationView> maxStationStartView = new List<MaxStationView>();
        private List<MaxStationView> maxStationRebateView = new List<MaxStationView>();
        private List<TrafficTimeView> trafficTimeViews = new List<TrafficTimeView>();
        List<StationSmartTicketList> stationSmartTicketLists = new List<StationSmartTicketList>();
        private List<StationSmartTicket> stationSmartTickets = new List<StationSmartTicket>();
        public ReportService(IRepository<StationSmartTicket> StationSmartTicket)
        {
            _StationSmartTicket = StationSmartTicket;
        }
        public ReportView RebateForRate(DateTime firstTime, DateTime lastTime)
        {
            float count = _StationSmartTicket.GetAll().Where(w => w.StartTime > firstTime && w.StartTime < lastTime).Count();
            float rebateCount = _StationSmartTicket.GetAll()
                .Where(w => w.FinishStationId == null &&
                w.StartTime > firstTime && w.StartTime < lastTime)
                .Count();
            float student = _StationSmartTicket.GetAll().Where(w =>w.SmartTicket.SmartTicketType.Name=="Ogrenci" && w.StartTime > firstTime && w.StartTime < lastTime).Count();
            reportView.RebateForRate_Rate = 100 / (count-student) * rebateCount;
            reportView.RebateForRate_Count = count;
            reportView.RebateForRate_RebateCount = rebateCount;
            reportView.RebateForRate_StandartCount = count - student;
            reportView.RebateForRate_StudentCount = student;
            return reportView;
        }
        
        public ReportView MaxStation(DateTime firstTime, DateTime lastTime)
        {

            var startStation = _StationSmartTicket.GetAll().Where(w => w.StartTime > firstTime && w.StartTime < lastTime).GroupBy(g => g.StartStationId).Where(w => w.Count() > 1)
                .Select(s =>
                new
                {
                    s.FirstOrDefault().StartStation.Name,
                    sum = s.Count()
                }).Distinct().Take(5).OrderByDescending(o => o.sum).ToList();
            var rebateStation = _StationSmartTicket.GetAll().Where(w => w.FinishStationId != null && w.StartTime > firstTime && w.StartTime < lastTime).GroupBy(g => g.FinishStationId).Where(w => w.Count() > 1)
                .Select(s =>
                new
                {
                    s.FirstOrDefault().FinishStation.Name,
                    sum = s.Count()
                }).Distinct().Take(5).OrderByDescending(o => o.sum).ToList();

            foreach (var item in startStation)
            {
                maxStationStartView.Add(
                    new MaxStationView() { StartStation = item.Name, StartStationSum = item.sum }
                    );
            }
            foreach (var item in rebateStation)
            {
                maxStationRebateView.Add(
                    new MaxStationView() { RebateStation = item.Name, RebateStationSum = item.sum }
                    );
            }
            reportView.MaxStationStart = maxStationStartView;
            reportView.MaxStationRebate = maxStationRebateView;
            return reportView;
        }
        public ReportView TrafficTime(DateTime firstTime, DateTime lastTime)
        {
            var time = _StationSmartTicket.GetAll().Where(w => w.StartTime > firstTime && w.StartTime < lastTime)
                .GroupBy(g => g.StartTime.Hour).Where(w => w.Count() > 1)
              .Select(s =>
              new
              {
                  s.FirstOrDefault().StartTime.Hour,
                  sum = s.Count()
              }).Distinct().Take(5).OrderByDescending(o => o.sum).ToList();
            foreach (var item in time)
            {
                trafficTimeViews.Add(
                    new TrafficTimeView() { Hour = item.Hour, Count = item.sum ,HourRange = item.Hour+1}
                    );
            }
            reportView.TrafficTimeViews = trafficTimeViews;
            
            return reportView;
        }
        public ReportView DataList(DateTime firstTime, DateTime lastTime,int take)
        {
            stationSmartTickets = _StationSmartTicket.GetAll().OrderByDescending(o => o.Id)
                .Where(w => w.StartTime > firstTime && w.StartTime < lastTime)
                .Take(take)
                .Include(i => i.StartStation)
                .Include(i=> i.FinishStation)
                .Include(i => i.SmartTicket.SmartTicketType)
                .ToList();
            reportView.StationSmartTicket = stationSmartTickets;
            return reportView;
        }
        public ReportView ExportExcel(DateTime firstTime, DateTime lastTime, int take)
        {
            stationSmartTickets = _StationSmartTicket.GetAll().OrderByDescending(o => o.Id)
                .Where(w => w.StartTime > firstTime && w.StartTime < lastTime)
                .Take(take)
                .Include(i => i.StartStation)
                .Include(i => i.FinishStation)
                .Include(i => i.SmartTicket.SmartTicketType)
                .ToList();
            foreach (var item in stationSmartTickets)
            {
                if (item.FinishStationId == null)
                {
                    stationSmartTicketLists.Add(
                 new StationSmartTicketList()
                 {
                     Id = item.Id,
                     StartStationId = item.StartStationId,
                     StartStationName = item.StartStation.Name,
                     FinishStationId = item.FinishStationId,
                     StartTime = item.StartTime,
                     FinishTime = item.FinishTime,
                     SmartTicketId = item.SmartTicketId,
                     SmartTicketType = item.SmartTicket.SmartTicketType.Name,
                     SmartTicketArrear = item.SmartTicket.Arrears,
                     Pay = item.Pay,
                     Rebate = item.Rebate,
                 });
                }
                else
                {
                    stationSmartTicketLists.Add(
                        new StationSmartTicketList()
                        {
                            Id = item.Id,
                            StartStationId = item.StartStationId,
                            StartStationName = item.StartStation.Name,
                            FinishStationId = item.FinishStationId,
                            FinishStationName = item.FinishStation.Name,
                            StartTime = item.StartTime,
                            FinishTime = item.FinishTime,
                            SmartTicketId = item.SmartTicketId,
                            SmartTicketType = item.SmartTicket.SmartTicketType.Name,
                            SmartTicketArrear = item.SmartTicket.Arrears,
                            Pay = item.Pay,
                            Rebate = item.Rebate,
                        });
                }
            }
            reportView.StationSmartTicketLists = stationSmartTicketLists;
            return reportView;
        }

    }
}
