using System.Collections.Generic;

namespace LoadingNavigationPropertiesOnDerivedTypes
{
    public class Phone
    {
        public Phone()
        {
            this.Locations = new HashSet<Location>();
        }

        public int Id { get; set; }
        public string Number { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
    }
}