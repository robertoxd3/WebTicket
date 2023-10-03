using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebTicket.ViewModel
{
    [Table("Unidades", Schema = "HUM")]
    public class Unidades
    {
        [Key]
        public string? CodigoUnidades { get; set; }
        public string? NombreSimple { get; set; }
    }
}
