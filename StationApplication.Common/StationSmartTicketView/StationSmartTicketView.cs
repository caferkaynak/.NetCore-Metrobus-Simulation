using StationApplication.Entity.Entities;
using System.Collections.Generic;

namespace StationApplication.Common.StationSmartTicketView
{
    public class StationSmartTicketView
    {
        public List<Station> Stations { get; set; }
        public Station Station { get; set; }
        public SmartTicket SmartTicket { get; set; }
        public StationSmartTicket StationSmartTicket { get; set; }
        public ServiceResult ServiceResult { get; set; }
    }
}
