using DataAccess.Context;
using Domain;
using Microsoft.EntityFrameworkCore;
using RepositoryInterface;

namespace DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ECommerceContext _eCommerceContext;

        public ProductRepository(ECommerceContext eCommerceContext)
        {
            _eCommerceContext = eCommerceContext;
        }

        public ProductRepository()
        {
            _eCommerceContext = new ECommerceContext();
        }

        public Product AddProduct(Product product)
        {
            _eCommerceContext.Products.Add(product);
            _eCommerceContext.SaveChanges();
            return product;
        }

        public IEnumerable<Product> GetAllProducts(string? category, string? brand, 
            string? text, int? minPrice, int? maxPrice, bool? promo)
        {
            IEnumerable<Product> ret = _eCommerceContext.Products
                .Include(p => p.Colors)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Where(p => p.Stock > 0);
            if (category != null)
            {
                ret = GetByCategory(ret, category);
            } 
            if (brand != null)
            {
                ret = GetByBrand(ret, brand);
            }
            if (text != null)
            {
                ret = FilterNameByText(ret, text);
            }
            if (minPrice != null)
            {
                ret = FilterByMinPrice(ret, (int)minPrice);
            }
            if (maxPrice != null)
            {
                ret = FilterByMaxPrice(ret, (int)maxPrice);
            }
            if (promo != null)
            {
                ret = FilterByIncludingPromo(ret, (bool)promo);
            }
            return ret;
        }

        private IEnumerable<Product> FilterByIncludingPromo(IEnumerable<Product> products, bool promo)
        {
            List<Product> ret = new List<Product>();
            foreach (Product prod in products)
            {
                if (!promo == prod.PromotionExcluded)
                {
                    ret.Add(prod);
                }
            }
            return ret;
        }

        private IEnumerable<Product> FilterByMaxPrice(IEnumerable<Product> products, int maxPrice)
        {
            List<Product> ret = new List<Product>();
            foreach (Product prod in products)
            {
                if (prod.Price <= maxPrice)
                {
                    ret.Add(prod);
                }
            }
            return ret;
        }

        private IEnumerable<Product> FilterByMinPrice(IEnumerable<Product> products, int minPrice)
        {
            List<Product> ret = new List<Product>();
            foreach (Product prod in products)
            {
                if (prod.Price >= minPrice)
                {
                    ret.Add(prod);
                }
            }
            return ret;
        }

        private IEnumerable<Product> FilterNameByText(IEnumerable<Product> products, string text)
        {
            List<Product> ret = new List<Product>();
            foreach (Product prod in products)
            {
                string prodName = prod.Name.ToUpper();
                if (prodName.Contains(text.ToUpper()))
                {
                    ret.Add(prod);
                }
            }
            return ret;
        }

        private IEnumerable<Product> GetByBrand(IEnumerable<Product> products, string brand)
        {
            List<Product> ret = new List<Product>();
            foreach (Product prod in products)
            {
                if (prod.Brand.brandName == brand)
                {
                    ret.Add(prod);
                }
            }
            return ret;
        }

        private IEnumerable<Product> GetByCategory(IEnumerable<Product> products, string category)
        {
            List<Product> ret = new List<Product>();
            foreach (Product prod in products)
            {
                if(prod.Category.categoryName == category)
                {
                    ret.Add(prod);
                }
            }
            return ret;
        }

        public Product GetProductById(Guid id) 
        {
            Product prod = _eCommerceContext.Products
                .Include(p => p.Colors)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(product => product.Id == id);

            return prod;
        }

        public Product UpdateProduct(Guid id, Product updatedProduct)
        {
            Product existingProduct = _eCommerceContext.Products
                .Include(p => p.Colors)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(prod => prod.Id == id);
            if (existingProduct != null && !existingProduct.Equals(updatedProduct))
            {
                existingProduct.Name = updatedProduct.Name;
                existingProduct.Description = updatedProduct.Description;
                existingProduct.Price = updatedProduct.Price;
                existingProduct.Stock = updatedProduct.Stock;
                existingProduct.PromotionExcluded = updatedProduct.PromotionExcluded;
                _eCommerceContext.SaveChanges();
            }
            return existingProduct;
        }

        public bool DeleteProduct(Guid id)
        {
            bool ret = false;
            Product prod = _eCommerceContext.Products
                .Include(p => p.Colors)
                .FirstOrDefault(product => product.Id == id);

            if (prod != null)
            {
                List<Color> colors = prod.Colors.ToList();
                foreach(Color color in colors)
                {
                    prod.Colors.Remove(color);
                }

                _eCommerceContext.Products.Remove(prod);
                _eCommerceContext.SaveChanges();
                ret = true;
            }
            return ret;
        }
    }
}