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
    public static bool makeOrder(string email, string name, string lastName,string phone, string postalCode, string? address,
        string? APM, decimal totalPrice)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO orders (email, user_first_name, user_last_name, " +
                                          "phone, postal_code, address, APM, total_price, status) " +
                                          "VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7, @value8, @value9, @value10)";
                    command.Parameters.AddWithValue("@value1", email);
                    command.Parameters.AddWithValue("@value2", name);
                    command.Parameters.AddWithValue("@value3", lastName);
                    command.Parameters.AddWithValue("@value4", phone);
                    command.Parameters.AddWithValue("@value5", postalCode);
                    command.Parameters.AddWithValue("@value6", address);
                    command.Parameters.AddWithValue("@value7", APM);
                    command.Parameters.AddWithValue("@value10", totalPrice);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error on making order for DB: " + ex.Message, Logger.LogLevel.Error);
                return false;
            }
        }
    }

    public static async Task makeOrderItems(int order_id, int item_id, int item_count)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO orders_items (order_id, item_id, item_count)" +
                                          " VALUES (@value1, @value2, @value3)";
                    command.Parameters.AddWithValue("@value1", order_id);
                    command.Parameters.AddWithValue("@vlaue2", item_id);
                    command.Parameters.AddWithValue("@value3", item_count);
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync("Error on making order items DB: " + ex.Message, Logger.LogLevel.Error);
            }
        }
    }
    public static async Task<bool> CreateOrderWithItemsAsync(string email, string name, string lastName, string phone,
        string postalCode, string address, string APM, decimal totalPrice, string status, HashSet<Item> items)
{
    using (var connection = new MySqlConnection(connectionString))
    {
        try
        {
            await connection.OpenAsync();
            int orderId;
            // Start a transaction
            using (var transaction = await connection.BeginTransactionAsync())
            {
                // Insert the order
                using (var command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = "INSERT INTO orders (email, user_first_name, user_last_name, " +
                                          "phone, postal_code, address, APM, total_price, status) " +
                                          "VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7, @value8, @value9)";
                    command.Parameters.AddWithValue("@value1", email);
                    command.Parameters.AddWithValue("@value2", name);
                    command.Parameters.AddWithValue("@value3", lastName);
                    command.Parameters.AddWithValue("@value4", phone);
                    command.Parameters.AddWithValue("@value5", postalCode);
                    command.Parameters.AddWithValue("@value6", address);
                    command.Parameters.AddWithValue("@value7", APM);
                    command.Parameters.AddWithValue("@value8", totalPrice);
                    command.Parameters.AddWithValue("@value9", status);
                    command.ExecuteNonQuery();
                    // ... (rest of your parameters)

                    await command.ExecuteNonQueryAsync();
                    orderId = (int)command.LastInsertedId;  // Get the newly created order ID
                }

                // Insert order items
                foreach (var item in items)
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = @"INSERT INTO orders_items (order_id, item_id, item_count) 
                                                VALUES (@orderId, @itemId, @itemCount)";
                        command.Parameters.AddWithValue("@orderId", orderId);
                        command.Parameters.AddWithValue("@itemId", item.getId());
                        command.Parameters.AddWithValue("@itemCount", item.getUserCount());
                        await command.ExecuteNonQueryAsync();
                    }
                }

                // Commit the transaction
                await transaction.CommitAsync();
                return true; 
            } 
        }
        catch (Exception ex)
        {
            await Logger.LogAsync("Error creating order with items: " + ex.Message, Logger.LogLevel.Error);
            return false;
        }
    }
}

    public static async Task<HashSet<Item>> getUserCart(string email)
    {
        HashSet<Item> userCart = new HashSet<Item>();
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT item_name, count, price FROM users_carts WHERE email = @value1";
                    command.Parameters.AddWithValue("@value1", email);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var item = new Item(reader["item_name"].ToString(), reader.GetDecimal("price"),
                                reader.GetInt32("count"));
                            userCart.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync("Error on getting user cart DB: " + ex.Message, Logger.LogLevel.Error);
                throw;
            }
        }
        return userCart;
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
                    command.CommandText = "SELECT id, name, price, count, path, description FROM items";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new Item(reader.GetInt32(0),reader.GetString(1), reader.GetDecimal(2), reader.GetInt32(3),
                                reader.GetString(4), reader.GetString(5)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error on get items: " + ex.Message, Logger.LogLevel.Error);
                throw;
            }
        }

        return items;
    }

    public static async Task addItemToCart(string email, string itemName, int count, decimal price)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                await connection.OpenAsync();
                
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            "INSERT INTO users_carts (email, item_name, count, price) VALUES (@value1, @value2,@value3,@value4)";
                        command.Parameters.AddWithValue("@value1", email);
                        command.Parameters.AddWithValue("@value2", itemName);
                        command.Parameters.AddWithValue("@value3", count);
                        command.Parameters.AddWithValue("@value4", price);
                        await command.ExecuteNonQueryAsync();
                    }
                
            }
            catch (Exception ex)
            {
                await Logger.LogAsync("Error on add item to cart DB:" + ex.Message, Logger.LogLevel.Error);
                throw;
            }
        }
    }
    
    
    public static async Task updateItemInCart(string email, string itemName, int count)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "UPDATE users_carts SET count = @value1 WHERE email = @value2 && item_name = @value3";
                    command.Parameters.AddWithValue("@value1", count);
                    command.Parameters.AddWithValue("@value2", email);
                    command.Parameters.AddWithValue("@value3", itemName);
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync("Error on update item in cart DB:" + ex.Message, Logger.LogLevel.Error);
                throw;
            }
        }
    }

    public static async Task<HashSet<Order>> getOrdersForUser(string email)
    {
        HashSet<Order> orders = new HashSet<Order>();
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT id,email, user_first_name, user_last_name, phone, postal_code, address, APM, " +
                        "item_name, item_count, total_price, status FROM orders WHERE email = @value1";
                    command.Parameters.AddWithValue("@value1", email);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            orders.Add(new Order(reader.GetInt32("id"), reader["email"].ToString(),
                                reader["user_first_name"].ToString(), reader["user_last_name"].ToString(),
                                reader["phone"].ToString(),
                                reader["postal_code"].ToString(), reader["address"].ToString(),
                                reader["APM"].ToString(),
                                reader["item_name"].ToString(), reader.GetInt32("count"),
                                reader.GetDecimal("total_price"),
                                reader["status"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync("Error on getting orders for user DB: " + ex.Message, Logger.LogLevel.Error);
                throw;
            }
        }
        return orders;
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