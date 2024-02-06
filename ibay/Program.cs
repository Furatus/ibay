using ibay.Services;

namespace ibay;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddTransient<IIbay, IbayEfService>();
        
        
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");
        app.MapControllers();
        app.Run();
    }
}