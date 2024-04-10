using Domain;
using Exceptions.LogicExceptions;
using Logic.Utils;
using LogicInterfaces;

namespace Logic
{
    public class PromotionLogic : IPromotionLogic
    {
        private IProductLogic _productLogic;
        public string path = "../PromotionsFiles";
        public PromotionLogic (IProductLogic productLogic)
        {
            this._productLogic = productLogic;
        }

        public string AdjustCartToStock(List<Product> products)
        {
            Dictionary<Guid, int> dic = CountProductsInPurchase(products);
            return DeleteSoldOutProducts(dic, products);
        }

        private Dictionary<Guid, int> CountProductsInPurchase(List<Product> products)
        {
            Dictionary<Guid, int> dic = new Dictionary<Guid, int>();
            foreach (Product p in products)
            {
                if (dic.ContainsKey(p.Id))
                {
                    dic[p.Id]++;
                }
                else
                {
                    dic.Add(p.Id, 1);
                }
            }
            return dic;
        }

        private string DeleteSoldOutProducts(Dictionary<Guid, int> countProductsInCart, List<Product> purchaseProducts)
        {
            string errorMessage = "";
            Dictionary<Product, int> soldOut = new Dictionary<Product, int>();

            foreach (Guid productId in countProductsInCart.Keys)
            {
                Product product = _productLogic.GetProductById(productId);
                int remainingStock = (product.Stock) - countProductsInCart[productId];
                if (remainingStock < 0)
                {
                    soldOut.Add(product, Math.Abs(remainingStock));
                    errorMessage += "- " + product.Name + ": " + soldOut[product] + " item/s" + "\n";
                }
            }

            if (soldOut.Count > 0)
            {
                foreach (Product s in soldOut.Keys)
                {
                    int deleted = 0;
                    purchaseProducts.RemoveAll(p =>
                    {
                        if (p.Equals(s))
                        {
                            deleted++;
                            return deleted <= soldOut[s];
                        }
                        return false;
                    });
                }

                if (purchaseProducts.Count == 0)
                {
                    throw new BadRequestException("Invalid cart: all products are sold out. Sorry!");
                }

                return "The following product / s have been removed from your cart" +
                    " due to insuficient stock: \n" + errorMessage;
            }
            return errorMessage;
        }

        public Promotion CalculateBestPromotion(List<Product> products)
        {
            var loadedPromotions = PromotionHelper.LoadPromotions(@path);
            var promotions = new List<Promotion>();

            foreach (IPromotionImplementation promotion in loadedPromotions)
            {
                promotions.Add(promotion.Calculate(products));
            }

            Promotion ret = promotions.OrderByDescending(p => p.Amount).First();

            if (ret.Amount == 0)
            {
                ret = null;
            }
            return ret;
        }
    }
}