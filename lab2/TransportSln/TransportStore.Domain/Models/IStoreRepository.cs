namespace TransportStore.Domain.Models
{
    public interface IStoreRepository
    {
        IQueryable<Transport> Transports { get; }
        IQueryable<Review> Reviews { get; }

        void SaveTransport(Transport t);
        void CreateTransport(Transport t);
        void DeleteTransport(Transport t);

        void CreateReview(Review r);
        void DeleteReview(Review r);
    }
}