using System.ComponentModel.DataAnnotations;

namespace WebTicket.ViewModel
{
    public class LoginRequestViewModel
    {
#nullable enable
        [Required(ErrorMessage = "Ingrese el nombre de usuario")]
        public string CodigoUsuario { get; set; }

        [Required(ErrorMessage = "Ingrese la clave")]
        public string ClaveUsuario { get; set; }
        public string? Token { get; set; }
        public string? PerfilUsuario { get; set; }
        public int? IdEscritorio { get; set; }
    }
}
