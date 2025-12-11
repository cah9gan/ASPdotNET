using System.ComponentModel.DataAnnotations;

namespace TransportStore.Models.ViewModels
{
    public class ProfileModel
    {
        public string Id { get; set; } = string.Empty; 
        
        [Display(Name = "Ім'я користувача")]
        public string UserName { get; set; } = string.Empty; 
        
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty; 
        
        [Phone]
        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; } = string.Empty; 

        [DataType(DataType.Password)]
        [Display(Name = "Новий пароль (залиште пустим, якщо не змінюєте)")]
        public string? NewPassword { get; set; }
    }
}