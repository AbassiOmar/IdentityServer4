
using AuthIds.server.CustomStoreApis.Infrastructure;
using System.Data;

namespace AuthIds.server.CustomStoreApis.Rpositories
{
    public abstract class BaseRepository
    {
        private readonly ConnectionStringFactory connectionStringFactory;

        public BaseRepository(ConnectionStringFactory connectionStringFactory)
        {
            this.connectionStringFactory = connectionStringFactory;
        }

        protected IDbConnection Connection
        {
            get
            {
                return this.connectionStringFactory.Create();
            }
        }
    }
}
