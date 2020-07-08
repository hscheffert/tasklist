using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
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
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) // Is User authenticated by Azure AD
            {
                var currentUser = _httpContextAccessor.HttpContext.User;
                var email = currentUser.FindFirst(ClaimTypes.Email);
                var user = Users.GetByEmail(email?.Value);

                var claims = new List<Claim>();
                claims.Add(new Claim("Role", "User"));

                if (user != null && user.IsSupervisor)
                {
                    claims.Add(new Claim("Role", "Admin"));
                    claims.Add(new Claim("AdminRole", "true"));                    
                }

                var identity = new ClaimsIdentity(claims, "UserSpecified");
                currentUser.AddIdentity(identity);
                User.AddIdentity(identity);

                if (user == null && !String.IsNullOrEmpty(email.Value))
                {
                    // Create a user for them.
                    Users.Save(new UserDTO()
                    {
                        Email = email.Value,
                        FirstName = currentUser.FindFirst(ClaimTypes.GivenName).Value,
                        LastName = currentUser.FindFirst(ClaimTypes.Surname).Value,
                        IsActive = true,
                        IsSupervisor = false, // or check their group
                        SupervisorId = null,
                    });
                }
            }
            else
            {
                // Trigger Azure AD authentication (using redirection)
                return Challenge(AzureADDefaults.OpenIdScheme);
            }

            // Redirect to home page is successfully authenticated
            return Redirect("/");
        }

        // GET api/auth/me
        [HttpGet]
        [Route("me")]
        public UserDTO GetMe()
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            var email = currentUser.FindFirst(ClaimTypes.Email);
            var user = Users.GetByEmail(email?.Value);

            return user;
        }
    }
}
