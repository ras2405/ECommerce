using Domain;
using LogicInterfaces;

namespace ThreeXTwo
{
    public class ThreeXTwo : IPromotionImplementation
    {
        public Promotion Calculate(List<Product> products)
        {
            Promotion P = new Promotion()
            {
                Type = Promotion.PromotionType.threeXtwo,
                Amount = 0
            };

            int minAmount = 3;

            bool condition = products.GroupBy(p => p.Category.categoryName)
                .Any(grupo => grupo.Count() >= 3);

            if (condition)
            {
                var minPriceProduct = products.OrderByDescending(p => p.Price).Last();
                P.Amount = minPriceProduct.Price;
            }
            return P;
        }
    }
}