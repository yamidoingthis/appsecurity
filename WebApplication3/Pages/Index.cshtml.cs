using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Model;

namespace WebApplication3.Pages
{
	[Authorize]
	public class IndexModel : PageModel
    {
        
        private readonly IHttpContextAccessor contxt;
       

        public IndexModel(IHttpContextAccessor httpContextAccessor)
        {
            contxt = httpContextAccessor;
            
            

        }

        public void OnGet()
        {

        }
    }
}