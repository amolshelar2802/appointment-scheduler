using System.Data;

namespace DoctorApi.Interface
{
    public interface IDNTConnectionFactory
    {
        IDbConnection GetConnection { get; }
        void CloseConnection();
    }
}
