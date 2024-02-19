using System.ComponentModel.DataAnnotations.Schema;

namespace WebTicket.ViewModel
{
    [Table("DetalleTicketAsesoria", Schema = "UAP")]
    public class DetalleTicketAsesoria
    {
      
            public int? IdDetalleTicketAsesoria { get; set; }
            public int? IdRegistro { get; set; }
            public int? IdLlamadaTicket { get; set; }
            public string? CodigoUnidad { get; set; }
            public int? IdOrden { get; set; }
            public string? MotivoAsistencia { get; set; }
             public string? NumeroTicket { get; set; }
    }
}
