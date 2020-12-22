using System;

namespace Api.Models
{
    public class PatientMessage
    {
        
        public int Id { get; set; }

        public Patient Patient { get; set; }
        
        public string Command { get; set; }

    }
}
