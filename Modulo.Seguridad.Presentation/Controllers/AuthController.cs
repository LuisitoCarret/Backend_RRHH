    namespace Modulo.Seguridad.Presentation.Controllers
{
    [ApiController]
    [Route("auth")]
    public sealed class AuthController: ControllerBase
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
            var (ok, data, reason ,error) = await _auth.LoginAsync(req.Email, req.Password);
            if (!ok || data is null)
            {
                return reason switch
                {
                    LoginFailureReason.InvalidInput => BadRequest(new { message = error }), // 400
                    LoginFailureReason.InvalidEmail => Unauthorized(new { message = error }), // 401
                    LoginFailureReason.InvalidPassword => Unauthorized(new { message = error }), // 401
                    LoginFailureReason.Inactive => StatusCode(StatusCodes.Status403Forbidden, new { message = error }), // 403
                    _ => Unauthorized(new { message = "Usuario o contraseña incorrectos" })
                };
            }

            var (token, exp) = IssueJwt(data.UsuarioId, data.Email, data.Roles, data.EmpleadoId);
            return Ok(new
            {
                token,
                expiresAt = exp,
                email = data.Email,
                roles = data.Roles,
                empleadoId = data.EmpleadoId
            });
        }

        private (string token, DateTime expiresAt) IssueJwt(long userId, string email, string[] roles, long? empleadoId)
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

            if (empleadoId.HasValue)
                claims.Add(new Claim("empleado_id", empleadoId.Value.ToString()));

            foreach (var r in roles.Distinct(StringComparer.OrdinalIgnoreCase))
                claims.Add(new Claim(ClaimTypes.Role, r));

      
            var expires = DateTime.UtcNow.AddMinutes(minutes);
            var jwt = new JwtSecurityToken(issuer, audience, claims, DateTime.UtcNow, expires, creds);
            return (new JwtSecurityTokenHandler().WriteToken(jwt), expires);
        }
    }
}
