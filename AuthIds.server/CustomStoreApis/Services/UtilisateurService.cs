using AuthIds.server.CustomStoreApi.Rpositories;
using AuthIds.server.ICustomStoreApis.Repositories;
using AuthIds.server.ICustomStoreApis.Services;
using AuthIds.server.Models;
using System.Threading.Tasks;

namespace AuthIds.server.CustomStoreApis.Services
{
    public class UtilisateurService : IUtilisateurService
    {
        private readonly IUserRepository userRepository;
        public UtilisateurService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<User> FindByUsername(string userName)
        {
            return await this.userRepository.FindAsync(userName);
        }

        public async Task<bool> ValidateAsync(string userId, string passWord)
        {
            return await this.userRepository.ValidatePassword(userId, passWord);
        }
    }
}
