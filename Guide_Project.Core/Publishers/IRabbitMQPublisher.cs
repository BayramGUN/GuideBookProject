namespace Guide_Project.Core.Publishers;

public interface IRabbitMQPublisher<TDto> 
    where TDto : class
{
    void Publish(TDto entity);
}