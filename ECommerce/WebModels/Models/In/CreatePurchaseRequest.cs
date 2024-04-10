using Domain;

namespace WebModels.Models.In
{
    public class CreatePurchaseRequest
    {
        public Guid UserId { get; set; }
        public List<Product> Products { get; set; }
        public Purchase.PaymentType PaymentMethod { get; set; }

        public Purchase ToEntity()
        {
            List<PurchasedProduct> purchasedProducts = Products.Select(product => new PurchasedProduct(product)).ToList();

            return new Purchase
            {
                UserId = this.UserId,
                Products = purchasedProducts,
                PaymentMethod = this.PaymentMethod
            };
        }
    }
}
