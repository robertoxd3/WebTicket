using System.Data;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebTicket.Concrete;
using WebTicket.Hubs;
using WebTicket.ViewModel;

namespace WebTicket.Controllers
{
    [Route("api/[controller]")]
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
        public IActionResult LlamadoTicket([FromBody]LlamadaTicketRequestViewModel llamada)
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
                switch (ex.Number)
                {
                    case 50000:
                        string mensaje = ex.Message;
                        return BadRequest(mensaje);
                        break;
                    default:
                        throw;
                }
            }
        }


        [HttpPost("ObtenerTicketFinalizados")]
        public IActionResult ObtenerTicketFinalizados([FromBody] LlamadaTicketRequestViewModel request)
        {
            try
            {
                var data = _context.LlamadaTicket.Where(l => l.Estado == "F" && l.CodigoUsuario == request.codigoUsuario)
                    .OrderByDescending(l=>l.FechaFinalizacion).ToList();
                return Ok(data);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
