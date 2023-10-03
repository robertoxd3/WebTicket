using Microsoft.AspNetCore.Mvc;
using WebTicket.Interface;
using WebTicket.ViewModel;


namespace WebTicket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : Controller
    {
        public readonly ITicket _tickets;

        public TicketController(ITicket ticket)
        {
            _tickets = ticket;
        }


        [HttpGet("getUnidades")]
        public IEnumerable<Unidades> GetUnidades()
        {
            IEnumerable<Unidades> results = new List<Unidades>();
            results = _tickets.GetUnidades();

            return results;
        }


        [HttpGet("getJson")]
        public JsonModel obtenerMostrarFila()
        {
            var result = _tickets.LeerJson();
            return result;
        }

        [HttpGet("getTipoFilas")]
        public IEnumerable<TipoDeFila> GetTipodeFilas()
        {
            IEnumerable<TipoDeFila> results = new List<TipoDeFila>();
            results = _tickets.GetTipodeFilas();

            return results;
        }

        //Almacena el Ticket en la bd ejecuta el procedimiento almacenado que genera el n°ticket y se comunica con el dispositivo para mandarlo a imprimir
        [HttpPost("guardarTicket")]
        public IActionResult GuardarTicket([FromBody]TicketFromBody modelTicket)
        {
            try
            {
                var results = _tickets.CrearTicket(modelTicket.codigoUnidad,modelTicket.idFila);
             
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }

        }




    }
}
