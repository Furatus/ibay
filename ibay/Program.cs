using System.Text;
using ibay.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ibay;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddTransient<IJwtAuthService, JwtAuthService>();
        builder.Services.AddTransient<IIbay, IbayEfService>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MON6SUPER6SECRET6EST6UN6GRAND6MOT6DE6PASSE6DUNE6TRENTAINE6DE6CARACTERES"))
            };
        });
        
        
        builder.Services.AddCors(c =>
        {
            c.AddDefaultPolicy(p =>
            {
                p.AllowAnyOrigin();
                p.AllowAnyMethod();
                p.AllowAnyHeader();
            });
        });
        
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");
        app.UseCors();
        app.MapControllers();
        app.Run();
    }
}