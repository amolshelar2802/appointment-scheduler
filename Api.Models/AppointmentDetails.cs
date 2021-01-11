using System;

namespace Api.Models
{
    public class AppointmentDetails : Appointment
    {
        
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public string Degree { get; set; }
        public string Specialty { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }

    }
	
	
	// public enum AppointmentStatus
	// {
		// Pending,
		// Accepted,
		// Rejected
	// }
}
