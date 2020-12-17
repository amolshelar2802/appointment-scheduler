using Dapper;
using Api.DAL.Interface;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Api.DAL.Implementation
{
    public class PatientsRepository : IPatientsRepository
    {
        private readonly IDNTConnectionFactory _connectionFactory;

        public PatientsRepository(IDNTConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public int AddPatient(Patient patient)
        {
            string procName = "spPatientInsert";
            var param = new DynamicParameters();
            int patientId = 0;
            
            param.Add("@Id", patient.Id, null, ParameterDirection.Output);
            param.Add("@FirstName", patient.FirstName);
            param.Add("@LastName", patient.LastName);
            param.Add("@DOB", patient.DOB);
            param.Add("@Gender", patient.Gender);
            param.Add("@Phone", patient.Phone);
            param.Add("@Email", patient.Email);

            try
            {
                SqlMapper.Execute(_connectionFactory.GetConnection,
                procName, param, commandType: CommandType.StoredProcedure);

                patientId = param.Get<int>("@Id");
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
            var SqlQuery = @"DELETE FROM Patients WHERE PatientID = @Id";

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
                // using (var multiResult = SqlMapper.QueryMultiple(_connectionFactory.GetConnection,
                // procName, param, commandType: CommandType.StoredProcedure))
                // {
                //     Patients = multiResult.ReadFirstOrDefault<Patient>();
                //     //patients.Territories = multiResult.Read<PatientsTerritory>().ToList();
                // }

                // using (var result = SqlMapper.Query(_connectionFactory.GetConnection,
                // procName, param, commandType: CommandType.StoredProcedure))
                // {
                //     Patients = result.ReadFirstOrDefault<Patient>();
                //     //patients.Territories = multiResult.Read<PatientsTerritory>().ToList();
                // }

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
                        FROM [PatientData].[dbo].[Patients]";

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
    }
}
