namespace Domain
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string categoryName { get; set; }
    }
}