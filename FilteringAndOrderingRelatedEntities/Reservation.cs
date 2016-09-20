using System;

namespace FilteringAndOrderingRelatedEntities
{
    public class Reservation
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ContactName { get; set; }

        public int RoomId { get; set; }

        public virtual Room Room { get; set; }
    }
}