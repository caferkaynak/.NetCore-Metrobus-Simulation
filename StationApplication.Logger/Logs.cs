using StationApplication.Common;
using StationApplication.Data;
using StationApplication.Entity.Entities;
using System;
using System.Linq;

namespace StationApplication.Logger
{
    public class Logs
    {
        private IRepository<StationSmartTicket> _StationSmartTicketRepository;
        public Logs(IRepository<StationSmartTicket> StationSmartTicketRepository)
        {
            _StationSmartTicketRepository = StationSmartTicketRepository;
        }
        public LogsView GetAll()
        {
            LogsView logsView = new LogsView();
            logsView.StationSmartTickets = _StationSmartTicketRepository.GetAll().ToList();
            return logsView;
        }
    }
}
