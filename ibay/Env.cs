using System.Text;
using dotenv.net;

namespace ibay;

public static class Env
{
    public static string JwtSecret
    {
        get
        {
            /*DotEnv.Load();
            var envvars = DotEnv.Read();

            return envvars["JWT_SECRET"];*/

            return Environment.GetEnvironmentVariable("JWT_SECRET")?? "DEFAULTSECRETKEY";
        }
    }
    
    public static string dbString
    {
        get
        {
            /*DotEnv.Load();
            var envvars = DotEnv.Read();

            return envvars["DB_CONNECTION_STRING"];*/
            
                string dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
                string dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
                string dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "ibay_api";
                string dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
                string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "postgres";
                
                StringBuilder connectionStringBuilder = new StringBuilder();
                connectionStringBuilder.Append($"Host={dbHost};");
                connectionStringBuilder.Append($"Port={dbPort};");
                connectionStringBuilder.Append($"Database={dbName};");
                connectionStringBuilder.Append($"Username={dbUser};");
                connectionStringBuilder.Append($"Password={dbPassword};");
                
                string connectionString = connectionStringBuilder.ToString();
                
                Console.WriteLine($"Chaîne de connexion à la base de données : {connectionString}");

                return connectionString;
                //return Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? "Host=localhost;Database=ibay_api;Username=postgres;Password=postgres";
            }
    }
}