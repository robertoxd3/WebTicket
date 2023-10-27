using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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


    }
}
