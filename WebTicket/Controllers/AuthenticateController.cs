﻿using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceStack;
using WebTicket.Interface;
using WebTicket.ViewModel;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace WebTicket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : Controller
    {
        private readonly IUsuarios _usuarios;
        private readonly IJwtGenerate _jwtGenerate;

        public AuthenticateController(IJwtGenerate jwtGenerate, IUsuarios usuarios)
        {
            _jwtGenerate = jwtGenerate;
            _usuarios = usuarios;
        }

        [HttpPost]
        public object Post([FromBody] LoginRequestViewModel value)
        {
            if (ModelState.IsValid)
            {
                var loginStatus =
                    _usuarios.AuthenticateUsuario(value.CodigoUsuario, value.ClaveUsuario);
                System.Diagnostics.Debug.WriteLine("Paso autenticate");
                if (loginStatus)
                {
                    try
                    {
                        value.Token = _jwtGenerate.CreateToken(value.CodigoUsuario);
                        value.ClaveUsuario = String.Empty;
                        value.PerfilUsuario = "";
                        Usuario escritorio = (Usuario)_usuarios.ObtenerInfoUsuario(value.CodigoUsuario);
                        value.IdEscritorio = escritorio.IdEscritorio;
                        value.CodigoUnidad = escritorio.CodigoUnidad;
                        value.NoEscritorio = escritorio.NoEscritorio;
                        value.codigoPad = escritorio.codigoPad;
                        value.idPad = escritorio.idPad;

                        return new HttpResult(value, HttpStatusCode.OK);
                    }
                    catch (Exception)
                    {
                        return BadRequest("�El usuario no ha sido configurado!");
                    }
                  
                }

                return  BadRequest("�El usuario no ha sido configurado!");

            }
            return BadRequest("�Usuario(a) y clave no v�lidos!");
        }


        [HttpPost("ObtenerEscUsuario")]
        public object ObtenerEscUsuario(UserRequestModel model)
        {
            var codigoUsuario = model.CodigoUsuario;
                if (codigoUsuario!=null)
                {
                    try
                    {

                        var result = _usuarios.ObtenerInfoUsuario(codigoUsuario);

                        return new HttpResult(result, HttpStatusCode.OK);
                    }
                    catch (Exception ex)
                    {
                        //return BadRequest("�El usuario no ha sido configurado!");

                        return new HttpError(HttpStatusCode.BadRequest, ex.Message);
                }

                }

                return BadRequest("�El usuario no ha sido configurado!");

        }
    }
}
