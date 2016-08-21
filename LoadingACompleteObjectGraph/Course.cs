using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadingACompleteObjectGraph
{
    public class Course
    {
        public Course()
        {
            this.Sections = new HashSet<Section>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public virtual ICollection<Section> Sections { get; set; }
    }
}
