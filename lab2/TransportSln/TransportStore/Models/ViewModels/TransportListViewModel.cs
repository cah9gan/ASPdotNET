using TransportStore.Models;

namespace TransportStore.Models.ViewModels
{
    public class TransportListViewModel
    {
        public IEnumerable<Transport> Transports { get; set; } = Enumerable.Empty<Transport>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
        public string? CurrentCategory { get; set; } 
    }
}