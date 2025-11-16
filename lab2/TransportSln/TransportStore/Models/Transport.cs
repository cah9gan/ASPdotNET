namespace TransportStore.Models
{
    public class Transport
    {
        public long? Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal PricePerHour { get; set; }
    }
}
