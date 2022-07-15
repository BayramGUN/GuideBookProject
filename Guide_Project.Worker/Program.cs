using Guide_Project.Data;
using Guide_Project.Worker;
using Guide_Project.Worker.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(ctx.Configuration.GetConnectionString("SqlCon"), action =>
            {
                action.MigrationsAssembly("Guide_Project.Data");
            });
        });
        services.AddSingleton(sp => new ConnectionFactory() 
        { 
            Uri = new Uri(ctx.Configuration.GetConnectionString("RabbitMQCon"))
        });
        services.AddMassTransit(x =>
        { 
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h => {
                    h.Username(ctx.Configuration["RabbitMQOptions:UserName"]);
                    h.Password(ctx.Configuration["RabbitMQOptions:Password"]);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddScoped<ImageWatermarkService>();
        services.AddHostedService<ImageWorker>();
        services.AddHostedService<ReportWorker>();
    })
    .Build();

await host.RunAsync();
