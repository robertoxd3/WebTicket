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
                var result = _context.LlamadaTicket.FromSqlRaw($"EXECUTE UAP.LlamadaTicketPantalla @idEscritorio,@idTipo,@CodigoUsuario ", idEsc, tipo, codUsuario).ToList();
                System.Diagnostics.Debug.WriteLine("Result2: " + result);
                if (result.Count() > 0)
                {
                    return Ok(result + "\nSe llamo al siguiente");
                }
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

         return Ok();
        }


    }
}
