
using Api.Models;
using System.Collections.Generic;

namespace Api.DAL.Interface
{
    public interface IPatientsRepository
    {
        IList<Patient> GetPatientsByQuery();
        Patient GetPatientById(int patientId);
        int AddPatient(Patient patient);
        bool UpdatePatient(int patientId, Patient patient);
        bool DeletePatient(int patientId); 
    }
}
