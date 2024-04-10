using Domain;
using Exceptions.LogicExceptions;
using LogicInterfaces;
using RepositoryInterface;

namespace Logic
{
    public class PurchaseLogic : IPurchaseLogic
    {
        private IPurchaseRepository _purchaseRepository;
        private IUserLogic _userLogic;
        private IProductLogic _productLogic;
        private IPromotionLogic _promotionLogic;

        public PurchaseLogic(IPurchaseRepository purchaseRepository, IUserLogic userLogic, 
                                    IProductLogic productLogic, IPromotionLogic promotionLogic)
        {
            _purchaseRepository = purchaseRepository;
            _userLogic = userLogic;
            _productLogic = productLogic;
            _promotionLogic = promotionLogic;
        }

        public IEnumerable<Purchase> GetPurchases()
        {
            return _purchaseRepository.GetPurchases();
        }

        public (Purchase, string) CreatePurchase(Purchase purchase)
        {
            try
            {
                _userLogic.GetUser(purchase.UserId);
            }
            catch (NotFoundException)
            {
                throw new BadRequestException("Invalid user id.");
            }

            foreach (PurchasedProduct purchasedProduct in purchase.Products)
            {
                try
                {
                    Product product = _productLogic.GetProductById(purchasedProduct.ProductId);
               
                }
                catch (NotFoundException)
                {
                    throw new BadRequestException("Invalid product.");
                }
            }
            Dictionary<Guid, int> productsInPurchase = CountProductsInPurchase(purchase.Products);
            string modifications = DeleteSoldOutProducts(productsInPurchase, purchase.Products);
            List<Product> products = purchase.Products.Cast<Product>().ToList();
            purchase.Promotion = _promotionLogic.CalculateBestPromotion(products);
            PaganzaPaymentDiscount(purchase);
            _purchaseRepository.CreatePurchase(purchase);
            return (UpdateStock(purchase), modifications);
        }

        private void PaganzaPaymentDiscount(Purchase purchase)
        {
            double total = 0 - purchase.Promotion.Amount;
            foreach (Product p in purchase.Products)
            {
                total += p.Price;
            }
            if (purchase.PaymentMethod == Purchase.PaymentType.Paganza)
            {
                purchase.Promotion.Amount += total * 0.1;
            }
        }

        public Dictionary<Guid, int> CountProductsInPurchase(List<PurchasedProduct> products)
        {
            Dictionary<Guid, int> dic = new Dictionary<Guid, int>();
            foreach (PurchasedProduct p in products)
            {
                if (dic.ContainsKey(p.ProductId))
                {
                    dic[p.ProductId]++;
                }
                else
                {
                    dic.Add(p.ProductId, 1);
                }
            }
            return dic;
        }

        public string DeleteSoldOutProducts(Dictionary<Guid, int> countProductsInPurchase, List<PurchasedProduct> purchaseProducts)
        {
            string errorMessage = "";
            Dictionary<Product, int> soldOut = new Dictionary<Product, int>();

            foreach (Guid productId in countProductsInPurchase.Keys)
            {
                Product product = _productLogic.GetProductById(productId);
                int remainingStock = (product.Stock) - countProductsInPurchase[productId];
                if (remainingStock < 0)
                {
                    soldOut.Add(product, Math.Abs(remainingStock));
                    errorMessage += "- " + product.Name + ": " +soldOut[product] + " item/s"+ "\n";
                }
            }

            if (soldOut.Count > 0)
            {
                foreach (Product s in soldOut.Keys)
                {
                    int deleted = 0;
                    purchaseProducts.RemoveAll(p =>
                    {
                        if (p.ProductId.Equals(s.Id))
                        {
                            deleted++;
                            return deleted <= soldOut[s];
                        }
                        return false;
                    });
                }

                if (purchaseProducts.Count == 0)
                {
                    throw new BadRequestException("Invalid purchase: all products are sold out. Sorry!");
                }
                return "The following product / s have been removed from your purchase due to insuficient stock: \n" + errorMessage;
            }
            return errorMessage;
        }

        private Purchase UpdateStock(Purchase purchase)
        {
            foreach (PurchasedProduct product in purchase.Products)
            {
                Product p = _productLogic.GetProductById(product.ProductId);
                Product productCopy = new Product()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Stock = p.Stock - 1,
                    Price = p.Price,
                    Brand = p.Brand,
                    Category = p.Category,
                    Colors = p.Colors,
                    PromotionExcluded = p.PromotionExcluded
                };
                _productLogic.UpdateProduct(product.ProductId, productCopy);
            }
            return purchase;
        }

        public Purchase GetPurchase(Guid id)
        {
            Purchase purchase = _purchaseRepository.GetPurchase(id);

            if (purchase == null)
            {
                throw new NotFoundException("Purchase not found.");
            }
            return purchase;
        }

        public IEnumerable<Purchase> GetUserPurchases(Guid id)
        {
            try
            {
                _userLogic.GetUser(id);
            }
            catch (NotFoundException ex)
            {
                throw new BadRequestException("Invalid user id.");
            }
            return _purchaseRepository.GetUserPurchases(id); ;
        }
    }
}