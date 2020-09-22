using AuthIds.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthIds.server.ICustomStoreApis.Repositories
{
   public interface IUserRepository
    {
        public Task<User> FindAsync(string userName);

        public Task<User> FindByIdAsync(long userId);

        public Task<bool> ValidatePassword(long userId); 
    }
}
