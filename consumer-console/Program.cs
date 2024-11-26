// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;
using MessageContract;


const string topic = "kafka.learning.orders";

var conf = new ConsumerConfig
{
    GroupId = "kafka-net-consumer",//"test-consumer-group",
    BootstrapServers = "localhost:9092",
    AutoOffsetReset = AutoOffsetReset.Earliest
};


    var consumer = new ConsumerBuilder<string, MessageContent>(conf)
        .SetValueDeserializer(new ByteArrayToJsonDeserializer<MessageContent>())
        .Build();

    consumer.Subscribe(topic);

    var cts = new CancellationTokenSource();
    Console.CancelKeyPress += (_, e) => {
        e.Cancel = true; // prevent the process from terminating.
        cts.Cancel();
    };

    try
    {
        while (true)
        {
            try
            {
                var receivedMessage = consumer.Consume(cts.Token);
                Console.WriteLine($"{receivedMessage.Message.Timestamp.UtcDateTime.ToString("HH:mm:ss")} : '{receivedMessage.Value.ToString()}' at: '{receivedMessage.TopicPartitionOffset}'.");
                if (cts.IsCancellationRequested)
                {
                    consumer.Close();
                    break;
                }
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Error occurred: {e.Error.Reason}");
            }
        }
    }
    catch (OperationCanceledException)
    {
        // Ensure the consumer leaves the group cleanly and final offsets are committed.
        consumer.Close();
    }
    finally
    {
        consumer.Dispose();
    }

Console.ReadLine();
