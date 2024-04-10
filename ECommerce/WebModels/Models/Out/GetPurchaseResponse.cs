using Domain;

namespace WebModels.Models.Out
{
    public class GetPurchaseResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<PurchasedProduct> Products { get; set; }
        public Promotion Promotion { get; set; }
        public DateTime Date { get; set; }
        public Purchase.PaymentType PaymentMethod { get; set; }

        public string Message { get; set; }

        public GetPurchaseResponse(Purchase purchase, string message)
        {
            Id = purchase.Id;
            UserId = purchase.UserId;
            Products = purchase.Products;
            Promotion = purchase.Promotion;
            Date = purchase.Date;
            PaymentMethod = purchase.PaymentMethod;
            Message = message;
        }
    }
}
