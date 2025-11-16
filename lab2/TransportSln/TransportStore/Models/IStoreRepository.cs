using System.Linq;
using TransportStore.Models;

namespace TransportStore.Models
{
    public interface IStoreRepository
    {
        IQueryable<Transport> Transports { get; }
    }
}
