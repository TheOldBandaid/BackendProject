using Microsoft.EntityFrameworkCore;
using WebApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    ));

var app = builder.Build();

app.MapGet("/", async (AppDbContext db) =>
{
    var users = await db.Users.ToListAsync();

    var html = $"""
        <!DOCTYPE html>
        <html lang="ru">
        <head>
            <meta charset="UTF-8">
            <title>Backend Project</title>
        </head>
        <body>
            <h1>Тестовая страница</h1>

            <h2>Информация о системе</h2>

            <p><b>.NET:</b> {Environment.Version}</p>
            <p><b>ОС:</b> {Environment.OSVersion}</p>
            <p><b>Имя компьютера:</b> {Environment.MachineName}</p>

            <h2>Пользователи</h2>

            <ul>
                {string.Join("", users.Select(user =>
                    $"<li>{user.Name} {user.Surname}</li>"))}
            </ul>
        </body>
        </html>
        """;

    return Results.Content(html, "text/html; charset=utf-8");
});

app.Run();