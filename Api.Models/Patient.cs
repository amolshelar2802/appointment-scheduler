using System;

namespace Api.Models
{
    public class Patient
    {
        

        public int Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string DOB { get; set; }

        public string Gender { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
