using Microsoft.EntityFrameworkCore;
using KontaktverwaltungService;

namespace KontaktverwaltungService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<KontaktverwaltungContext>(options =>
            options.UseSqlServer(
            builder.Configuration.GetConnectionString("MeineDatenbank"))
            );




            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
