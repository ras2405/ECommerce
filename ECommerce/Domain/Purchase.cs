namespace Domain
{
    public class Purchase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public List<PurchasedProduct> Products { get; set; }
        public Promotion Promotion { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public PaymentType PaymentMethod { get; set; }

        public enum PaymentType
        {
            Visa,
            Mastercard,
            Santander,
            Itau,
            BBVA,
            Paypal,
            Paganza
        }
    }
}
