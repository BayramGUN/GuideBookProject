using Guide_Project.Data;
using Guide_Project.Worker;
using Guide_Project.Worker.Jobs;
using Guide_Project.Worker.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Quartz;
using RabbitMQ.Client;
using SendGrid.Extensions.DependencyInjection;

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
        //SendGrid Api From https://app.sendgrid.com to send email.
        services.AddSendGrid(options =>
        options.ApiKey = ctx.Configuration["SendGrid:SendGridApiKey"]);

        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            q.ScheduleJob<MailSendJob>(trigger => trigger
                .WithIdentity("SendRecurringMailTrigger")
                .WithSimpleSchedule(s =>
                    s.WithIntervalInSeconds(15)
                    .RepeatForever()
                )
                .WithDescription("This trigger will run every week to send report emails.")
            );
        });
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
        services.AddScoped<ImageWatermarkService>();
        services.AddHostedService<ImageWorker>();
        services.AddHostedService<ReportWorker>();
    })
    .Build();

await host.RunAsync();
