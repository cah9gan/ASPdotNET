using System.ComponentModel.DataAnnotations;

namespace TransportStore.BlazorClient.Models
{
    public class TransportDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Тип транспорту є обов'язковим")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Модель є обов'язковою")]
        public string Model { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, 100000, ErrorMessage = "Ціна повинна бути від 1 до 100000")]
        public decimal PricePerHour { get; set; }
    }
}