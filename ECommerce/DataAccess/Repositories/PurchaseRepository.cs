using DataAccess.Context;
using Domain;
using Microsoft.EntityFrameworkCore;
using RepositoryInterface;

namespace DataAccess.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private ECommerceContext _eCommerceContext;

        public PurchaseRepository(ECommerceContext context)
        {
            _eCommerceContext = context;
        }

        public Purchase CreatePurchase(Purchase purchase)
        {
            _eCommerceContext.Purchases.Add(purchase);
            _eCommerceContext.SaveChanges();
            return purchase;
        }

        public IEnumerable<Purchase> GetPurchases()
        {
            return _eCommerceContext.Purchases
                .Include(p => p.Products)
                .Include(p => p.Promotion);
        }

        public Purchase GetPurchase(Guid id)
        {
            return _eCommerceContext.Purchases
                .Include(p => p.Products)
                .Include(p => p.Promotion)
                .FirstOrDefault(user => user.Id == id);
        }

        public IEnumerable<Purchase> GetUserPurchases(Guid id)
        {
            return _eCommerceContext.Purchases
                .Where(purchase => purchase.UserId == id)
                .Include(p => p.Products)
                .Include(p=> p.Promotion);
        }
    }
}
