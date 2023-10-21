using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<IssueBook> IssueBooks { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;
                                        Database = LibraryDB; 
                                        Trusted_Connection = true");
        }

        public LibraryContext()
        {
            /*
            Database.EnsureDeleted();
            Database.EnsureCreated();
            */
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Book>()
            .HasOne(b => b.FirstAuthor)
            .WithMany()
            .HasForeignKey(b => b.FirstAuthorId)
            .OnUpdate(UpdateBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany()
                .HasForeignKey(b => b.PublisherId)
                .OnUpdate(UpdateBehavior.Cascade);

            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Reader)
                .WithMany()
                .HasForeignKey(i => i.ReaderId)
                .OnUpdate(UpdateBehavior.Cascade);

            modelBuilder.Entity<IssueBook>()
                .HasOne(ib => ib.Book)
                .WithMany()
                .HasForeignKey(ib => ib.BookId)
                .OnUpdate(UpdateBehavior.Cascade);

            modelBuilder.Entity<IssueBook>()
                .HasOne(ib => ib.Issue)
                .WithMany()
                .HasForeignKey(ib => ib.IssueId)
                .OnUpdate(UpdateBehavior.Cascade);*/

        }
    }

}
