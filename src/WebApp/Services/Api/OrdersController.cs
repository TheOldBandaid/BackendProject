using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using WebApp.Models;

namespace WebApp.Controllers;

[Route("api/orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly string _connectionString;

    public OrdersController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
                            ?? "Server=mysql_db;Database=appDB;User=user;Password=password;";
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int? id)
    {
        var orders = new List<Order>();
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = id.HasValue 
            ? "SELECT ID, user_id, product_name, price FROM orders WHERE ID = @id" 
            : "SELECT ID, user_id, product_name, price FROM orders";

        using var command = new MySqlCommand(query, connection);
        if (id.HasValue)
        {
            command.Parameters.AddWithValue("@id", id.Value);
        }

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            orders.Add(new Order
            {
                Id = reader.GetInt32("ID"),
                UserId = reader.GetInt32("user_id"),
                ProductName = reader.GetString("product_name"),
                Price = reader.GetDecimal("price")
            });
        }

        if (id.HasValue && orders.Count == 0)
            return NotFound(new { message = "Заказ не найден" });

        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Order order)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "INSERT INTO orders (user_id, product_name, price) VALUES (@user_id, @product_name, @price); SELECT LAST_INSERT_ID();";
        
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@user_id", order.UserId);
        command.Parameters.AddWithValue("@product_name", order.ProductName);
        command.Parameters.AddWithValue("@price", order.Price);

        var id = await command.ExecuteScalarAsync();
        order.Id = Convert.ToInt32(id);

        return Created(string.Empty, order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Order order)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "UPDATE orders SET user_id = @user_id, product_name = @product_name, price = @price WHERE ID = @id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@user_id", order.UserId);
        command.Parameters.AddWithValue("@product_name", order.ProductName);
        command.Parameters.AddWithValue("@price", order.Price);

        int rows = await command.ExecuteNonQueryAsync();
        if (rows == 0)
            return NotFound(new { message = "Заказ не найден для обновления" });

        return Ok(new { message = "Заказ успешно обновлен" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "DELETE FROM orders WHERE ID = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        int rows = await command.ExecuteNonQueryAsync();
        if (rows == 0)
            return NotFound(new { message = "Заказ не найден для удаления" });

        return Ok(new { message = "Заказ успешно удален" });
    }
}