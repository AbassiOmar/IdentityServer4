using AuthIds.server.Models;
using System.Threading.Tasks;

namespace AuthIds.server.ICustomStoreApis.Services
{
    public interface IUtilisateurService
    {
        public Task<User> FindByUsername(string userName);
        public Task<bool> ValidateAsync(string userId, string passWord);
    }
}
