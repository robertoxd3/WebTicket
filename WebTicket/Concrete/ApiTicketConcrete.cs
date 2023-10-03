using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebTicket.Interface;
using WebTicket.ViewModel;
using Microsoft.Data.SqlClient;
using System.Data;
using WebTicket.Concrete.Function;

namespace WebTicket.Concrete
{
    public class ApiTicketConcrete: ITicket
    {

        protected readonly DatabaseContext _context;
        public ApiTicketConcrete(DatabaseContext context) => _context = context;


        public List<Unidades> GetUnidades()
        {
            var obj = LeerJson();
        
            return _context.Unidades.Select(n => new Unidades
            {
                CodigoUnidades = n.CodigoUnidades,
                NombreSimple = n.NombreSimple
            }).Where(u => u.CodigoUnidades.StartsWith(obj.config.idPad) && u.CodigoUnidades.Trim() != obj.config.idPad)
            .ToList();

        }

        public JsonModel LeerJson()
        {
            string rutaArchivo = @"C:\KioskoPGRConfig\ConfigTicketeroPADs.json";
          
                if (System.IO.File.Exists(rutaArchivo))
                {
                try
                {
                    string contenidoJson = System.IO.File.ReadAllText(rutaArchivo);

                    // Deserializar el JSON en un objeto C#.
                    var obj = JsonSerializer.Deserialize<JsonModel>(contenidoJson);
                    return obj;
                }catch (Exception ex)
                {
                    return new JsonModel();
                }
                }
            return new JsonModel();
        }

        public List<TipoDeFila> GetTipodeFilas()
        {
            return _context.TipoDeFila.Select(n => new TipoDeFila
            {
                IdFila = n.IdFila,
                NombreFila = n.NombreFila,
                Activo=n.Activo,
                Letra=n.Letra,
                Peso=n.Peso,
            }).ToList();
        }

        public bool CrearTicket(string codigoUnidad, int idFila)
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
     
            var result = _context.Database.ExecuteSql($"EXECUTE [UAP].CreacionTicket {param1}, {param2}");
            //System.Diagnostics.Debug.WriteLine(result);

            if (result>0)
            {
                ImprimirTicket(codigoUnidad,idFila);
                return true;
            }
            else
                return false;
        }

        public Ticket ImprimirTicket(string codigoUnidad, int idFila)
        {         
            //obtnere el idControlTicket dependiendo del codigoUnidad y el idFila seleccionado en el frontend
            var constrolTicket = _context.ControlTicket
            .Where(ct => ct.CodigoUnidades == codigoUnidad && ct.IdTipoFila == idFila)
            .ToList();
            //System.Diagnostics.Debug.WriteLine("PRUEBA 1: " + constrolTicket[0].IdControlTicket);
          
            //Obtener el ultimo ticket ingresado 
            var resultado = _context.Ticket
            .Where(t => t.IdControlTicket == constrolTicket[0].IdControlTicket)
            .OrderByDescending(t => t.FechaTicket)
            .FirstOrDefault();
            //System.Diagnostics.Debug.WriteLine("PRUEBA 2: " + resultado.NumeroTicket);

            var obj = LeerJson();
            var pagadurias = _context.Pagaduria
                    .Where(p => p.CodigoUnidadOrganizacional == obj.config.codigoPad)
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


            Ticket ticket = new Ticket();
            ticket.IdTicket = resultado.IdTicket;
            ticket.NumeroTicket = resultado.NumeroTicket;
            ticket.FechaTicket=resultado.FechaTicket; 
            string fechaComoString = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            int ret = 0;
            if ((ret = BxlPrint.PrinterOpen("USB:001", 1000)) != BxlPrint.BXL_SUCCESS)
            {
                System.Diagnostics.Debug.WriteLine("Fallo al conectar con bixolon");
            }
            else
            {
               
                BxlPrint.SetCharacterSet(BxlPrint.BXL_CS_PC858);
                BxlPrint.SetInterChrSet(BxlPrint.BXL_ICS_SPAIN);

                BxlPrint.PrintText("PGR\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_BOLD | BxlPrint.BXL_FT_UNDERLINE, BxlPrint.BXL_TS_2WIDTH | BxlPrint.BXL_TS_1HEIGHT);
  
                BxlPrint.PrintText("PROCURADURIA " + pagadurias.Departamento + "\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText(fechaComoString+"\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText(unidad.NombreSimple + "\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("\nNo. Ticket:\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText(resultado.NumeroTicket+"\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_1WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                
                
                BxlPrint.PrintText("\nGRACIAS POR SU PREFERENCIA", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
               

                Int32 lResult = BxlPrint.LineFeed(5);

                if (lResult != BxlPrint.BXL_SUCCESS)
                {
                    System.Diagnostics.Debug.WriteLine("Error en el lineamiento" + lResult);
                }
                
                BxlPrint.PrinterClose();
            }
            return ticket;
           
        }



    }
}
