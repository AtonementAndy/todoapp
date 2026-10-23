using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;
using TodoApp.Infrastructure.DI;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/todoapp-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddHealthChecks()
       .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnString")!);

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseHsts();

    app.MapOpenApi().AllowAnonymous();

    app.MapScalarApiReference(options =>
    {
        options.Title = "ToDo List App";
        options.Theme = ScalarTheme.BluePlanet;
        options.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
    }).AllowAnonymous();
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

app.MapHealthChecks("/health");
app.MapHealthChecks("/alive", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("live")
});

app.Run();
