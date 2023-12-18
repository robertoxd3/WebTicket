using Microsoft.EntityFrameworkCore;
using static ServiceStack.Diagnostics.Events;
using System.Text.RegularExpressions;
using WebTicket.ViewModel;
using WebTicket.Interface;
using Microsoft.AspNetCore.SignalR;
using WebTicket.Concrete;

namespace WebTicket.Hubs
{
    public class TranferidosHub:Hub
    {
       // private readonly IHubTransferidos _hubData;
        protected readonly DatabaseContext _context;
        public TranferidosHub(DatabaseContext context)
        {
            //_hubData = hubData;
            _context = context;
        }

        public async Task JoinGroup(string groupName, Usuario usuario)
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
