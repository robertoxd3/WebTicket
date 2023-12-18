using System.ComponentModel.DataAnnotations.Schema;

namespace WebTicket.ViewModel
{
    [Table("ProgramarIndisponiblidad", Schema = "UAP")]
    public class ProgramarIndisponibilidad
    {
        public int? IdProgramarIndiponibilidad { get; set; }
        public int? IdEscritorio { get; set; }
        public DateTime FechaInicio { get; set; }
        public int? HorasNoDisponible { get; set; }
    }
}
