using WebTicket.ViewModel;

namespace WebTicket.Interface
{
    public interface IHubData
    {
        object GetTicketInQueue(Usuario user);
        object GetTicketByUser(Usuario user);
    }
}
