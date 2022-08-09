using IdentityServerExample.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServerExample.CliamProvider
{
    public class ClaimProvider : IClaimsTransformation
    {

        private UserManager<User> _userManager { get; set; }

        public ClaimProvider(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                ClaimsIdentity claimsIdentity = principal.Identity as ClaimsIdentity;

                User user = await _userManager.FindByNameAsync(claimsIdentity.Name);

                if (user != null)
                {
                    if (user.BirthDay != null)
                    {
                        if (!principal.HasClaim(c => c.Type == "City"))
                        {
                            Claim cityClaim = new Claim("City", user.City, ClaimValueTypes.String, "Internal");
                            claimsIdentity.AddClaim(cityClaim);
                        }
                    }
                } 
            }
            return principal;
        }
    }
}
