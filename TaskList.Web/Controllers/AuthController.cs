using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskList.Business.Helpers;
using TaskList.Core.DTOs;

namespace TaskList.Web.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated) // Is User authenticated by Azure AD
            {
                var currentUser = _httpContextAccessor.HttpContext.User;
                var email = currentUser.GetCurrentUserEmail();
                var user = Users.GetByEmail(email);

                if (user == null && !String.IsNullOrEmpty(email))
                {
                    // Create a user for them.
                    Users.Save(new UserDTO()
                    {
                        Email = email,
                        FirstName = currentUser.FindFirstValue(ClaimTypes.GivenName),
                        LastName = currentUser.FindFirstValue(ClaimTypes.Surname),
                        IsActive = true,
                        IsSupervisor = false,
                        SupervisorId = null,
                    }, email);
                }
            }
            else
            {
                // Trigger Azure AD authentication (using redirection)
                return Challenge(AzureADDefaults.OpenIdScheme);
            }

            // Redirect to home page if successfully authenticated
            return Redirect("/");
        }

        // GET api/auth/me
        [HttpGet]
        [Route("me")]
        public UserDTO GetMe()
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            var email = currentUser.GetCurrentUserEmail();
            var user = Users.GetByEmail(email);

            // Note: Would be good to add some check here for user.IsSupervisor compared to the AdminRole claim
            // to ensure they are still in sync...but for now, ignoring that scenario and assuming they log in/out often.

            return user;
        }
    }
}
