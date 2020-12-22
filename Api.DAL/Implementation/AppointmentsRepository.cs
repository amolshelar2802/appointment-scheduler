using Dapper;
using Api.DAL.Interface;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Api.DAL.Implementation
{
    public class AppointmentsRepository : IAppointmentsRepository
    {

        private readonly IDNTConnectionFactory _connectionFactory;

        public AppointmentsRepository(IDNTConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        #region [Patients]

         public int AddPatient(Patient patient)
        {
            string procName = "spPatientInsert";
            var param = new DynamicParameters();
            int patientId = 0;
            
            param.Add("@Id", patient.Id);
            param.Add("@FirstName", patient.FirstName);
            param.Add("@LastName", patient.LastName);
            param.Add("@DOB", patient.DOB);
            param.Add("@Gender", patient.Gender);
            param.Add("@Phone", patient.Phone);
            param.Add("@Email", patient.Email);

            try
            {
                patientId = SqlMapper.Execute(_connectionFactory.GetConnection,
                procName, param, commandType: CommandType.StoredProcedure);
            }
            finally
            {
                _connectionFactory.CloseConnection();
            }

            return patientId;
        }

        public bool DeletePatient(int PatientId)
        {
            bool IsDeleted = true;            
            var SqlQuery = @"DELETE FROM Patients WHERE id = @Id";

            using (IDbConnection conn = _connectionFactory.GetConnection)
            {
                var rowsaffected = conn.Execute(SqlQuery, new { Id = PatientId });
                if (rowsaffected <= 0)
                {
                    IsDeleted = false;  
                }
            }
            return IsDeleted;
        }

        public Patient GetPatientById(int patientId)
        {
            var Patient = new Patient();
            var procName = "spPatientFetch";
            var param = new DynamicParameters();
            param.Add("@PatientId", patientId);

            try
            {
                using (IDbConnection conn = _connectionFactory.GetConnection)
                {   
                    var result =  conn.Query<Patient>(procName, param: param, commandType: CommandType.StoredProcedure);
                    Patient = result.FirstOrDefault();
                }                
            }
            finally
            {
                _connectionFactory.CloseConnection();
            }

            return Patient;
        }


        public IList<Patient> GetPatientsByQuery()
        {
            var EmpList = new List<Patient>();
            var SqlQuery = @"SELECT [Id]
                            ,[FirstName]
                            ,[LastName]
                            ,[DOB]
                            ,[Gender]
                            ,[Phone]
                            ,[Email]
                            ,[Address]
                            ,[CreatedDate]
                        FROM [Patients]";

            using (IDbConnection conn = _connectionFactory.GetConnection)
            {   
                var result =  conn.Query<Patient>(SqlQuery);
                return result.ToList();
            }
        }


        public bool UpdatePatient(int PatientId, Patient patient)
        {
            string procName = "spPatientUpdate";
            var param = new DynamicParameters();
            bool IsSuccess = true;
            
            param.Add("@PatientId", patient.Id);
            param.Add("@FirstName", patient.FirstName);
            param.Add("@LastName", patient.LastName);
            param.Add("@DOB", patient.DOB);
            param.Add("@Gender", patient.Gender);
            param.Add("@Phone", patient.Phone);
            param.Add("@Email", patient.Email);

            try
            {
                var rowsAffected = SqlMapper.Execute(_connectionFactory.GetConnection,
                procName, param, commandType: CommandType.StoredProcedure);
                if (rowsAffected <= 0)
                {
                    IsSuccess = false;
                }                
            }
            finally
            {
                _connectionFactory.CloseConnection();
            }

            return IsSuccess;
        }


        #endregion


        #region [Doctors]

        public bool AddDoctor(Doctor doctor)
        {
            string procName = "spDoctorInsert";
            var param = new DynamicParameters();
            int rowsaffected = 0;
            
            param.Add("@Id", doctor.Id);
            param.Add("@FirstName", doctor.FirstName);
            param.Add("@LastName", doctor.LastName);
            param.Add("@Degree", doctor.Degree);
            param.Add("@Specialty", doctor.Specialty);
            param.Add("@Phone", doctor.Phone);
            param.Add("@Email", doctor.Email);

            try
            {
                rowsaffected = SqlMapper.Execute(_connectionFactory.GetConnection,
                procName, param, commandType: CommandType.StoredProcedure);
            }
            finally
            {
                _connectionFactory.CloseConnection();
            }

            return rowsaffected > 0;
        }

        public bool DeleteDoctor(int DoctorId)
        {
            bool IsDeleted = true;            
            var SqlQuery = @"DELETE FROM Doctors WHERE Id = @Id";

            using (IDbConnection conn = _connectionFactory.GetConnection)
            {
                var rowsaffected = conn.Execute(SqlQuery, new { Id = DoctorId });
                if (rowsaffected <= 0)
                {
                    IsDeleted = false;  
                }
            }
            return IsDeleted;
        }

        public Doctor GetDoctorById(int doctorId)
        {
            var Doctor = new Doctor();
            var procName = "spDoctorFetch";
            var param = new DynamicParameters();
            param.Add("@DoctorId", doctorId);

            try
            {
                using (IDbConnection conn = _connectionFactory.GetConnection)
                {   
                    var result =  conn.Query<Doctor>(procName, param: param, commandType: CommandType.StoredProcedure);
                    Doctor = result.FirstOrDefault();
                }
            }
            finally
            {
                _connectionFactory.CloseConnection();
            }

            return Doctor;
        }

        public IList<Doctor> GetDoctorsByQuery()
        {
            var EmpList = new List<Doctor>();
            var SqlQuery = @"SELECT [Id]
                            ,[FirstName]
                            ,[LastName]
                            ,[Degree]
                            ,[Specialty]
                            ,[Phone]
                            ,[Email]
                            ,[Address]
                            ,[CreatedDate]
                        FROM [dbo].[Doctors]";

            using (IDbConnection conn = _connectionFactory.GetConnection)
            {   
                var result =  conn.Query<Doctor>(SqlQuery);
                return result.ToList();
            }
        }

        public bool UpdateDoctor(int DoctorId, Doctor doctor)
        {
            string procName = "spDoctorUpdate";
            var param = new DynamicParameters();
            bool IsSuccess = true;
            
            param.Add("@DoctorId", doctor.Id);
            param.Add("@FirstName", doctor.FirstName);
            param.Add("@LastName", doctor.LastName);
            param.Add("@Degree", doctor.Degree);
            param.Add("@Specialty", doctor.Specialty);
            param.Add("@Phone", doctor.Phone);
            param.Add("@Email", doctor.Email);

            try
            {
                var rowsAffected = SqlMapper.Execute(_connectionFactory.GetConnection,
                procName, param, commandType: CommandType.StoredProcedure);
                if (rowsAffected <= 0)
                {
                    IsSuccess = false;
                }                
            }
            finally
            {
                _connectionFactory.CloseConnection();
            }

            return IsSuccess;
        }

        #endregion

        #region [Appointments]

        public Appointment GetAppointmentsByDoctorId(int doctorId)
        {
            var appointment = new Appointment();
            var procName = "spGetAppointmentsByDoctorId";
            var param = new DynamicParameters();
            param.Add("@DoctorId", doctorId);

            try
            {
                using (IDbConnection conn = _connectionFactory.GetConnection)
                {   
                    var result =  conn.Query<Appointment>(procName, param: param, commandType: CommandType.StoredProcedure);
                    appointment = result.FirstOrDefault();
                }
            }
            finally
            {
                _connectionFactory.CloseConnection();
            }

            return appointment;
        }


        public Appointment GetAppointmentsByPatientId(int patientId)
        {
            var appointment = new Appointment();
            var procName = "spGetAppointmentsByPatientId";
            var param = new DynamicParameters();
            param.Add("@patientId", patientId);

            try
            {
                using (IDbConnection conn = _connectionFactory.GetConnection)
                {   
                    var result =  conn.Query<Appointment>(procName, param: param, commandType: CommandType.StoredProcedure);
                    appointment = result.FirstOrDefault();
                }
            }
            finally
            {
                _connectionFactory.CloseConnection();
            }

            return appointment;
        }

        public bool AddAppointment(Appointment appointment)
        {
            string procName = "spAppointmentInsert";
            var param = new DynamicParameters();
            int rowsaffected = 0;
            
            param.Add("@Id", appointment.Id);
            param.Add("@PatientId", appointment.PatientId);
            param.Add("@DoctorId", appointment.DoctorId);
            param.Add("@SlotTime", appointment.SlotTime);
            param.Add("@AppointmentDate", appointment.AppointmentDate);
            param.Add("@Status", appointment.Status);
            

            try
            {
                rowsaffected = SqlMapper.Execute(_connectionFactory.GetConnection,
                procName, param, commandType: CommandType.StoredProcedure);
            }
            finally
            {
                _connectionFactory.CloseConnection();
            }

            return rowsaffected > 0;
        }


        public bool DeleteAppointment(int appointmentId)
        {
            string procName = "spAppointmentDelete";
            var param = new DynamicParameters();
            int rowsaffected = 0;
            
            param.Add("@Id", appointmentId);

            try
            {
                rowsaffected = SqlMapper.Execute(_connectionFactory.GetConnection,
                                    procName, param, commandType: CommandType.StoredProcedure);
            }
            finally
            {
                _connectionFactory.CloseConnection();
            }

            return rowsaffected > 0;
        }

        #endregion
    }
}
