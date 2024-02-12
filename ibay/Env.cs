using dotenv.net;

namespace ibay;

public static class Env
{
    public static string JwtSecret
    {
        get
        {
            DotEnv.Load();
            var envvars = DotEnv.Read();

            return envvars["JWT_SECRET"];
        }
    }
    
    public static string dbString
    {
        get
        {
            DotEnv.Load();
            var envvars = DotEnv.Read();

            return envvars["DB_CONNECTION_STRING"];
        }
    }
}