using System.Buffers.Text;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebTicket.Concrete.Function;
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

        [HttpPost("getUnidades")]
        public object GetUnidades()
        {
            string jsonConfiguracion = Request.Headers["Configuracion"];
            //System.Diagnostics.Debug.WriteLine("Prueba 1: " + jsonConfiguracion);

            if (jsonConfiguracion != null)
            {
                // Deserializar la cadena JSON
                var configuracion = JsonConvert.DeserializeObject<JsonModel>(jsonConfiguracion);
         
               var results=_tickets.GetUnidades(configuracion);

                return results;
            }
            return new Unidades[0];
        }

        [HttpPost("getUnidadesUser")]
        public object GetUnidades([FromBody] Usuario usuario)
        {
                var results = _tickets.GetUnidades(usuario);
                return results;
        }


        [HttpGet("getTipoFilas")]
        public IEnumerable<TipoDeFila> GetTipodeFilas()
        {
            IEnumerable<TipoDeFila> results = new List<TipoDeFila>();
            results = _tickets.GetTipodeFilas();

            return results;
        }


        [HttpPost("cambiarEstadoEjecutivo")]
        public HttpResponseMessage CambiarEstadoEjecutivo([FromBody] UpdateEjecutivo ejecutivo)
        {
            var resp = new HttpResponseMessage();
            var res = _tickets.CambiarEstadoEjecutivo(ejecutivo);
            if (res)
            {
                resp.StatusCode = HttpStatusCode.OK;
                resp.ReasonPhrase = "Estado actualizado";
            }
            else
            {
                resp.StatusCode = HttpStatusCode.BadRequest;
                resp.ReasonPhrase = "Error al actualizar";
            }

            return resp;
        }

        [HttpPost("obtenerEstadoEjecutivo")]
        public bool ObtenerEstadoEjecutivo([FromBody] UpdateEjecutivo ejecutivo)
        {
            try
            {
                var res = _tickets.ObtenerEstadoEjecutivo(ejecutivo.codigoUsuario);
                return res;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        //Almacena el Ticket en la bd ejecuta el procedimiento almacenado que genera el n°ticket y se comunica con el dispositivo para mandarlo a imprimir
        [HttpPost("guardarTicket")]
        public IActionResult GuardarTicket([FromBody]TicketFromBody modelTicket)
        {
            try
            {
                string jsonConfiguracion = Request.Headers["Configuracion"];
                //System.Diagnostics.Debug.WriteLine("Prueba 3: " + jsonConfiguracion);

                if (jsonConfiguracion != null)
                {
                    // Deserializar la cadena JSON
                    var configuracion = JsonConvert.DeserializeObject<JsonModel>(jsonConfiguracion);
                  //  System.Diagnostics.Debug.WriteLine("Prueba 4: " + configuracion.config.codigoPad);
                    var results = _tickets.CrearTicket(modelTicket.codigoUnidad, modelTicket.idFila,configuracion);
                    return Ok(results);
                }
                else
                {
                    return BadRequest();
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }

        }

        [HttpPost("ProgramarDisponibilidad")]
        public object ProgramarDisponibilidad([FromBody] ProgramarIndisponibilidad model)
        {
            return _tickets.ProgramarIndisponibilidad(model);
        }

        [HttpPost("ObtenerProgramados")]
        public object ObtenerProgramados([FromBody] ProgramarIndisponibilidad model)
        {
            return _tickets.ObtenerProgramados(model);
        }

        [HttpPost("BorrarProgramados")]
        public object BorrarProgramados([FromBody] ProgramarIndisponibilidad model)
        {
            return _tickets.BorrarProgramados(model);
        }

        [HttpPost("ModificarProgramados")]
        public object ModificarProgramados([FromBody] ProgramarIndisponibilidad model)
        {
            return _tickets.ModificarProgramados(model);
        }

    }
}
