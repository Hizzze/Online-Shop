namespace WebApplication2;
using Microsoft.Extensions.Configuration;
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
    
}