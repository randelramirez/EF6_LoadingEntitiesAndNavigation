﻿using System;
using System.Data.Entity;
using System.Linq;
using Z.EntityFramework.Plus;

namespace FilteringAndOrderingRelatedEntities
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialize and seed the database
            using (var context = new DataContext())
            {
                var hotel = new Hotel { Name = "Grand Seasons Hotel" };
                var r101 = new Room { Rate = 79.95M, Hotel = hotel };
                var es201 = new ExecutiveSuite { Rate = 179.95M, Hotel = hotel };
                var es301 = new ExecutiveSuite { Rate = 299.95M, Hotel = hotel };

                var res1 = new Reservation
                {
                    StartDate = DateTime.Parse("3/12/2010"),
                    EndDate = DateTime.Parse("3/14/2010"),
                    ContactName = "Roberta Jones",
                    Room = es301
                };
                var res2 = new Reservation
                {
                    StartDate = DateTime.Parse("1/18/2010"),
                    EndDate = DateTime.Parse("1/28/2010"),
                    ContactName = "Bill Meyers",
                    Room = es301
                };
                var res3 = new Reservation
                {
                    StartDate = DateTime.Parse("2/5/2010"),
                    EndDate = DateTime.Parse("2/6/2010"),
                    ContactName = "Robin Rosen",
                    Room = r101
                };

                es301.Reservations.Add(res1);
                es301.Reservations.Add(res2);
                r101.Reservations.Add(res3);

                hotel.Rooms.Add(r101);
                hotel.Rooms.Add(es201);
                hotel.Rooms.Add(es301);

                context.Hotels.Add(hotel);
                context.SaveChanges();
            }

            using (var context = new DataContext())
            {
                // https://msdn.microsoft.com/en-us/data/jj574232.aspx#explicitFilter
                context.Configuration.LazyLoadingEnabled = false;

                // Assume we have an instance of hotel
                var hotel = context.Hotels.First();

                // Explicit loading with Load() provides opportunity to filter related data 
                // obtained from the Include() method 
                context.Entry(hotel)
                       .Collection(x => x.Rooms)
                       .Query()
                       .Include(y => y.Reservations)
                       .Where(y => y is ExecutiveSuite && y.Reservations.Any())
                       .Load();

                Console.WriteLine("Executive Suites for {0} with reservations", hotel.Name);

                foreach (var room in hotel.Rooms)
                {
                    Console.WriteLine("\nExecutive Suite {0} is {1} per night", room.Id,
                                      room.Rate.ToString("C"));
                    Console.WriteLine("Current reservations are:");
                    foreach (var res in room.Reservations.OrderBy(r => r.StartDate))
                    {
                        Console.WriteLine("\t{0} thru {1} ({2})", res.StartDate.ToShortDateString(),
                                          res.EndDate.ToShortDateString(), res.ContactName);
                    }
                }
            }

            using (var context = new DataContext())
            {
                // works the same if lazy loading is enabled
                context.Configuration.LazyLoadingEnabled = false;

                var q = context.Hotels.Select(h =>
                new
                {
                    Hotel = h,
                    RoomsAndReservations = h.Rooms.Where(r => r is ExecutiveSuite && r.Reservations.Any())
                    .Select(b =>
                    new
                    {
                        Rooms = b,
                        Reservations = b.Reservations
                    })
                }).First();

                Console.WriteLine("Executive Suites for {0} with reservations", q.Hotel.Name);


                foreach (var room in q.RoomsAndReservations)
                {
                    Console.WriteLine("\nExecutive Suite {0} is {1} per night", room.Rooms.Id,
                                      room.Rooms.Rate.ToString("C"));
                    Console.WriteLine("Current reservations are:");
                    foreach (var res in room.Reservations.OrderBy(b => b.StartDate))
                    {
                        Console.WriteLine("\t{0} thru {1} ({2})", res.StartDate.ToShortDateString(),
                                          res.EndDate.ToShortDateString(), res.ContactName);
                    }
                }
            }

            using (var context = new DataContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                var hotel = context.Hotels
                // Include only executive suite with a reservation
                .IncludeFilter(x => x.Rooms.Where(y => y is ExecutiveSuite && y.Reservations.Any()))
                // Include only reservation from executive suite
                .IncludeFilter(x => x.Rooms.Where(y => y is ExecutiveSuite).Select(z => z.Reservations))
                .First();

                Console.WriteLine("Executive Suites for {0} with reservations", hotel.Name);

                foreach (var room in hotel.Rooms)
                {
                    Console.WriteLine("\nExecutive Suite {0} is {1} per night", room.Id,
                                      room.Rate.ToString("C"));
                    Console.WriteLine("Current reservations are:");
                    foreach (var res in room.Reservations.OrderBy(r => r.StartDate))
                    {
                        Console.WriteLine("\t{0} thru {1} ({2})", res.StartDate.ToShortDateString(),
                                          res.EndDate.ToShortDateString(), res.ContactName);
                    }
                }
            }

            Console.WriteLine("Press <enter> to continue...");
            Console.ReadLine();
        }
    }
}
