using Guide_Project.Data;
using Guide_Project.Worker;
using Guide_Project.Worker.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(context.Configuration.GetConnectionString("SqlCon"), action =>
            {
                action.MigrationsAssembly("Guide_Project.Data");
            });
        });
        services.AddSingleton(sp => new ConnectionFactory() 
        { 
            Uri = new Uri(context.Configuration.GetConnectionString("RabbitMQCon"))
        });
        services.AddMassTransit(x =>
        { 
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddScoped<ImageWatermarkService>();
        services.AddHostedService<ImageWorker>();
    })
    .Build();

await host.RunAsync();
