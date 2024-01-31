using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Model;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
    public class LogoutModel : PageModel
    {
		private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AuthDbContext authDbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, AuthDbContext authDbContext, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor )
        {
            this.signInManager = signInManager;
            this.authDbContext = authDbContext;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        public void OnGet()
        {
        }

		public async Task<IActionResult> OnPostLogoutAsync()
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                var user = await userManager.GetUserAsync(User);

                // Check if the user is not null before accessing its properties
                if (user != null)
                {
                    var log = new AuditLog
                    {
                        UserId = user.Id,
                        Action = "Logged Out",
                        Time = DateTime.Now
                    };

                    authDbContext.AuditLogTable.Add(log);
                    authDbContext.SaveChanges();
                    await signInManager.SignOutAsync();
                    HttpContext.Session.Remove("UserId");
                    HttpContext.Session.Remove("AuthToken");
                    Response.Cookies.Delete("AuthToken");
                    user.AuthToken = null;
                    
                }
            }

            return RedirectToPage("Login");
        }
        public async Task<IActionResult> OnPostDontLogoutAsync()
		{

			return RedirectToPage("Index");
		}
	}
}
