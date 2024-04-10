using Domain;

namespace LogicInterfaces
{
    public interface IPromotionImplementation
    {
        Promotion Calculate(List<Product> products);
    }
}
