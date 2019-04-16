using StationApplication.Entity.Entities;
using System;
using System.Collections.Generic;

namespace StationApplication.Common.ReportView
{
    public class ReportView
    {
        public float RebateForRate_Rate { get; set; }
        public float RebateForRate_RebateCount { get; set; }
        public float RebateForRate_Count { get; set; }
        public float RebateForRate_StudentCount { get; set; }
        public float RebateForRate_StandartCount { get; set; }
        public byte DataList_Take { get; set; }
        public List<StationSmartTicket> StationSmartTicket {get;set;}
        public List<MaxStationView> MaxStationStart { get; set; }
        public List<MaxStationView> MaxStationRebate { get; set; }
        public List<StationSmartTicketList> StationSmartTicketLists { get; set; }
        public DateTime FirstTime { get; set; }
        public DateTime LastTime { get; set; }
        public List<TrafficTimeView> TrafficTimeViews { get; set; }
    }
}
