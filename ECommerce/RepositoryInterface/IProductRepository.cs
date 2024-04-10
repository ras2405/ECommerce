using Domain;

namespace RepositoryInterface
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts(string? category, string? brand, string? text,
            int? minPrice, int? maxPrice, bool? promo);
        Product GetProductById(Guid id);
        Product AddProduct(Product product);
        Product UpdateProduct(Guid id, Product product);
        bool DeleteProduct(Guid id);
    }
}