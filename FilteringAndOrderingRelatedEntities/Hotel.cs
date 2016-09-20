using System.Collections.Generic;

namespace FilteringAndOrderingRelatedEntities
{
    public class Hotel
    {
        public Hotel()
        {
            this.Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }

}