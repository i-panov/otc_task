using System;
using Microsoft.EntityFrameworkCore;
using otc_task.Models;

namespace otc_task
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=otc.db");
        }
    }
}