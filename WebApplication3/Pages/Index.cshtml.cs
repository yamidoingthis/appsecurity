using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WebApplication3.Model;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager; 
        private readonly AuthDbContext _authDbContext;
        public string DecryptedCreditCard { get; private set; }
        private readonly IHttpContextAccessor _contxt; 

 
        public IndexModel(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, AuthDbContext authDbContext)
        {
            _userManager = userManager;
            _contxt = httpContextAccessor;
            _authDbContext = authDbContext;
        }

        public string UserId { get; set; }
        public string AuthToken { get; set; }
        public ApplicationUser CurrentUser { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

           
            var user = await _userManager.GetUserAsync(User);
            var session = _contxt.HttpContext.Session;

            UserId = session.GetString("UserId");
            AuthToken = session.GetString("AuthToken");

            if (user == null)
            {
                return Page();
            }

           
            CurrentUser = user;

            _contxt.HttpContext.Session.SetString("UserId", user.Id);
            _contxt.HttpContext.Session.SetString("AuthToken", "YourAuthToken");

            // Example: Retrieve UserId and AuthToken from session
            var userIdFromSession = _contxt.HttpContext.Session.GetString("UserId");
            var authTokenFromSession = _contxt.HttpContext.Session.GetString("AuthToken");

            var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
            var protector = dataProtectionProvider.CreateProtector("MySecretKey");
            DecryptedCreditCard = protector.Unprotect(CurrentUser.CreditCardNo);

            var log = new AuditLog
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                Action = "Logged In"
            };
            _authDbContext.AuditLogTable.Add(log);
            _authDbContext.SaveChanges();

            return Page();
        }
    }
}
