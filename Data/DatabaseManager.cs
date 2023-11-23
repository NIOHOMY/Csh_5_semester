using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication1.Models;
using System.Security.Policy;



using Publisher = WebApplication1.Models.Publisher;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Data
{
    public class DatabaseManager : Controller
    {
        private readonly LibraryContext _context;

        public DatabaseManager(LibraryContext context)
        {
            _context = context;
        }


        public Reader? GetReaderById(int readerId)
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

        public Book? GetBookById(int bookId)
        {
            try
            {
                return _context.Books.Include(b => b.FirstAuthor).Include(b => b.Publisher).FirstOrDefault(book => book.BookId == bookId);
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

        public List<Book> SearchBooksByTitleOrFirstAuthor(string searchString)
        {
            var result = _context.Books
                .Include(b => b.FirstAuthor)
                .Where(b => b.Title.ToLower().Contains(searchString.ToLower()) || b.FirstAuthor.Name.ToLower().Contains(searchString.ToLower()))
                .ToList();

            return result;
        }

        public List<Book> SearchAvailableBooksByTitleOrFirstAuthor(string searchString)
        {
            var result = _context.Books
                .Include(b => b.FirstAuthor)
                .Where(b => (b.Title.ToLower().Contains(searchString.ToLower()) || b.FirstAuthor.Name.ToLower().Contains(searchString.ToLower())) && b.NumberOfExamples>0)
                .ToList();

            return result;
        }


        public Issue? GetIssueById(int issueId)
        {
            try
            {
                return _context.Issues
                    .Include(b => b.Reader)
                    .Include(b => b.Books)
                        .ThenInclude(b => b.FirstAuthor)
                    .FirstOrDefault(issue => issue.IssueId == issueId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Произошла ошибка при получении выдачи по ID:", ex);
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public List<Issue> SearchIssuesByIdOrReader(string searchString)
        {
            var result = _context.Issues
                .Include(b => b.Reader).Include(issue => issue.Books).ThenInclude(b => b.FirstAuthor)
                .Where(b => b.IssueId.ToString().Contains(searchString) ||
                            b.Reader.LastName.ToLower().Contains(searchString.ToLower()) ||
                            b.Reader.FirstName.ToLower().Contains(searchString.ToLower()) ||
                            b.Reader.Patronymic.ToLower().Contains(searchString.ToLower()))
                .ToList();

            return result;
        }

        public Publisher? GetPublisherById(int publisherId)
        {
            try
            {
                return _context.Publishers.FirstOrDefault(publisher => publisher.PublisherId == publisherId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении издательства по ID:");
                Console.WriteLine(ex.Message);

                Debug.WriteLine("Произошла ошибка при получении издательства по ID:");
                Debug.WriteLine(ex.Message);

                return null;
            }
        }
        public Author? GetAuthorById(int authorId)
        {
            try
            {
                return _context.Authors.FirstOrDefault(author => author.AuthorId == authorId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении автора по ID:");
                Console.WriteLine(ex.Message);

                Debug.WriteLine("Произошла ошибка при получении автора по ID:");
                Debug.WriteLine(ex.Message);

                return null;
            }
        }

        public void UpdateIssue(Issue updatedIssue)
        {
            try
            {
                var existingIssue = _context.Issues
                    .Include(issue => issue.Books)
                    .FirstOrDefault(issue => issue.IssueId == updatedIssue.IssueId);

                if (existingIssue != null)
                {
                    existingIssue.IssueDate = updatedIssue.IssueDate;
                    existingIssue.ReturnDate = updatedIssue.ReturnDate;
                    existingIssue.isСonfirmed = updatedIssue.isСonfirmed;
                    existingIssue.ReaderId = updatedIssue.ReaderId;
                    // Обновление списка книг можно выполнить в соответствии с требованиями вашей системы

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Произошла ошибка при обновлении выдачи:", ex);
                Debug.WriteLine(ex.Message);
            }
        }
        public void UpdateIssue(int id,Issue issue, int[] selectedBooks, int[] selectedBooksToDelete)
        {
            var existingIssue = _context.Issues
                .Include(i => i.Books)
                .FirstOrDefault(i => i.IssueId == id);
            if (existingIssue != null)
            {
                existingIssue.IssueDate = issue.IssueDate;
                existingIssue.ReturnDate = issue.ReturnDate;
                
                //existingIssue.ReaderId = issue.ReaderId;

                // Получаем список уже существующих книг в выпуске
                var existingBookIds = existingIssue.Books.Select(b => b.BookId).ToList();
                if (selectedBooks != null && !existingIssue.isСonfirmed)
                {

                    // Находим новые книги, которые должны быть добавлены
                    var newBookIds = selectedBooks.Except(existingBookIds).ToList();

                    // Добавляем новые книги в выпуск
                    foreach (var bookId in newBookIds)
                    {
                        var bookToAdd = _context.Books.FirstOrDefault(b => b.BookId == bookId);
                        //DecreaseNumberOfExamples(bookId);
                        var book = _context.Books.FirstOrDefault(b => b.BookId == bookId);
                        book.NumberOfExamples -= 1;
                        existingIssue.Books.Add(bookToAdd);
                        existingIssue.Price += bookToAdd.Price;
                    }

                }
                if (selectedBooksToDelete != null)
                {
                    // Удаляем книги из выпуска
                    foreach (var bookId in selectedBooksToDelete)
                    {
                        var bookToRemove = existingIssue.Books.FirstOrDefault(b => b.BookId == bookId);
                        //IncreaseNumberOfExamples(bookId);
                        var book = _context.Books.FirstOrDefault(b => b.BookId == bookId);
                        book.NumberOfExamples += 1;
                        existingIssue.Books.Remove(bookToRemove);
                        existingIssue.Price -= existingIssue.isСonfirmed? 0 : bookToRemove.Price;
                    }

                }
                existingIssue.isСonfirmed = existingIssue.isСonfirmed? existingIssue.isСonfirmed : issue.isСonfirmed;
            }

            _context.SaveChanges();
        }
        public void UpdateIssueStatus(int id, bool status)
        {
            var existingIssue = _context.Issues
                .Include(i => i.Books)
                .FirstOrDefault(i => i.IssueId == id);
            if (existingIssue != null)
            {
                existingIssue.isСonfirmed = status;
                _context.SaveChanges();
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
                //book.FirstAuthor = _context.Authors.FirstOrDefault(author => author.AuthorId == book.FirstAuthorId);
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
        public void UpdateBook(Book book)
        {
            try
            {
                _context.Books.Update(book);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при обновлении книги:");
                Console.WriteLine(ex.Message);

                Debug.WriteLine("Произошла ошибка при обновлении книги:");
                Debug.WriteLine(ex.Message);
            }
        }
        public void UpdateReader(Reader reader)
        {
            try
            {
                var readerOrig = _context.Readers.FirstOrDefault(x => x.ReaderId == reader.ReaderId);
                if (readerOrig != null)
                {
                    readerOrig.Address = reader.Address;
                    readerOrig.PhoneNumber = reader.PhoneNumber;
                    readerOrig.FirstName = reader.FirstName;
                    readerOrig.LastName = reader.LastName;
                    readerOrig.Patronymic = reader.Patronymic;

                    _context.Readers.Update(readerOrig);
                    _context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при обновлении читателя:");
                Console.WriteLine(ex.Message);

                Debug.WriteLine("Произошла ошибка при обновлении читателя:");
                Debug.WriteLine(ex.Message);
            }
        }
        public void UpdatePublisher(Publisher publisher)
        {
            try
            {
                _context.Publishers.Update(publisher);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при обновлении издательства:");
                Console.WriteLine(ex.Message);

                Debug.WriteLine("Произошла ошибка при обновлении издательства:");
                Debug.WriteLine(ex.Message);
            }
        }
        public void UpdateAuthor(Author author)
        {
            try
            {
                _context.Authors.Update(author);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при обновлении автора:");
                Console.WriteLine(ex.Message);

                Debug.WriteLine("Произошла ошибка при обновлении автора:");
                Debug.WriteLine(ex.Message);
            }
        }
        public void AddIssue(Issue issue)
        {
            try
            {
                _context.Issues.Add(issue);
                foreach (Book book in issue.Books)
                {
                    DecreaseNumberOfExamples(book.BookId);
                    issue.Price += book.Price;
                }
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

        public List<Author>? GetAllAuthors()
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
                //List<Book> book = _context.Books.Include(b => b.FirstAuthor).Include(b => b.Publisher).Where(b => b.NumberOfExamples > 0).ToList();
                return _context.Books.Include(b => b.FirstAuthor).Include(b => b.Publisher).Where(b => b.NumberOfExamples > 0).ToList();
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
        public List<Book> GetArchivedBooks()
        {
            try
            {
                return _context.Books.Include(b => b.FirstAuthor).Include(b => b.Publisher).Where(b => b.NumberOfExamples == 0).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении списка книг в архиве:");
                Console.WriteLine(ex.Message);

                Debug.WriteLine("Произошла ошибка при получении списка книг в архиве:");
                Debug.WriteLine(ex.Message);

                return new List<Book>();
            }
        }
        public List<Issue> GetAllIssues()
        {
            try
            {
                return _context.Issues.Include(b => b.Reader).Include(b => b.Books)
                    .ThenInclude(b => b.FirstAuthor)
                    .ToList();
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
        public List<Issue> GetIssuesByStatus(bool isConfirmed)
        {
            try
            {
                return _context.Issues.Include(b => b.Reader).Include(b => b.Books)
                    .ThenInclude(b => b.FirstAuthor).Where(i => i.isСonfirmed == isConfirmed)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении списка выдач с фильтром:");
                Console.WriteLine(ex.Message);

                Debug.WriteLine("Произошла ошибка при получении списка выдач с фильтром:");
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
                    Debug.WriteLine("Выдача не найдена.");
                    return false;
                }
                var Books = GetIssueBooksByIssueId(issueId);
                foreach (Book book in Books)
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

        internal Task<string?> GetBookById(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
