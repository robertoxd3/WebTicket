using System.ComponentModel.DataAnnotations.Schema;

namespace WebTicket.ViewModel
{
    [Table("Escritorio", Schema = "UAP")]
    public class Escritorio
    {
        public int? IdEscritorio { get; set; }
        public string? CodigoUnidad { get; set; }

        public string? CodigoPagaduria { get; set; }

        public string? NoEscritorio { get; set; }

        public string? Disponibilidad { get; set; }

        public string? CodigoUsuario { get; set; }
    }
}
