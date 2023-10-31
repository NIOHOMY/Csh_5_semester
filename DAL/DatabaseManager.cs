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
    public class DatabaseManager
    {
        private readonly LibraryContext _context;

        public DatabaseManager(LibraryContext context)
        {
            _context = context;
        }

        public Reader GetReaderById(int readerId)
        {
            try
            {
                return _context.Readers.FirstOrDefault(r => r.ReaderId == readerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении читателя из базы данных:");
                Console.WriteLine(ex.Message);
                Debug.WriteLine("Произошла ошибка при получении читателя из базы данных:");
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public List<Book> GetIssueBooksByIssueId(int issueId)
        {
            try
            {
                return _context.Issues
                    .Where(issue => issue.IssueId == issueId)
                    .SelectMany(issue => issue.Books)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении списка книг по ID выдачи:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при получении списка книг по ID выдачи:");
                Debug.WriteLine(ex.Message);
                return new List<Book>();
            }
        }

        public Book GetBookById(int bookId)
        {
            try
            {
                return _context.Books.FirstOrDefault(book => book.BookId == bookId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении книги по ID:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при получении книги по ID:");
                Debug.WriteLine(ex.Message);
                
                return null;
            }
        }

        public void IncreaseNumberOfExamples(int bookId, int quantity=1)
        {
            Book? book = _context.Books.Find(bookId);
            if (book != null)
            {
                book.NumberOfExamples += quantity;
                _context.SaveChanges();
            }
        }

        public void DecreaseNumberOfExamples(int bookId, int quantity=1)
        {
            Book? book = _context.Books.Find(bookId);
            if (book != null && book.NumberOfExamples >= quantity)
            {
                book.NumberOfExamples -= quantity;
                _context.SaveChanges();
            }
        }

        public void AddAuthor(Author author)
        {
            try
            {
                _context.Authors.Add(author);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при добавлении автора:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при добавлении автора:");
                Debug.WriteLine(ex.Message);
                
            }
        }

        public void AddBook(Book book)
        {
            try
            {
                _context.Books.Add(book);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при добавлении книги:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при добавлении книги:");
                Debug.WriteLine(ex.Message);
                
            }
        }

        public void AddIssue(Issue issue)
        {
            try
            {
                _context.Issues.Add(issue);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при добавлении выдачи:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при добавлении выдачи:");
                Debug.WriteLine(ex.Message);
                
            }
        }


        public void AddPublisher(Publisher publisher)
        {
            try
            {
                _context.Publishers.Add(publisher);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при добавлении издателя:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при добавлении издателя:");
                Debug.WriteLine(ex.Message);
                
            }
        }

        public void AddReader(Reader reader)
        {
            try
            {
                _context.Readers.Add(reader);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при добавлении читателя:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при добавлении читателя:");
                Debug.WriteLine(ex.Message);
                
            }
        }

        public List<Author> GetAllAuthors()
        {
            try
            {
                return _context.Authors.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении списка авторов:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при получении списка авторов:");
                Debug.WriteLine(ex.Message);
                
                return new List<Author>();
            }
        }

        public List<Book> GetAllBooks()
        {
            try
            {
                return _context.Books.Include(b => b.FirstAuthor).Include(b => b.Publisher).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении списка книг:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при получении списка книг:");
                Debug.WriteLine(ex.Message);
                
                return new List<Book>();
            }
        }

        public List<Book> GetAllAvailableBooks()
        {
            try
            {
                return _context.Books.Where(b => b.NumberOfExamples > 0).ToList();   
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении списка доступных книг:");
                Console.WriteLine(ex.Message);

                Debug.WriteLine("Произошла ошибка при получении списка доступных книг:");
                Debug.WriteLine(ex.Message);

                return new List<Book>();
            }
        }

        public List<Issue> GetAllIssues()
        {
            try
            {
                return _context.Issues.Include(i => i.Reader).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении списка выдач:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при получении списка выдач:");
                Debug.WriteLine(ex.Message);
                
                return new List<Issue>();
            }
        }

        public List<Publisher> GetAllPublishers()
        {
            try
            {
                return _context.Publishers.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении списка издателей:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при получении списка издателей:");
                Debug.WriteLine(ex.Message);
                
                return new List<Publisher>();
            }
        }

        public List<Reader> GetAllReaders()
        {
            try
            {
                return _context.Readers.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении списка читателей:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при получении списка читателей:");
                Debug.WriteLine(ex.Message);
                
                return new List<Reader>();
            }
        }

        public bool DeleteAuthor(int authorId)
        {
            try
            {
                Author? author = _context.Authors.Find(authorId);

                if (author != null)
                {
                    _context.Authors.Remove(author);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при удалении автора:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при удалении автора:");
                Debug.WriteLine(ex.Message);
                
                return false;
            }
        }

        public bool DeleteBook(int bookId)
        {
            try
            {
                Book? book = _context.Books.Find(bookId);

                if (book != null)
                {
                    _context.Books.Remove(book);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при удалении книги:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при удалении книги:");
                Debug.WriteLine(ex.Message);
                
                return false;
            }
        }

        public bool DeleteIssue(int issueId)
        {
            try
            {
                Issue? issue = _context.Issues.FirstOrDefault(i => i.IssueId == issueId);

                if (issue == null)
                {
                    Console.WriteLine("Выдача не найдена.");
                    return false;
                }
                foreach (Book book in issue.Books)
                {
                    IncreaseNumberOfExamples(book.BookId);
                }
                _context.Issues.Remove(issue);
                _context.SaveChanges();

                Console.WriteLine("Выдача успешно удалена.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при удалении выдачи:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при удалении выдачи:");
                Debug.WriteLine(ex.Message);
                
                return false;
            }
        }

        public bool DeletePublisher(int publisherId)
        {
            try
            {
                Publisher? publisher = _context.Publishers.Find(publisherId);

                if (publisher != null)
                {
                    _context.Publishers.Remove(publisher);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при удалении издателя:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при удалении издателя:");
                Debug.WriteLine(ex.Message);
                
                return false;
            }
        }

        public bool DeleteReader(int readerId)
        {
            try
            {
                Reader? reader = _context.Readers.Find(readerId);

                if (reader != null)
                {
                    _context.Readers.Remove(reader);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при удалении читателя:");
                Console.WriteLine(ex.Message);
                
                Debug.WriteLine("Произошла ошибка при удалении читателя:");
                Debug.WriteLine(ex.Message);
                
                return false;
            }
        }
    }
}
