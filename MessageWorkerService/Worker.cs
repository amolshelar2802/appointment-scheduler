using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Azure.Messaging.ServiceBus;

namespace MessageWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly AppSettings _appSettings;

        static string connectionString = "Endpoint=sb://amol-service-bus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=LpyL17/4gzckMYLDejoLApmRKOo7UeBKb8wE6eAQezE=";
        static string topicName = "sampletopic";
        static string subscriptionName = "testsubscribe";

        bool stopProcess = false;
        public Worker(ILogger<Worker> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                _logger.LogInformation("connection string " + _appSettings.AzureServiceBusSettings.ConnectionString);
                _logger.LogInformation("Topics " + _appSettings.AzureServiceBusSettings.TopicName);
                _logger.LogInformation("Subscription " + _appSettings.AzureServiceBusSettings.Subscription);

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
                             _logger.LogInformation("StopProcessingAsync");
                            await processor.StopProcessingAsync();
                            await this.StopAsync(stoppingToken);
                        }
                        //await processor.StopProcessingAsync();
                        //await Task.Delay(1000, stoppingToken);
                    }
                }
        }





        private async Task ObjectMessageHandler(ProcessMessageEventArgs args)
        {

            string body = args.Message.Body.ToString();
            //Console.WriteLine($"Received: {body} from subscription: {subscriptionName}");
            
            _logger.LogInformation("Received Body : " + body );

            if(body.ToLower() == "stop")
            {
                stopProcess = true;
                
            }

            // var patient = JsonSerializer.Deserialize<Patient>(body);
            // _patientRepository.AddPatient(patient);

            // complete the message. messages is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

        private Task ObjectErrorHandler(ProcessErrorEventArgs args)
        {
            //Console.WriteLine(args.Exception.ToString());
            _logger.LogError(args.Exception.ToString(), args);
            return Task.CompletedTask;
        }

    }
}
