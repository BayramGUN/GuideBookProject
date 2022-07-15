using System.Text;
using System.Text.Json;
using Guide_Project.Core.DTOs;
using Guide_Project.Core.Publishers;
using Guide_Project.Core.Services;
using Guide_Project.Service.Services;
using RabbitMQ.Client;

namespace Guide_Project.Service.Publishers;

public class ReportFileMQPublisher<TDto> : IRabbitMQPublisher<ReportMessageDto>
{
    private readonly IRabbitMQService _rabbitMQService;

    public ReportFileMQPublisher(IRabbitMQService rabbitMQService)
    {
        _rabbitMQService = rabbitMQService;
    }
    public void Publish(ReportMessageDto reportMessageDto)
    {
        var channel = _rabbitMQService.Connect();
        var bodyString = JsonSerializer.Serialize(reportMessageDto);
        var bodyByte = Encoding.UTF8.GetBytes(bodyString);
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(
            exchange: ReportFileMQService.ExchangeName, 
            routingKey: ReportFileMQService.ReporterRoute, 
            basicProperties: properties,
            body: bodyByte.ToArray()
        );
    }

}