using AuthIds.server.ICustomStoreApis.Repositories;
using AuthIds.server.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthIds.server.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository userRepository;
        public ProfileService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        // pour charger les revendication pour un utilisateur
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                if(!string.IsNullOrEmpty(context.Subject.Identity.Name))
                {
                    var user = await this.userRepository.FindAsync(context.Subject.Identity.Name);

                        if(user!=null)
                    {
                        var claims = GetUserClaims(user);
                        context.IssuedClaims = claims.ToList().Where(c => context.RequestedClaimTypes.Contains(c.Type)).ToList();
                    }
                }
                else
                {
                    //get subject from context (this was set ResourceOwnerPasswordValidator.ValidateAsync),
                    //where and subject was set to my user id.
                    var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

                    if (!string.IsNullOrEmpty(userId?.Value) && long.Parse(userId.Value) > 0)
                    {
                        //get user from db (find user by user id)
                        var user = await this.userRepository.FindByIdAsync(long.Parse(userId.Value));

                        // issue the claims for the user
                        if (user != null)
                        {
                            var claims = ResourceOwnerPasswordValidator.GetUserClaims(user);

                            context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        // check si l'utilisateur est autorisé d'obtenir des jetons
        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                //get subject from context (set in ResourceOwnerPasswordValidator.ValidateAsync),
                var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "user_id");

                if (!string.IsNullOrEmpty(userId?.Value) && long.Parse(userId.Value) > 0)
                {
                    var user = await this.userRepository.FindByIdAsync(long.Parse(userId.Value));

                    if (user != null)
                    {
                        if (user.IsActive)
                        {
                            context.IsActive = user.IsActive;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //handle error logging
            }
        }

        private  IEnumerable<Claim> GetUserClaims(User user)
        {
            return new List<Claim>
            {
            new Claim("user_id", user.UserId.ToString() ?? ""),
            new Claim(JwtClaimTypes.Name, (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName)) ? (user.FirstName + " " + user.LastName) : ""),
            new Claim(JwtClaimTypes.GivenName, user.FirstName  ?? ""),
            new Claim(JwtClaimTypes.FamilyName, user.LastName  ?? ""),
            new Claim(JwtClaimTypes.Email, user.Email  ?? ""),

            //roles
            new Claim(JwtClaimTypes.Role, user.Role)
            };
        }
    }
}
