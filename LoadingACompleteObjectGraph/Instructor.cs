using System.Collections.Generic;

namespace LoadingACompleteObjectGraph
{
    public class Instructor
    {
        public Instructor()
        {
            this.Sections = new HashSet<Section>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Section> Sections { get; set; }
    }
}