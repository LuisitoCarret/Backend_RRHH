using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modulo.Seguridad.Domain.Entities
{
    public class Rol
    {
        public long RolId { get; set; }
        public string Nombre { get; set; } = "";
        public string Slug { get; set; } = "";
    }
}
