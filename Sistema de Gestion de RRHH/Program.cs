
var builder = WebApplication.CreateBuilder(args);

// Controllers de presentación
builder.Services.AddControllers()
    .AddApplicationPart(typeof(AssemblyMarker).Assembly);


builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ====== C O R S  (lee Cors:* de appsettings.json) ======
builder.Services.AddCorsPolicies(builder.Configuration);
// ====== Autenticación / Autorización (JWT) ======
builder.Services.AddSeguridadModule(builder.Configuration);

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
app.UseSwagger();
app.UseSwaggerUI();

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
