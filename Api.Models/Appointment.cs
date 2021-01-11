using System;

namespace Api.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public string SlotTime { get; set; }
        public string AppointmentDate { get; set; }
		public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        

    }
	
	
	// public enum AppointmentStatus
	// {
		// Pending,
		// Accepted,
		// Rejected
	// }
}
