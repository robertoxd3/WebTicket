using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebTicket.ViewModel;

namespace WebTicket.Interface
{
    public interface ITicket
    {
        //JsonModel LeerJson();
        List<Unidades> GetUnidades(JsonModel json);
        object CrearTicket(string codigoUnidad, int idFila, JsonModel json);
        List<TipoDeFila> GetTipodeFilas();

        bool CambiarEstadoEjecutivo(UpdateEjecutivo ejecutivo);
        bool ObtenerEstadoEjecutivo(string codigoUsuario);

        TicketImprimir ImprimirTicket(string codigoUnidad, int idFila, JsonModel json);

        object ProgramarIndisponibilidad(ProgramarIndisponibilidad model);
        object ObtenerProgramados(ProgramarIndisponibilidad model);

        object BorrarProgramados(ProgramarIndisponibilidad model);

    }
}
