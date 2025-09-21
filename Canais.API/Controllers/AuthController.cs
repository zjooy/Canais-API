using Canais.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Canais.API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {

        [HttpPost]
        public IActionResult Auth(string username, string password)
        {
            if (username == "joyce" && password == "12345")
            {
                var token = Token.GenerateToken(new Domain.Entities.ReclamacoesEntity());
                return Ok(token);
            }

            return BadRequest("usuario ou senha incorreto.");
        }
    }
}
