namespace WebApp.Services.Drawer;

public static class DrawerEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/drawer", (int? num, int? width, int? height, DrawerService drawer) =>
        {
            if (num is null)
                return Results.BadRequest("Параметр num обязателен.");

            try
            {
                var svg = drawer.Draw(
                    num.Value,
                    width ?? 150,
                    height ?? 150
                );

                return Results.Content(
                    svg,
                    "image/svg+xml; charset=utf-8"
                );
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}