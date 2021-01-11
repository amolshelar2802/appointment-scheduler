using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

using Api.DAL.Implementation;
using Api.DAL.Interface;
using Api.Models;

namespace DoctorDataConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    
                    services.AddTransient<IDNTConnectionFactory, AppointmentConnectionFactory>();            
                    services.AddSingleton<IAppointmentsRepository, AppointmentsRepository>();
                    //services.AddScoped<IPatientsRepository, PatientsRepository>();

                     var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", false)
                                .Build();

                    services.AddOptions();
                    services.Configure<AppSettings>(configuration.GetSection("AppSettings"));


                });
    }
}
