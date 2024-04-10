using Domain;
using LogicInterfaces;

namespace ThreeXOne
{
    public class ThreeXOne : IPromotionImplementation
    {
        public Promotion Calculate(List<Product> products)
        {
            Promotion P = new Promotion()
            {
                Type = Promotion.PromotionType.threeXone,
                Amount = 0
            };

            bool condition = products.GroupBy(p => p.Brand.brandName)
                .Any(grupo => grupo.Count() >= 3);

            if (condition)
            {
                Product first = products.OrderBy(p => p.Price).First();
                P.Amount += first.Price;
                products.Remove(first);
                P.Amount += products.OrderBy(p => p.Price).First().Price;
            }
            return P;
        }
    }
}