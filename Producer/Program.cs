using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
            arguments: null);

        int count = 0;
        while (true)
        {
            string message = $"MyMessage. Count: {count}";
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
            Console.WriteLine($" [x] Sent {message}");

            count++;

            await Task.Delay(5000); 
        }
    }
}
