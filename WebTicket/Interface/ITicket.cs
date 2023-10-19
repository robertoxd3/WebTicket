using Microsoft.IdentityModel.Tokens;
using WebTicket.ViewModel;

namespace WebTicket.Interface
{
    public interface ITicket
    {
        //JsonModel LeerJson();
        List<Unidades> GetUnidades(JsonModel json);
        bool CrearTicket(string codigoUnidad, int idFila, JsonModel json);
        List<TipoDeFila> GetTipodeFilas();

        Ticket ImprimirTicket(string codigoUnidad, int idFila, JsonModel json);

    }
}
