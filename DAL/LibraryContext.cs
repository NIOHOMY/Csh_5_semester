using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;
using System.Diagnostics;

namespace LibraryManagementSystem.DAL
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Issue> Issues { get; set; }
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
            try
            {
                modelBuilder.Entity<Author>().HasData(
                    new Author { AuthorId = 1, Name = "Лев Толстой" },
                    new Author { AuthorId = 2, Name = "Федор Достоевский" },
                    new Author { AuthorId = 3, Name = "Антуан де Сент-Экзюпери" }
                );

                modelBuilder.Entity<Publisher>().HasData(
                    new Publisher { PublisherId = 1, NameOfPublisher = "Эксмо", City = "Москва" },
                    new Publisher { PublisherId = 2, NameOfPublisher = "АСТ", City = "Санкт-Петербург" }
                );

                modelBuilder.Entity<Book>().HasData(
                    new Book
                    {
                        BookId = 1,
                        Title = "Война и мир",
                        FirstAuthorId = 1,
                        YearOfPublication = 1869,
                        Price = 1000,
                        NumberOfExamples = 10,
                        PublisherId = 1
                    },
                    new Book
                    {
                        BookId = 2,
                        Title = "Преступление и наказание",
                        FirstAuthorId = 2,
                        YearOfPublication = 1866,
                        Price = 800,
                        NumberOfExamples = 1,
                        PublisherId = 2
                    },
                    new Book
                    {
                        BookId = 3,
                        Title = "Маленький принц",
                        FirstAuthorId = 3,
                        YearOfPublication = 1943,
                        Price = 500,
                        NumberOfExamples = 15,
                        PublisherId = 1
                    }, new Book
                    {
                        BookId = 4,
                        Title = "Честный вор",
                        FirstAuthorId = 2,
                        YearOfPublication = 1848,
                        Price = 700,
                        NumberOfExamples = 2,
                        PublisherId = 2
                    }
                );

                modelBuilder.Entity<Reader>().HasData(
                    new Reader
                    {
                        ReaderId = 1,
                        FirstName = "Иван",
                        LastName = "Иванов",
                        Patronymic = "Иванович",
                        Address = "ул. Пушкина, д. 10, кв. 5",
                        PhoneNumber = "+7 (999) 123-45-67"
                    },
                    new Reader
                    {
                        ReaderId = 2,
                        FirstName = "Петр",
                        LastName = "Петров",
                        Patronymic = "Петрович",
                        Address = "ул. Лермонтова, д. 20, кв. 30",
                        PhoneNumber = "+7 (999) 765-43-21"
                    }
                );

                base.OnModelCreating(modelBuilder);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при создании начальных данных:");
                Console.WriteLine(ex.Message);
                Debug.WriteLine("Произошла ошибка при создании начальных данных:");
                Debug.WriteLine(ex.Message);
            }
        }

    }

}
