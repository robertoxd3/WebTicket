namespace WebTicket.ViewModel
{
    public class Usuario
    {
        public string CodigoUsuario { get; set; }
        public string ClaveUsuario { get; set; }
        public string? Token { get; set; }
        public string? PerfilUsuario { get; set; }
        public int? IdEscritorio { get; set; }

        public string? CodigoUnidad { get; set; }

        //public string? PA {get; set; }
    }
}
