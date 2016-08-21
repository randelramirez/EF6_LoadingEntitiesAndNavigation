using System.Collections.Generic;

namespace LoadingNavigationPropertiesOnDerivedTypes
{
    public class JobSite : Location
    {
        public JobSite()
        {
            this.Foremen = new HashSet<Foreman>();
            this.Plumbers = new HashSet<Plumber>();
        }

        public string JobSiteName { get; set; }

        public virtual ICollection<Foreman> Foremen { get; set; }

        public virtual ICollection<Plumber> Plumbers { get; set; }
    }
}