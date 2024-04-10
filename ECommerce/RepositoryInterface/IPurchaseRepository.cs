using Domain;

namespace RepositoryInterface
{
    public interface IPurchaseRepository
    {
        public Purchase CreatePurchase(Purchase purchase);
        public IEnumerable<Purchase> GetPurchases();
        public Purchase GetPurchase(Guid id);
        public IEnumerable<Purchase> GetUserPurchases(Guid id);
    }
}
