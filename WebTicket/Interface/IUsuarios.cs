namespace WebTicket.Interface
{
    public interface IUsuarios
    {
        bool AuthenticateUsuario(string codigoUsuario, string claveUsuario);
    }
}
