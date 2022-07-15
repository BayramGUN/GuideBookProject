using Guide_Project.Core.Services;
using Guide_Project.Service.Services;
using Guide_Project.Worker.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Guide_Project.Worker;

public class ImageWorker : BackgroundService
{
    private readonly ILogger<ImageWorker> _logger;
    private readonly IRabbitMQService _rabbitMQService;
    private IModel _channel;
    private readonly ImageWatermarkService _watermark;
    private readonly WaterMarkMqService _watermarkMqService;
    public ImageWorker(ILogger<ImageWorker> logger, ImageWatermarkService watermark, IRabbitMQService rabbitMQService)
    {
        _logger = logger;
        _rabbitMQService = rabbitMQService;
        _watermark = watermark;
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
        _channel.BasicConsume(_watermarkMqService.QueueName, false, consumer);
        consumer.Received += _watermark.Consumer_Recieved;
        return Task.CompletedTask;
    }



    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }
}
