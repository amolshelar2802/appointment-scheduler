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
