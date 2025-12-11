using System.ComponentModel.DataAnnotations;

namespace TransportStore.Models.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Введіть ім'я користувача")]
        public string Name { get; set; } = string.Empty; 

        [Required(ErrorMessage = "Введіть Email")]
        [EmailAddress(ErrorMessage = "Некоректний формат Email")]
        public string Email { get; set; } = string.Empty; 

        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty; 

        [Required(ErrorMessage = "Підтвердіть пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}