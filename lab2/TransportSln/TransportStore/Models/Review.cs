using System.ComponentModel.DataAnnotations;

namespace TransportStore.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Будь ласка, введіть ваше ім'я")]
        public string User { get; set; } = string.Empty;

        [Required(ErrorMessage = "Будь ласка, напишіть коментар")]
        [StringLength(500, ErrorMessage = "Коментар не може перевищувати 500 символів")]
        public string Comment { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Оцінка має бути від 1 до 5")]
        public int Rating { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public long TransportId { get; set; }
        
        public Transport? Transport { get; set; }
    }
}