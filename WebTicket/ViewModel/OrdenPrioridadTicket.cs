using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTicket.ViewModel
{
    [Table("OrdenPrioridadTicket", Schema = "UAP")]
    public class OrdenPrioridadTicket
    {
        [Key]
            public int IdOrden { get; set; }
            public int IdTiket { get; set; }
            [Required]
            [StringLength(25)]
            public string NumeroTicket { get; set; }
            public int Orden { get; set; }
            public int IdControlTiket { get; set; }
            [Required]
            [StringLength(1)]
            public string Espera { get; set; }
            [Required]
            [StringLength(20)]
            public string CodigoUnidades { get; set; }
        
    }
}
