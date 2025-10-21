global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using Modulo.Seguridad.Presentation;
global using System.Text;
global using Sistema_de_Gestion_de_RRHH.Configuration;

global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using System.Text;
global using Modulo.Seguridad.Presentation;          // ya lo tenías
global using Modulo.Empleados.Presentation;          // ← NUEVO (AssemblyMarker)
global using Modulo.Empleados.Infrastructure;        // ← NUEVO (AddEmpleadosModule)
global using Modulo.Seguridad.Infrastructure;        // (si tenías algo similar)