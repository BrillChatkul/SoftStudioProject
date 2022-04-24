using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webBudda.Models
{
    public class UserModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class UserFirebase
    {
        public string Id { get; set; }
        public string Email { get; set;}
        public string password { get; set; }
        public Boolean Ban { get; set; }
    }
}
