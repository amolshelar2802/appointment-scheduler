using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Api.DAL.Implementation;
using Api.DAL.Interface;
using Api.Models;
using DoctorApi.MessageBroker.Interface;


namespace DoctorApi.Controllers
{
    [ApiController]
    [Route("api/doctors/")]
    public class DoctorController : ControllerBase
    {
        
        private readonly ILogger<DoctorController> _logger;
        private readonly IConfiguration _configuration;

        private readonly IDoctorsRepository _doctorRepository;

        private readonly IDoctorMessagePublisher _doctorMessagePublisher;

        public List<Doctor> doctors = new List<Doctor>();

        public DoctorController(ILogger<DoctorController> logger, 
                                IConfiguration iConfig, 
                                IDoctorsRepository doctorRepository,
                                IDoctorMessagePublisher doctorMessagePublisher)
        {
            _logger = logger;
            _configuration = iConfig;
            _doctorRepository = doctorRepository;
            _doctorMessagePublisher = doctorMessagePublisher;
        }

        [HttpGet]
        
        public ActionResult<IList<Doctor>> Get()
        {
            var allDoctors = _doctorRepository.GetDoctorsByQuery();
            //return new HttpResponseMessage(HttpStatusCode.OK);
            //return Request.CreateResponse(HttpStatusCode.OK, allDoctors);
            return Ok(allDoctors);
        }

        [HttpPost]
        //public void Post([FromBody]Doctor doctor)
        public async Task<ActionResult> Post([FromBody]Doctor doctor)
        {
            //doctors.Add(doctor);
            int newId = _doctorRepository.AddDoctor(doctor);

            if(newId <= 0)
            {
                return BadRequest();
            }

            DoctorMessage doctorMessage = new DoctorMessage()
            {
                Id = newId,
                Doctor = doctor,
                Command = "add_doctor"
            };

            await _doctorMessagePublisher.PublishMessage(doctorMessage);

            return Ok();

        }
        
        
        [HttpGet("{id}")]
        public ActionResult<Doctor> Get(int id)
        {
            var doctor = _doctorRepository.GetDoctorById(id);

            if (doctor == null) {
                return NotFound(id);
            }

            return Ok(doctor);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]Doctor doctor)
        {
            // var retval = _doctorRepository.UpdateDoctor(id, doctor);
            // return Ok(retval);

            var retval = _doctorRepository.UpdateDoctor(id, doctor);

            if(!retval)
            {
                return BadRequest();
            }

            DoctorMessage doctorMessage = new DoctorMessage()
            {
                Id = id,
                Doctor = doctor,
                Command = "update_doctor"
            };

            await _doctorMessagePublisher.PublishMessage(doctorMessage);

            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            //_doctorRepository.DeleteDoctor(id);

            var retval = _doctorRepository.DeleteDoctor(id);
            if(!retval)
            {
                return BadRequest();
            }

            DoctorMessage doctorMessage = new DoctorMessage()
            {
                Id = id,
                Doctor = null,
                Command = "delete_doctor"
            };

            await _doctorMessagePublisher.PublishMessage(doctorMessage);

            return Ok();

        }

        [Route("[action]")]
        [HttpGet]
        public string ConfigTest()
        {
             var constring = _configuration.GetSection("ConfigTest").Value;
             return constring;
        }
        
/*
        [HttpGet]
        public Doctor Get(int id)
        {
            var doctor = _doctorRepository.GetDoctorById(id);
            return doctor;
        }


        // [Route("[action]")]
        // [HttpGet]
        // public string ReadConfig()
        // {
        //     var constring = _configuration.GetSection("ConnectionString").Value;
        //     return constring;
        // }

        

        private IList<Doctor> GetDoctors()
        {

            List<Doctor> doctors = new List<Doctor>() {

                new Doctor(){
                Id = 1,
                FirstName = "John",
                LastName = "Devis",
                DOB = "10/10/1981",
                Email = "jdavis@gmail.com",
                Gender = "M",
                Phone = "987458965"
                },

                new Doctor(){
                Id = 2,
                FirstName = "Jenny",
                LastName = "Cruise",
                DOB = "10/10/1975",
                Email = "jcruise@gmail.com",
                Gender = "F",
                Phone = "895484232"
                } 
            };

            return doctors;
        }
*/
    }
}
