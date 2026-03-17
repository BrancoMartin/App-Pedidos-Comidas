using Application.Interfaces;
using Application.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App_PedidosComidas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authenticationController : ControllerBase
    {
        public readonly IAuthenticationService _AuthenticationService;

        public authenticationController(IAuthenticationService authenticationService)
        {
            _AuthenticationService = authenticationService;
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialsDto credentials)
        {
            var token = await _AuthenticationService.Authenticate(credentials);

            if (token == null)
                return Unauthorized("Teléfono o contraseña incorrectos");

            return Ok(new { Token = token });
        }
    }
}