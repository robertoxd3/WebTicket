using Microsoft.IdentityModel.Tokens;
using WebTicket.ViewModel;

namespace WebTicket.Interface
{
    public interface ITicket
    {
        //JsonModel LeerJson();
        List<Unidades> GetUnidades(JsonModel json);
        TicketImprimir CrearTicket(string codigoUnidad, int idFila, JsonModel json);
        List<TipoDeFila> GetTipodeFilas();

        TicketImprimir ImprimirTicket(string codigoUnidad, int idFila, JsonModel json);

    }
}
