using Guide_Project.Core.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Guide_Project.BackgroundService;

public class ImageWaterMarkWorker : BackgroundService
{
    private readonly IRabbitMQService _rabbitMqService;
    private readonly ILogger<ImageWaterMarkWorker> _logger;
    private readonly IModel _channel;

    public ImageWaterMarkWorker(ILogger<ImageWaterMarkWorker> logger, IRabbitMQService rabbitMqService)
    {
        _logger = logger;
        _rabbitMqService = rabbitMqService;
    }
     public override Task StartAsync(CancellationToken cancellationToken)
    {
        _channel = _rabbitMqService.Connect();
        
        return base.StartAsync(cancellationToken);
    } 
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        _channel.BasicQos(0, 1, false);

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            _channel.BasicConsme(_rabbitMqService.QueueName, false, consumer);
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
    private Task Consumer_Recieved(object sender, BasicDeliverEventArgs @event)
    {
        
    }
}
