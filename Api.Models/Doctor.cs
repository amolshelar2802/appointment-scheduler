using System;

namespace Api.Models
{
    public class Doctor
    {
        
        public int Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Degree { get; set; }

        public string Specialty { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

		public DateTime CreatedDate { get; set; }

    }
}
