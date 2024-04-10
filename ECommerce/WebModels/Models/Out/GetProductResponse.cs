using Domain;

namespace WebModels.Models.Out
{
    public class GetProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        public int Stock { get; set; }
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public List<Color> Colors { get; set; }
        public bool PromotionExcluded { get; set; }

        public GetProductResponse(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            Stock = product.Stock;
            Brand = product.Brand;
            Category = product.Category;
            Colors = product.Colors;
            PromotionExcluded = product.PromotionExcluded;
        }

        public override bool Equals(Object obj)
        {
            bool result = false;
            GetProductResponse product = (GetProductResponse)obj;
            if (this.Name == product.Name
                && this.Description == product.Description
                && this.Price == product.Price
                && this.Brand == product.Brand
                && this.Category == product.Category
                && this.Colors.SequenceEqual(product.Colors)
                && this.PromotionExcluded == product.PromotionExcluded)
            {
                result = true;
            }
            return result;
        }
    }
}
