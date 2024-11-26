using Confluent.Kafka;
using MessageContract;
using System.Diagnostics;

namespace producer_web_api.Services
{
    public class Producer : IProducer
    {
        const string topic = "kafka.learning.orders";

        public async Task<MessageContent> SendMessageAsync(Message<string, MessageContent> message)
        {

            ProducerConfig config = new ProducerConfig
            {
                // User-specific properties that you must set
                BootstrapServers = "localhost:9092",

                // exact once
                EnableIdempotence = true,
                MaxInFlight = 5,
                MessageSendMaxRetries = 3,

                //SaslUsername = "<SSAL_USERNAME>",
                //SaslPassword = "<SSAL_PASSWORD>",

                // Fixed properties
                SecurityProtocol = SecurityProtocol.Plaintext,//SaslSsl, //Plaintext,SaslPlaintext
                //SaslMechanism = SaslMechanism.Plain,
                Acks = Acks.All
            };

            var producer = new ProducerBuilder<string, MessageContent>(config)
                .SetValueSerializer(new JsonToByteArraySerializer<MessageContent>())
                .Build();

            try
            {

                producer.Produce(topic, message,
                (deliveryReport) =>
                {
                    if (deliveryReport.Error.IsError)
                    {
                        throw new ProduceException<string, MessageContent>(deliveryReport.Error, deliveryReport);
                    }
                    else
                    {
                        Debug.WriteLine(deliveryReport.Key + "\t" + deliveryReport.Value + "\t" + deliveryReport.Timestamp);
                    }
                });

                producer.Flush(TimeSpan.FromSeconds(20));

                return await Task.FromResult(message.Value);

            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                producer.Dispose();
            }

        }
    }
}
