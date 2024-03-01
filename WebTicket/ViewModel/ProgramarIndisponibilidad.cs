using System.ComponentModel.DataAnnotations.Schema;

namespace WebTicket.ViewModel
{
    [Table("ProgramarIndisponibilidad", Schema = "UAP")]
    public class ProgramarIndisponibilidad
    {
        public int? IdProgramarIndisponibilidad { get; set; }
        public int? IdEscritorio { get; set; }

        public int? IdAccionPersonal { get; set; }

        public string? CodigoUsuario { get; set; }
        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public string? Motivo { get; set; }

    }
}
