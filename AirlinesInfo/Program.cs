﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "Q",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        Console.WriteLine("Waiting for info...");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("{0}", message);
        };
        channel.BasicConsume(queue: "Q",
                             autoAck: true,
                             consumer: consumer);
        Console.ReadLine();

    }
}