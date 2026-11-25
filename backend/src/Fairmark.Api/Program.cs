using Fairmark.Api.ErrorHandling;
using Fairmark.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Every error response is ProblemDetails (ADR-002, section 5).
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddHealthChecks();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseExceptionHandler();  // exceptions -> GlobalExceptionHandler
app.UseStatusCodePages();   // empty error responses (for example, 404 for an unknown URL) -> ProblemDetails

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();       // /openapi/v1.json
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

// Migrations are not applied on startup (ADR-002, section 4).
app.Run();

// Lets the integration tests start the API with WebApplicationFactory<Program>.
public partial class Program { }
