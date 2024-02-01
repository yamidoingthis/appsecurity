using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
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
        private readonly AuthDbContext authDbContext;
        private UserManager<ApplicationUser> userManager { get; }

        private readonly SignInManager<ApplicationUser> signInManager;
        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, AuthDbContext authDbContext)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            contxt = httpContextAccessor;
            this.authDbContext = authDbContext;
        }
        public void OnGet()
        {
        }
        public class CaptchaResponse
        {
            public bool success { get; set; }
        }
		public bool CheckCaptcha()
		{
			string Response = Request.Form["g-recaptcha-response"];
			bool valid = false;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LddrmApAAAAAERo1EMFFTuvI895ZLPTGvqgjWAP&response=" + Response);
			try
			{
				using (WebResponse wResponse = request.GetResponse())
				{
					using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
					{
						string jsonResponse = readStream.ReadToEnd();
						var data = JsonSerializer.Deserialize<CaptchaResponse>(jsonResponse);
						valid = Convert.ToBoolean(data.success);
					}
				}
				return valid;
			}
			catch (WebException ex)
			{
				throw ex;
			}

		}


	public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
				if (!CheckCaptcha())
				{
					ModelState.AddModelError("", "Captcha is not valid");
					return Page();
				}
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
