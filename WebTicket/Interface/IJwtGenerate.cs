namespace WebTicket.Interface
{
    public interface IJwtGenerate
    {
        string CreateToken(string codigoUsuario);
    }
}
