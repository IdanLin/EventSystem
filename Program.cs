
using EventSystem.Services;
using EventSystem_ClassLibrary.Repository;

namespace EventSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<EventRepository>();
            builder.Services.AddScoped<SessionRepository>();
            builder.Services.AddScoped<UserRepository>();

            builder.Services.AddScoped<EventService>();
            builder.Services.AddScoped<SessionService>();
            builder.Services.AddScoped<UserService>();


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
