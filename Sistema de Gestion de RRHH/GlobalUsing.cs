global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using Modulo.Seguridad.Presentation;
global using System.Text;
global using Sistema_de_Gestion_de_RRHH.Configuration;
global using Modulo.Empleados.Presentation;
global using System.Reflection;
global using FluentValidation;
global using Modulo.Empleados.Application.Validators;
global using Modulo.Contratos.Presentation;     // ← nuevo
global using Modulo.Contratos.Infrastructure;   // ← nuevo
global using Modulo.Contratos.Application.Validators; // ← para registrar validadores
global using Modulo.Asistencias.Presentation;
global using Modulo.Asistencias.Infrastructure;