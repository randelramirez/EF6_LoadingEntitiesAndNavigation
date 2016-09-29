using System;
using System.Linq;
using System.Data.Entity;

namespace AggregateOperationsOnRelatedEntities
{
    // Executing Aggregate Operations on Related Entities

    // You want to apply an aggregate operator on a related entity collection without loading the entire collection.

    class Program
    {
        static void Main(string[] args)
        {
            // initialize and seed the database
            using (var context = new DataContext())
            {
                var order = new Order { CustomerName = "Jenny Craig", OrderDate = DateTime.Parse("3/12/2010") };
                var item1 = new OrderItem { Order = order, Shipped = 3, SKU = 2827, UnitPrice = 12.95M };
                var item2 = new OrderItem { Order = order, Shipped = 1, SKU = 1918, UnitPrice = 19.95M };
                var item3 = new OrderItem { Order = order, Shipped = 3, SKU = 392, UnitPrice = 8.95M };
                order.OrderItems.Add(item1);
                order.OrderItems.Add(item2);
                order.OrderItems.Add(item3);
                context.Orders.Add(order);
                context.SaveChanges();
            }

            using (var context = new DataContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                // Assume we have an instance of Order
                var order = context.Orders.First();

                // Get the total order amount
                var amt = context.Entry(order)
                .Collection(x => x.OrderItems)
                .Query()
                .Sum(y => y.Shipped * y.UnitPrice);
                Console.WriteLine("Order Number: {0}", order.Id);
                Console.WriteLine("Order Date: {0}", order.OrderDate.ToShortDateString());
                Console.WriteLine("Order Total: {0}", amt.ToString("C"));

                var q = context.Orders.Include(o => o.OrderItems).Select(b => new { Order = b, Sum = b.OrderItems.Sum(i => i.Shipped * i.UnitPrice) }).First();
            }

            Console.WriteLine();
            Console.WriteLine();

            // same result, single trip to the database
            using (var context = new DataContext())
            {
                context.Configuration.LazyLoadingEnabled = false;

                var q = context.Orders.Include(o => o.OrderItems).Select(b => new { Order = b, Sum = b.OrderItems.Sum(i => i.Shipped * i.UnitPrice) }).First();
                Console.WriteLine("Order Number: {0}", q.Order.Id);
                Console.WriteLine("Order Date: {0}", q.Order.OrderDate.ToShortDateString());
                Console.WriteLine("Order Total: {0}", q.Sum.ToString("C"));

               
            }

            Console.ReadKey();
        }
    }
}
