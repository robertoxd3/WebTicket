using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebTicket.ViewModel
{
    [Table("LlamadaTicket", Schema = "UAP")]
    public class LlamadaTicket
    {
            [Key]
            public int? IdLlamadaTicket { get; set; }

         
            public int? IdEscritorio { get; set; }

 
            public int? IdOrden { get; set; }

    
            [MaxLength(25)]
            public string? NumeroTicket { get; set; }

      
            [MaxLength(1)]
            public string? Estado { get; set; }

            public DateTime? FechaLlamada { get; set; }
            public DateTime? FechaFinalizacion { get; set; }
            public string? CodigoUsuario { get; set; }

    }
}
