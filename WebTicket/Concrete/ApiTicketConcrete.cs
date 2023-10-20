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

        /*public JsonModel LeerJson()
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
        */

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

        public bool CrearTicket(string codigoUnidad, int idFila, JsonModel json)
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

            if ((BxlPrint.PrinterOpen("USB:001", 1000)) != BxlPrint.BXL_SUCCESS && (BxlPrint.PrinterOpen("COM4:19200", 1000) != BxlPrint.BXL_SUCCESS))
            {
                System.Diagnostics.Debug.WriteLine("Fallo al conectar con bixolon");
                return false;
            }
            else
            {
                
                result = _context.Database.ExecuteSql($"EXECUTE [UAP].CreacionTicket {param1}, {param2}");
                System.Diagnostics.Debug.WriteLine("Else conectado "+result);
            }

            if (result>0)
            {
                //System.Diagnostics.Debug.WriteLine("VARIABLES: " + codigoUnidad+ " v2: "+idFila);
                //System.Diagnostics.Debug.WriteLine("JSON ANTES DE: " + json.config.codigoPad);
                ImprimirTicket(codigoUnidad,idFila,json);
                return true;
            }
            else
                return false;
        }

        public Ticket ImprimirTicket(string codigoUnidad, int idFila, JsonModel json)
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


            Ticket ticket = new Ticket();
            ticket.IdTicket = resultado.IdTicket;
            ticket.NumeroTicket = resultado.NumeroTicket;
            ticket.FechaTicket=resultado.FechaTicket; 
            string fechaComoString = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            BxlPrint.SetCharacterSet(BxlPrint.BXL_CS_WPC1252);
            BxlPrint.SetInterChrSet(BxlPrint.BXL_ICS_SPAIN);
            BxlPrint.PrintText("PGR\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_BOLD | BxlPrint.BXL_FT_UNDERLINE, BxlPrint.BXL_TS_2WIDTH | BxlPrint.BXL_TS_1HEIGHT);  
                BxlPrint.PrintText("PROCURADURÍA " + pagadurias.Departamento + "\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText(fechaComoString+"\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText(unidad.NombreSimple + "\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("\nNo. Ticket:\n\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText(resultado.NumeroTicket+"\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_3WIDTH | BxlPrint.BXL_TS_1HEIGHT);

                BxlPrint.PrintText("\nContactanos\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_BOLD | BxlPrint.BXL_FT_UNDERLINE, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("Centro de llamadas\n(+503) 2231-9484 opción 1\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("WhatsApp (+503) 7607-9013" + "\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("LESSA (+503) 7095-7080\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("Atención a Mujeres Victimas de\n Violencia 'Estamos Contigo'\n(+503) 2231-9595" + "\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("Correo electrónico\n atencion.virtual@pgres.gob.sv" + "\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                BxlPrint.PrintText("Sitio Web PGR: www.pgr.gob.sv\n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);

                Int32 lResult = BxlPrint.LineFeed(3);

                if (lResult != BxlPrint.BXL_SUCCESS)
                {
                    System.Diagnostics.Debug.WriteLine("Error en el lineamiento" + lResult);
                }
                
                BxlPrint.PrinterClose();
            
            return ticket;
           
        }



    }
}
