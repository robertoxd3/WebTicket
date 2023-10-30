using Microsoft.EntityFrameworkCore;
using WebTicket.Interface;

namespace WebTicket.Concrete
{
    public class ApiUsuarioConcrete: IUsuarios
    {
        private readonly DatabaseContext _context;

        public ApiUsuarioConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public bool AuthenticateUsuario(string codigoUsuario,string claveUsuario)
        {
            //var result = (from usuarios in _context.SIS_Usuarios
            //              where usuarios.CodigoUsuario == codigoUsuario
            //              select usuarios).Count();
            var result = _context.Database.ExecuteSql($"SELECT CodigoUsuario, [dbo].[SIS_FU_DesencriptarClave](ClaveUsuario) as ClaveUsuario FROM [SIS_Usuarios] WHERE CodigoUsuario = '{codigoUsuario}' AND [dbo].[SIS_FU_DesencriptarClave](ClaveUsuario) = '{claveUsuario}';");
            System.Diagnostics.Debug.WriteLine("Result: " + result);

            //var sql = "SELECT CodigoUsuario, [dbo].[SIS_FU_DesencriptarClave](ClaveUsuario) as ClaveUsuario FROM SIS_Usuarios WHERE CodigoUsuario = 'DOESCOBARM' AND [dbo].[SIS_FU_DesencriptarClave](ClaveUsuario) = 'DO'";

            //var result = _context.SIS_Usuarios
            //    .FromSqlRaw(sql)
            //    .ToList();

            //var query = $"SELECT CodigoUsuario, [dbo].[SIS_FU_DesencriptarClave](ClaveUsuario) as ClaveUsuario FROM [SIS_Usuarios] WHERE CodigoUsuario = '{codigoUsuario}' AND [dbo].[SIS_FU_DesencriptarClave](ClaveUsuario) = '{claveUsuario}';";
            return result > 0;
        }
    }
}
