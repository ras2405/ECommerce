using Exceptions.LogicExceptions;

namespace Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public List<Color> Colors { get; set; }
        public bool PromotionExcluded { get; set; }


        public override bool Equals(Object obj)
        {
            if (obj == null || !(obj is Product))
            {
                return false;
            }

            Product product = (Product)obj;

            return this.Name == product.Name
                && this.Description == product.Description
                && this.Price == product.Price
                && this.Brand.brandName == product.Brand.brandName
                && this.Category.categoryName == product.Category.categoryName
                && this.EqualColors(this.Colors,product.Colors)
                && this.PromotionExcluded == product.PromotionExcluded
                && this.Stock == product.Stock;
        }

        private bool EqualColors(List<Color> colors1, List<Color> colors2)
        {
            return colors1.Count == colors2.Count && colors1.All(color1 => colors2.Any(color2 => color1.colorName == color2.colorName));
        }

        private bool AreEqual(object obj1, object obj2)
        {
            if (obj1 == null || obj2 == null)
            {
                return obj1 == obj2;
            }
            return obj1.Equals(obj2);
        }

        public void SelfValidation()
        {
            if(this.Stock <= 0)
                throw new BadRequestException("Invalid Stock: The product must have a positive amount of stock");
        }
    }
}