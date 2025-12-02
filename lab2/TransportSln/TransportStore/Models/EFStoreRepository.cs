using Microsoft.EntityFrameworkCore;

namespace TransportStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private StoreDbContext context;
        public EFStoreRepository(StoreDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Transport> Transports => context.Transports
            .Include(t => t.Reviews);
            
        public IQueryable<Review> Reviews => context.Reviews;

        public void CreateTransport(Transport t)
        {
            context.Add(t);
            context.SaveChanges();
        }

        public void SaveTransport(Transport t)
        {
            if (t.Id == 0 || t.Id == null)
            {
                context.Transports.Add(t);
            }
            else
            {
                Transport? dbEntry = context.Transports.FirstOrDefault(p => p.Id == t.Id);
                
                if (dbEntry != null)
                {
                    dbEntry.Type = t.Type;
                    dbEntry.Model = t.Model;
                    dbEntry.PricePerHour = t.PricePerHour;
                }
            }
            context.SaveChanges();
        }

        public void DeleteTransport(Transport t)
        {
            context.Remove(t);
            context.SaveChanges();
        }


        public void CreateReview(Review r)
        {
            context.Reviews.Add(r);
            context.SaveChanges();
        }

        public void DeleteReview(Review r)
        {
            context.Reviews.Remove(r);
            context.SaveChanges();
        }
    }
}