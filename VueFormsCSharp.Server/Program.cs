using Microsoft.AspNetCore.Diagnostics;
using VueFormsCSharp.Server;

DbLoggerInitializer.InitializeLogDb();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();

app.MapOpenApi("/api");

FileLogger.LogInfo("Application started");

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerFeature?.Error;

        if (exception != null)
        {
            var urlPath = exceptionHandlerFeature?.Path ?? "Unknown";
            FileLogger.LogCritical($"Unhandled exception occurred at {urlPath}", exception);
        }

        var response = new { error = "Internal Server Error" };
        await context.Response.WriteAsJsonAsync(response);
    });
});

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();

app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.MapFallback(pattern: "/api/{segment}/{*rest}", async context =>
{
    context.Response.StatusCode = 404;
    context.Response.ContentType = "application/json";

    await context.Response.WriteAsJsonAsync("Not Found");
});

app.MapFallbackToFile("/index.html");

app.Run();

