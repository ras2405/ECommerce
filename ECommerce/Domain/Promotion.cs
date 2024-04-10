namespace Domain
{
    public  class Promotion
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public double Amount { get; set; }
        public PromotionType Type { get; set; }
        public enum PromotionType
        {
            twenty,
            total,
            threeXtwo,
            threeXone
        }

        public Promotion() { }
        
    }
}