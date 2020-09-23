using AuthIds.server.ICustomStoreApis.Services;
using AuthIds.server.Models;
using System.Threading.Tasks;

namespace AuthIds.server.Services
{
    public class UserService
    {
        private readonly IUtilisateurService utilisateurservice;

        public UserService(IUtilisateurService utilisateurservice)
        {
            this.utilisateurservice = utilisateurservice;
        }

        public async Task<User> FindByUsername(string userName)
        {
            return await this.utilisateurservice.FindByUsername(userName);
        }

        public async Task<bool> ValidateUserAsync(string userId,string passWord)
        {
            return await this.utilisateurservice.ValidateAsync(userId, passWord);
        }
    }
}
