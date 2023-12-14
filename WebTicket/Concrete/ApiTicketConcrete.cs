using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebTicket.Interface;
using WebTicket.ViewModel;
using Microsoft.Data.SqlClient;
using System.Data;
using WebTicket.Concrete.Function;
using Microsoft.Identity.Client;

namespace WebTicket.Concrete
{
    public class ApiTicketConcrete: ITicket
    {

        protected readonly DatabaseContext _context;
        public ApiTicketConcrete(DatabaseContext context) => _context = context;


        public List<Unidades> GetUnidades(JsonModel json)
        {
            //var obj = LeerJson();
        
            return _context.Unidades.Select(n => new Unidades
            {
                CodigoUnidades = n.CodigoUnidades,
                NombreSimple = n.NombreSimple
            }).Where(u => u.CodigoUnidades.StartsWith(json.config.idPad) && u.CodigoUnidades.Trim() != json.config.idPad)
            .ToList();

        }

        

        public List<TipoDeFila> GetTipodeFilas()
        {
            return _context.TipoDeFila
                .Where(n => n.Activo==true)
                .Select(n => new TipoDeFila
                {
                    IdFila = n.IdFila,
                    NombreFila = n.NombreFila,
                    Activo = n.Activo,
                    Letra = n.Letra,
                    Peso = n.Peso,
                })
                .ToList();

        }

        public bool CambiarEstadoEjecutivo(UpdateEjecutivo ejecutivo)
        {
            try
            {
                if (ejecutivo.estado)
                {
                    var escritorio = _context.Escritorio.FirstOrDefault(e => e.CodigoUsuario == ejecutivo.codigoUsuario);
                    if (escritorio != null)
                    {
                        escritorio.Disponibilidad = "S";
                        _context.SaveChanges();
                    }
                    return true;
                }
                else
                {
                    var escritorio = _context.Escritorio.FirstOrDefault(e => e.CodigoUsuario == ejecutivo.codigoUsuario);
                    if (escritorio != null)
                    {
                        escritorio.Disponibilidad = "N";
                        _context.SaveChanges();
                    }
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool ObtenerEstadoEjecutivo(string ejecutivo)
        {
            try
            {

                var escritorio = _context.Escritorio.FirstOrDefault(e => e.CodigoUsuario == ejecutivo);
                if (escritorio.Disponibilidad == "S") 
                    return true;
                else
                    return false;

            }
            catch (Exception)
            {
                return false;
            }

        }


        public TicketImprimir CrearTicket(string codigoUnidad, int idFila, JsonModel json)
        {
            try
            {
                // Crear los parámetros para el procedimiento almacenado
                var param1 = new SqlParameter("@codigoUnidad", SqlDbType.NVarChar)
                {
                    Value = codigoUnidad
                };

                var param2 = new SqlParameter("@idFila", SqlDbType.Int)
                {
                    Value = idFila
                };

                var result = 0;
                System.Diagnostics.Debug.WriteLine("Param1: "+param1 +"\nParam2: "+param2);
                if (validarDisponibilidad(codigoUnidad))
                {

                    result = _context.Database.ExecuteSql($"EXECUTE [UAP].CreacionTicket {param1}, {param2}");
                }

                if (result > 0)
                {
   
                    return ImprimirTicket(codigoUnidad, idFila, json);
                }
                else
                    return new TicketImprimir();
                
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        private bool validarDisponibilidad(string codigoUnidad)
        {
            var countEjecutivos = _context.Escritorio
                        .Where(e => e.CodigoUnidad == codigoUnidad && e.Disponibilidad == "S")
                        .Count();
            if(countEjecutivos> 2)
            {
                return true;
            }
            else
            {
                var ejecutivo = _context.Escritorio
                        .Where(e => e.CodigoUnidad == codigoUnidad && e.Disponibilidad == "S").First();

                //var revisar= _context

                    return true;
            }
        }

        public TicketImprimir ImprimirTicket(string codigoUnidad, int idFila, JsonModel json)
        {         
            //obtnere el idControlTicket dependiendo del codigoUnidad y el idFila seleccionado en el frontend
            var controlTicket = _context.ControlTicket
            .Where(ct => ct.CodigoUnidades == codigoUnidad && ct.IdTipoFila == idFila)
            .FirstOrDefault();
          
            //Obtener el ultimo ticket ingresado 
            var resultado = _context.Ticket
            .Where(t => t.IdControlTicket == controlTicket.IdControlTicket)
            .OrderByDescending(t => t.FechaTicket)
            .FirstOrDefault();

 
            var pagadurias = _context.Pagaduria
                    .Where(p => p.CodigoUnidadOrganizacional == json.config.codigoPad)
                    .Select(p => new
                    {
                        p.CodigoPagaduria,
                        p.PagaduriaNombre,
                        Departamento = p.Departamento.ToUpper().Replace("Á", "A").Replace("Ó", "O").Replace("Ñ", "N"),
                        p.CodigoUnidadOrganizacional,
                        p.CodigoUnidades
                    }).FirstOrDefault();

            var unidad = _context.Unidades
            .Where(ct => ct.CodigoUnidades == codigoUnidad)
            .Select(u=> new{
                u.CodigoUnidades,
                NombreSimple = u.NombreSimple.Replace("Ñ","N").Replace("Ó","O")
            }).FirstOrDefault();


            TicketImprimir ticket = new TicketImprimir();
            string fechaComoString = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            ticket.IdTicket = resultado.IdTicket;
            ticket.NumeroTicket = resultado.NumeroTicket;
            ticket.FechaTicket=fechaComoString;
            ticket.Departamento = pagadurias.Departamento;
    
            ticket.NombreSimple = unidad.NombreSimple;

            return ticket;
           
        }



    }
}
