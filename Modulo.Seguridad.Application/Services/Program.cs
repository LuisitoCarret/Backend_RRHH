using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using BCrypt.Net;
namespace Modulo.Seguridad.Application.Services
{
    

    class Program
    {
        static void Main()
        {
            var plain = "Temp#1234";
            var hash = BCrypt.Net.BCrypt.HashPassword(plain, workFactor: 11);
            Console.WriteLine(hash);

            // Comprobación opcional:
            Console.WriteLine(BCrypt.Net.BCrypt.Verify(plain, hash)); // True
        }
    }
}