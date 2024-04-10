namespace Domain
{
    public class PurchasedProduct : Product
    {
        public Guid ProductId { get; set; }


        public PurchasedProduct() { }

        public PurchasedProduct(Product product)
        {
            Id = Guid.NewGuid();
            ProductId = product.Id;
            Name = product.Name;
            Price = product.Price;
            Brand = product.Brand;
            Category = product.Category;
            Colors = product.Colors;
            Description = product.Description;
        }
    }
}
