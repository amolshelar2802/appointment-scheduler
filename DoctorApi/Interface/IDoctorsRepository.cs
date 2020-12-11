
using Api.Models;
using System.Collections.Generic;

namespace DoctorApi.Interface
{
    public interface IDoctorsRepository
    {
        IList<Doctor> GetDoctorsByQuery();
        Doctor GetDoctorById(int doctorId);
        int AddDoctor(Doctor doctor);
        bool UpdateDoctor(int doctorId, Doctor doctor);
        bool DeleteDoctor(int doctorId); 
    }
}
