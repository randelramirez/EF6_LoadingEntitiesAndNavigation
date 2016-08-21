using System.Collections.Generic;

namespace DeferredLoadingOfRelatedEntities
{
    public class Department
    {
        public Department()
        {
            this.Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}