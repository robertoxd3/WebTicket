using Microsoft.Data.SqlClient;
using System.Data;
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

        public bool AuthenticateUsuario(string codigoUsuario, string claveUsuario)
        {
            var param1 = new SqlParameter("@CodigoUsuario", SqlDbType.NVarChar)
            {
                Value = codigoUsuario
            };

            var param2 = new SqlParameter("@ClaveUsuario", SqlDbType.NVarChar)
            {
                Value = claveUsuario
            };

            var result = _context.SIS_Usuarios
           .FromSqlRaw($"SELECT * FROM [SIS_Usuarios] WHERE CodigoUsuario = @CodigoUsuario AND [dbo].[SIS_FU_DesencriptarClave](ClaveUsuario) = @ClaveUsuario", param1, param2)
           .ToList();
            System.Diagnostics.Debug.WriteLine("Result2: " + result);

            return result.Count() > 0;
        }
    }
}
