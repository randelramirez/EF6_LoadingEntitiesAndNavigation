using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingIncludeWithOtherLINQQueryOperators
{
    public class Event
    {
        public int Id { get; set; }

        public string EventName { get; set; }

        public DateTime EventDate { get; set; }

        public int ClubId { get; set; }

        public virtual Club Club { get; set; }
    }
}
