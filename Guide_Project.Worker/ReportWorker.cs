using Guide_Project.Core.Services;
using Guide_Project.Service.Services;
using Guide_Project.Worker.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Guide_Project.Worker;

public class ReportWorker : BackgroundService
{
    private readonly ILogger<ReportWorker> _logger;
    private readonly IRabbitMQService _rabbitMQService;
    private IModel _channel;
    private readonly ReportFileCreaterService _reportFile;
    private readonly ReportFileMQService _reportFileMqService;
    public ReportWorker(ILogger<ReportWorker> logger, ReportFileCreaterService reportFile, IRabbitMQService rabbitMQService)
    {
        _logger = logger;
        _rabbitMQService = rabbitMQService;
        _reportFile = reportFile;
    }
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _channel = _rabbitMQService.Connect();
        _channel.BasicQos(0, 1, false);

        return base.StartAsync(cancellationToken);
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        _channel.BasicConsume(_reportFileMqService.QueueName, false, consumer);
        consumer.Received += _reportFile.Consumer_Recieved;
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }
}
