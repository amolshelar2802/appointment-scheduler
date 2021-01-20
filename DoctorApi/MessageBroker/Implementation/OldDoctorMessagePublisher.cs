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


namespace DoctorApi.MessageBroker.Implementation
{
    class OldDoctorMessagePublisher : IDoctorMessagePublisher
    {
        private string _connectionString;
        private string _topicName;

        private readonly AppSettings _appSettings;

        public OldDoctorMessagePublisher(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _connectionString = _appSettings.AzureServiceBusSettings.ConnectionString; //"Endpoint=sb://amol-service-bus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=LpyL17/4gzckMYLDejoLApmRKOo7UeBKb8wE6eAQezE=";
            _topicName = _appSettings.AzureServiceBusSettings.DoctorAzureServiceBusSettings.TopicName;//"sampletopic";
        }
        
        public async Task PublishMessage(DoctorMessage doctorMessage)
        {
            // create a Service Bus client 
            await using (ServiceBusClient client = new ServiceBusClient(_connectionString))
            {
                // create a sender for the topic
                ServiceBusSender sender = client.CreateSender(_topicName);
                
                string jsonString = JsonSerializer.Serialize(doctorMessage);
                
                await sender.SendMessageAsync(new ServiceBusMessage(jsonString));
                //Console.WriteLine($"Sent a single message to the topic: {topicName}");
            }
        }


    }

}