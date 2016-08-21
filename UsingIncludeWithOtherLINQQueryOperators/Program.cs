using System;
using System.Data.Entity;
using System.Linq;

namespace UsingIncludeWithOtherLINQQueryOperators
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize and seed database
            using (var context = new DataContext())
            {
                var club = new Club { Name = "Star City Chess Club", City = "New York" };
                club.Events.Add(new Event
                {
                    EventName = "Mid Cities Tournament",
                    EventDate = DateTime.Parse("1/09/2010"),
                    Club = club
                });
                club.Events.Add(new Event
                {
                    EventName = "State Finals Tournament",
                    EventDate = DateTime.Parse("2/12/2010"),
                    Club = club
                });
                club.Events.Add(new Event
                {
                    EventName = "Winter Classic",
                    EventDate = DateTime.Parse("12/18/2009"),
                    Club = club
                });
                context.Clubs.Add(club);
                context.SaveChanges();
            }


            using (var context = new DataContext())
            {
                var events = from ev in context.Events
                             where ev.Club.City == "New York"
                             group ev by ev.Club into g
                             select g.FirstOrDefault(e1 => e1.EventDate == g.Min(evt => evt.EventDate));

                var eventsMethodBasedSyntax = context.Events.Where(ev => (ev.Club.City == "New York"))
                    .GroupBy(ev => ev.Club)
                    .Select(g => g.FirstOrDefault(e1 => 
                    (e1.EventDate == g.Min(evt => evt.EventDate))));

                var eventWithClub = events.Include("Club").First();
                //var eventWithClubStronglyTypedInclude = events.Include(e => e.Club).First();
                Console.WriteLine("The next New York club event is:");
                Console.WriteLine("\tEvent: {0}", eventWithClub.EventName);
                Console.WriteLine("\tDate: {0}", eventWithClub.EventDate.ToShortDateString());
                Console.WriteLine("\tClub: {0}", eventWithClub.Club.Name);
            }

            /*
                We start by creating a Club and three Events. In the query, we grab all of the events at clubs in New York, group them
                by club, and find the first one in date order. Note how the FirstOrDefault() LINQ extension method is cleverly
                embedded in the Select, or projection, operation. However, the events variable holds just the expression. It hasn’t
                executed anything on the database yet.

                Next we leverage the Include() method to eagerly load information from the related Club entity object using the
                variable, events, from the first LINQ query as the input for the second LINQ query. This is an example of composing
                LINQ queries—breaking a more complex LINQ query into a series of smaller queries, where the variable of the
                preceding query is in the source of the query.

                
             */

            Console.ReadKey();
        }
    }
}
