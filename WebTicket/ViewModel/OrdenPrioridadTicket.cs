using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTicket.ViewModel
{
    [Table("OrdenPrioridadTicket", Schema = "UAP")]
    public class OrdenPrioridadTicket
    {
            public int? IdOrden { get; set; }
            public int? IdTiket { get; set; }
      
            public string? NumeroTicket { get; set; }
            public int? Orden { get; set; }
            public int? IdControlTiket { get; set; }
  
            public string? Espera { get; set; }

            public string? CodigoUnidades { get; set; }
            public string? Redirigir { get; set; }
            public string? CodigoUnidadRedirigir { get; set; }

    }
}
