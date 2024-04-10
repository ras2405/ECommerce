using Domain;

namespace WebModels.Models.In
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        public int Stock { get; set; }
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public List<Color> Colors { get; set; }
        public bool PromotionExcluded { get; set; }

        public CreateProductRequest(Product product)
        {
            Name = product.Name;
            Description = product.Description;
            Price = product.Price; 
            Brand = product.Brand;
            Category = product.Category;
            Colors = product.Colors;
            PromotionExcluded = product.PromotionExcluded;
            Stock = product.Stock;
        }

        public CreateProductRequest() {}

        public Product toEntity()
        {
            return new Product
            {
                Name = this.Name,
                Description = this.Description,
                Price = this.Price,
                Brand = this.Brand,
                Category = this.Category,
                Colors = this.Colors,
                PromotionExcluded = this.PromotionExcluded,
                Stock = this.Stock
            };
        }
    }
}
