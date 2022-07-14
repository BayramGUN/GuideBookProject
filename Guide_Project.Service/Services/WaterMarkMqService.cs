using Guide_Project.Core.Services;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Guide_Project.Service.Services;

public class WaterMarkMqService : IRabbitMQService
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private readonly ILogger<WaterMarkMqService> _logger;

    public static string ExchangeName = "ImageDirectExchange";
    public static string Route = "ImageDirectExchange";
    public string QueueName = "RabbitMqImagemarkerQueue";


    public WaterMarkMqService(ConnectionFactory connectionFactory, ILogger<WaterMarkMqService> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }
    public IModel Connect()
    {
        _connection = _connectionFactory.CreateConnection();

        if(_channel is {IsOpen : true})
            return _channel;

        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(ExchangeName, type:"direct", true, false);

        _channel.QueueDeclare(
            QueueName,
            durable: true, 
            exclusive: false, 
            autoDelete: false,
            null
        );

        _channel.QueueBind(
            exchange:ExchangeName, 
            queue:QueueName, 
            routingKey: Route    
        );

        _logger.LogInformation("Connected with RabbitMQ!");

        return _channel;

    }
    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();

        _connection?.Close(); 
        _connection?.Dispose();

        _logger.LogInformation("Disconnected with RabbitMQ!"); 
    }

}