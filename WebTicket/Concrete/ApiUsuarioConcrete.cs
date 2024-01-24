using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using WebTicket.Interface;
using WebTicket.ViewModel;
using ServiceStack;
using System.Net;

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
            //System.Diagnostics.Debug.WriteLine("Result2: " + result);

            return result.Count() > 0;
        }

        public object ObtenerInfoUsuario(string codigoUsuario)
        {
            try
            {
                var result = (from e in _context.Escritorio
                              join p in _context.Pagaduria on e.CodigoPagaduria equals p.CodigoPagaduria
                              where e.CodigoUsuario.Trim() == codigoUsuario
                              select new
                              {
                                  idEscritorio = e.IdEscritorio,
                                  noEscritorio = e.NoEscritorio,
                                  codigoUnidad = e.CodigoUnidad,
                                  idPad = p.CodigoUnidades,
                                  codigoPad = p.CodigoUnidadOrganizacional,
                              }).FirstOrDefault();
                if(result == null)
                {
                    return new HttpError(HttpStatusCode.BadRequest, "Usuario No Configurado");
                }
                else
                {
                    Usuario res = new Usuario();
                    res.IdEscritorio = result.idEscritorio;
                    res.NoEscritorio = result.noEscritorio;
                    res.CodigoUnidad = result.codigoUnidad;
                    res.idPad = result.idPad.Trim();
                    res.codigoPad = result.codigoPad.Trim();
                    return res;
                }

            }
            catch (Exception ex)
            {

                return new HttpError(HttpStatusCode.BadRequest, ex.Message);
            }
           
        }
    }
}
