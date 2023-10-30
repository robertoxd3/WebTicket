using Microsoft.IdentityModel.Tokens;
using ServiceStack.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebTicket.Interface;

namespace WebTicket.Concrete
{
    public class ApiJwtConcrete: IJwtGenerate
    {
        //private readonly JwtConfig _config;

        //public ApiJwtConcrete(JwtConfig config)
        //{
        //    _config = config;
        //}

        public string CreateToken(string codigoUsuario)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
        {
            new(ClaimTypes.Name, codigoUsuario)
        };

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credential
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }

    }
}
