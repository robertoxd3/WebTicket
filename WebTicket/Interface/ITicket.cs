using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebTicket.ViewModel;

namespace WebTicket.Interface
{
    public interface ITicket
    {
        //JsonModel LeerJson();
        object GetUnidades(JsonModel json);
        object GetUnidades(Usuario json);
        object CrearTicket(string codigoUnidad, int idFila, JsonModel json);
        List<TipoDeFila> GetTipodeFilas();

        bool CambiarEstadoEjecutivo(UpdateEjecutivo ejecutivo);
        bool ObtenerEstadoEjecutivo(string codigoUsuario);

        object ImprimirTicket(string codigoUnidad, int idFila, JsonModel json, string mensajeTiempo);

        object ProgramarIndisponibilidad(ProgramarIndisponibilidad model);
        object ObtenerProgramados(ProgramarIndisponibilidad model);

        object BorrarProgramados(ProgramarIndisponibilidad model);

        object ModificarProgramados(ProgramarIndisponibilidad model);

    }
}
