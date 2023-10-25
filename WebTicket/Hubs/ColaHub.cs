using Microsoft.AspNetCore.SignalR;
using WebTicket.ViewModel;

namespace WebTicket.Hubs
{
    public class ColaHub : Hub
    {

        public async Task ReceiveDataUpdate(List<LlamadaTicket> data)
        {
            await Clients.All.SendAsync("ReceiveDataUpdate", data);
        }

        //public async Task JoinGroup(string groupId)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        //}

        //public async Task AddUserToGroup(string groupName)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        //}

        //public async Task DataUpdate(string groupId, string data)
        //{
        //    await Clients.Group(groupId).SendAsync("ReceiveDataUpdate", data);
        //}

    }
}
