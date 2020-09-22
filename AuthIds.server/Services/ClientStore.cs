using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AuthIds.server.Services
{
    public class ClientStore : IClientStore
    {

        private readonly ILogger logger;
        public ClientStore()
        {
           
        }
        public Task<Client> FindClientByIdAsync(string clientId)
        {
            throw new System.NotImplementedException();
        }
    }
}
