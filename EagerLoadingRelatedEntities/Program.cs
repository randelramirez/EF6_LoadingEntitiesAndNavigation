using System;
using System.Data.Entity;
using System.Linq;

namespace EagerLoadingRelatedEntities
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialize and seed the database
            using (var context = new DataContext())
            {
                var web = new CustomerType
                {
                    Description = "Web Customer"
                };
                var retail = new CustomerType
                {
                    Description = "Retail Customer"
                };
                var customer = new Customer { Name = "Joan Smith", CustomerType = web };
                customer.CustomerEmails.Add(new CustomerEmail
                { Email = "jsmith@gmail.com" });
                customer.CustomerEmails.Add(new CustomerEmail { Email = "joan@smith.com" });
                context.Customers.Add(customer);
                customer = new Customer { Name = "Bill Meyers", CustomerType = retail };
                customer.CustomerEmails.Add(new CustomerEmail
                { Email = "bmeyers@gmail.com" });
                context.Customers.Add(customer);
                context.SaveChanges();
            }

            using (var context = new DataContext())
            {
                // Include() method with a string-based query path to the
                // corresponding navigation properties
                var customers = context.Customers
                .Include("CustomerType")
                .Include("CustomerEmails");
                Console.WriteLine("Customers");
                Console.WriteLine("=========");
                foreach (var customer in customers)
                {
                    Console.WriteLine("{0} is a {1}, email address(es)", customer.Name,
                    customer.CustomerType.Description);
                    foreach (var email in customer.CustomerEmails)
                    {
                        Console.WriteLine("\t{0}", email.Email);
                    }
                }
            }

            using (var context = new DataContext())
            {
                // Include() method with a strongly typed query path to the
                // corresponding navigation properties
                var customerTypes = context.CustomerTypes
                .Include(x => x.Customers.Select(y => y.CustomerEmails));

                Console.WriteLine("\nCustomers by Type");
                Console.WriteLine("=================");
                foreach (var customerType in customerTypes)
                {
                    Console.WriteLine("Customer type: {0}", customerType.Description);
                    foreach (var customer in customerType.Customers)
                    {
                        Console.WriteLine("{0}", customer.Name);
                        foreach (var email in customer.CustomerEmails)
                        {
                            Console.WriteLine("\t{0}", email.Email);
                        }
                    }
                }

                Console.ReadKey();

                /*
                    An alternative, loading the parent and related entities (keep in mind that our object graph is a set of parent/child
                    entities based on relationships, similar to parent/child database tables with foreign key relationships) at once, is known
                    as eager loading. This approach can be efficient when you know, up front, that you will require a large set of related
                    data, as it can retrieve all data (both from the parent and related entities) in a single query.

                    to fetch the object graph all at once, we use the Include() method twice. In the first use, we start
                    the object graph with Customer and include an entity reference to the CustomerType entity. This is on the one side
                    of the one-to-many association. Then, in the subsequent Include() method (contained in the same line of code,
                    chained together), we get the many side of the one-to-many association, bringing along all of the instances of the
                    CustomerEmail entity for the customer. By chaining together the Include() method twice in a fluent API manner,
                    we fetch referenced entities from both of the Customer’s navigation properties. Note that in this example we use string
                    representations of the navigation properties, separated by the “.” character, to identify the related entity objects.
                    The string representation is referred as the query path of the related objects.

                    In the following foreach construct, we perform the exact same operation, but using strongly typed query paths.
                    Note here how we use lambda expressions to identify each of the related entities. The strongly typed usage provides us
                    with both IntelliSense, compile-time safety and refactoring support.

                    is generated from usage of the Include() method. Entity
                    Framework automatically removes data that is duplicated by the query
                 */
            }
        }
    }
}
