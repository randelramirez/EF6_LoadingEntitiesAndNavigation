using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingIncludeWithOtherLINQQueryOperators
{
    public class Club
    {
        public Club()
        {
            Events = new HashSet<Event>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
