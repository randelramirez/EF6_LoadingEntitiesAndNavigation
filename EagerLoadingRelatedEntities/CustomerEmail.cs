namespace EagerLoadingRelatedEntities
{
    public class CustomerEmail
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string Email { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
