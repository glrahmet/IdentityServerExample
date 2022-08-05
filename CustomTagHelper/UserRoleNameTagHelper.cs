using IdentityServerExample.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerExample.CustomTagHelper
{

    [HtmlTargetElement("td", Attributes = "user-role")]
    public class UserRoleNameTagHelper : TagHelper
    {

        public UserManager<User> _userManager { get; set; }

        public UserRoleNameTagHelper(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HtmlAttributeName("user-role")]
        public string UserId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            User user = await _userManager.FindByIdAsync(UserId);
            IList<string> roles = await _userManager.GetRolesAsync(user);

            string html = string.Empty;

            roles.ToList().ForEach(x =>
            {
                html += $"<span>{x}</span>";
            });


            output.Content.SetHtmlContent(html);
        }
    }
}
