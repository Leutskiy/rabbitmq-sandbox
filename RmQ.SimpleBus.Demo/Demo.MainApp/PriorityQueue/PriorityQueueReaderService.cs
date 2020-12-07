using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.MainApp.PriorityQueue
{
    public sealed class PriorityQueueReaderService : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;

        private readonly ILogger<PriorityQueueReaderService> _logger;

        public PriorityQueueReaderService(IOptions<RabbitMqConfiguration> rabbitMqOptions, ILogger<PriorityQueueReaderService> logger, ICloneable conta)
        {
            _logger = logger;

            _hostname = rabbitMqOptions.Value.Hostname;
            _queueName = rabbitMqOptions.Value.QueueName;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;

            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password,
            };

            // set the heartbeat timeout to 60 seconds https://www.rabbitmq.com/heartbeats.html
            factory.RequestedHeartbeat = TimeSpan.FromSeconds(60);

            _connection = factory.CreateConnection();

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        private void HandleMessage(Notifaction notifaction, int priority)
        {
            _logger.LogInformation($"{nameof(notifaction)}: {notifaction.Id} and {notifaction.MessageText} --- {priority}");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _logger.LogInformation("Create the comsnumer!");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                _logger.LogInformation("Starting...");

                var messagePriority = ea.BasicProperties.Priority;

                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var notificationModel = JsonConvert.DeserializeObject<Notifaction>(content);

                HandleMessage(notificationModel, messagePriority);

                _logger.LogInformation("Finished!");

                _channel.BasicAck(ea.DeliveryTag, false);

                _logger.LogInformation("The Ack action  is performed!");
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(_queueName, false, consumer);

            _logger.LogInformation("Subscribing is finished!");

            return Task.CompletedTask;
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();

            base.Dispose();
        }
    }
}
