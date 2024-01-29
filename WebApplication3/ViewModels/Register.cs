using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WebApplication3.ViewModels
{
    public class Register
    {

        [Required]
        public string FullName { get; set; }

        [Required]
        
        public string CreditCardNo { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string MobileNo { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{12,}$", ErrorMessage = "Password must be at least 12 characters and include at least one lowercase letter, one uppercase letter, one digit, and one special character.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? Photo { get; set; }

        [Required]
        public string AboutMe { get; set; }


    }
}
