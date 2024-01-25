using System.Data;
using System.Net;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using WebTicket.Concrete;
using WebTicket.Hubs;
using WebTicket.ViewModel;

namespace WebTicket.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ColaController : Controller
    {
        private readonly IHubContext<ColaHub> _hubContext;
        protected readonly DatabaseContext _context;
        private List<LlamadaTicket> datos;

        public ColaController(IHubContext<ColaHub> hubContext, DatabaseContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        [HttpGet("sendDataAll")]
        public async Task<IActionResult> GetDataFromDatabaseAndSendToClients()
        {
            // Realizar la actualización en la base de datos
             datos=_context.LlamadaTicket.ToList();

            // Notificar a los clientes sobre la actualización
            await _hubContext.Clients.All.SendAsync("ReceiveDataUpdate", datos);

            return Ok();
        }

        [HttpPost("ProcedimientoTicket")]
        public object LlamadoTicket([FromBody]LlamadaTicketRequestViewModel llamada)
        {

            var idEsc = new SqlParameter("@idEscritorio", SqlDbType.NVarChar)
            {
                Value = llamada.idEscritorio
            };

            var tipo = new SqlParameter("@idTipo", SqlDbType.NVarChar)
            {
                Value = llamada.idTipo
            };
            var codUsuario = new SqlParameter("@CodigoUsuario", SqlDbType.NVarChar)
            {
                Value = llamada.codigoUsuario
            };

            try
            {
                var result = _context.Database.ExecuteSqlRaw($"EXECUTE UAP.LlamadaTicketPantalla @idEscritorio,@idTipo,@CodigoUsuario ", idEsc, tipo, codUsuario);

                var data = _context.LlamadaTicket.Where(l => l.Estado == "I" && l.CodigoUsuario == llamada.codigoUsuario).ToList();
                return Ok(data);
                //if(result > 0) {
                //    var data = _context.LlamadaTicket.Where(l => l.Estado == "I" && l.CodigoUsuario== llamada.codigoUsuario).ToList();
                //    return Ok(data);
                //}
            }
            catch (SqlException ex)
            {
 
                return new HttpError(HttpStatusCode.BadRequest, ex.Message);

            //switch (ex.Number)
            //{
            //    case 50000:
            //        string mensaje = ex.Message;
            //        return Ok(mensaje);
            //        break;
            //    default:
            //        throw;
            //}
        }
        }


        [HttpPost("ObtenerTicketFinalizados")]
        public IActionResult ObtenerTicketFinalizados([FromBody] LlamadaTicketRequestViewModel request)
        {
            try
            {
                var data = _context.LlamadaTicket.Where(l => l.Estado == "F" && l.CodigoUsuario == request.codigoUsuario)
                    .OrderByDescending(l=>l.FechaFinalizacion).Take(25).ToList();
                return Ok(data);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("TransferirTicket")]
        public bool TransferirTicket([FromBody] OrdenPrioridadTicket ticket)
        {
            try
            {
                var orden = _context.OrdenPrioridadTicket.FirstOrDefault(o => o.IdOrden == ticket.IdOrden);

                if (orden != null)
                {
                    orden.Orden = 0;
                    orden.Espera = "R";
                    orden.Redirigir = "S";
                    orden.CodigoUnidadRedirigir = ticket.CodigoUnidadRedirigir;

                    _context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        [HttpPost("Transferir")]
        public object GetTransferidos([FromForm] LlamadaTicketRequestViewModel data)
        {
            try
            {
                var resultado = (from o in _context.OrdenPrioridadTicket
                                 join l in _context.LlamadaTicket on o.IdOrden equals l.IdOrden
                                 join u in _context.Unidades on o.CodigoUnidadRedirigir equals u.CodigoUnidades
                                 where o.Orden == 0 && o.Espera == "R" && l.CodigoUsuario == data.codigoUsuario
                                 select new
                                 {
                                     NumeroTicket = l.NumeroTicket,
                                     FechaLlamada = l.FechaLlamada,
                                     CodigoUnidades = o.CodigoUnidades,
                                     CodigoUnidadRedirigir = o.CodigoUnidadRedirigir,
                                     UnidadRedirigir = u.NombreSimple
                                 }).ToList();
                if (resultado != null)
                    return Ok(resultado);
                else
                    return BadRequest(resultado);
            }
            catch (SqlException ex)
            {
                return BadRequest( "Error en la Base de datos" + ex);
            }
        }

    }
}
