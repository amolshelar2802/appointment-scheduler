using System.Data;

namespace Api.DAL.Interface
{
    public interface IDNTConnectionFactory
    {
        IDbConnection GetConnection { get; }
        void CloseConnection();
    }
}
