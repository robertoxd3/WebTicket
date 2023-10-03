using Microsoft.IdentityModel.Tokens;
using WebTicket.ViewModel;

namespace WebTicket.Interface
{
    public interface ITicket
    {
        JsonModel LeerJson();
        List<Unidades> GetUnidades();
        bool CrearTicket(string codigoUnidad, int idFila);
        List<TipoDeFila> GetTipodeFilas();

        Ticket ImprimirTicket(string codigoUnidad, int idFila);

    }
}
