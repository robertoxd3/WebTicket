using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebTicket.Interface;
using WebTicket.ViewModel;
using Microsoft.Data.SqlClient;
using System.Data;
using WebTicket.Concrete.Function;
using Microsoft.Identity.Client;
using ServiceStack;
using System.Net;
using System.Diagnostics;
using Azure.Core;
using System.Globalization;
using System;

namespace WebTicket.Concrete
{
    public class ApiTicketConcrete: ITicket
    {

        protected readonly DatabaseContext _context;
        public ApiTicketConcrete(DatabaseContext context) => _context = context;


        public object GetUnidades(JsonModel json)
        {

            var resultado = (from u in _context.Unidades
                             where u.CodigoUnidades.StartsWith(json.config.idPad) && u.CodigoUnidades.Trim() != json.config.idPad
                             select new
                             {
                                 u.NombreSimple,
                                 u.CodigoUnidades
                             })

                 .Union(
                    from uo in _context.UnidadesOtras
                    where uo.CodigoUnidades.StartsWith(json.config.idPad) && uo.CodigoUnidades.Trim() != json.config.idPad
                    select new
                    {
                        uo.NombreSimple,
                        uo.CodigoUnidades
                    }
                 ).ToList();


            return resultado;




        }

        public object GetUnidades(Usuario json)
        {
            var resultado = (from u in _context.Unidades
                             where u.CodigoUnidades.StartsWith(json.idPad) && u.CodigoUnidades.Trim() != json.idPad
                             select new
                             {
                                 u.NombreSimple,
                                 u.CodigoUnidades
                             })

                .Union(
                   from uo in _context.UnidadesOtras
                   where uo.CodigoUnidades.StartsWith(json.idPad) && uo.CodigoUnidades.Trim() != json.idPad
                   select new
                   {
                       uo.NombreSimple,
                       uo.CodigoUnidades
                   }
                ).ToList();


            return resultado;

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


        public object CrearTicket(string codigoUnidad, int idFila, JsonModel json)
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
                var resultado = validarDisponibilidad(codigoUnidad);
             
                if (resultado.Item1)
                {

                    result = _context.Database.ExecuteSql($"EXECUTE [UAP].CreacionTicket {param1}, {param2}");
                    if (result > 0)
                    {
                        return ImprimirTicket(codigoUnidad, idFila, json, resultado.Item2);
                    }
                    else
                        return new HttpResult(true, HttpStatusCode.OK);

                }
                else
                {
               
                    return new HttpError(HttpStatusCode.BadRequest, "Por favor, consulte con el despacho del procurador para solventar su atención o llame al 2231-9484 opcion 1, se espera servicio " + resultado.Item2);
                }

            }
            catch (Exception)
            {
                return new HttpError(HttpStatusCode.BadRequest, "No se pudo generar el ticket");
            }
          
        }


        public (bool, string) validarDisponibilidad(string codigoUnidad)
        {
            var ejecutivos = _context.Escritorio
                        .Where(e => e.CodigoUnidad == codigoUnidad && e.Disponibilidad == "S").ToList();
            var countEjecutivos = ejecutivos.Count();

            if (countEjecutivos == 0)
            {
                return (false, "No Hay Ejecutivos configurados para esta unidad");
            }
            if (countEjecutivos >= 2)
            {
                List<object> resulEjecutivosProgramados = new List<object>();
                List<string> validarProgramados = new List<string>();
                List<DateTime> fechasEjecutivo = new List<DateTime>();
                DateTime fechaMenor = new DateTime(2050, 12, 31);
                var banderaSinIndisponibilidad = 0;
                foreach (var eje in ejecutivos)
                {
                    DateTime fechaHoy = DateTime.Now.Date;
                    var revisar = _context.ProgramarIndisponibilidad.Where(x => x.IdEscritorio == eje.IdEscritorio && x.FechaInicio.Value.Date == fechaHoy).OrderByDescending(x => x.IdProgramarIndiponibilidad).FirstOrDefault();
                    if (revisar != null)
                    {
                        DateTime fechaActual = revisar.FechaInicio.Value;
                        //fechaActual = DateTime.Now;
                        DateTime limite = DateTime.Today.AddHours(15).AddMinutes(30);
                        if (fechaActual.Date == revisar.FechaInicio?.Date)
                        {
                            DateTime nuevaFecha = fechaActual.AddHours((double)revisar.HorasNoDisponible);
                            if (nuevaFecha > limite)
                            {
                                validarProgramados.Add("S");
                                
                                //return (false, "Hasta Mañana.");
                            }
                            else
                            {
                                validarProgramados.Add("N");
                                fechasEjecutivo.Add(nuevaFecha);
                                if(nuevaFecha<fechaMenor)
                                    fechaMenor=nuevaFecha;
                                // return (true, "" + nuevaFecha.ToString("HH:mm:ss"));
                            }
                        }
                    }
                    else
                    {
                        banderaSinIndisponibilidad++;
                    }

                }
                var contadorV = 0;
                foreach (var v in validarProgramados)
                {
                    if(v=="S")
                    {
                       contadorV++;
                    }
                }
                if (contadorV == ejecutivos.Count)
                {
                    return (false, "Hasta Mañana.");
                }
                else
                {
                    if (validarProgramados.Count == 0)
                    {
                        if (ejecutivos.Count > fechasEjecutivo.Count)
                            return (true, "OK");
                        else
                            return (true, fechaMenor.ToString("HH:mm:ss"));
                    }
                    else
                    {
                        if(validarProgramados.Count>0 && fechasEjecutivo.Count==0)
                            return (true, "OK");
                        else if(banderaSinIndisponibilidad>0)
                            return (true, "OK");
                        else
                            return (true, fechaMenor.ToString("HH:mm:ss"));
                    }
                }
            }
            else
            {
                var ejecutivo = _context.Escritorio
                        .Where(e => e.CodigoUnidad == codigoUnidad && e.Disponibilidad == "S").First();
                DateTime fechaHoy = DateTime.Now.Date;
                var revisar = _context.ProgramarIndisponibilidad.Where(x => x.IdEscritorio == ejecutivo.IdEscritorio && x.FechaInicio.Value.Date == fechaHoy).OrderByDescending(x=>x.IdProgramarIndiponibilidad).FirstOrDefault();
                if (revisar!=null)
                {
                    DateTime fechaActual = revisar.FechaInicio.Value;
                    DateTime limite = DateTime.Today.AddHours(15).AddMinutes(30);
                    if (fechaActual.Date == revisar.FechaInicio?.Date )
                    {
                        DateTime nuevaFecha = fechaActual.AddHours((double)revisar.HorasNoDisponible);
                        if (nuevaFecha > limite)
                        {
                             
                            return(false, "Hasta Mañana.");
                        }
                        else
                        {
                            return (true, ""+ nuevaFecha.ToString("HH:mm:ss"));
                        }
                    }

                }
                return (true, "OK");
            }
        }

