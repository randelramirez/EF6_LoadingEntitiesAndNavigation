using System.Collections.Generic;

namespace EagerLoadingRelatedEntities
{
    public class Customer
    {
        public Customer()
        {
            this.CustomerEmails = new HashSet<CustomerEmail>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int CustomerTypeId { get; set; }

        public virtual CustomerType CustomerType { get; set; }

        public virtual ICollection<CustomerEmail> CustomerEmails { get; set; }
    }
}
