using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebTicket.ViewModel
{
    [Table("LlamadaTicket", Schema = "UAP")]
    public class LlamadaTicket
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int IdLlamadaTicket { get; set; }

            [Required]
            public int IdEscritorio { get; set; }

            [Required]
            public int IdOrden { get; set; }

            [Required]
            [MaxLength(25)]
            public string NumeroTicket { get; set; }

            [Required]
            [MaxLength(1)]
            public string Estado { get; set; }
        
    }
}
