namespace Domain
{
    public class Brand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string brandName { get; set; }
    }
}
