using DataTransferObjects.User;
using DBLayer;
using LayerData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Server.MiddleWare;
using PasswordManager.Server.Utilities;
using Serilog;
using Services;
using Services.Audit;

namespace PasswordManager.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionstring = builder.Configuration.GetSection("ConnectionStrings").GetSection("sqlConnection").Value;
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                builder.Services.AddDbContext<MSSQLContext>(option => option.UseSqlServer(connectionstring));
            }
            catch (Exception ex)
            {
                Log.Fatal("Can NOT connect to the database - {0}\nDetails:\n{1}\n\n", DateTime.Now, ex.ToString());
            }

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddMemoryCache(); 
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    //Need to be placed in serilog file
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("===== JWT FAILED =====");
                        Console.WriteLine(context.Exception.GetType().FullName);
                        Console.WriteLine(context.Exception.Message);
                        Console.WriteLine(context.Exception.ToString());

                        return Task.CompletedTask;
                    },
                };
                var oidc = builder.Configuration.GetSection("oauth2");
                options.MetadataAddress = oidc["openidConfig"];
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = oidc["Issuer"],
                    ValidAudience = oidc["Audiance"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKeys = Services.Audit.JwtWebKeyProcessor.GetADFSKeys(builder.Configuration)
                };
            });
            builder.Services.AddScoped<CookieAuthenticationMiddleware>();

            builder.Services.AddSerilog(Log.Logger);
            builder.Services.AddScoped<MSSQLContext>();
            builder.Services.AddScopedFromBaseClass(typeof(Data));
            builder.Services.AddScoped<SvcClient>();
            builder.Services.AddScoped<SvcTeam>();
            builder.Services.AddScoped<SvcUser>();
            builder.Services.AddScoped<SvcCredential>();
            builder.Services.AddScoped<SvcEncryption>();
            builder.Services.AddScoped<UserSession>();
            builder.Services.AddScoped<SvcLdapSync>();
            builder.Services.AddScoped<JwtManager>();
            builder.Services.AddSingleton<MasterPassword>();
            builder.Services.AddScoped<AuditMiddleware>();
            builder.Services.AddTransient<StartUpEncryptionValidation>();

            var app = builder.Build();
            app.UsePathBase("/api");
            app.UseRouting();
            app.UseDefaultFiles();
            app.MapStaticAssets();
            app.UseHttpsRedirection();
            
            app.UseMiddleware<CookieAuthenticationMiddleware>();
            app.UseMiddleware<AuditMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
