

var builder = WebApplication.CreateBuilder(args);

// =======================================================================
//  CONTROLADORES DE MÓDULOS (API Gateway unifica todos los controllers)
// =======================================================================
builder.Services.AddControllers()
    .AddApplicationPart(Assembly.Load("Modulo.Empleados.Presentation"))
    .AddApplicationPart(Assembly.Load("Modulo.Seguridad.Presentation"))
    .AddApplicationPart(Assembly.Load("Modulo.Contratos.Presentation"))
    .AddApplicationPart(Assembly.Load("Modulo.Asistencias.Presentation"))
    .AddApplicationPart(Assembly.Load("Modulo.Evaluaciones.Presentation"))
    .AddApplicationPart(Assembly.Load("Modulo.Reclutamiento.Presentation"));

// =======================================================================
//  VALIDATORS - FluentValidation
// =======================================================================
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmpleadoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateContractValidator>();

// =======================================================================
//  YARP Reverse Proxy
// =======================================================================
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddEndpointsApiExplorer();

// =======================================================================
//  SWAGGER (incluye autenticación JWT + documentación XML por módulo)
// =======================================================================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RHPlus - API Gateway",
        Version = "v1",
        Description = "Gateway de los módulos Seguridad, Empleados, Contratos, Asistencias, Evaluaciones y Reclutamiento."
    });

    // ---------- JWT SECURITY ----------
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Autenticación JWT. Usa: **Bearer {tu_token}**",
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

    // ---------- XML DOCUMENTATION ----------
    var basePath = AppContext.BaseDirectory;

    void AddXml(string project)
    {
        var xml = $"{project}.xml";
        var xmlPath = Path.Combine(basePath, xml);
        if (File.Exists(xmlPath))
            c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }

    AddXml("ApiGateway"); // tu proyecto principal
    AddXml("Modulo.Seguridad.Presentation");
    AddXml("Modulo.Empleados.Presentation");
    AddXml("Modulo.Contratos.Presentation");
    AddXml("Modulo.Asistencias.Presentation");
    AddXml("Modulo.Evaluaciones.Presentation");
    AddXml("Modulo.Reclutamiento.Presentation");
});

// Para acceder al HttpContext más tarde
builder.Services.AddHttpContextAccessor();

// =======================================================================
//  CORS (usa "Cors:*" del appsettings.json)
// =======================================================================
builder.Services.AddCorsPolicies(builder.Configuration);

// =======================================================================
//  REGISTRO DE MÓDULOS (Clean Architecture)
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
//  SWAGGER (SIEMPRE ACTIVADO, PRODUCCIÓN INCLUIDA)
// =======================================================================
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RHPlus API Gateway v1");
    c.RoutePrefix = "swagger"; // URL: /swagger
});

// =======================================================================
//  PIPELINE
// =======================================================================
app.UseHttpsRedirection();

app.UseRouting();

// CORS antes de Auth
app.UseDefaultCors();

app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();

// Endpoint básico de prueba
app.MapGet("/prueba", () => Results.Ok("Hola RHPlus!"));

app.Run();
