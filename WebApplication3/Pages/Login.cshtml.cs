using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WebApplication3.Model;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
    [ValidateAntiForgeryToken]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }
        private readonly IHttpContextAccessor contxt;
        private UserManager<ApplicationUser> userManager { get; }

        private readonly SignInManager<ApplicationUser> signInManager;
        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            contxt = httpContextAccessor;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                
                var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password, false,
                 lockoutOnFailure: true);
                if (identityResult.Succeeded)
                {
                    var user = await userManager.FindByEmailAsync(LModel.Email);
                    var userEmail = User.FindFirstValue(ClaimTypes.Email);
                    return RedirectToPage("Index");
                }
                ModelState.AddModelError("", "Username or Password incorrect");
                if (identityResult.IsLockedOut)
                {
                    ModelState.AddModelError("", "Account is locked out, please try again in 1 minute");
                    return Page();
                }

            }
            return Page();
        }
    }
}
