var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger también en producción (opcional pero útil para validar en Somee)
// Si NO lo quieres en prod, vuelve a poner el if (app.Environment.IsDevelopment())
app.UseSwagger();
app.UseSwaggerUI();

// Redirección HTTPS (déjala; Somee suele manejar SSL. Si ves bucle/redirect raro, coméntala)
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//  Endpoint mínimo para validar el pipeline
app.MapGet("/prueba", () => Results.Ok("Hola"));

app.Run();
