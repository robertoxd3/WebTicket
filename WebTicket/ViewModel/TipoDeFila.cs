using System.ComponentModel.DataAnnotations.Schema;

namespace WebTicket.ViewModel
{
    [Table("TipoDeFila", Schema = "UAP")]
    public class TipoDeFila
    {
        public int? IdFila { get; set; }
        public string? NombreFila { get; set; }
        public string? Letra { get; set; }
        public int? Peso { get; set; }
        public bool? Activo { get; set; }
    }
}
