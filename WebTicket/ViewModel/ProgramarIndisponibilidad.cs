using System.ComponentModel.DataAnnotations.Schema;

namespace WebTicket.ViewModel
{
    [Table("ProgramarIndisponibilidad", Schema = "UAP")]
    public class ProgramarIndisponibilidad
    {
        public string? IdProgramarIndiponibilidad { get; set; }
        public string? IdEscritorio { get; set; }
        public DateTime? FechaInicio { get; set; }
        public string? HorasNoDisponible { get; set; }
    }
}
