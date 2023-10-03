using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebTicket.ViewModel
{
    [Table("Pagaduria", Schema = "HUM")]
    public class Pagaduria
    {
        [Key]
        [Column("CodigoPagaduria")]
        public string? CodigoPagaduria { get; set; }

        [Column("Pagaduria")]
        public string? PagaduriaNombre { get; set; }

        [Column("Departamento")]
        public string? Departamento { get; set; }

        [Column("CodigoUnidadOrganizacional")]
        public string? CodigoUnidadOrganizacional { get; set; }

        [Column("CodigoUnidades")]
        public string? CodigoUnidades { get; set; }
    }
}
