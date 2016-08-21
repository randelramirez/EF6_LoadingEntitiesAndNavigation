using System.Collections.Generic;

namespace LoadingACompleteObjectGraph
{
    public class Section
    {
        public Section()
        {
            this.Students = new HashSet<Student>();
        }

        public int Id { get; set; }

        public int InstructorId { get; set; }

        public int CourseId { get; set; }

        public virtual Instructor Instructor { get; set; }

        public virtual Course Course { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}