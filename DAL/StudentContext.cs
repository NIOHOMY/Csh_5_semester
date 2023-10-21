using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.DAL
{
    public class StudentContext : DbContext
    {
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;
                                        Database = StrudentDB; 
                                        Trusted_Connection = true");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while configuring the database: {ex.Message}");
                Console.WriteLine($"An error occurred while configuring the database: {ex.Message}");
            }
        }

        public StudentContext()
        {
            /*
            Database.EnsureDeleted();
            Database.EnsureCreated();
            */
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                modelBuilder.Entity<Group>().HasData(
                    new Group { GroupId = 1, GroupName = "ФИИТ" },
                    new Group { GroupId = 2, GroupName = "МОАИС" },
                    new Group { GroupId = 3, GroupName = "ПМИ" }
                );

                base.OnModelCreating(modelBuilder);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while configuring the model: {ex.Message}");
                Console.WriteLine($"An error occurred while configuring the model: {ex.Message}");
            }
        }
    }

}
