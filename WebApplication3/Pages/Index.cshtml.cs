using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WebApplication3.Model;

namespace WebApplication3.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager; 
        public string DecryptedCreditCard { get; private set; }
        private readonly IHttpContextAccessor _contxt; 

 
        public IndexModel(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _contxt = httpContextAccessor;
        }

        public ApplicationUser CurrentUser { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
           
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Page();
            }

           
            CurrentUser = user;
            var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
            var protector = dataProtectionProvider.CreateProtector("MySecretKey");
            DecryptedCreditCard = protector.Unprotect(CurrentUser.CreditCardNo);

            return Page();
        }
    }
}
