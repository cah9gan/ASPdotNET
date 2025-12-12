using TransportStore.Domain.Models; 

namespace TransportStore.Models.ViewModels
{
    public class CartViewModel
    {
        public Cart Cart { get; set; } = new Cart();
        
        public string ReturnUrl { get; set; } = string.Empty;
    }
}