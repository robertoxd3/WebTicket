using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTicket.ViewModel
{
    [Table("Asesoria", Schema = "UAP")]
    public class Asesoria
    {
        [Key]
        public int IdRegistro { get; set; }
        public string? IdPad { get; set; }
        public int CodigoPersona { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Dui { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime Fecha { get; set; }
        public int IdRegistroRemitente { get; set; }
        public string? MotivoAsistencia { get; set; }
        public string TipoAsistencia { get; set; }
        public string IdLugarDerivacion { get; set; }
        public int IdRegistroRemitente1 { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public int IdFila { get; set; }
        public string? Lgbtiq { get; set; }
        public string? DFisica { get; set; }
        public string? DSensorial { get; set; }
        public string? DCognitiva { get; set; }
        public string? DPsiquica { get; set; }
        public string? DMultiple { get; set; }
        public string? TerceraEdad { get; set; }
        public string? PueblosOrig { get; set; }
        public int? AcoNinos { get; set; }
        public int? AcoNinas { get; set; }
        public int? AcoAdolH { get; set; }
        public int? AcoAdolM { get; set; }
        public string CodigoUsuario { get; set; }
        public string? CodigoPretencion { get; set; }
        public string? NumeroExpediente { get; set; }
        public string? ResultadoAsistencia { get; set; }
        public string? UsuarioAtiende { get; set; }
        public string? ArchivarCaso { get; set; }
        public int? BenefMujeresDe0a8 { get; set; }
        public int? BenefHombresDe0a8 { get; set; }
        public int? BenefMujeresDe8a12 { get; set; }
        public int? BenefHombresDe8a12 { get; set; }
        public int? BenefMujeresDe12a18 { get; set; }
        public int? BenefHombresDe12a18 { get; set; }
        public int? BenefMujeresDe18a60 { get; set; }
        public int? BenefHombresDe18a60 { get; set; }
        public int? BenefMujeresMayoresDe60 { get; set; }
        public int? BenefHombresMayoresDe60 { get; set; }
        public string? EtapaResolucionProceso { get; set; }
    }
}
