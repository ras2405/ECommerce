using Domain;
using Exceptions.LogicExceptions;
using LogicInterfaces;
using RepositoryInterface;

namespace Logic
{
    public class ProductLogic : IProductLogic
    {
        private IProductRepository _productRepository;

        public ProductLogic(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<Product> GetAllProducts(string? category, string? brand, 
            string? text, int? minPrice, int? maxPrice, bool? promo)
        {
            if (_productRepository != null)
            {
                return _productRepository.GetAllProducts(category, brand, text, minPrice, maxPrice, promo);
            }
            else
            {
                throw new Exception("Can't get all products");
            }
        }

        public Product GetProductById(Guid id) 
        {
            if (_productRepository != null)
            {
                Product product = _productRepository.GetProductById(id);
                if (product is null)
                    throw new NotFoundException("Product not found");
                return product;
            }
            else
            {
                throw new Exception("Can't get product");
            }
        }

        public Product AddNewProduct(Product product)
        {
            product.SelfValidation();
            if (_productRepository != null)
            {
                return _productRepository.AddProduct(product);
            }
            else
            {
                throw new Exception("Can't add new product");
            }
        }

        public Product UpdateProduct(Guid id, Product product)
        {
            if (_productRepository != null)
            {
                Product prod = _productRepository.UpdateProduct(id, product);
                if (prod != null)
                {
                    return prod;
                }
                else
                {
                    throw new NotFoundException("Product not found");
                }
            }
            else
            {
                throw new Exception("Can't update product");
            }
        }

        public void DeleteProduct(Guid id)
        {
            if (_productRepository != null)
            {
                if (!_productRepository.DeleteProduct(id))
                {
                    throw new NotFoundException("Product with id:" + id + "does not exist");
                }
            }
            else
            {
                throw new Exception("Can't delete product");
            }
        }
    }
}