using ServiceStack;
using System.Net;
using WebTicket.ViewModel;

namespace WebTicket.Concrete
{
    public class TicketHubConcrete
    {
        protected readonly DatabaseContext _context;
        public TicketHubConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<HttpResult> GetTicketInQueue(string groupName, Usuario usuario)
        {
            var list = (from o in _context.OrdenPrioridadTicket
                        join c in _context.ControlTicket on o.IdControlTiket equals c.IdControlTicket
                        join t in _context.Ticket on o.IdTiket equals t.IdTicket
                        join u in _context.Unidades on c.CodigoUnidades equals u.CodigoUnidades
                        join e in _context.Escritorio on c.CodigoUsuario equals e.CodigoUsuario
                        where c.CodigoUnidades == usuario.CodigoUnidad && o.Espera == "S"
                        select new
                        {
                            Ticket = t,
                            Orden = o.Orden,
                            CodigoUsuario = c.CodigoUsuario,
                            CodigoUnidades = c.CodigoUnidades,
                            NombreSimple = u.NombreSimple,
                            IdEscritorio = e.IdEscritorio
                        }).OrderBy(o => o.Orden).ToList();
            return new HttpResult(list, HttpStatusCode.OK);
        }

        public async Task<HttpResult> GetTicketByUser(string groupName, Usuario usuario)
        {
            var list = _context.LlamadaTicket.Where(l => l.Estado == "I" /*&& l.CodigoUsuario==codigoUsuario*/).ToList();
            return new HttpResult(list, HttpStatusCode.OK);
        }
    }
}
