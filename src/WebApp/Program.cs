using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Services.Drawer;
using WebApp.Services.Sorter;
using WebApp.Services.ServerInfo;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    ));
builder.Services.AddSingleton<DrawerService>();
builder.Services.AddSingleton<SelectionSortService>();
builder.Services.AddSingleton<ServerInfoService>();

var app = builder.Build();
DrawerEndpoint.Map(app);
SortEndpoint.Map(app);
ServerInfoEndpoint.Map(app);

app.MapGet("/", async (AppDbContext db) =>
{
    var users = await db.Users.ToListAsync();

    var usersHtml = string.Join("", users.Select(user =>
        $"<tr><td>{user.Id}</td><td>{user.Name}</td><td>{user.Surname}</td></tr>"));

    var html = $"""
        <!DOCTYPE html>
        <html lang="ru">
        <head>
            <meta charset="UTF-8">
            <title>Конфигурация серверного приложения</title>
            <link rel="stylesheet" href="/style.css">>
        </head>

        <body>
            <h1>Конфигурация серверного приложения</h1>

            <h2>Программная платформа</h2>
            <p><b>Язык программирования:</b> C#</p>
            <p><b>.NET:</b> {Environment.Version}</p>
            <p><b>ASP.NET Core:</b> {typeof(Microsoft.AspNetCore.Builder.WebApplication).Assembly.GetName().Version}</p>
            <p><b>Среда выполнения:</b> {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}</p>

            <h2>Сервер и операционная система</h2>
            <p><b>Веб-сервер:</b> Kestrel</p>
            <p><b>Операционная система:</b> {Environment.OSVersion}</p>
            <p><b>Архитектура:</b> {System.Runtime.InteropServices.RuntimeInformation.OSArchitecture}</p>
            <p><b>Имя компьютера:</b> {Environment.MachineName}</p>

            <h2>База данных</h2>
            <p><b>СУБД:</b> MySQL</p>
            <p><b>База данных:</b> appDB</p>
            <p><b>Таблица:</b> users</p>

            <table border="1" cellpadding="8">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Имя</th>
                        <th>Фамилия</th>
                    </tr>
                </thead>
                <tbody>
                    {usersHtml}
                </tbody>
            </table>

            <h2>Контейнеризация</h2>
            <p><b>Средство:</b> Docker</p>
            <p><b>Оркестрация контейнеров:</b> Docker Compose</p>
        </body>
        </html>
        """;

    return Results.Content(html, "text/html; charset=utf-8");
});

app.Run();