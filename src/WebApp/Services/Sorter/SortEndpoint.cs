using System.Text;

namespace WebApp.Services.Sorter;

public static class SortEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/sort", (string? array, SelectionSortService sorter) =>
        {
            if (string.IsNullOrWhiteSpace(array))
                return Results.BadRequest("Параметр array обязателен.");

            try
            {
                var numbers = array
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                var sorted = sorter.Sort(numbers);

                var originalText = string.Join(", ", numbers);
                var sortedText = string.Join(", ", sorted);

                var html = $"""
                    <!DOCTYPE html>
                    <html lang="ru">
                    <head>
                        <meta charset="UTF-8">
                        <title>Сортировка выбором</title>
                        <link rel="stylesheet" href="/style.css">
                    </head>
                    <body>
                        <h1>Сортировка выбором (вариант 4)</h1>

                        <h2>Исходный массив</h2>
                        <p>{originalText}</p>

                        <h2>Отсортированный массив</h2>
                        <p>{sortedText}</p>
                    </body>
                    </html>
                    """;

                return Results.Content(
                    html,
                    "text/html; charset=utf-8"
                );
            }
            catch (FormatException)
            {
                return Results.BadRequest(
                    "Массив должен содержать только целые числа, разделённые запятыми."
                );
            }
        });
    }
}