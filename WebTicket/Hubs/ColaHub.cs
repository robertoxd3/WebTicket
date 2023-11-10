using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using WebTicket.Concrete;
using WebTicket.ViewModel;

namespace WebTicket.Hubs
{
    public class ColaHub : Hub
    {
        protected readonly DatabaseContext _context;
        //protected readonly IColaHubClient _hubData;

        public ColaHub(DatabaseContext context)
        {
            _context = context;
            //_hubData = hubData;
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
            var data = await _context.OrdenPrioridadTicket
            .OrderByDescending(c => c.Orden)
            .ToListAsync();


            var resultado = (from o in _context.OrdenPrioridadTicket
                             join t in _context.Ticket on o.IdTiket equals t.IdTicket
                             join c in _context.ControlTicket on t.IdControlTicket equals c.IdControlTicket
                             join u in _context.Unidades on c.CodigoUnidades equals u.CodigoUnidades
                             select new
                             {
                                 o.IdOrden,
                                 o.IdTiket,
                                 o.NumeroTicket,
                                 o.Orden,
                                 o.Espera,
                                 t.FechaTicket,
                                 u.NombreSimple,
                                 c.CodigoUsuario
                             }).OrderByDescending(x=>x.Orden)
                             .Where(i => i.CodigoUsuario == "JIRIVASG")
                             .ToList();


            // Enviar los datos al cliente que se conecta
            await Clients.Group(groupName).SendAsync("Ticket", resultado);

        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("LeftUser", $"{Context.ConnectionId} salió del canal");
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
    
            //await BroadcastNumberOfUsers(Users);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("NewUser", $"{Context.ConnectionId} entró al canal");
            await Conectando(groupName);
        }

        //public async Task GetTicketPerUser(string user)
        //{
        //    var response = _hubData.GetTicketUser(user);
        //    var group = "nal" + user.Trim();
        //    await Clients.Group(group).SendAsync("getNewActivitiesList", response);
        //}



        public async Task Notification(string groupName, Notificacion notificacion)
        {
            await Clients.Group(groupName).SendAsync("Notification", notificacion);
            //string? escritorio = notificacion.escritorio;
            //await Clients.Group(groupName).SendAsync(new Notificacion
            //{
            //    numeroTicket = notificacion.numeroTicket,
            //    escritorio = notificacion.escritorio
            //});
        }

    }
}
