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

        public LogoutModel(SignInManager<ApplicationUser> signInManager, AuthDbContext authDbContext, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.authDbContext = authDbContext;
            this.userManager = userManager;
        }
        public void OnGet()
        {
        }

		public async Task<IActionResult> OnPostLogoutAsync()
		{
            
            var user = await userManager.GetUserAsync(User);
            var log = new AuditLog
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                Action = "Logged Out"
            };
            authDbContext.AuditLogTable.Add(log);
            authDbContext.SaveChanges();
            await signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToPage("Login");
		}
		public async Task<IActionResult> OnPostDontLogoutAsync()
		{

			return RedirectToPage("Index");
		}
	}
}
