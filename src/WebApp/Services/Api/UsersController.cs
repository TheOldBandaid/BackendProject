using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using WebApp.Models;

namespace WebApp.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly string _connectionString;

    public UsersController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
                            ?? "Server=mysql_db;Database=appDB;User=user;Password=password;";
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int? id)
    {
        var users = new List<User>();
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = id.HasValue 
            ? "SELECT ID, name, surname FROM users WHERE ID = @id" 
            : "SELECT ID, name, surname FROM users";

        using var command = new MySqlCommand(query, connection);
        if (id.HasValue)
        {
            command.Parameters.AddWithValue("@id", id.Value);
        }

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            users.Add(new User
            {
                Id = reader.GetInt32("ID"),
                Name = reader.GetString("name"),
                Surname = reader.GetString("surname")
            });
        }

        if (id.HasValue && users.Count == 0)
            return NotFound(new { message = "Пользователь не найден" });

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "INSERT INTO users (name, surname) VALUES (@name, @surname); SELECT LAST_INSERT_ID();";
        
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@name", user.Name);
        command.Parameters.AddWithValue("@surname", user.Surname);

        var id = await command.ExecuteScalarAsync();
        user.Id = Convert.ToInt32(id);

        return Created(string.Empty, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] User user)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "UPDATE users SET name = @name, surname = @surname WHERE ID = @id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@name", user.Name);
        command.Parameters.AddWithValue("@surname", user.Surname);

        int rows = await command.ExecuteNonQueryAsync();
        if (rows == 0)
            return NotFound(new { message = "Пользователь не найден для обновления" });

        return Ok(new { message = "Пользователь успешно обновлен" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "DELETE FROM users WHERE ID = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        int rows = await command.ExecuteNonQueryAsync();
        if (rows == 0)
            return NotFound(new { message = "Пользователь не найден для удаления" });

        return Ok(new { message = "Пользователь успешно удален" });
    }
}