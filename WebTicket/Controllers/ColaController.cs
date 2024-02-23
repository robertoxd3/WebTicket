using System.Data;
using System.Net;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using WebTicket.Concrete;
using WebTicket.Hubs;
using WebTicket.ViewModel;

namespace WebTicket.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ColaController : Controller
    {
        private readonly IHubContext<ColaHub> _hubContext;
        protected readonly DatabaseContext _context;
        private List<LlamadaTicket> datos;

        public ColaController(IHubContext<ColaHub> hubContext, DatabaseContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        [HttpGet("sendDataAll")]
        public async Task<IActionResult> GetDataFromDatabaseAndSendToClients()
        {
            // Realizar la actualización en la base de datos
             datos=_context.LlamadaTicket.ToList();

            // Notificar a los clientes sobre la actualización
            await _hubContext.Clients.All.SendAsync("ReceiveDataUpdate", datos);

            return Ok();
        }

        [HttpPost("ProcedimientoTicket")]
        public object LlamadoTicket([FromBody]LlamadaTicketRequestViewModel llamada)
        {

            var idEsc = new SqlParameter("@idEscritorio", SqlDbType.NVarChar)
            {
                Value = llamada.idEscritorio
            };

            var tipo = new SqlParameter("@idTipo", SqlDbType.NVarChar)
            {
                Value = llamada.idTipo
            };
            var codUsuario = new SqlParameter("@CodigoUsuario", SqlDbType.NVarChar)
            {
                Value = llamada.codigoUsuario
            };

            try
            {
                var result = _context.Database.ExecuteSqlRaw($"EXECUTE UAP.LlamadaTicketPantalla @idEscritorio,@idTipo,@CodigoUsuario ", idEsc, tipo, codUsuario);

                var data = _context.LlamadaTicket.Where(l => l.Estado == "I" && l.CodigoUsuario == llamada.codigoUsuario).ToList();
                return Ok(data);
                //if(result > 0) {
                //    var data = _context.LlamadaTicket.Where(l => l.Estado == "I" && l.CodigoUsuario== llamada.codigoUsuario).ToList();
                //    return Ok(data);
                //}
            }
            catch (SqlException ex)
            {
 
                return new HttpError(HttpStatusCode.BadRequest, ex.Message);

            //switch (ex.Number)
            //{
            //    case 50000:
            //        string mensaje = ex.Message;
            //        return Ok(mensaje);
            //        break;
            //    default:
            //        throw;
            //}
        }
        }


        [HttpPost("ObtenerTicketFinalizados")]
        public IActionResult ObtenerTicketFinalizados([FromBody] LlamadaTicketRequestViewModel request)
        {
            try
            {
                var data = _context.LlamadaTicket.Where(l => l.Estado == "F" && l.CodigoUsuario == request.codigoUsuario)
                    .OrderByDescending(l=>l.FechaFinalizacion).ToList();
                return Ok(data);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetHistorialUnidad/{codigoUnidad}")]
        public object GetHistorialUnidad(string codigoUnidad)
        {
            try
            {
                var unidad = (from u in _context.Unidades
                              where u.CodigoUnidades == (codigoUnidad)
                              select new
                              {
                                  NombreSimple = u.NombreSimple,
                                  u.CodigoUnidades
                              })

                    .Union(
                       from uo in _context.UnidadesOtras
                       where uo.CodigoUnidades == (codigoUnidad)
                       select new
                       {
                           NombreSimple = uo.NombreSimple,
                           uo.CodigoUnidades
                       }
                    ).FirstOrDefault();

                var resultado = (from l in _context.LlamadaTicket
                                 join e in _context.Escritorio on l.IdEscritorio equals e.IdEscritorio
                                 where e.CodigoUnidad == codigoUnidad
                                 select new
                                 {
                                     IdLlamadaTicket=l.IdLlamadaTicket,
                                     NumeroTicket = l.NumeroTicket,
                                     FechaLlamada = l.FechaLlamada,
                                     FechaFinalizacion = l.FechaFinalizacion,
                                     CodigoUnidad = e.CodigoUnidad,
                                     Unidad = unidad.NombreSimple,
                                     CodigoUsuario=e.CodigoUsuario,
                                     IdEscritorio = e.IdEscritorio,
                                 }).ToList().OrderByDescending(x=>x.IdLlamadaTicket);
                return new HttpResult(resultado, HttpStatusCode.OK);
            }
            catch (SqlException ex)
            {
                return new HttpError(HttpStatusCode.BadRequest,
                   "Error obtener los datos" + ex.Message.ToString());
            }
        }

        [HttpGet("GetHistorialUsuario/{codigoUnidad}")]
        public object GetHistorialUsuario(string codigoUnidad)
        {
            //string codigoUsuario
            try
            {
                DateTime fechaHoy=DateTime.Now.Date;



                var fechaActual = DateTime.Now.Date;


                var resultado = (from l in _context.LlamadaTicket
                                 join e in _context.Escritorio on l.IdEscritorio equals e.IdEscritorio
                                 where e.CodigoUnidad == codigoUnidad && l.FechaLlamada.Value.Date == fechaHoy
                                 select new
                                 {
                                     IdLlamadaTicket = l.IdLlamadaTicket,
                                     NumeroTicket = l.NumeroTicket,
                                     FechaLlamada = l.FechaLlamada,
                                     FechaFinalizacion = l.FechaFinalizacion,
                                     CodigoUnidad = e.CodigoUnidad,
                                     CodigoUsuario = e.CodigoUsuario,
                                     IdEscritorio = e.IdEscritorio,
                                     IdOrden= l.IdOrden
                                 }).ToList().OrderByDescending(x => x.IdLlamadaTicket);
                return new HttpResult(resultado, HttpStatusCode.OK);
            }
            catch (SqlException ex)
            {
                return new HttpError(HttpStatusCode.BadRequest,
                   "Error obtener los datos" + ex.Message.ToString());
            }
        }

        [HttpPost("ValidarDetalleTicketAsesoria")]
        public object ValidarDetalleTicketAsesoria(DetalleAsesoriaRequest req)
        {
            try
            {

                var resultado = (from l in _context.DetalleTicketAsesoria
                                 where l.IdOrden == req.IdOrden && l.IdRegistro!=null
                                 select new
                                 {
                                     idRegistro=l.IdRegistro

                                 }).FirstOrDefault();

                if (resultado == null)
                {
                    return new HttpError(HttpStatusCode.BadRequest,"No se encontro");
                }
                var resAsesoria = (from a in _context.Asesoria
                                 where a.IdRegistro == resultado.idRegistro
                                 select new
                                 {
                                     Datos = a,
                                     IdRegistro=resultado.idRegistro,
                                 }).FirstOrDefault();


                return new HttpResult(resAsesoria, HttpStatusCode.OK);
            }
            catch (SqlException ex)
            {
                return new HttpError(HttpStatusCode.BadRequest,
                   "Error obtener los datos" + ex.Message.ToString());
            }
        }

        [HttpPost("ActualizarMotivoDetalleAsesoria")]
        public object ActualizarMotivoDetalleAsesoria(DetalleAsesoriaRequest req)
        {
         


             try
            {
   
                var detalleTicket = _context.DetalleTicketAsesoria.Where(x => x.IdOrden == req.IdOrden && x.IdRegistro == null).FirstOrDefault();
                if (detalleTicket != null)
                {
                    var idReg = new SqlParameter("@idRegistro", SqlDbType.NVarChar)
                    {
                        Value = req.IdRegistro
                    };

                    var idDet = new SqlParameter("@idDetalle", SqlDbType.NVarChar)
                    {
                        Value = detalleTicket.IdDetalleTicketAsesoria
                    };
                    var result = _context.Database.ExecuteSqlRaw($"EXECUTE [UAP].[ActualizarDetalle] @idRegistro,@idDetalle", idReg, idDet);
                    //detalleTicket.MotivoAsistencia = req.MotivoAsistencia;
                    //detalleTicket.IdRegistro = req.IdRegistro;
                    //_context.SaveChanges();
                    return new HttpResult(detalleTicket, HttpStatusCode.OK);
                }
                else
                {
                    var detalleTicketVal = _context.DetalleTicketAsesoria.Where(x => x.IdOrden == req.IdOrden && x.IdRegistro != null).OrderByDescending(x => x.IdDetalleTicketAsesoria).FirstOrDefault();
                    if (detalleTicketVal != null)
                    {
                        var idReg = new SqlParameter("@idRegistro", SqlDbType.NVarChar)
                        {
                            Value = req.IdRegistro
                        };

                        var idDet = new SqlParameter("@idDetalle", SqlDbType.NVarChar)
                        {
                            Value = detalleTicket.IdDetalleTicketAsesoria
                        };
                        var result = _context.Database.ExecuteSqlRaw($"EXECUTE [UAP].[ActualizarDetalle] @idRegistro,@idDetalle", idReg, idDet);
                        //detalleTicketVal.MotivoAsistencia = req.MotivoAsistencia;
                        //detalleTicketVal.IdRegistro = req.IdRegistro;
                        //_context.SaveChanges();
                        return new HttpResult(detalleTicketVal, HttpStatusCode.OK);
                    }
                    return new HttpResult(detalleTicket, HttpStatusCode.OK);
                }
            }
            catch (SqlException ex)
            {
                return new HttpError(HttpStatusCode.BadRequest,
                   "Error obtener los datos" + ex.Message.ToString());
            }
        }


        [HttpPost("TransferirTicket")]
        public bool TransferirTicket([FromBody] OrdenPrioridadTicket ticket)
        {
            try
            {
                var orden = _context.OrdenPrioridadTicket.FirstOrDefault(o => o.IdOrden == ticket.IdOrden);

                if (orden != null)
                {
                    orden.Orden = 0;
                    orden.Espera = "R";
                    orden.Redirigir = "S";
                    orden.CodigoUnidadRedirigir = ticket.CodigoUnidadRedirigir;

                    _context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        [HttpPost("Transferir")]
        public object GetTransferidos([FromForm] LlamadaTicketRequestViewModel data)
        {
            try
            {
                var resultado = (from o in _context.OrdenPrioridadTicket
                                 join l in _context.LlamadaTicket on o.IdOrden equals l.IdOrden
                                 join u in _context.Unidades on o.CodigoUnidadRedirigir equals u.CodigoUnidades
                                 where o.Orden == 0 && o.Espera == "R" && l.CodigoUsuario == data.codigoUsuario
                                 select new
                                 {
                                     NumeroTicket = l.NumeroTicket,
                                     FechaLlamada = l.FechaLlamada,
                                     CodigoUnidades = o.CodigoUnidades,
                                     CodigoUnidadRedirigir = o.CodigoUnidadRedirigir,
                                     UnidadRedirigir = u.NombreSimple
                                 }).ToList();
                if (resultado != null)
                    return Ok(resultado);
                else
                    return BadRequest(resultado);
            }
            catch (SqlException ex)
            {
                return BadRequest( "Error en la Base de datos" + ex);
            }
        }

        [HttpPost("ActualizarDetalleAsesoria")]
        public object ActualizarDetalleAsesoria( DetalleAsesoriaRequest data)
        {
            try
            {
                var detalleTicket = _context.DetalleTicketAsesoria.FirstOrDefault(d => d.IdLlamadaTicket == data.IdLlamadaTicket);
                if (detalleTicket != null)
                {
                    detalleTicket.IdRegistro = data.IdRegistro;
                    detalleTicket.MotivoAsistencia = data.MotivoAsistencia;
                    _context.SaveChanges();
                    return new HttpResult(detalleTicket, HttpStatusCode.OK);
                }
                else
                {
                    return new HttpError(HttpStatusCode.BadRequest,"No se encontro el campo IdDetalleTicketAsesoria");
                }
            }
            catch (SqlException ex)
            {
                return new HttpError(HttpStatusCode.BadRequest,
                   "Error al modificar detalleAsesoria" + ex.Message.ToString());
            }
        }

        [HttpPost("ReaperturaTicket")]
        public object ReaperturaTicket(ReaperturaTicketRequest req)
        {

            var idEsc = new SqlParameter("@idEscritorio", SqlDbType.NVarChar)
            {
                Value = req.IdEscritorio
            };

            var idLla = new SqlParameter("@idLlamada", SqlDbType.NVarChar)
            {
                Value = req.IdLlamadaTicket
            };

            var codUsuario = new SqlParameter("@CodigoUsuario", SqlDbType.NVarChar)
            {
                Value = req.CodigoUsuario
            };

            try
            {
                var result = _context.Database.ExecuteSqlRaw($"EXECUTE UAP.ReaperturaTicket @idEscritorio,@idLlamada,@CodigoUsuario ", idEsc, idLla, codUsuario);
                if (result > 0)
                {
                    return new HttpResult(result, HttpStatusCode.OK);
                }
                else
                {
                    return new HttpError(HttpStatusCode.BadRequest, "No se pudo reaperturar");
                }
                
            }
            catch (SqlException ex)
            {

                return new HttpError(HttpStatusCode.BadRequest, ex.Message);
            }
        }

    }
}
