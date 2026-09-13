var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () =>
{
    var html = $"""
        <!DOCTYPE html>
        <html lang="ru">
        <head>
            <meta charset="UTF-8">
            <title>Тестовая страница</title>
        </head>
        <body>
            <h1>Тестовая страница</h1>

            <h2>Информация о системе</h2>

            <p><b>.NET:</b> {Environment.Version}</p>
            <p><b>ОС:</b> {Environment.OSVersion}</p>
            <p><b>Имя компьютера:</b> {Environment.MachineName}</p>
        </body>
        </html>
        """;

    return Results.Content(html, "text/html; charset=utf-8");
});

app.Run();