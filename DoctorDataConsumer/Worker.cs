using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Azure.Messaging.ServiceBus;
using System.Text.Json;
using System.Text.Json.Serialization;

using Api.DAL.Implementation;
using Api.DAL.Interface;
using Api.Models;


namespace DoctorDataConsumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly AppSettings _appSettings;
        private readonly IAppointmentsRepository _appointmentRepository;

        private readonly string connectionString;
        private readonly string topicName;
        private readonly string subscriptionName;

        bool stopProcess = false;


        public Worker(ILogger<Worker> logger, IOptions<AppSettings> appSettings, IAppointmentsRepository appointmentRepository)
        {
            _logger = logger;
            _appointmentRepository = appointmentRepository;
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));

            connectionString = _appSettings.AzureServiceBusSettings.ConnectionString;
            topicName = _appSettings.AzureServiceBusSettings.DoctorAzureServiceBusSettings.TopicName;
            subscriptionName = _appSettings.AzureServiceBusSettings.DoctorAzureServiceBusSettings.Subscription;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                _logger.LogInformation("connection string " + connectionString);
                _logger.LogInformation("Topics " + topicName);
                _logger.LogInformation("Subscription " + subscriptionName);

                await using (ServiceBusClient client = new ServiceBusClient(connectionString))
                {
                    // create a processor that we can use to process the messages
                    ServiceBusProcessor processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());

                    _logger.LogInformation("ServiceBusProcessor running at: {time}", DateTimeOffset.Now);

                    // add handler to process messages
                    processor.ProcessMessageAsync += ObjectMessageHandler;

                    _logger.LogInformation("ProcessMessageAsync running at: {time}", DateTimeOffset.Now);

                    // add handler to process any errors
                    processor.ProcessErrorAsync += ObjectErrorHandler;

                    _logger.LogInformation("ProcessErrorAsync running at: {time}", DateTimeOffset.Now);

                    // start processing 
                    await processor.StartProcessingAsync();

                    _logger.LogInformation("StartProcessingAsync running at: {time}", DateTimeOffset.Now);

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        if(stopProcess)
                        {
                            _logger.LogInformation("Stopping background service.....");
                            _logger.LogInformation("StopProcessingAsync");
                            await processor.StopProcessingAsync();
                            await this.StopAsync(stoppingToken);

                            _logger.LogInformation("Background service terminated.....");
                        }
                        //await processor.StopProcessingAsync();
                        //await Task.Delay(1000, stoppingToken);
                    }
                }
        }


        private async Task ObjectMessageHandler(ProcessMessageEventArgs args)
        {

            try
            {
                string body = args.Message.Body.ToString();
                //Console.WriteLine($"Received: {body} from subscription: {subscriptionName}");
                
                _logger.LogInformation("Received Body : " + body );

                if(body.ToLower() == "stop")
                {
                    _logger.LogInformation("STOP message received.....");
                    _logger.LogInformation("STOP command triggering.....");
                    stopProcess = true;
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

                // complete the message. messages is deleted from the queue. 
                await args.CompleteMessageAsync(args.Message);

            }
            catch(Exception ex)
            {
                _logger.LogInformation("Excption : " + ex.Message + "\r\n" + ex.StackTrace);
            }

            
        }

        private Task ObjectErrorHandler(ProcessErrorEventArgs args)
        {
            //Console.WriteLine(args.Exception.ToString());
            _logger.LogError(args.Exception.ToString(), args);
            return Task.CompletedTask;
        }

        
    }
}
