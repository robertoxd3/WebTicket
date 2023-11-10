using System.Net;
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
                    value.Token = _jwtGenerate.CreateToken(value.CodigoUsuario);
                    value.ClaveUsuario = String.Empty;
                    value.PerfilUsuario = "";
                    
                    return new HttpResult(value, HttpStatusCode.OK);
                }

                return  BadRequest("�El usuario no ha sido configurado!");

            }
            return BadRequest("�Usuario(a) y clave no v�lidos!");
        }
    }
}
