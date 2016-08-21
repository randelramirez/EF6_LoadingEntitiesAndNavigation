using System;
using System.Data.Entity;
using System.Linq;

namespace LoadingACompleteObjectGraph
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var context = new DataContext())
            {
                var course = new Course { Title = "Biology 101" };
                var fred = new Instructor { Name = "Fred Jones" };
                var julia = new Instructor { Name = "Julia Canfield" };
                var section1 = new Section { Course = course, Instructor = fred };
                var section2 = new Section { Course = course, Instructor = julia };

                var jim = new Student { Name = "Jim Roberts" };
                jim.Sections.Add(section1);
                var jerry = new Student { Name = "Jerry Jones" };
                jerry.Sections.Add(section2);
                var susan = new Student { Name = "Susan O'Reilly" };
                susan.Sections.Add(section1);
                var cathy = new Student { Name = "Cathy Ryan" };
                cathy.Sections.Add(section2);
                course.Sections.Add(section1);
                course.Sections.Add(section2);
                context.Students.Add(jim);
                context.Students.Add(jerry);
                context.Students.Add(susan);
                context.Students.Add(cathy);
                context.Courses.Add(course);
                context.SaveChanges();
            }

            // String query path argument for the Include method
            using (var context = new DataContext())
            {
                var graph = context.Courses
                .Include("Sections.Instructor")
                .Include("Sections.Students");
                Console.WriteLine("Courses");
                Console.WriteLine("=======");
                foreach (var course in graph)
                {
                    Console.WriteLine("{0}", course.Title);
                    foreach (var section in course.Sections)
                    {
                        Console.WriteLine("\tSection: {0}, Instrutor: {1}", section.Id,
                        section.Instructor.Name);
                        Console.WriteLine("\tStudents:");
                        foreach (var student in section.Students)
                        {
                            Console.WriteLine("\t\t{0}", student.Name);
                        }
                        Console.WriteLine("\n");
                    }
                }
            }

            // Strongly typed query path argument for the Include method
            using (var context = new DataContext())
            {
                var graph = context.Courses
                .Include(x => x.Sections.Select(y => y.Instructor))
                .Include(x => x.Sections.Select(z => z.Students));
                Console.WriteLine("Courses");
                Console.WriteLine("=======");
                var result = graph.ToList();
                foreach (var course in graph)
                {
                    Console.WriteLine("{0}", course.Title);
                    foreach (var section in course.Sections)
                    {
                        Console.WriteLine("\tSection: {0}, Instrutor: {1}", section.Id,
                        section.Instructor.Name);
                        Console.WriteLine("\tStudents:");
                        foreach (var student in section.Students)
                        {
                            Console.WriteLine("\t\t{0}", student.Name);
                        }
                        Console.WriteLine("\n");
                    }
                }
            }
            Console.WriteLine("Press <enter> to continue...");
            Console.ReadLine();
        }
    }
}
