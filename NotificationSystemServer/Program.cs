namespace NotificationSystemServer
{
    using Google;
    using Microsoft.EntityFrameworkCore;
    using NotificationSystemServer.Data;
    using NotificationSystemServer.Workers;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ClientDbContext>(o =>
                o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

            builder.Services.AddScoped<NotificationWorker>();
            builder.Services.AddHostedService(provider =>
            {
                var scopedProvider = provider.CreateScope().ServiceProvider;
                return scopedProvider.GetRequiredService<NotificationWorker>();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}