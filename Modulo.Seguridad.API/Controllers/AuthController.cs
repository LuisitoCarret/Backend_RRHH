using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Modulo.Seguridad.Application.Contracts;
using Modulo.Seguridad.Application.Services;

namespace Modulo.Seguridad.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthUseCase _auth;
        private readonly IConfiguration _config;
        public AuthController(IAuthUseCase auth, IConfiguration config)
        {
            _auth = auth;
            _config = config;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest req)
        {
            var (ok, data, error) = await _auth.LoginAsync(req.Email, req.Password);
            if (!ok || data is null) return Unauthorized(new { message = error });

            var (token, exp) = IssueJwt(data.UsuarioId, data.Email, data.Roles);
            return Ok(new
            {
                token,
                expiresAt = exp,
                email = data.Email,
                roles = data.Roles
            });
        }

        private (string token, DateTime expiresAt) IssueJwt(long userId, string email, string[] roles)
        {
            var key = _config["Jwt:Key"]!;
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var minutes = int.TryParse(_config["Jwt:ExpiresMinutes"], out var m) ? m : 60;

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.Email, email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var r in roles.Distinct(StringComparer.OrdinalIgnoreCase))
                claims.Add(new Claim(ClaimTypes.Role, r));

            var expires = DateTime.UtcNow.AddMinutes(minutes);
            var jwt = new JwtSecurityToken(issuer, audience, claims, DateTime.UtcNow, expires, creds);
            return (new JwtSecurityTokenHandler().WriteToken(jwt), expires);
        }
    }
}
