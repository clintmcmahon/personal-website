---
title: "How to use RabbitMQ with a C# producer and a C# consumer"
description: "Use RabbitMQ to create a system with a C# producer and a C# consumer to save messages into a PostgreSQL database using Entity Framework Core."
date: "2023-03-29"
draft: false
slug: "how-to-use-rabbitmq-with-a-c-producer-and-a-c-consumer"
tags:
---

<p>In this blog post, we will explore how RabbitMQ can be used to create a system with a C# producer and a C# consumer, and how the consumer can save messages into a PostgreSQL database using Entity Framework as the ORM. To make this even easier to set up, we will use Docker to run a local instance of RabbitMQ.</p><p>Suppose you're working for a weather monitoring company that provides real-time weather data analysis for businesses. To do this, you need access to a high volume data source like the National Oceanic and Atmospheric Administration's (NOAA) Global Forecast System (GFS). The GFS provides access to global weather data, which can be a massive amount of data to process in real-time. In this situation, a messaging service like RabbitMQ can help you manage this data effectively.</p><p>The producer will be written in C# and will connect to the GFS API to retrieve weather data. It will then send this data to RabbitMQ as messages. The consumer will also be written in C# and will receive these messages from RabbitMQ. It will then use Entity Framework as the ORM to save the messages into a PostgreSQL database.</p><p>To get started, we need to set up a local instance of RabbitMQ using Docker. First, we need to <a href="https://www.docker.com/get-started/">install Docker</a> on our machine. Once Docker is installed, we can run the following command in the terminal to pull the latest version of the RabbitMQ image:</p><p><code>docker pull rabbitmq</code></p><p>Once the image is downloaded, we can run the following command to start a RabbitMQ container:</p><p><code>docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:latest</code></p><p>This command starts a RabbitMQ container with the name <code>some-rabbit</code> and sets the hostname to <code>my-rabbit</code>. We also map the default RabbitMQ ports (<code>5672</code> and <code>15672</code>) to the corresponding ports on our machine so that we can access the RabbitMQ web interface.</p><p>Now that we have RabbitMQ up and running, we can proceed with the C# producer and consumer code. First, we need to establish a connection to the GFS API using a library like RestSharp. We can then create a RabbitMQ connection and channel using the RabbitMQ .NET client library. Once we have the channel, we can publish messages to RabbitMQ using the channel's BasicPublish method. Here's some sample code to give you an idea of what this might look like:</p><pre><code class="language-csharp">// Connect to the GFS APIvar client = new RestClient("https://api.weather.gov"); 
var request = new RestRequest("gridpoints/MLB/25,69/forecast", Method.GET); 
var response = client.Execute(request); var data = response.Content;

// Create a RabbitMQ connection and channel
var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel()) {
// Declare the exchange and queue channel.ExchangeDeclare(exchange: "weather_data", type: ExchangeType.Fanout);
channel.QueueDeclare(queue: "weather_data_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        // Publish the message to RabbitMQ
        var message = Encoding.UTF8.GetBytes(data); channel.BasicPublish(exchange: "weather_data", routingKey: "", basicProperties: null, body: message);

        Console.WriteLine(" [x] Sent {0}", data);
    }</code></pre><p>The consumer can also be written in C# using the RabbitMQ .NET client library. It can receive messages from RabbitMQ using the channel's BasicConsume method. Once it receives a message,it can use Entity Framework to save the message to the PostgreSQL database. Here's an example of what the consumer code might look like:</p><pre><code class="language-csharp">// Create a RabbitMQ connection and channel

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel()) {
// Declare the queue  
 channel.QueueDeclare(queue: "weather_data_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        // Create a consumer
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =&gt; {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);
            // Save the message to the database using Entity Framework
            using (var dbContext = new WeatherDbContext()) {
                var weatherData = new WeatherData { Data = message }; dbContext.WeatherData.Add(weatherData);
                dbContext.SaveChanges();
    	}
    };

// Start consuming messages from RabbitMQ channel.BasicConsume(queue: "weather_data_queue", autoAck: true, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine(); }</code></pre><p>In this example, we used Docker to run a local instance of RabbitMQ on our machine. We then used C# to create a producer that connected to the GFS API and sent messages to RabbitMQ, and a consumer that received these messages from RabbitMQ and used Entity Framework to save them to a PostgreSQL database. </p><p>This system can be used to manage real-time data and process it effectively, making it a valuable tool for monitoring and analysis in a high transactional environment.</p>
