using System.ComponentModel.DataAnnotations;

namespace WebTicket.ViewModel
{
    public class SIS_Usuarios
    {
        [Key]
        public string? CodigoUsuario { get; set; }
        public string? ApellidosUsuario { get; set; }
        public string? NombresUsuario { get; set; }
        public string? ClaveUsuario { get; set; }
        public string? CodigoHorarioTrabajo { get; set; }
        public string? CorreoElectronicoUsuario { get; set; }
        public string? MovilMensajeCortoUsuario { get; set; }
        public string? CodigoUsuarioSupervisor { get; set; }
        public string? ActivoInactivoUsuario { get; set; }
        public decimal? TiempoRefrescoPantallaSegundos { get; set; }
        public string? CodigoEstiloPantalla { get; set; }
        public string? CodigoZonaContacto { get; set; }
        public string? CodigoCallCenter { get; set; }
        public string? CodigoRolUsuario { get; set; }
        public string? GenerarContactosDeColegas { get; set; }
        public string? ProveedorServicioCelular { get; set; }
        public string? UbicacionFisicaUsuario { get; set; }
    }
}
