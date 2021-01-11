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

namespace DoctorApi.MessageBroker.Interface
{
    public interface IDoctorMessagePublisher
    {
        Task PublishMessage(DoctorMessage doctorMessage);

    }

}