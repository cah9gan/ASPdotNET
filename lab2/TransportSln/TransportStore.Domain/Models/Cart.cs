namespace TransportStore.Domain.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public void AddItem(Transport transport, int quantity)
        {
            CartLine? line = Lines
                .Where(p => p.Transport.Id == transport.Id)
                .FirstOrDefault();

            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Transport = transport,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Transport transport) =>
            Lines.RemoveAll(l => l.Transport.Id == transport.Id);

        public decimal ComputeTotalValue() =>
            Lines.Sum(e => e.Transport.PricePerHour * e.Quantity);

        public void Clear() => Lines.Clear();
    }

    public class CartLine
    {
        public int CartLineID { get; set; }
        public Transport Transport { get; set; } = new();
        public int Quantity { get; set; }
    }
}