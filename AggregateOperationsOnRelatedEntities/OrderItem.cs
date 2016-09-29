using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregateOperationsOnRelatedEntities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int SKU { get; set; }

        public int Shipped { get; set; }

        public decimal UnitPrice { get; set; }

        public virtual Order Order { get; set; }
    }
}
