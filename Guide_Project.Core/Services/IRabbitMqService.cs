
using RabbitMQ.Client;

namespace Guide_Project.Core.Services;

public interface IRabbitMQService
{
    IModel Connect();
    void Dispose();
}