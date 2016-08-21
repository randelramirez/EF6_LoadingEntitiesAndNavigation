using System;
using System.Data.Entity;
using System.Linq;

namespace UsingIncludeWithOtherLINQQueryOperators
{
    class Program
    {
        static void Main(string[] args)
        {
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

                Console.WriteLine();
                var eventWithClub = events.Include("Club").First();
                //var eventWithClubStronglyTypedInclude = events.Include(e => e.Club).First();
                Console.WriteLine("The next New York club event is:");
                Console.WriteLine("\tEvent: {0}", eventWithClub.EventName);
                Console.WriteLine("\tDate: {0}", eventWithClub.EventDate.ToShortDateString());
                Console.WriteLine("\tClub: {0}", eventWithClub.Club.Name);
            }

            Console.ReadKey();
        }
    }
}
