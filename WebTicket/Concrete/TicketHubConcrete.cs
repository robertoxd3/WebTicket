using ServiceStack;
using System.Net;
using WebTicket.Interface;
using WebTicket.ViewModel;

namespace WebTicket.Concrete
{
    public class TicketHubConcrete : IHubData
    {
        protected readonly DatabaseContext _context;
        public TicketHubConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public object GetTicketInQueue(Usuario usuario)
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

        public object GetTicketByUser( Usuario usuario)
        {
            var list = _context.LlamadaTicket.Where(l => l.Estado == "I" && l.CodigoUsuario==usuario.CodigoUsuario).ToList();
            return new HttpResult(list, HttpStatusCode.OK);
        }

        public object GetTicketLlamada(string codigoUnidad)
        {
            var resultado = (from l in _context.LlamadaTicket
                            join o in _context.OrdenPrioridadTicket on l.IdOrden equals o.IdOrden
                            where (o.CodigoUnidades.StartsWith(codigoUnidad) && !o.CodigoUnidades.Trim().Equals(codigoUnidad) && l.Estado == "I")
                            select new
                            {
                                IdLlamadaTicket = l.IdLlamadaTicket,
                                IdEscritorio=l.IdEscritorio,
                                IdOrden=l.IdOrden,
                                NumeroTicket=l.NumeroTicket,
                                Estado=l.Estado,
                                CodigoUsuario= l.CodigoUsuario

                            }).ToList();
            //var list = _context.LlamadaTicket.Where(l => l.Estado == "I").ToList();
            return new HttpResult(resultado, HttpStatusCode.OK);
        }
    }
}
