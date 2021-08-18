using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using WebMvc.Models;

namespace WebMvc.Services
{
    public class IdentityService : IIdentityService<ApplicationUser>
    {
        public ApplicationUser Get(IPrincipal principal) //this method will get called after the token service issues the token
        {
            if (principal is ClaimsPrincipal claims) //if this principal is a claim i.e if it is already a Token then
            {
                var user = new ApplicationUser() //a new User,  already a token
                {
                    Email = claims.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value ?? "", //Read the first claim, thats the preferred_username(will already be selected) ands thats email address and thast the same thing as id
                    Id = claims.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value ?? "", //same information for both email and id
                };

                return user;

            }

            throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}
