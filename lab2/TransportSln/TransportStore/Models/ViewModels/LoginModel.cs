using System.ComponentModel.DataAnnotations;

namespace TransportStore.Models.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введіть ім'я користувача або Email")]
        public string Name { get; set; } = string.Empty; 

        [Required(ErrorMessage = "Введіть пароль")]
        [UIHint("password")]
        public string Password { get; set; } = string.Empty; 

        public string ReturnUrl { get; set; } = "/";
    }
}