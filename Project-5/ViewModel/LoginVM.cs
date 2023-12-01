using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Project_5.ViewModel
{
    public class LoginVM
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
