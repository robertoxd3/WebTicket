using System.Buffers.Text;
using System.Drawing;
using System.Drawing.Printing;
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
        public IEnumerable<Unidades> GetUnidades()
        {
            string jsonConfiguracion = Request.Headers["Configuracion"];
            //System.Diagnostics.Debug.WriteLine("Prueba 1: " + jsonConfiguracion);

            if (jsonConfiguracion != null)
            {
                // Deserializar la cadena JSON
                var configuracion = JsonConvert.DeserializeObject<JsonModel>(jsonConfiguracion);
                //System.Diagnostics.Debug.WriteLine("Prueba 2: " + configuracion.config.codigoPad);
                IEnumerable<Unidades> results = new List<Unidades>();
                results = _tickets.GetUnidades(configuracion);

                return results;
            }
           return new Unidades[0];
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

        [HttpGet("printInfo")]
        public bool printInfo()
        {
            if ((BxlPrint.PrinterOpen("USB:001", 1000)) != BxlPrint.BXL_SUCCESS)
            {
                System.Diagnostics.Debug.WriteLine("Fallo al conectar con bixolon");
                return false;
            }
            else
            {
                BxlPrint.SetCharacterSet(BxlPrint.BXL_CS_WPC1252);
                BxlPrint.SetInterChrSet(BxlPrint.BXL_ICS_SPAIN);

                BxlPrint.PrintText("PGR\n\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_BOLD | BxlPrint.BXL_FT_UNDERLINE, BxlPrint.BXL_TS_2WIDTH | BxlPrint.BXL_TS_1HEIGHT);
                BxlPrint.PrintText("¿Tiene consultas sobre el\nestado de los expedientes? \n\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("¿Desea conocer los requisitos\npara iniciar proceso legal?\n\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("¿Necesita orientación sobre un\nservicio legal o dar\nseguimiento a un expediente?\n\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("¿Quiere dar seguimiento al pago\n de cuota alimenticia?\n\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("El área de Atención Virtual\npermite realizar estas acciones\ndesde la distancia,contáctanos a través de los diferentes medios\n\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);

                BxlPrint.PrintText("Centro de llamadas\n2231-9484 opción 1" + "\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("WhatsApp 7607-9013" + "\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("Lengua de Señas Salvadoreña\nLESSA (+503) 7095-7080" + "\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("Correo electronico\natencion.virtual@pgres.gob.sv" + "\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("Sitio Web PGR: www.pgr.gob.sv" + "\n\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);


                BxlPrint.PrintText("Para atender casos de violencia\ncontra la Mujer de manera\nconfidencial mediante atención\nlegal y psicológica. (2231-9595)" + "\n\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("Si eres una persona sorda y\ndeseas asesoría legal o consulta\nsobre un trámite te ofrecemos\nnuestros servicios en\nlínea con atención en Lengua de\nSeñas Salvadoreña (LESSA). \n\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("No olvides que en la\nProcuraduría General de la\nRepublica todos nuestros\nservicios son totalmente gratuitos\n¡Estamos para servirte!", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                //deja espacio de dos lineas entre tickets
                Int32 lResult = BxlPrint.LineFeed(4);
                if (lResult != BxlPrint.BXL_SUCCESS)
                {
                    System.Diagnostics.Debug.WriteLine("Error en el lineamiento" + lResult);
                }
                BxlPrint.PrinterClose();
                return true;
            }
        }

        [HttpGet("printInfo2")]
        public bool printInfo2()
        {
            if ((BxlPrint.PrinterOpen("USB:001", 1000)) != BxlPrint.BXL_SUCCESS)
            {
                System.Diagnostics.Debug.WriteLine("Fallo al conectar con bixolon");
                return false;
            }
            else
            {
                BxlPrint.SetCharacterSet(BxlPrint.BXL_CS_WPC1252);
                BxlPrint.SetInterChrSet(BxlPrint.BXL_ICS_SPAIN);

                BxlPrint.PrintText("PGR\n\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_BOLD | BxlPrint.BXL_FT_UNDERLINE, BxlPrint.BXL_TS_2WIDTH | BxlPrint.BXL_TS_1HEIGHT);
                BxlPrint.PrintText("¿Tienes quejas o denuncias en la PGR? \n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("¿Quieres denunciar un acto de\n corrupción por parte de una\npersona trabajadora de la\nPROCURADURÍA? lo puedes hacer\npor las siguientes vías:\n\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
               

                BxlPrint.PrintText("2231-9525 \n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("7786-2058" + "\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("oficinadequejas@pgres.gob.sv\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("A través de link de quejas que\n está en el portal de la PGR \n\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("Luego una persona te va a\ncontactar para atender o dar\n respuesta a tu caso.", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);

                Int32 lResult = BxlPrint.LineFeed(4);
                if (lResult != BxlPrint.BXL_SUCCESS)
                {
                    System.Diagnostics.Debug.WriteLine("Error en el lineamiento" + lResult);
                }
                BxlPrint.PrinterClose();
                return true;

            }
        }

      
    }
}
