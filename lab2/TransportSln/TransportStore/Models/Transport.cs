using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportStore.Models
{
    public class Transport
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "Будь ласка, вкажіть тип транспорту")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Будь ласка, вкажіть модель")]
        public string Model { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ціна повинна бути додатною")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PricePerHour { get; set; }

        public string Description { get; set; } = string.Empty;

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}