namespace Domain
{
    public class Session
    {
        public Guid AuthToken { get; set; }
        public User User { get; set; }

        public Session()
        {
            AuthToken = Guid.NewGuid();
        }
    }
}
