using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;
using WebApplication3.Model;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
    [ValidateAntiForgeryToken]
    public class RegisterModel : PageModel
    {

        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }

        [BindProperty]
        public Register RModel { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }



        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");

                var user = new ApplicationUser()
                {
					UserName = HttpUtility.HtmlEncode(RModel.Email),
					Email = HttpUtility.HtmlEncode(RModel.Email),
					FullName = HttpUtility.HtmlEncode(RModel.FullName),
					CreditCardNo = protector.Protect(HttpUtility.HtmlEncode(RModel.CreditCardNo)),
					Gender = HttpUtility.HtmlEncode(RModel.Gender),
					MobileNo = HttpUtility.HtmlEncode(RModel.MobileNo),
					DeliveryAddress = HttpUtility.HtmlEncode(RModel.DeliveryAddress),
					AboutMe = HttpUtility.HtmlEncode(RModel.AboutMe)
				};

                // Set the password using UserManager
                var result = await userManager.CreateAsync(user, RModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToPage("Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return Page();
        }
    }
}


