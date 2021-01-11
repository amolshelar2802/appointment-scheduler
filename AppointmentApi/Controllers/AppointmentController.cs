using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Api.DAL.Interface;
using Api.DAL.Implementation;
using Api.Models;


namespace AppointmentApi.Controllers
{
    [ApiController]
    [Route("api/appointments/")]
    public class AppointmentController : ControllerBase
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAppointmentsRepository _appointmentsRepository;

        public AppointmentController(ILogger<AppointmentController> logger, 
                                        IConfiguration iConfig, 
                                        IAppointmentsRepository appointmentsRepository)
        {
            _logger = logger;
            _appointmentsRepository = appointmentsRepository;
            _configuration = iConfig;
        }

        [Route("[action]/doctorId")]
        [HttpGet]
        public ActionResult<IList<AppointmentDetails>> GetAppointmentsByDoctorId(int doctorId)
        {
            var appointments = _appointmentsRepository.GetAppointmentsByDoctorId(doctorId);
            return Ok(appointments);
        }

        [Route("[action]/patientId")]
        [HttpGet]
        public ActionResult<IList<AppointmentDetails>> GetAppointmentsByPatientId(int patientId)
        {
            var appointments = _appointmentsRepository.GetAppointmentsByPatientId(patientId);
            return Ok(appointments);
        }

        [HttpPost]
        [Route("[action]")]
        public void AddAppointment([FromBody]Appointment appointment)
        {
            _appointmentsRepository.AddAppointment(appointment);
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<Doctor> GetDoctorById(int doctorId)
        {
            var doctor = _appointmentsRepository.GetDoctorById(doctorId);
            if (doctor == null) {
                return NotFound(doctorId);
            }
            return Ok(doctor);
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<Patient> GetPatientById(int id)
        {
            var patient = _appointmentsRepository.GetPatientById(id);

            if (patient == null) {
                return NotFound(id);
            }

            return Ok(patient);
        }


        [HttpDelete]
        [Route("[action]")]
        public void DeleteAppointment(int appointmentId)
        {
            _appointmentsRepository.DeleteAppointment(appointmentId);
        }



    }
}
