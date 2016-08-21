using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryingInMemoryEntities
{
    class Program
    {
        static void Main(string[] args)
        {
            int desertSunId;

            using (var context = new DataContext())
            {
                var starCity = new Club { Name = "Star City Chess Club", City = "New York" };
                var desertSun = new Club { Name = "Desert Sun Chess Club", City = "Phoenix" };
                var palmTree = new Club { Name = "Palm Tree Chess Club", City = "San Diego" };

                context.Clubs.Add(starCity);
                context.Clubs.Add(desertSun);
                context.Clubs.Add(palmTree);

                context.SaveChanges();

                desertSunId = desertSun.ClubId;
            }

            using (var context = new DataContext())
            {
                Console.WriteLine("\nLocal Collection Behavior");
                Console.WriteLine("=================");

                Console.WriteLine("\nNumber of Clubs Contained in Local Collection: {0}", context.Clubs.Local.Count);
                Console.WriteLine("=================");

                Console.WriteLine("\nClubs Retrieved from Context Object");
                Console.WriteLine("=================");
                foreach (var club in context.Clubs.Take(2))
                {
                    Console.WriteLine("{0} is located in {1}", club.Name, club.City);
                }

                Console.WriteLine("\nClubs Contained in Context Local Collection");
                Console.WriteLine("=================");
                foreach (var club in context.Clubs.Local)
                {
                    Console.WriteLine("{0} is located in {1}", club.Name, club.City);
                }

                context.Clubs.Find(desertSunId);

                Console.WriteLine("\nClubs Retrieved from Context Object - Revisted");
                Console.WriteLine("=================");
                foreach (var club in context.Clubs)
                {
                    Console.WriteLine("{0} is located in {1}", club.Name, club.City);
                }

                Console.WriteLine("\nClubs Contained in Context Local Collection - Revisted");
                Console.WriteLine("=================");
                foreach (var club in context.Clubs.Local)
                {
                    Console.WriteLine("{0} is located in {1}", club.Name, club.City);
                }

                // Get reference to local observable collection 
                var localClubs = context.Clubs.Local;

                // Add new Club
                var lonesomePintId = -999;
                localClubs.Add(new Club
                {
                    City = "Portland",
                    Name = "Lonesome Pine",
                    ClubId = lonesomePintId
                });

                // Remove Desert Sun club
                localClubs.Remove(context.Clubs.Find(desertSunId));

                Console.WriteLine("\nClubs Contained in Context Object - After Adding and Deleting");
                Console.WriteLine("=================");
                foreach (var club in context.Clubs)
                {
                    Console.WriteLine("{0} is located in {1} with a Entity State of {2}",
                                      club.Name, club.City, context.Entry(club).State);
                }

                Console.WriteLine("\nClubs Contained in Context Local Collection - After Adding and Deleting");
                Console.WriteLine("=================");
                foreach (var club in localClubs)
                {
                    Console.WriteLine("{0} is located in {1} with a Entity State of {2}",
                                      club.Name, club.City, context.Entry(club).State);
                }
                /*
                    Interestingly, in the context, we see that the Desert Sun Club has been marked for deletion, but we do not see the
                    newly added Lonesome Pine Club. Keep in mind that Lonesome Pine has been added to the Context object, but we
                    have not yet called the SaveChanges() operation to update the underlying data store.
                    
                    we see the newly added Lonesome Pine Club, but we no longer see the
                    Desert Sun Club that is marked for deletion. The default behavior of the Local collection is to hide any entities that are
                    marked for deletion, as these objects are no longer valid.  
                    
                    The bottom line: Accessing the Local collection never causes a query to be sent to the database; accessing the
                    context object always causes a query to be sent to the database.
                    
                    To summarize, each entity set exposes a property called Local, which is an observable collection that mirrors the
                    contents of the underlying context object. As demonstrated in this recipe, querying the Local Collection can be very
                    efficient in that doing so never generates a SQL query to the underlying data store.               
                */
            }

            Console.ReadLine();

        }
    }
}

