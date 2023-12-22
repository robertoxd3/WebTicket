using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using System.Net;
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

        public async Task Conectando(string groupName,string codigoUsuario)
        {
            var data = _context.LlamadaTicket.Where(l => l.Estado == "I" /*&& l.CodigoUsuario==codigoUsuario*/).ToList();
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
                        where c.CodigoUnidades==usuario.CodigoUnidad && o.Espera == "S"
                        select new
                        {
                            Ticket = t,
                            Orden = o.Orden,
                            CodigoUsuario = c.CodigoUsuario,
                            CodigoUnidades = c.CodigoUnidades,
                            NombreSimple = u.NombreSimple,
                            IdEscritorio = e.IdEscritorio
                        }).OrderBy(o=>o.Orden).ToList();

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
            
        }

        public async Task Notification(string groupName, Notificacion notificacion)
        {
            await Clients.Group(groupName).SendAsync("Notification", notificacion);
        }

        public async Task GetTicketTransferencias(string groupName, string codigoUnidad)
        {
            var resultado = (from o in _context.OrdenPrioridadTicket
                             join l in _context.LlamadaTicket on o.IdOrden equals l.IdOrden
                             join u in _context.Unidades on o.CodigoUnidadRedirigir equals u.CodigoUnidades
                             where o.Orden == 0 && o.Espera == "R" && o.CodigoUnidadRedirigir == codigoUnidad
                             select new
                             {
                                 NumeroTicket = l.NumeroTicket,
                                 FechaLlamada = l.FechaLlamada,
                                 CodigoUnidades = o.CodigoUnidades,
                                 CodigoUnidadRedirigir = o.CodigoUnidadRedirigir,
                                 UnidadRedirigir = u.NombreSimple
                             }).ToList();
            await Clients.Group(groupName).SendAsync("getTicketTransferencias", resultado);
        }




    }
}
