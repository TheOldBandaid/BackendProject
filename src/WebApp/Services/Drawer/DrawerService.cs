using System.Text;

namespace WebApp.Services.Drawer;

public class DrawerService
{
    public string Draw(int num, int width = 150, int height = 150)
    {
        if (num < 1 || num > 12)
            throw new ArgumentException("Номер фигуры должен быть от 1 до 12.");

        if (width < 20 || height < 20 || width > 350 || height > 300)
            throw new ArgumentException("Размеры должны быть от 20 до 350.");

        var type = (num - 1) % 4;
        var colorIndex = (num - 1) / 4;

        var color = colorIndex switch
        {
            0 => "red",
            1 => "green",
            2 => "blue",
            _ => "gray"
        };

        var svg = new StringBuilder();

        svg.Append("""
            <svg width="500" height="400"
                viewBox="0 0 500 400"
                xmlns="http://www.w3.org/2000/svg">
            """);

        switch (type)
        {
            case 0:
                svg.Append($"""
                    <rect x="{250 - width / 2}"
                        y="{200 - height / 2}"
                        width="{width}"
                        height="{height}"
                        fill="{color}" />
                    """);
                break;

            case 1:
                var radius = Math.Min(width, height) / 2;

                svg.Append($"""
                    <circle cx="250"
                            cy="200"
                            r="{radius}"
                            fill="{color}" />
                    """);
                break;

            case 2:
                svg.Append($"""
                    <ellipse cx="250"
                            cy="200"
                            rx="{width}"
                            ry="{height}"
                            fill="{color}" />
                    """);
                break;

            case 3:
                svg.Append($"""
                    <polygon points="
                        250,{200 - height / 2}
                        {250 + width / 2},{200 + height / 2}
                        {250 - width / 2},{200 + height / 2}"
                        fill="{color}" />
                    """);
                break;
        }

        svg.Append("</svg>");

        return svg.ToString();
    }
}