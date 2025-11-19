namespace Modulo.Seguridad.Presentation.Controllers
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

        /// <summary>
        /// Inicia sesión en el sistema y genera un token JWT para autenticación.
        /// </summary>
        /// <remarks>
        /// Este endpoint valida las credenciales del usuario y, si son correctas,
        /// retorna un token JWT junto con información adicional como email, roles
        /// y el empleado asociado.
        ///
        /// **Ejemplo de Request:**
        /// 
        ///     POST /auth/login
        ///     {
        ///         "email": "admin@rrhhplus.com",
        ///         "password": "12345678"
        ///     }
        ///
        /// **Ejemplo de Response exitoso:**
        /// 
        ///     {
        ///         "token": "{jwt_token}",
        ///         "expiresAt": "2025-10-20T18:45:00Z",
        ///         "email": "admin@rrhhplus.com",
        ///         "roles": ["admin"],
        ///         "empleadoId": 12
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Credenciales correctas. Devuelve token JWT.</response>
        /// <response code="400">Datos de entrada inválidos.</response>
        /// <response code="401">Credenciales incorrectas.</response>
        /// <response code="403">Usuario inactivo o sin permisos.</response>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest req)
        {
            var (ok, data, reason, error) = await _auth.LoginAsync(req.Email, req.Password);

            if (!ok || data is null)
            {
                return reason switch
                {
                    LoginFailureReason.InvalidInput => BadRequest(new { message = error }),
                    LoginFailureReason.InvalidEmail => Unauthorized(new { message = error }),
                    LoginFailureReason.InvalidPassword => Unauthorized(new { message = error }),
                    LoginFailureReason.Inactive => StatusCode(StatusCodes.Status403Forbidden, new { message = error }),
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