        public object ImprimirTicket(string codigoUnidad, int idFila, JsonModel json,string mensajeTiempo)
        {
            try
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

                //var unidad = _context.Unidades
                //.Where(ct => ct.CodigoUnidades == codigoUnidad)
                //.Select(u=> new{
                //    u.CodigoUnidades,
                //    NombreSimple = u.NombreSimple.Replace("Ñ","N").Replace("Ó","O")
                //}).FirstOrDefault();

                var unidad = (from u in _context.Unidades
                              where u.CodigoUnidades==(codigoUnidad) 
                              select new
                              {
                                  NombreSimple = u.NombreSimple,
                                  u.CodigoUnidades
                              })

                    .Union(
                       from uo in _context.UnidadesOtras
                       where uo.CodigoUnidades==(codigoUnidad)
                       select new
                       {
                           NombreSimple = uo.NombreSimple,
                           uo.CodigoUnidades
                       }
                    ).FirstOrDefault();


                TicketImprimir ticket = new TicketImprimir();
                string fechaComoString = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                ticket.IdTicket = resultado.IdTicket;
                ticket.NumeroTicket = resultado.NumeroTicket;
                ticket.FechaTicket = fechaComoString;
                ticket.Departamento = pagadurias.Departamento;

                ticket.NombreSimple = unidad.NombreSimple;


                //return ticket;
                return new HttpResult(ticket, HttpStatusCode.OK)
                {
                    StatusDescription = mensajeTiempo
                };
            }
            catch (Exception)
            {

                return new HttpError(HttpStatusCode.BadRequest, "F");
            }
            

        }

        public object ProgramarIndisponibilidad(ProgramarIndisponibilidad model)
        {
            try
            {
                DateTime currentTime = TimeZoneInfo.ConvertTime((DateTime)(model?.FechaInicio), TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));

                ProgramarIndisponibilidad indisponibilidad = new ProgramarIndisponibilidad();
                //indisponibilidad.IdProgramarIndiponibilidad = 1;
                indisponibilidad.IdEscritorio = model.IdEscritorio;
                indisponibilidad.FechaInicio = currentTime;
                indisponibilidad.HorasNoDisponible = model.HorasNoDisponible;
                indisponibilidad.Motivo = model.Motivo;
                _context.ProgramarIndisponibilidad.Add(indisponibilidad);
                return _context.SaveChanges() > 0 ? new HttpResult(true, HttpStatusCode.OK) : new HttpResult(false, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return new HttpError(HttpStatusCode.BadRequest, ex.Message);
            }
            

        }

        public object ObtenerProgramados(ProgramarIndisponibilidad model)
        {
           var result= _context.ProgramarIndisponibilidad.Where(x=>x.IdEscritorio==model.IdEscritorio).OrderByDescending(x=>x.FechaInicio).ToList();
            return new HttpResult(result, HttpStatusCode.OK);

        }

        public object BorrarProgramados(ProgramarIndisponibilidad model)
        {
            ProgramarIndisponibilidad i = new ProgramarIndisponibilidad();
            i.IdProgramarIndiponibilidad = model.IdProgramarIndiponibilidad;
            i.IdEscritorio=model.IdEscritorio;
            i.FechaInicio=model.FechaInicio;
            i.HorasNoDisponible=model.HorasNoDisponible;

            var result = _context.ProgramarIndisponibilidad.Where(x => x.IdProgramarIndiponibilidad == model.IdProgramarIndiponibilidad).First();
            var res= _context.ProgramarIndisponibilidad.Remove(result);

            if(_context.SaveChanges() > 0)
            {
                return new HttpResult(res, HttpStatusCode.OK);
            }
            else
            {
                return new HttpResult(res, HttpStatusCode.OK);
            }

           // return  ? new HttpResult(res, HttpStatusCode.OK) : new HttpResult(res, HttpStatusCode.OK);

        }

        public object ModificarProgramados(ProgramarIndisponibilidad model)
        {

            try
            {

                DateTime currentTime = TimeZoneInfo.ConvertTime((DateTime)(model?.FechaInicio), TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
                model.FechaInicio = currentTime;
                var res = _context.ProgramarIndisponibilidad.Update(model);
                if (_context.SaveChanges() > 0)
                {
                    return new HttpResult(true, HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResult(false, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {

                return new HttpError(HttpStatusCode.BadRequest, ex.Message);
            }

        }


    }
}
