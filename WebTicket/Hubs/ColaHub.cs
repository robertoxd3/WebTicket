using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using WebTicket.Concrete;
using WebTicket.Interface;
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
            //var data = (from o in _context.OrdenPrioridadTicket
            //                join c in _context.ControlTicket on o.IdControlTiket equals c.IdControlTicket
            //                join t in _context.Ticket on o.IdTiket equals t.IdTicket
            //                join u in _context.Unidades on c.CodigoUnidades equals u.CodigoUnidades
            //                join e in _context.Escritorio on c.CodigoUsuario equals e.CodigoUsuario
            //                where c.CodigoUsuario == usuario.CodigoUsuario && o.Espera == "S"
            //                select new
            //                {
            //                    Ticket = t,
            //                    Orden = o.Orden,
            //                    CodigoUsuario = c.CodigoUsuario,
            //                    CodigoUnidades = c.CodigoUnidades,
            //                    NombreSimple = u.NombreSimple,
            //                    IdEscritorio = e.IdEscritorio
            //                }).ToList();

            var data = _context.LlamadaTicket.Where(l => l.Estado == "I").ToList();
            // Enviar los datos al cliente que se conecta
            await Clients.Group(groupName).SendAsync("InitialData", data);

        }

        public async Task ObtenerUltimoTicket(string groupName, Usuario usuario)
        {
            var data = _context.LlamadaTicket.Where(l => l.Estado == "I" && l.CodigoUsuario==usuario.CodigoUsuario).ToList();
            await Clients.Group(groupName).SendAsync("obtenerUltimoTicket", data);

        }

        public async Task ObtenerTicketEnCola(string groupName, Usuario usuario)
        {
            var data = (from o in _context.OrdenPrioridadTicket
                        join c in _context.ControlTicket on o.IdControlTiket equals c.IdControlTicket
                        join t in _context.Ticket on o.IdTiket equals t.IdTicket
                        join u in _context.Unidades on c.CodigoUnidades equals u.CodigoUnidades
                        join e in _context.Escritorio on c.CodigoUsuario equals e.CodigoUsuario
                        where c.CodigoUsuario == usuario.CodigoUsuario && o.Espera == "S"
                        select new
                        {
                            Ticket = t,
                            Orden = o.Orden,
                            CodigoUsuario = c.CodigoUsuario,
                            CodigoUnidades = c.CodigoUnidades,
                            NombreSimple = u.NombreSimple,
                            IdEscritorio = e.IdEscritorio
                        }).ToList();


            // Enviar los datos al cliente que se conecta
            await Clients.Group(groupName).SendAsync("Ticket", data);
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

        public async Task JoinGroup(string groupName,Usuario usuario)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("NewUser", $"{Context.ConnectionId} entró al canal");
            await Conectando(groupName);
            await ObtenerTicketEnCola(groupName,usuario);
        }

        public async Task Notification(string groupName, Notificacion notificacion)
        {
            await Clients.Group(groupName).SendAsync("Notification", notificacion);

        }



    }
}
