using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebTicket.ViewModel
{
    [Table("UnidadesOtras", Schema = "HUM")]
    public class UnidadesOtras
    {
            [Key]
            public string? CodigoUnidades { get; set; }
            public string? NombreSimple { get; set; }

            public string? CodigoUnidadesSup { get; set; }

            public string? Unidades { get; set; }
        
    }
}
