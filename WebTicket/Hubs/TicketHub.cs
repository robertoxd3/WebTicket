using System.Text.RegularExpressions;
using WebTicket.Interface;
using ServiceStack;
using static ServiceStack.Diagnostics.Events;
using Microsoft.AspNetCore.SignalR;
using WebTicket.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace WebTicket.Hubs
{
    public class TicketHub: Hub
    {
        private readonly IHubData _hubData;

        public TicketHub(IHubData hubData)
        {
            _hubData = hubData;
        }

        // Join to general hub; with it you can get the list of processes
        public async void Join(string groupName)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName.Trim());
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        // Remove to hub connection
        public async Task Remove(string user)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, user);
        }

        // Get list of processes version per user
        public async Task GetTicketInQueue(string groupName, Usuario usuario)
        {
            var response = _hubData.GetTicketInQueue(usuario);
            await Clients.Group(groupName).SendAsync("getTicketInQueue", response);
        }

        // Get list of new activities per user
        public async Task GetTicketByUser(string groupName, Usuario usuario)
        {
            var response = _hubData.GetTicketByUser(usuario);

            await Clients.Group(groupName).SendAsync("getTicketByUser", response);
        }

        public async Task GetTicketLlamada(string groupName, string codigoUnidad)
        {
            var response = _hubData.GetTicketLlamada(codigoUnidad);

            await Clients.Group(groupName).SendAsync("getTicketLlamada", response);
        }

        public async Task Notification(string groupName, Notificacion notificacion)
        {
            await Clients.Group(groupName).SendAsync("Notification", notificacion);
        }

        //public async Task GetTicketTransferencias(string groupName, string codigoUnidad)
        //{
        //    var response = _hubData.GetTicketTransferencia(codigoUnidad.Trim());

        //    await Clients.Group(groupName).SendAsync("getTicketTransferencias", response);
        //}



    }
}
