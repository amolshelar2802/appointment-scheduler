using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Api.DAL.Implementation;
using Api.DAL.Interface;
using Api.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Azure.Messaging.ServiceBus;


namespace DoctorDataConsumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly AppSettings _appSettings;
        private readonly IAppointmentsRepository _appointmentRepository;
        private readonly string amqpUri;
        private readonly string doctorQueueName;

        //bool stopProcess = false;


        public Worker(ILogger<Worker> logger, IOptions<AppSettings> appSettings, IAppointmentsRepository appointmentRepository)
        {
            _logger = logger;
            _appointmentRepository = appointmentRepository;
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));

            amqpUri = _appSettings.RabbitMQSettings.AmqpUri;
            doctorQueueName = _appSettings.RabbitMQSettings.DoctorQueueName;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() =>
                {
                    ConsumeMessage(stoppingToken);
                });
        }


        public void ConsumeMessage(CancellationToken stoppingToken)
        {
            
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            _logger.LogInformation("listeneing on " + amqpUri);
            _logger.LogInformation("QueueName " + doctorQueueName);

            var factory = new ConnectionFactory
            {
                Uri = new Uri(amqpUri)
            };
            //var factory = new ConnectionFactory() { HostName = "localhost:15672" };
            var connection = factory.CreateConnection();
            
            _logger.LogInformation("Connection established to " + amqpUri);

            var channel = connection.CreateModel();

            _logger.LogInformation("channel created...");

            channel.QueueDeclare(doctorQueueName,
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            _logger.LogInformation("Queue declared...");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += MessageHandler;

            _logger.LogInformation("Message handler attached as an event...");

            channel.BasicConsume(doctorQueueName, true, consumer);
            
        }

        private void MessageHandler(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation("Received Body : " + message);

            if(message.ToLower() == "stop")
            {
                _logger.LogInformation("STOP message received.....");
                _logger.LogInformation("STOP command triggering.....");
                //stopProcess = true;
            }
            else
            {
                    var doctorMessage = JsonSerializer.Deserialize<DoctorMessage>(body);

                    Doctor doctor = null;
                    if(doctorMessage == null)
                    {
                        return;
                    }
                    if(doctorMessage.Doctor != null)
                    {
                        doctor = doctorMessage.Doctor;    
                    }
                    
                    var doctorId = doctorMessage.Id;

                    var command = doctorMessage.Command.ToLower();

                    switch(command)
                    {
                        case "add_doctor":
                        {
                            doctor.Id = doctorId;
                            _appointmentRepository.AddDoctor(doctor);
                            break;
                        }

                        case "update_doctor":
                        {
                            _appointmentRepository.UpdateDoctor(doctorId, doctor);
                            break;
                        }

                        case "delete_doctor":
                        {
                            _appointmentRepository.DeleteDoctor(doctorId);
                            break;
                        }

                    }

                }
        }
        
    }
}
