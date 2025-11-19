var builder = WebApplication.CreateBuilder(args);

// Controllers de presentacion
builder.Services.AddControllers()
    .AddApplicationPart(Assembly.Load("Modulo.Empleados.Presentation"))
    .AddApplicationPart(Assembly.Load("Modulo.Seguridad.Presentation"))
    .AddApplicationPart(Assembly.Load("Modulo.Contratos.Presentation"))
    .AddApplicationPart(Assembly.Load("Modulo.Asistencias.Presentation"))
    .AddApplicationPart(Assembly.Load("Modulo.Evaluaciones.Presentation"))
    .AddApplicationPart(Assembly.Load("Modulo.Reclutamiento.Presentation"));


builder.Services.AddValidatorsFromAssemblyContaining<CreateEmpleadoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateContractValidator>();
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RHPlus - API Gateway",
        Version = "v1",
        Description = "Gateway de módulos Seguridad, Empleados, Contratos, Asistencias, Evaluaciones y Reclutamiento."
    });

    // --- Configuración de JWT en Swagger ---
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Bearer. Ejemplo: **Bearer {tu_token_jwt}**",
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

    // (Opcional) XML comments si los generas en el .csproj
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }
});

builder.Services.AddHttpContextAccessor();

// ====== C O R S  (lee Cors:* de appsettings.json) ======
builder.Services.AddCorsPolicies(builder.Configuration);

//Seguridad
builder.Services.AddSeguridadModule(builder.Configuration);

//Empleados
builder.Services.AddEmpleadosModule(builder.Configuration);

//Contratos
builder.Services.AddContratosModule(builder.Configuration);
//Asistencias
builder.Services.AddAsistenciasModule(builder.Configuration);
//Reclutamiento
builder.Services.AddReclutamientoModule(builder.Configuration);
//Evaluaciones
builder.Services.AddEvaluacionesModule(builder.Configuration);

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


// ====== Swagger ======
// ====== Swagger (habilitado en todos los entornos, incluido producción) ======
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RHPlus API Gateway v1");
    // Ruta /swagger (https://rhplus.somee.com/swagger)
    c.RoutePrefix = "swagger";
});


// ====== Pipeline ======
app.UseHttpsRedirection();

app.UseRouting();

// CORS ANTES de AuthN/AuthZ
app.UseDefaultCors();

app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapControllers();

//  Endpoint mínimo para validar el pipeline
app.MapGet("/prueba", () => Results.Ok("Hola"));

app.Run();
