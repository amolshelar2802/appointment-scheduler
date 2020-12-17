
namespace Api.Models
{
    public class AppSettings
    {
        public PatientDB PatientDB { get; set;}

        public DoctorDB DoctorDB { get; set;}

        public AzureServiceBusSettings AzureServiceBusSettings { get; set;}
    }

    public class PatientDB
    {
        public string ConnectionString { get; set;}

    }

    public class DoctorDB
    {
        public string ConnectionString { get; set;}

    }

    public class AzureServiceBusSettings
    {
		public string ConnectionString { get; set;}
        public PatientAzureServiceBusSettings PatientAzureServiceBusSettings { get; set;}
        public DoctorAzureServiceBusSettings DoctorAzureServiceBusSettings { get; set;}
    }
	
	public class BaseAzureServiceBusSettings
	{
		public string TopicName { get; set;}
        public string Subscription { get; set;}
	}

    public class PatientAzureServiceBusSettings
    {
        public string TopicName { get; set;}
        public string Subscription { get; set;}
    }
	
	public class DoctorAzureServiceBusSettings
    {
        public string TopicName { get; set;}
        public string Subscription { get; set;}
    }

}
