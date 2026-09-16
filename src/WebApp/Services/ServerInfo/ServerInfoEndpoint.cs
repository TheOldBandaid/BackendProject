using System.Net;

namespace WebApp.Services.ServerInfo;

public static class ServerInfoEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/server-info", (ServerInfoService service) =>
        {
            var info = service.GetServerInfo();

            var html = "<!DOCTYPE html>" +
                    "<html lang=\"ru\">" +
                    "<head>" +
                    "<meta charset=\"UTF-8\">" +
                    "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">" +
                    "<title>Информация о сервере</title>" + 
                    "</head>" +
                    "<body>" +
                    "<div class=\"container\">" +
                    "<h1>Информационно-административная страница сервера</h1>" +
                    "<p>Информация получена с помощью Unix-команд, выполняемых внутри серверного контейнера.</p>" +

                    "<div class=\"section\">" +
                    "<h2>Содержимое текущего каталога — ls -la</h2>" +
                    "<pre>" + WebUtility.HtmlEncode(info["ls"]) + "</pre>" +
                    "</div>" +

                    "<div class=\"section\">" +
                    "<h2>Запущенные процессы — ps aux</h2>" +
                    "<pre>" + WebUtility.HtmlEncode(info["ps"]) + "</pre>" +
                    "</div>" +

                    "<div class=\"section\">" +
                    "<h2>Текущий пользователь — whoami</h2>" +
                    "<pre>" + WebUtility.HtmlEncode(info["whoami"]) + "</pre>" +
                    "</div>" +

                    "<div class=\"section\">" +
                    "<h2>Информация о пользователе — id</h2>" +
                    "<pre>" + WebUtility.HtmlEncode(info["id"]) + "</pre>" +
                    "</div>" +

                    "<div class=\"section\">" +
                    "<h2>Информация о системе — uname -a</h2>" +
                    "<pre>" + WebUtility.HtmlEncode(info["uname"]) + "</pre>" +
                    "</div>" +

                    "</div>" +
                    "</body>" +
                    "</html>";

            return Results.Content(
                html,
                "text/html; charset=utf-8"
            );
        });
    }
}