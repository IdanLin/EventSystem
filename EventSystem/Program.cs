
using EventSystem.Services;
using EventSystem_ClassLibrary.Models;
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
            builder.Services.AddScoped<SessionRegistrationRepository>();

            builder.Services.AddScoped<EventService>();
            builder.Services.AddScoped<SessionService>();
            builder.Services.AddScoped<UserService>();

            builder.Services.AddMemoryCache();
            builder.Services.AddHttpClient();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
            {
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("corsapp");
            app.UseAuthorization();
            app.MapControllers();
            
            app.Run();
        }
    }
}
