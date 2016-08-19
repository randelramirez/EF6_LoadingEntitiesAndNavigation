using System.Collections.Generic;

namespace LazyLoading
{
    public class CustomerType
    {
        public CustomerType()
        {
            this.Customers = new HashSet<Customer>();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
