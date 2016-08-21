using System.Collections.Generic;

namespace DeferredLoadingOfRelatedEntities
{
    public class Company
    {
        public Company()
        {
            Departments = new HashSet<Department>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
    }
}