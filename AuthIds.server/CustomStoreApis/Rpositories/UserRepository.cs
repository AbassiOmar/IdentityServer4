using AuthIds.server.CustomStoreApis.Infrastructure;
using AuthIds.server.CustomStoreApis.Rpositories;
using AuthIds.server.ICustomStoreApis.Repositories;
using AuthIds.server.Models;
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
            var user = new User()
            {
                UserId=1111,
                Login = "aaa",
                FirstName = "abassi",
                LastName = "omar",
                PasseWord = "0000",
            };

            return  user;
        }

        public async  Task<User> FindByIdAsync(long userId)
        {
             var user = new User()
            {
                UserId = 1111,
                Login = "aaa",
                FirstName = "abassi",
                LastName = "omar",
                PasseWord = "0000",
            };

            return user;
        }

        public Task<bool> ValidatePassword(long userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
