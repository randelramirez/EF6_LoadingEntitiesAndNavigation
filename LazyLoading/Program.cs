using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyLoading
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
                var customers = context.Customers;
                Console.WriteLine("Customers");
                Console.WriteLine("=========");
                // Only information from the Customer entity is requested
                foreach (var customer in customers)
                {
                    Console.WriteLine("Customer name is {0}", customer.Name);
                }
                // Now, application is requesting information from the related entities, CustomerType
                // and CustomerEmail, resulting in Entity Framework generating separate queries to each
                // entity object in order to obtain the requested information.
                foreach (var customer in customers)
                {
                    Console.WriteLine("{0} is a {1}, email address(es)", customer.Name,
                    customer.CustomerType.Description);
                    foreach (var email in customer.CustomerEmails)
                        Console.WriteLine("\t{0}", email.Email);
                }

                // Extra credit:
                // If you enable SQL Profiler, the following query will not requery the database
                // for related data. Instead, it will return the in-memory data from the prior query.
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
            
            Console.ReadKey();

            /*
                By default, Entity Framework loads only entities that you specifically request. This is known as lazy loading, and it
                is an important principle to keep in mind. The alternative, loading the parent and every associated entity, known as
                eager loading, may load a much larger object graph into memory than you need, not to mention the added overhead
                of retrieving, marshaling, and materializing a larger amount of data.

                In this example, we start by issuing a query against the Customer entity to load all customers. Interestingly, the
                query itself is not executed immediately, but rather when we first enumerate the Customer entity in the first foreach
                construct. This behavior follows the principle of deferred loading upon which LINQ is built.

                In the first foreach construct, we only request data elements from the underlying Customer table and not any
                data from the CustomerType or CustomerEmail table. In this case, Entity Framework only queries the Customer table
                and not the related CustomerType or CustomerEmail tables.

                Then, in the second foreach construct, we explicitly reference the Description property from the CustomerType entity
                and the Email property from the CustomerEmail entity. Directly accessing these properties results in Entity Framework
                generating a query to each related table for the requested data. It’s important to understand that Entity Framework generates
                a separate query the first time either of the related tables are accessed. Once a query has been invoked for a property from
                a related entity, Entity Framework will mark the property as loaded and will retrieve the data from memory as opposed to
                requerying the underlying table over and over again. In this example, four separate queries are generated for child data:
                    • A select statement against CustomerType and CustomerEmail for Joan Smith
                    • A select statement against CustomerType and CustomerEmail for Bill Meyers
                
                This separate query for each child table works well when a user is browsing your application and requests
                different data elements depending on his or her needs at the moment. It can improve application response time,
                since data is retrieved as needed with a series of small queries, as opposed to loading a large amount of data up front,
                potentially causing a delay in rendering the view to the user.

                This approach, however, is not so efficient when you know, up front, that you will require a large set of data from
                related tables. In those cases, a query with eager loading may be a better option as it can retrieve all of the data
                (from both the parent and related tables) in a single query.

                The last code block, entitled ‘Extra Credit,’ demonstrates that once child properties are loaded, Entity Framework will
                retrieve their values from in-memory and not requery the database. Turn on the SQL Server Profiler Tool, run the example
                and note how the ‘Extra Credit’ code block does not generate SQL Select statements when child properties are referenced.
             */
        }
    }
}
