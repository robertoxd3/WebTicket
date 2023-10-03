namespace WebTicket.ViewModel
{
    public class JsonModel
    {
        public Config? config { get; set; }
    }

    public class Config
    {
        public string? idPad { get; set; }
        public string? codigoPad { get; set; }
        public bool? mostrarTipoFila { get; set; }
    }


}
