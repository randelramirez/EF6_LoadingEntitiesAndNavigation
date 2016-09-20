using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilteringAndOrderingRelatedEntities
{
    public class Room
    {
        public Room()
        {
            this.Reservations = new HashSet<Reservation>();
        }

        public int Id { get; set; }

        public decimal Rate { get; set; }

        public int HotelId { get; set; }

        public virtual Hotel Hotel { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
