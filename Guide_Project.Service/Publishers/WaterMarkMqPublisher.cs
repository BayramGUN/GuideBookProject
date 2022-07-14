using System.Text;
using System.Text.Json;
using Guide_Project.Core.DTOs;
using Guide_Project.Core.Publishers;
using Guide_Project.Core.Services;
using Guide_Project.Service.Services;
using RabbitMQ.Client;

namespace Guide_Project.Service.Publishers;

public class WaterMarkMqPublisher<TDto> : IRabbitMQPublisher<CustomerDto>
{
    private readonly IRabbitMQService _rabbitMQService;

    public WaterMarkMqPublisher(IRabbitMQService rabbitMQService)
    {
        _rabbitMQService = rabbitMQService;
    }
    public void Publish(CustomerDto customerDto)
    {
        var channel = _rabbitMQService.Connect();
        var bodyString = JsonSerializer.Serialize(customerDto.Image);
        var bodyByte = Encoding.UTF8.GetBytes(bodyString);
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(
            exchange: WaterMarkMqService.ExchangeName, 
            routingKey: WaterMarkMqService.Route, 
            basicProperties: properties,
            body: bodyByte.ToArray()
        );
    }

}