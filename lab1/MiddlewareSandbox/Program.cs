using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// 1️ Middleware — підрахунок кількості запитів

int requestCount = 0;
app.Use(async (context, next) =>
{
    requestCount++;
    if (context.Request.Path == "/count")
    {
        await context.Response.WriteAsync($"The amount of processed requests is {requestCount}.");
    }
    else
    {
        await next();
    }
});


// 2 Middleware — перевірка параметра custom
app.Use(async (context, next) =>
{
    if (context.Request.Query.ContainsKey("custom"))
    {
        await context.Response.WriteAsync("You’ve hit a custom middleware!");
    }
    else
    {
        await next();
    }
});


// 3️ Middleware — логування запитів

app.Use(async (context, next) =>
{
    var method = context.Request.Method;
    var path = context.Request.Path;
    Console.WriteLine($"[{DateTime.Now}] {method} {path}");
    await next();
});


// 4️ Middleware — перевірка API-ключа

const string validApiKey = "12345"; // свій ключ
app.Use(async (context, next) =>
{
    if (!context.Request.Headers.TryGetValue("X-API-KEY", out var key) || key != validApiKey)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsync("Forbidden: Invalid or missing API key.");
        return;
    }

    await next();
});

// Приклад фінальної кінцевої точки

app.MapGet("/", () => "Hello from Middleware Sandbox!");

app.Run();
