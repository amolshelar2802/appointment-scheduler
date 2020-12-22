using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Api.DAL.Interface;
using Api.Models;
using Microsoft.Extensions.Options;

namespace Api.DAL.Implementation
{
    public class AppointmentConnectionFactory : IDNTConnectionFactory
    {
        private IDbConnection _connection;
        //private readonly IOptions<NorthWindConfiguration> _configs;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly AppSettings _appSettings;

        public AppointmentConnectionFactory(IConfiguration iConfig, IOptions<AppSettings> appSettings)
        {
            //_configs = Configs;
            _configuration = iConfig;
            _appSettings = appSettings.Value;
            _connectionString = _appSettings.AppointmentDB.ConnectionString;
            //_connectionString = _configuration.GetSection("ConnectionString").Value;


        }

        public IDbConnection GetConnection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(_connectionString);
                }
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                return _connection;
            }
        }

        public void CloseConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
