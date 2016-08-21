using System;
using System.Data.Entity;
using System.Linq;

namespace DeferredLoadingOfRelatedEntities
{
    /*
        You have an instance of an entity, and you want to defer the loading of two or more related entities in a single query.
        Especially important here is how we use the Load() method to avoid requerying the same entity twice.
     */
    class Program
    {
        static void Main(string[] args)
        {
            // initialize and seed the database
            using (var context = new DataContext())
            {
                var company = new Company { Name = "Acme Products" };
                var acc = new Department { Name = "Accounting", Company = company };
                var ship = new Department { Name = "Shipping", Company = company };
                var emp1 = new Employee { Name = "Jill Carpenter", Department = acc };
                var emp2 = new Employee { Name = "Steven Hill", Department = ship };
                context.Employees.Add(emp1);
                context.Employees.Add(emp2);
                context.SaveChanges();
            }

            // First approach
            using (var context = new DataContext())
            {
                // Assume we already have an employee
                var jill = context.Employees.First(o => o.Name == "Jill Carpenter");
                // Get Jill's Department and Company, but we also reload Employees
                var results = context.Employees
                .Include("Department.Company")
                .First(o => o.Id == jill.Id);

                var resultsStronglyTypedInclude = context.Employees.Include(e => e.Department.Company)
                    .First(o => o.Id == jill.Id); ;
                Console.WriteLine("{0} works in {1} for {2}",
                jill.Name, jill.Department.Name, jill.Department.Company.Name);
            }
            /*
                If we didn’t already have an instance of the Employee entity, we could simply use the Include() method with a
                query path Department.Company. This is essentially the approach we take in earlier queries. The disadvantage of this
                approach is that it retrieves all of the columns for the Employee entity. In many cases, this might be an expensive
                operation. Because we already have this object in the context, it seems wasteful to gather these columns again from
                the database and transmit them across the wire.
             */

            using (var context = new DataContext())
            {
                // Assume we already have an employee
                var jill = context.Employees.Where(o => o.Name == "Jill Carpenter").First();
                // Leverage the Entry, Query, and Include methods to retrieve Department and Company data
                // without requerying the Employee table
                context.Entry(jill).Reference(x => x.Department).Query().Include(y => y.Company).Load();
                Console.WriteLine();
                Console.WriteLine("{0} works in {1} for {2}",
                jill.Name, jill.Department.Name, jill.Department.Company.Name);
            }
            /*
                In the second query, we use the Entry() method exposed by the DbContext object to access the Employee object
                and perform operations against it. We then chain the Reference() and Query() methods from the DbReferenceEntity
                class to return a query to load the related Department object from the underlying data store. Additionally, we chain
                the Include() method to pull in the related Company information. As desired, this query retrieves both Department
                and Company data without needlessly requerying the data store for Employees data, which has already been loaded
                into the context. 
            */

            Console.ReadKey();
        }
    }
}
