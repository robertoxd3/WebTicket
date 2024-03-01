using System.ComponentModel.DataAnnotations.Schema;

namespace WebTicket.ViewModel
{
    [Table("AccionesPersonal", Schema = "HUM")]
    public class AccionesPersonal
    {
            public int? IdAccionPersonal { get; set; }
            public string? AccionPersonal { get; set; }

            public string? ActivaInactiva { get; set; }

    }
}
