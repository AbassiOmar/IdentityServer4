using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace AuthIds.server.CustomStoreApis.Infrastructure
{
    public class ConnectionStringFactory
    {
        public string connectionString { get; set; }
        public ConnectionStringFactory(IOptionsMonitor<ConnectionOpstions> options)
        {
            this.connectionString = options.CurrentValue?.ConnectionStringAuthIDS;
        }
        public IDbConnection Create()
        {
            return new SqlConnection(this.connectionString);
        }

    }
}
