using Microsoft.EntityFrameworkCore;
using static ServiceStack.Diagnostics.Events;
using System.Text.RegularExpressions;
using WebTicket.ViewModel;
using WebTicket.Interface;
using Microsoft.AspNetCore.SignalR;
using WebTicket.Concrete;
using ServiceStack;

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

        public async Task JoinGroup(string groupName)
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
            var unidades = (from u in _context.Unidades
            where u.CodigoUnidades == codigoUnidad
                            select new
            {
                                 u.NombreSimple,
                                 u.CodigoUnidades
                             })

                .Union(
                   from uo in _context.UnidadesOtras
                   where uo.CodigoUnidades == codigoUnidad
            select new
            {
                uo.NombreSimple,
                uo.CodigoUnidades
                   }
                ).FirstOrDefault();

            var resultado = (from ot in _context.OrdenPrioridadTicket
                            join t in _context.Ticket on ot.IdTiket equals t.IdTicket
                            //join u in _context.Unidades on ot.CodigoUnidadRedirigir equals u.CodigoUnidades
                            where ot.Espera == "R" && ot.CodigoUnidadRedirigir == codigoUnidad
                            select new
                            {
                                NumeroTicket = ot.NumeroTicket,
                                CodigoUnidades = ot.CodigoUnidades,
                                UnidadRedirigir = unidades.NombreSimple,
                                CodigoUnidadRedirigir = ot.CodigoUnidadRedirigir,
                                FechaLlamada = t.FechaTicket,
                            }).ToList();

            await Clients.Group(groupName).SendAsync("getTicketTransferencias", resultado);
        }
    }
}
