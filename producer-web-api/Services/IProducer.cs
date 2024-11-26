using Confluent.Kafka;
using MessageContract;

namespace producer_web_api.Services
{
    public interface IProducer
    {
        Task<MessageContent> SendMessageAsync(Message<string, MessageContent> message);
    }
}
