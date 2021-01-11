using System;

namespace Api.Models
{
    public class DoctorMessage
    {
        
        public int Id { get; set; }

        public Doctor Doctor { get; set; }
        
        public string Command { get; set; }

    }
}
