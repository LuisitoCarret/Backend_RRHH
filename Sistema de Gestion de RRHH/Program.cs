using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sistema_de_Gestion_de_RRHH.Configuration;
using Modulo.Empleados.Application.Validators;
using Modulo.Contratos.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// =======================================================================
//  CONTROLADORES DE MÓDULOS
// =======================================================================
builder.Services.AddControllers()
    .AddApplicationPart(Assembly.Load("Modulo.Seguridad.Presentation"))
    .AddApplicationPart(Assembly.Load("Modulo.Empleados.Presentation"))
    .AddApplicationPart(Assembly.Load("Modulo.Contratos.Presentation"))
    .AddApplicationPart(Assembly.Load("Modulo.Asistencias.Presentation"))
    .AddApplicationPart(Assembly.Load("Modulo.Reclutamiento.Presentation"))
    .AddApplicationPart(Assembly.Load("Modulo.Evaluaciones.Presentation"));

// =======================================================================
//  VALIDATORS (FluentValidation)
// =======================================================================
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmpleadoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateContractValidator>();

// =======================================================================
//  YARP REVERSE PROXY
// =======================================================================
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddEndpointsApiExplorer();

// =======================================================================
//  SWAGGER — CON JWT + XML PARA TODOS LOS MÓDULOS
// =======================================================================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RHPlus - API Gateway",
        Version = "v1",
        Description = "Gateway de los módulos Seguridad, Empleados, Contratos, Asistencias, Evaluaciones y Reclutamiento."
    });

    // ------------ JWT ------------
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Bearer. Formato: Bearer {token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });

    // ------------ XML COMMENTS PARA TODOS LOS MÓDULOS ------------
    var basePath = AppContext.BaseDirectory;

    string[] xmlFiles =
    {
        // OJO: este nombre debe coincidir con <AssemblyName> del Api.Gateway
        "Api.Gateway.xml",
        "Modulo.Seguridad.Presentation.xml",
        "Modulo.Empleados.Presentation.xml",
        "Modulo.Contratos.Presentation.xml",
        "Modulo.Asistencias.Presentation.xml",
        "Modulo.Evaluaciones.Presentation.xml",
        "Modulo.Reclutamiento.Presentation.xml"
    };

    foreach (var xml in xmlFiles)
    {
        var xmlPath = Path.Combine(basePath, xml);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        }
    }
});

builder.Services.AddHttpContextAccessor();

// =======================================================================
//  CORS
// =======================================================================
builder.Services.AddCorsPolicies(builder.Configuration);

// =======================================================================
//  MÓDULOS DE LA ARQUITECTURA LIMPIA
// =======================================================================
builder.Services.AddSeguridadModule(builder.Configuration);
builder.Services.AddEmpleadosModule(builder.Configuration);
builder.Services.AddContratosModule(builder.Configuration);
builder.Services.AddAsistenciasModule(builder.Configuration);
builder.Services.AddReclutamientoModule(builder.Configuration);
builder.Services.AddEvaluacionesModule(builder.Configuration);

// =======================================================================
//  AUTENTICACIÓN JWT
// =======================================================================
var key = builder.Configuration["Jwt:Key"] ?? throw new Exception("Jwt:Key missing");
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.RequireHttpsMetadata = false;
        opt.SaveToken = true;
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// =======================================================================
//  SWAGGER — SIEMPRE ACTIVADO (INCLUYE PRODUCCIÓN)
// =======================================================================
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RHPlus API Gateway v1");
    c.RoutePrefix = "swagger"; // URL final: /swagger
});

// =======================================================================
//  PIPELINE
// =======================================================================
app.UseHttpsRedirection();

app.UseRouting();

// CORS antes de autenticación/autorización
app.UseDefaultCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Endpoint de prueba rápido
app.MapGet("/prueba", () => Results.Ok("Hola RHPlus!"));

app.Run();
