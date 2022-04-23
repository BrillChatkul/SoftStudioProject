using System.ComponentModel.DataAnnotations;

namespace webBudda.Models
{
    public class SignInModel
    {
        [Key]
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
