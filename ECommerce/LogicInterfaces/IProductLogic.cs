using Domain;

namespace LogicInterfaces
{
    public interface IProductLogic
    {
        IEnumerable<Product> GetAllProducts(string? category, string? brand, string? text, 
            int? minPrice, int? maxPrice, bool? promo);
        Product GetProductById(Guid id);
        Product AddNewProduct(Product product);
        Product UpdateProduct(Guid id, Product product2);
        void DeleteProduct(Guid id);
    }
}