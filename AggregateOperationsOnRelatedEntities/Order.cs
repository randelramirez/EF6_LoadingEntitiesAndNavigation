using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregateOperationsOnRelatedEntities
{
    public class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }

        public System.DateTime OrderDate { get; set; }

        public string CustomerName { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
