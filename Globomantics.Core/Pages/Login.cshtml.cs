using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Globomantics.Core.Pages
{
    public class LoginModel : PageModel
    {
        public IActionResult OnGet()
        {
            return LocalRedirect("/");
        }
    }
}
