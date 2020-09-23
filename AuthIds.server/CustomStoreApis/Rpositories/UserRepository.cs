using AuthIds.server.CustomStoreApis.Infrastructure;
using AuthIds.server.CustomStoreApis.Rpositories;
using AuthIds.server.ICustomStoreApis.Repositories;
using AuthIds.server.Models;
using Dapper;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AuthIds.server.CustomStoreApi.Rpositories
{
    public class UserRepository :BaseRepository, IUserRepository
    {

        public UserRepository(ConnectionStringFactory connectionStringFactory)
            :base(connectionStringFactory)
        {

        }

        public async Task<User> FindAsync(string userName)
        {
            using(IDbConnection dbconn =this.Connection )
            {

                var query = @"SELECT * FROM Utilisateur where FirstName = @userName";
                dbconn.Open();

                var res = await dbconn.QueryAsync<User>(query, new { @userName = userName });
                return res.FirstOrDefault();
            }
        }

        public async  Task<User> FindByIdAsync(long userId)
        {
            using (IDbConnection dbconn = this.Connection)
            {

                var query = @"SELECT * FROM User where userId = @userId";
                dbconn.Open();

                var res = await dbconn.QueryAsync<User>(query, new { userId = userId });
                return res.FirstOrDefault();
            }
        }

        public async Task<bool> ValidatePassword(string userName, string passeWord)
        {
            var user = await FindAsync(userName);
            if (user == null) return false;
            if (string.Equals(passeWord, user.PasseWord)) return true;
            return false;
        }
    }
}
