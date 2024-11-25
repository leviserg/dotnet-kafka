using Confluent.Kafka;
using System.Text.Json;

namespace MessageContract
{
    public class JsonToByteArraySerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            return JsonSerializer.SerializeToUtf8Bytes(data);
        }
    }
}
