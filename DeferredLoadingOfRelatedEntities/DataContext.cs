using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeferredLoadingOfRelatedEntities
{
    public class DataContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Employee> Employees { get; set; }

       
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*
                Note: Company is 1-tomany with Department and Department and is one-to-many with Employee, 
                had Employee been a one-to-many(or Department many-to-one, a Department has an Employee, and Employee has many Department)
                this would have been a many-to-many  with payload type of data relationship between the three entities

                Again, Department has many employees, and an employee has a department. A reason why this is not a many-to-many with a payload type
                of relationship
            */
            modelBuilder.Entity<Department>()
                .HasKey(d => d.Id)
                .HasMany(d => d.Employees)
                .WithRequired(d => d.Department)
                .HasForeignKey(d => d.DepartmentId);

            modelBuilder.Entity<Employee>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Company>()
                .HasKey(c => c.Id)
                .HasMany(c => c.Departments)
                .WithRequired(c => c.Company)
                .HasForeignKey(c => c.CompanyId);

        }
    }
}
