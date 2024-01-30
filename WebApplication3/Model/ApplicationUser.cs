using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string CreditCardNo { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string DeliveryAddress { get; set; }
        public string Email { get; set; }
        public string AboutMe { get; set; }
       
        public string? AuthToken { get; set; }
      
    }
}
