using Application.Interfaces;
using Application.Models.Request;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthenticationService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<string?> Authenticate(CredentialsDto credentials)
        {
            var user = await _userRepository.GetUserByPhoneAsync(credentials.Telefono);

            if (user == null)
            {
                throw new Exception("El usuario con ese numero telefonico no existe");    
            }

            if (user.Contraseña != credentials.Password)
            {
                throw new Exception("la contraseña ingresada no es la correcta"); 
            }

            var secret = _config["Authentication:SecretForKey"];

            if (string.IsNullOrEmpty(secret))
            {
                throw new Exception("La clave JWT no está configurada");
            }

            // Generar token JWT
            var securityPassword = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_config["Authentication:SecretForKey"]));

            var signature = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>
            {

                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
                new Claim(ClaimTypes.Role, user.Rol.ToString()),          
                new Claim(ClaimTypes.Name, user.Nombre),                  
                new Claim(ClaimTypes.MobilePhone, user.Telefono),
            };

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _config["Authentication:Issuer"],
                audience: _config["Authentication:Audience"],
                claims: claimsForToken,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signature
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}