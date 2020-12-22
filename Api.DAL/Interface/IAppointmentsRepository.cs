
using Api.Models;
using System.Collections.Generic;

namespace Api.DAL.Interface
{
    public interface IAppointmentsRepository
    {
        int AddPatient(Patient patient);
        bool DeletePatient(int PatientId);
        Patient GetPatientById(int patientId);
        IList<Patient> GetPatientsByQuery();
        bool UpdatePatient(int PatientId, Patient patient);

        bool AddDoctor(Doctor doctor);
        bool DeleteDoctor(int DoctorId);
        Doctor GetDoctorById(int doctorId);
        IList<Doctor> GetDoctorsByQuery();
        bool UpdateDoctor(int DoctorId, Doctor doctor);

        Appointment GetAppointmentsByDoctorId(int doctorId);
        Appointment GetAppointmentsByPatientId(int patientId);
        bool AddAppointment(Appointment appointment);
        bool DeleteAppointment(int appointmentId);

    }
}
