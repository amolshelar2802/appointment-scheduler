
using Microsoft.Extensions.Options;
using Microsoft.Extensions;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DoctorApi.Interface
{
    public class DNTConnectionFactory : IDNTConnectionFactory
    {
        private IDbConnection _connection;
        //private readonly IOptions<NorthWindConfiguration> _configs;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DNTConnectionFactory(IConfiguration iConfig)
        {
            //_configs = Configs;
            _configuration = iConfig;
            _connectionString = _configuration.GetSection("ConnectionString").Value;

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
