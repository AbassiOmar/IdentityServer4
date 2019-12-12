using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AuthIds.server.Models;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthIds.server.Controllers
{
    [SecurityHeaders]
    public class AccountController : Controller
    {
        private static readonly TimeSpan RememberMeLoginDuration = TimeSpan.FromMinutes(20);
        private readonly IIdentityServerInteractionService interaction = null;
        private readonly ILogger logger = null;
        private readonly TestUserStore users;
        private readonly IEventService events;
        private readonly IClientStore clientStore;

        public AccountController(
          ILogger<AccountController> logger,
          IIdentityServerInteractionService interaction,
           IEventService events,
           IClientStore clientStore,
        TestUserStore users = null)
        {
            this.interaction = interaction;
            this.logger = logger;
            this.users = users ?? new TestUserStore(TestUsers.Users);
            this.events = events;
            this.clientStore = clientStore;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string returnUrl = null)
        {
            var context = await this.interaction.GetAuthorizationContextAsync(returnUrl);

            LoginViewModel model = await this.BuildLoginViewModelAsync(new LoginViewModel { ReturnUrl = returnUrl });

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button

            if (ModelState.IsValid)
            {
                model.ReturnUrl = HttpUtility.UrlDecode(model.ReturnUrl);
                this.logger.LogInformation($"Login : {model.Login}");
                if (this.users.ValidateCredentials(model.Login, model.Password))
                {
                    var user = this.users.FindByUsername(model.Login);
                    await this.events.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.SubjectId, user.Username, clientId: context?.ClientId));
                    AuthenticationProperties props = null;

                    if (model.RememberLogin)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(RememberMeLoginDuration)
                        };
                    }

                    await HttpContext.SignInAsync(user.SubjectId, user.Username, props);
                    if (context != null)
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                    }
                }
                await this.events.RaiseAsync(new UserLoginFailureEvent(model.Login, "invalid credentials"));

                this.ModelState.AddModelError(string.Empty, "Login ou mot de passe incorrect");
            }
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = await this.BuildLogoutViewModelAsync(logoutId);
            return await this.Logout(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {

            var vm = await this.BuildLoggedOutViewModelAsync(model.LogoutId);
            await this.HttpContext.SignOutAsync();

            var user = this.HttpContext.User;

            if (user?.Identity.IsAuthenticated == true)
            {
                await this.events.RaiseAsync(new UserLogoutSuccessEvent(user.GetSubjectId(), user.GetDisplayName()));
                this.logger.LogInformation($"Logout : {user.GetDisplayName()}");
            }

            return this.View("LoggedOut", vm);
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel model)
        {
            var context = await this.interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            return new LoginViewModel
            {
                ReturnUrl = model.ReturnUrl,
                Login = model.Login,
                RememberLogin = model.RememberLogin,
            };
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                return vm;
            }

            var context = await interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {

            var logout = await this.interaction.GetLogoutContextAsync(logoutId);

            return new LoggedOutViewModel
            {
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = logout?.ClientId,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };
        }
    }
}