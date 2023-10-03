using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTicket.ViewModel
{
    [Table("ControlTicket", Schema = "UAP")]
    public class ControlTicket
    {
        [Key]
        public int? IdControlTicket { get; set; }
        public int? IdTipoFila { get; set; }
        public string? CodigoPagaduria { get; set; }
        public string? CodigoUnidades { get; set; }
        public int? Correlativo { get; set; }
        public string? CodigoUsuario { get; set; }
        public DateTime? FechaSecuencia { get; set; }
    }
}
