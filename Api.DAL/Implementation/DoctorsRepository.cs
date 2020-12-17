using Dapper;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Api.DAL.Interface;

namespace Api.DAL.Implementation
{
    public class DoctorsRepository : IDoctorsRepository
    {
        private readonly IDNTConnectionFactory _connectionFactory;

        public DoctorsRepository(IDNTConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public int AddDoctor(Doctor doctor)
        {
            string procName = "spDoctorInsert";
            var param = new DynamicParameters();
            int doctorId = 0;
            
            param.Add("@Id", doctor.Id, null, ParameterDirection.Output);
            param.Add("@FirstName", doctor.FirstName);
            param.Add("@LastName", doctor.LastName);
            param.Add("@Degree", doctor.Degree);
            param.Add("@Specialty", doctor.Specialty);
            param.Add("@Phone", doctor.Phone);
            param.Add("@Email", doctor.Email);

            try
            {
                SqlMapper.Execute(_connectionFactory.GetConnection,
                procName, param, commandType: CommandType.StoredProcedure);

                doctorId = param.Get<int>("@Id");
            }
            finally
            {
                _connectionFactory.CloseConnection();
            }

            return doctorId;
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
                // using (var multiResult = SqlMapper.QueryMultiple(_connectionFactory.GetConnection,
                // procName, param, commandType: CommandType.StoredProcedure))
                // {
                //     Doctors = multiResult.ReadFirstOrDefault<Doctor>();
                //     //doctors.Territories = multiResult.Read<DoctorsTerritory>().ToList();
                // }

                // using (var result = SqlMapper.Query(_connectionFactory.GetConnection,
                // procName, param, commandType: CommandType.StoredProcedure))
                // {
                //     Doctors = result.ReadFirstOrDefault<Doctor>();
                //     //doctors.Territories = multiResult.Read<DoctorsTerritory>().ToList();
                // }

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
    }
}
