using System;

namespace StationApplication.Common.ReportView
{
    public class StationSmartTicketList
    {
        public int Id { get; set; }
        public int StartStationId { get; set; }
        public string StartStationName { get; set; }
        public int? FinishStationId { get; set; }
        public string FinishStationName { get; set; }
        public int SmartTicketId { get; set; }
        public string SmartTicketType { get; set; }
        public double SmartTicketArrear { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public double Pay { get; set; }
        public double? Rebate { get; set; }
    }
}
