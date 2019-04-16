using Microsoft.EntityFrameworkCore;
using StationApplication.Common.StationSmartTicketView;
using StationApplication.Data;
using StationApplication.Entity.Entities;
using System.Linq;
using StationApplication.Common;
using System;

namespace StationApplication.Service
{
    public interface IStationSmartTicketService
    {
        StationSmartTicketView List(SmartTicket smartTicket);
        void Add(StationSmartTicketView stationSmartTicketView);
        void Update(StationSmartTicketView stationSmartTicketView);
    }
    public class StationSmartTicketService : IStationSmartTicketService
    {
        private readonly AppSettings _AppSettings;
        private IRepository<StationSmartTicket> _StationSmartTicketRepository;
        private IRepository<Station> _StationRepository;
        private IRepository<SmartTicket> _SmartTicketRepository;
        private ServiceResult serviceResult = new ServiceResult();
        public StationSmartTicketService(IRepository<StationSmartTicket> StationSmartTicketRepository,
            IRepository<Station> StationRepository,
            IRepository<SmartTicket> SmartTicketRepository,
            AppSettings AppSettings)
        {
            _StationSmartTicketRepository = StationSmartTicketRepository;
            _StationRepository = StationRepository;
            _SmartTicketRepository = SmartTicketRepository;
            _AppSettings = AppSettings;
        }
        public StationSmartTicketView List(SmartTicket smartTicket)
        {
            StationSmartTicketView stationSmartTicketView = new StationSmartTicketView();
            stationSmartTicketView.Stations = _StationRepository.GetAll().OrderBy(o => o.StartDistance).ToList();
            stationSmartTicketView.SmartTicket = _SmartTicketRepository.GetAll().Where(w => w.Id == smartTicket.Id).Include(i => i.SmartTicketType).FirstOrDefault();
            return stationSmartTicketView;
        }
        public void Add(StationSmartTicketView stationSmartTicketView)
        {
            StationSmartTicket stationSmartTicket = new StationSmartTicket();
            stationSmartTicket.SmartTicketId = stationSmartTicketView.SmartTicket.Id;
            stationSmartTicket.StartStationId = stationSmartTicketView.Station.Id;
            stationSmartTicket.StartTime = DateTime.Now;
            stationSmartTicket.Pay = stationSmartTicketView.ServiceResult.Pay;
            _StationSmartTicketRepository.Add(stationSmartTicket);
        }
        public void Update(StationSmartTicketView stationSmartTicketView)
        {
            var result = _StationSmartTicketRepository.GetAll().Where(w => w.Id == stationSmartTicketView.ServiceResult.Id).FirstOrDefault();
            if (result != null)
            {
                result.FinishStationId = stationSmartTicketView.Station.Id;
                result.FinishTime = DateTime.Now;
                result.Rebate = stationSmartTicketView.ServiceResult.Refund;
                _StationSmartTicketRepository.Update(result);
            }
        }
    }
}
