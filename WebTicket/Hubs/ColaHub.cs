using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebTicket.Concrete;
using WebTicket.ViewModel;

namespace WebTicket.Hubs
{
    public class ColaHub : Hub
    {
        protected readonly DatabaseContext _context;
        public ColaHub(DatabaseContext context)
        {
            _context = context;
        }

        public async Task ReceiveDataUpdate(List<LlamadaTicket> data)
        {
            await Clients.All.SendAsync("ReceiveDataUpdate", data);
        }

        public async Task SendToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("NewMessage", message);
        }

        public async Task Conectando(string groupName)
        {
            var data = await _context.LlamadaTicket.ToListAsync();
            // Enviar los datos al cliente que se conecta
            await Clients.Group(groupName).SendAsync("InitialData", data);

            // await base.OnConnectedAsync();
        }

        public async Task ObtenerTicketEnCola(string groupName)
        {
            var data = await _context.Ticket
            .OrderByDescending(t => t.FechaTicket)
            .ToListAsync();

            // Enviar los datos al cliente que se conecta
            await Clients.Group(groupName).SendAsync("Ticket", data);

        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("LeftUser", $"{Context.ConnectionId} salió del canal");
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("NewUser", $"{Context.ConnectionId} entró al canal");
            await Conectando(groupName);
        }
    }
}
