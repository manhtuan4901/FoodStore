using System.ComponentModel.DataAnnotations;

namespace FoodStore.Models.ViewModel
{
    public class RegisterVM
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? PasswordConfirm { get; set; }
    }
}
