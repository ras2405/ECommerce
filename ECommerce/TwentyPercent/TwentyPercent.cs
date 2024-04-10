using Domain;
using LogicInterfaces;

namespace TwentyPercent
{
    public class TwentyPercent : IPromotionImplementation
    {
        public Promotion Calculate(List<Product> products)
        {
            Promotion P = new Promotion()
            {
                Type = Promotion.PromotionType.twenty,
                Amount = 0
            };

            double maxPrice = double.MinValue;

            foreach (Product p in products)
            {
                if (p.Price > maxPrice)
                {
                    maxPrice = p.Price;
                }
            }
            P.Amount = maxPrice * 0.2;
            return P;
        }
    }
}