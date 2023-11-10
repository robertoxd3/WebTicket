namespace WebTicket.Hubs
{
    public interface IColaHubClient
    {
        Task SendOffersToUser(List<string> message);
    }
}
