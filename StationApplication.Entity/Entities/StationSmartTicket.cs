using System;

namespace StationApplication.Entity.Entities
{
    public class StationSmartTicket :BaseEntity<int>
    {
        public int StartStationId { get; set; }
        public virtual Station StartStation { get; set; }
        public int? FinishStationId { get; set; }
        public virtual Station FinishStation { get; set; }
        public int SmartTicketId { get; set; }
        public virtual SmartTicket SmartTicket { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public double Pay { get; set; }
        public double? Rebate { get; set; }
    }
}
