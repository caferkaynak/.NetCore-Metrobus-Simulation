using StationApplication.Entity.Entities;
using System.Collections.Generic;

namespace StationApplication.Common.SmartTicketTypeView
{
    public class SmartTicketTypeView
    {
        public SmartTicketType SmartTicketType { get; set; }
        public List<SmartTicketType> SmartTicketTypes { get; set; }
    }
}
