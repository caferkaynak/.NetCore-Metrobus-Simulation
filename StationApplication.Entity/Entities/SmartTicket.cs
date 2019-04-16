using System.ComponentModel.DataAnnotations;

namespace StationApplication.Entity.Entities
{
    public class SmartTicket:BaseEntity<int>
    {
        public double Arrears { get; set; }
        public bool Status { get; set; }
        [Required]
        public string UniqueCode { get; set; }
        public int SmartTicketTypeId { get; set; }
        public virtual SmartTicketType SmartTicketType { get; set; }
    }
}
