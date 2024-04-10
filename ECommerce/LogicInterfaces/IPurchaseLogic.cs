using Domain;

namespace LogicInterfaces
{
    public interface IPurchaseLogic
    {
        public (Purchase,string) CreatePurchase(Purchase purchase);
        public IEnumerable<Purchase> GetPurchases();
        public Purchase GetPurchase(Guid id);
        public IEnumerable<Purchase> GetUserPurchases(Guid id);

        public string DeleteSoldOutProducts(Dictionary<Guid, int> countProductsInPurchase, List<PurchasedProduct> purchaseProducts);

        public Dictionary<Guid, int> CountProductsInPurchase(List<PurchasedProduct> products);

    }
}
