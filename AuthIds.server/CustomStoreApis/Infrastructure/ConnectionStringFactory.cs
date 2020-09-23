using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace AuthIds.server.CustomStoreApis.Infrastructure
{
    public class ConnectionStringFactory
    {
        public string connectionString { get; set; }
        public ConnectionStringFactory(IOptionsMonitor<ConnectionOptions> options)
        {
            this.connectionString = options.CurrentValue?.AuthIDS;
        }
        public IDbConnection Create()
        {
            return new SqlConnection(this.connectionString);
        }

    }
}
