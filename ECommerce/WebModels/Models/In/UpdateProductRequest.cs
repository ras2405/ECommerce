using Domain;

namespace WebModels.Models.In
{
    public class UpdateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        public int Stock { get; set; }
 
        public bool PromotionExcluded { get; set; }

        public UpdateProductRequest(Product product)
        {
            Name = product.Name;
            Description = product.Description;
            Price = product.Price; 
            PromotionExcluded = product.PromotionExcluded;
            Stock = product.Stock;
        }

        public UpdateProductRequest() {}

        public Product toEntity()
        {
            return new Product
            {
                Name = this.Name,
                Description = this.Description,
                Price = this.Price,
                PromotionExcluded = this.PromotionExcluded,
                Stock = this.Stock
            };
        }
    }
}
