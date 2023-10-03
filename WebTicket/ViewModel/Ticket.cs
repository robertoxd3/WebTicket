using System.ComponentModel.DataAnnotations.Schema;

namespace WebTicket.ViewModel
{
    [Table("Ticket", Schema = "UAP")]
    public class Ticket
    {
        public int? IdTicket { get; set; }
        public string? NumeroTicket { get; set; }
        public string? NumeroDui { get; set;}
        public string? Telefono { get; set;}
        public DateTime? FechaTicket { get; set;}
        public int? IdControlTicket { get; set;}
    }
}
