using Guide_Project.Worker;
using Guide_Project.Worker.Services;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
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
