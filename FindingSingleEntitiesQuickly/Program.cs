using System;
using System.Linq;

namespace FindingSingleEntitiesQuickly
{
    class Program
    {
        static void Main(string[] args)
        {
            int starCityId;
            int desertSunId;
            int palmTreeId;

            using (var context = new DataContext())
            {
                var starCity = new Club { Name = "Star City Chess Club", City = "New York" };
                var desertSun = new Club { Name = "Desert Sun Chess Club", City = "Phoenix" };
                var palmTree = new Club { Name = "Palm Tree Chess Club", City = "San Diego" };
                context.Clubs.Add(starCity);
                context.Clubs.Add(desertSun);
                context.Clubs.Add(palmTree);
                context.SaveChanges();
                // SaveChanges() returns newly created Id value for each club
                starCityId = starCity.ClubId;
                desertSunId = desertSun.ClubId;
                palmTreeId = palmTree.ClubId;
            }

            using (var context = new DataContext())
            {
                var starCity = context.Clubs.SingleOrDefault(x => x.ClubId == starCityId);
                starCity = context.Clubs.SingleOrDefault(x => x.ClubId == starCityId);
                starCity = context.Clubs.Find(starCityId);
                var desertSun = context.Clubs.Find(desertSunId);
                var palmTree = context.Clubs.AsNoTracking().SingleOrDefault(x => x.ClubId == palmTreeId);
                palmTree = context.Clubs.Find(palmTreeId);
                var lonesomePintId = -999;
                context.Clubs.Add(new Club { City = "Portland", Name = "Lonesome Pine", ClubId = lonesomePintId, });
                var lonesomePine = context.Clubs.Find(lonesomePintId);
                var nonexistentClub = context.Clubs.Find(10001);
            }

            Console.ReadKey();

            /*
                When querying against the context object, a round trip will always be made to the database to retrieve requested data,
                even if that data has already been loaded into the context object in memory. When the query completes, entity objects
                that do not exist in the context are added and then tracked. By default, if the entity object is already present in the
                context, it is not overwritten with more recent database values.

                However, the DbSet object, which wraps each of our entity objects, exposes a Find() method. Specifically, Find()
                expects an argument that represents the primary key of the desired object. Find() is very efficient, as it will first search
                the underlying context for the target object. If the object is not found, it then automatically queries the underlying data
                store. If still not found, Find() simply returns NULL to the caller. Additionally, Find() will return entities that have
                been added to the context (think, having a state of “Added”), but not yet saved to the underlying database. Fortunately,
                the Find() method is available with any of three modeling approaches: Database First, Model First, or Code First.

                In this example, we start by adding three new clubs to the Club entity collection. Note how we are able to
                reference the newly created Id for each Club entity immediately after the call to SaveChanges(). The context will
                return the Id for the new object immediately after the SaveChanges() operation completes.

                We next query the Clubs entity from the DbContext object to return the StarCity Club entity. Note how we
                leverage the SingleOrDefault() LINQ extension method, which returns exactly one object, or NULL, if the object
                does not exist in the underlying data store. SingleOrDefault() will throw an exception if more than one object
                with the search criteria is found. SingleOfDefault() is an excellent approach to querying entities by a primary key
                property. If you should desire the first object when many exist, consider the FirstOrDefault() method.

                
             */
        }
    }
}
