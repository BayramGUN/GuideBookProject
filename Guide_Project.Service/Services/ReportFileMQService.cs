using Guide_Project.Core.Services;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Guide_Project.Service.Services;
//There is a problem because I did same jobs. It maybe Causing a violation of SOLID rules 
public class ReportFileMQService : IRabbitMQService
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private readonly ILogger<ReportFileMQService> _logger;

    public static string ExchangeName = "ReportFileDirectExchange";
    public static string ReporterRoute = "reportFile-route";
    public string QueueName = "queue-excel-reporter";
    public ReportFileMQService(ConnectionFactory connectionFactory, ILogger<ReportFileMQService> logger)
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
            exchange: ExchangeName, 
            queue: QueueName, 
            routingKey: ReporterRoute    
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