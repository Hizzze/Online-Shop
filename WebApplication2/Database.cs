using System.ComponentModel.Design;
using Microsoft.Extensions.Configuration;
using Hash;
using Microsoft.Extensions.Logging.Configuration;
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
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(1) FROM accounts WHERE email = @value1";
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
            try
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO accounts (email, password) VALUES(@value1, @value2)";
                    command.Parameters.AddWithValue("@value1", email);
                    command.Parameters.AddWithValue("@value2", password);
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync("Error on registration: " + ex.Message, Logger.LogLevel.Error);
                throw;
            }
        }
    }

    public static bool onBuyItem(string name, int count)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE items SET count = count - @value1 WHERE name = @value2";
                    command.Parameters.AddWithValue("@value1", count);
                    command.Parameters.AddWithValue("@value2", name);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error on buy item (database): " + ex.Message, Logger.LogLevel.Error);
                return false;
            }
        }
    }

    public static async Task<bool> verifyUserData(string email, string password)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT email, password FROM accounts WHERE email = @value1";
                    command.Parameters.AddWithValue("@value1", email);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string temp = reader["email"].ToString();
                            string pass = reader["password"].ToString();
                            if (email.Equals(temp) && pass.Equals(Hash.Hash.HashPassword(password)))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync("Error on verify user: " + ex.Message, Logger.LogLevel.Error);
                return false;
            }
        }

        return false;
    }
    public static List<Item> getItemsList()
    {
        List<Item> items = new List<Item>();
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT name, price, count, path, description FROM items";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new Item(reader.GetString(0), reader.GetDecimal(1), reader.GetInt32(2),
                                reader.GetString(3), reader.GetString(4)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogAsync("Error on get items: " + ex.Message, Logger.LogLevel.Error);
                throw;
            }
        }

        return items;
    }

    public static async Task<(string?, string?, string?, string?)> getUserInfoDatabase(string email)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT name, phone, address, postalcode FROM users WHERE email = @value1";
                    command.Parameters.AddWithValue("@value1", email);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return (reader.IsDBNull(reader.GetOrdinal("name")) ? "null" : reader["name"].ToString(),
                                reader.IsDBNull(reader.GetOrdinal("phone")) ? "null" : reader["phone"].ToString(),
                                reader.IsDBNull(reader.GetOrdinal("address")) ? "null" : reader["address"].ToString(),
                                reader.IsDBNull(reader.GetOrdinal("postalcode")) ? "null" : reader["postalcode"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync("Error on getting user info DB: " + ex.Message, Logger.LogLevel.Error);
            }
        }

        return ("null", "null", "null", "null");
    }
    /*public static async Task getAccountsList()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT "
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync("Error on getting accounts list: " + ex.Message, Logger.LogLevel.Error);
                throw;
            }
        }
    }*/
}