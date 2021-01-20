 using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Api.Models;
using Microsoft.Extensions.Options;
using DoctorApi.MessageBroker.Interface;
using RabbitMQ.Client;

namespace DoctorApi.MessageBroker.Implementation
{
    class DoctorMessagePublisher : IDoctorMessagePublisher
    {
        private string _amqpUri;
        private string _queueName;
        private readonly AppSettings _appSettings;

        public DoctorMessagePublisher(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _amqpUri = _appSettings.RabbitMQSettings.AmqpUri; 
            _queueName = _appSettings.RabbitMQSettings.DoctorQueueName;
        }
        

        public async Task PublishMessage(DoctorMessage doctorMessage)
        {
            await Task.Run(() =>
                {
                    Publish(doctorMessage);
                });
        }

        public void Publish(DoctorMessage doctorMessage)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_amqpUri)
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(_queueName,
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            string jsonString = JsonSerializer.Serialize(doctorMessage);
            var body = Encoding.UTF8.GetBytes(jsonString);
            channel.BasicPublish("", _queueName, null, body);


        }


    }

}