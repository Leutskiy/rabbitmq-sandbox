using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.MainApp.PriorityQueue
{
	public sealed class PriorityQueueRmQ<TData>
	{
		private readonly Random _random = new Random();

		private readonly string _hostname;
		private readonly string _password;
		private readonly string _queueName;
		private readonly string _username;

		private IConnection _connection;

		private readonly Dictionary<string, object> _arguments;

		public PriorityQueueRmQ(IOptions<RabbitMqConfiguration> rabbitMqOptions)
		{
			_queueName = rabbitMqOptions.Value.QueueName;
			_hostname = rabbitMqOptions.Value.Hostname;
			_username = rabbitMqOptions.Value.UserName;
			_password = rabbitMqOptions.Value.Password;

			_arguments = new Dictionary<string, object>(1);
			_arguments.Add("x-max-priority", 3);

			CreateConnection();
		}

		public void Enqueue(TData data)
		{
			if (ConnectionExists())
			{
				using (var channel = _connection.CreateModel())
				{
					channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: _arguments);

					var json = JsonConvert.SerializeObject(data);
					var body = Encoding.UTF8.GetBytes(json);

					var properties = channel.CreateBasicProperties();
					properties.Priority = (byte)_random.Next(1, 3);

					channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
				}
			}
		}

		private void CreateConnection()
		{
			try
			{
				var factory = new ConnectionFactory
				{
					HostName = _hostname,
					UserName = _username,
					Password = _password
				};

				_connection = factory.CreateConnection();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Could not create connection: {ex.Message}");
			}
		}

		private bool ConnectionExists()
		{
			if (_connection != null)
			{
				return true;
			}

			CreateConnection();

			return _connection != null;
		}
	}
}
