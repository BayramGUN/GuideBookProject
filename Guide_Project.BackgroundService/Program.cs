using Guide_Project.BackgroundService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        
    })
    .Build();

await host.RunAsync();
