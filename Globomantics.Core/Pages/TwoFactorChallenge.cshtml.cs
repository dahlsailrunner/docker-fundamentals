using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Globomantics.Core.Pages
{
    public class TwoFactorChallengeModel : PageModel
    {
        //private readonly UserManager<CustomUser> _userManager;

        //public TwoFactorChallengeModel(
        //    UserManager<CustomUser> userManager)
        //{
        //    _userManager = userManager;
        //}

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", 
                MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Verification Code")]
            public string Code { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl)
        {
            //var user = await _userManager.GetUserAsync(User);
            //if (user == null)
            //{
            //    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            //}
            //if (!user.TwoFactorEnabled)
            //{
            //    return NotFound($"User does not have two-factor authentication enabled. '{_userManager.GetUserId(User)}'.");
            //}

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //var user = await _userManager.GetUserAsync(User);
            //if (user == null)
            //{
            //    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            //}

            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //// Strip spaces and hypens
            //var verificationCode = Input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            //var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
            //    user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            //if (!is2faTokenValid)
            //{
            //    ModelState.AddModelError("Input.Code", "Verification code is invalid.");
            //    return Page();
            //}
            //Response.Cookies.Append("MfaChallengeCompleted", "true", new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = true,
            //    SameSite = SameSiteMode.Strict
            //});
            return LocalRedirect(returnUrl);
        }
    }
}
