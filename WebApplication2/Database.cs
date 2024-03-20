using System.ComponentModel.Design;
using Microsoft.Extensions.Configuration;
using Hash;
using MySqlConnector;
namespace WebApplication2;

public class Database
{
    private static string connectionString;

    static Database()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfigurationRoot config = builder.Build();
        connectionString = config.GetConnectionString("DefaultConnection");
        
    }

    public static async Task<bool> IsUserRegistered(string email)
    {
        
        using (var connection = new MySqlConnection(connectionString))
            
        {
            bool exists = false;
            try
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("SELECT COUNT(1) FROM users WHERE email = @value1", connection))
                {
                    command.Parameters.AddWithValue("@value1", email);

                    exists = Convert.ToInt32(await command.ExecuteScalarAsync()) > 0;
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync("Error on check registration: " + ex.Message, Logger.LogLevel.Error);
            }
            return exists;
        }
    }
    public static async Task RegisterInDatabase(string email, string password)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            password = Hash.Hash.HashPassword(password);
            try
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("INSERT INTO users (email, password) VALUES(@value1, @value2)"))
                {
                    command.Parameters.AddWithValue("@value1", email);
                    command.Parameters.AddWithValue("@value2", password);
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync("Error on registration: " + ex.Message, Logger.LogLevel.Error);
                throw;
            }
        }
    }
}