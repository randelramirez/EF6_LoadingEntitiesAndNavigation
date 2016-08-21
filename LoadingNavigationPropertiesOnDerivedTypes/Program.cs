using System;
using System.Data.Entity;
using System.Linq;

namespace LoadingNavigationPropertiesOnDerivedTypes
{

    /*
        You have a model with one or more derived types that are in a Has-a relationship (wherein one object is a part of
        another object) with one or more other entities. You want to eagerly load all of the related entities in one round trip
        to the database.
     */
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new DataContext())
            {
                var foreman1 = new Foreman { Name = "Carl Ramsey" };
                var foreman2 = new Foreman { Name = "Nancy Ortega" };
                var phone = new Phone { Number = "817 867-5309" };
                var jobsite = new JobSite
                {
                    JobSiteName = "City Arena",
                    Address = "123 Main",
                    City = "Anytown",
                    State = "TX",
                    ZIPCode = "76082",
                    Phone = phone
                };
                jobsite.Foremen.Add(foreman1);
                jobsite.Foremen.Add(foreman2);
                var plumber = new Plumber { Name = "Jill Nichols", Email = "JNichols@plumbers.com", JobSite = jobsite };
                context.Tradesmen.Add(plumber);
                context.SaveChanges();
            }

            using (var context = new DataContext())
            {
                var plumber =
                    context.Tradesmen.OfType<Plumber>().Include("JobSite.Phone").Include("JobSite.Foremen").First();

                var plumberStonglyTypedInclude = context.Tradesmen.OfType<Plumber>().Include(t => t.JobSite.Phone).Include(t => t.JobSite.Foremen).First();

                Console.WriteLine("Plumber's Name: {0} ({1})", plumber.Name, plumber.Email);
                Console.WriteLine("Job Site: {0}", plumber.JobSite.JobSiteName);
                Console.WriteLine("Job Site Phone: {0}", plumber.JobSite.Phone.Number);
                Console.WriteLine("Job Site Foremen:");
                foreach (var boss in plumber.JobSite.Foremen)
                {
                    Console.WriteLine("\t{0}", boss.Name);
                }
            }

            Console.WriteLine("Press <enter> to continue...");
            Console.ReadLine();

            /*
                Our query starts by selecting instances of the derived type Plumber. To fetch them, we use the OfType<Plumber>()
                method. The OfType<>() method selects instances of the given subtype from the entity set.
             */

            /*
                The resulting query is somewhat complex; involving several joins and sub-selects. The alternative, leveraging the
                default lazy loading behavior of Entity Framework, would require several round trips to the database and could result
                in a performance hit, especially if we retrieved many Plumbers.
             */
        }
    }
}
