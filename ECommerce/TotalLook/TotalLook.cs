using Domain;
using LogicInterfaces;

namespace TotalLook
{
    public class TotalLook : IPromotionImplementation
    {
        public Promotion Calculate(List<Product> products)
        {
            Promotion P = new Promotion()
            {
                Type = Promotion.PromotionType.total,
                Amount = 0
            };

            int minAmount = 3;

            var condition = products
                .SelectMany(p => p.Colors)
                .GroupBy(color => color)
                .Where(group => group.Count() >= minAmount)
                .Select(group => group.Key);

            if (condition.Count() > 0)
            {
                var maxPriceProduct = products.OrderByDescending(p => p.Price).First();
                P.Amount = maxPriceProduct.Price * 0.5;
            }
            return P;
        }
    }
}