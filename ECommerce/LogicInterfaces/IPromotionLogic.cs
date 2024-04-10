using Domain;

namespace LogicInterfaces
{
    public interface IPromotionLogic
    {
        Promotion CalculateBestPromotion(List<Product> products);

        string AdjustCartToStock(List<Product> products);
    }
}