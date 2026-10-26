using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Services.Drawer;
using WebApp.Services.Sorter;
using WebApp.Services.ServerInfo;
using System.Net;
using WebApp.Models;

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
builder.Services.AddControllers();

var app = builder.Build();
DrawerEndpoint.Map(app);
SortEndpoint.Map(app);
ServerInfoEndpoint.Map(app);
app.MapControllers();

app.MapGet("/catalog", async (AppDbContext db) =>
{
    var products = await db.Products
        .AsNoTracking()
        .OrderBy(p => p.Id)
        .ToListAsync();

    var itemsHtml = string.Join("", products.Select(p =>
        $"""
        <article>
            <h2><a href="/products/{p.Id}">{WebUtility.HtmlEncode(p.Name)}</a></h2>
            <p>{WebUtility.HtmlEncode(p.Description)}</p>
            <p><b>Цена:</b> {p.Price:F2} руб.</p>
            <a href="/products/{p.Id}">Подробнее о блюде</a>
        </article>
        <hr>
        """));

    var html = $"""
        <!DOCTYPE html>
        <html lang="ru">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1">
            <title>Меню пиццерии</title>
            <link rel="stylesheet" href="/style.css">
        </head>
        <body>
            <h1>Меню пиццерии</h1>
            <p><a href="/">Главная</a> |
                <a href="/delivery.html">Доставка</a> |
                <a href="/contacts.html">Контакты</a></p>
            {itemsHtml}
        </body>
        </html>
        """;

    return Results.Content(html, "text/html; charset=utf-8");
});

app.MapGet("/products/{id:int}", async (int id, AppDbContext db) =>
{
    var product = await db.Products
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == id);

    if (product is null)
        return Results.NotFound("Блюдо не найдено.");

    var name = WebUtility.HtmlEncode(product.Name);
    var description = WebUtility.HtmlEncode(product.Description);

    var html = $"""
        <!DOCTYPE html>
        <html lang="ru">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1">
            <title>{name}</title>
            <link rel="stylesheet" href="/style.css">
        </head>
        <body>
            <h1>{name}</h1>
            <p>{description}</p>
            <p><b>Цена:</b> {product.Price:F2} руб.</p>
            <p><a href="/catalog">Вернуться в меню</a></p>
        </body>
        </html>
        """;

    return Results.Content(html, "text/html; charset=utf-8");
});


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