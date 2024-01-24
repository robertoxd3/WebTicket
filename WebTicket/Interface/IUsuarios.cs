using WebTicket.ViewModel;

namespace WebTicket.Interface
{
    public interface IUsuarios
    {
        bool AuthenticateUsuario(string codigoUsuario, string claveUsuario);

        object ObtenerInfoUsuario(string codigoUsuario);
    }
}
