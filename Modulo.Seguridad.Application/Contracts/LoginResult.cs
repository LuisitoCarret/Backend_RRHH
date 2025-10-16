using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modulo.Seguridad.Application.Contracts
{
    public sealed class LoginResult
    {
        public long UsuarioId { get; set; }
        public string Email { get; set; } = "";
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}
