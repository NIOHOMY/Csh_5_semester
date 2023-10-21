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
    public class DatabaseManager
    {
        private readonly LibraryContext _context;

        public DatabaseManager(LibraryContext context)
        {
            _context = context;
        }

        public void AddAuthor(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void AddIssue(Issue issue)
        {
            _context.Issues.Add(issue);
            _context.SaveChanges();
        }

        public void AddIssueBook(IssueBook issueBook)
        {
            _context.IssueBooks.Add(issueBook);
            _context.SaveChanges();
        }

        public void AddPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            _context.SaveChanges();
        }

        public void AddReader(Reader reader)
        {
            _context.Readers.Add(reader);
            _context.SaveChanges();
        }

        public List<Author> GetAllAuthors()
        {
            return _context.Authors.ToList();
        }

        public List<Book> GetAllBooks()
        {
            return _context.Books.Include(b => b.FirstAuthor).Include(b => b.Publisher).ToList();
        }

        public List<Issue> GetAllIssues()
        {
            return _context.Issues.Include(i => i.Reader).ToList();
        }

        public List<IssueBook> GetAllIssueBooks()
        {
            return _context.IssueBooks.Include(ib => ib.Book).Include(ib => ib.Issue).ToList();
        }

        public List<Publisher> GetAllPublishers()
        {
            return _context.Publishers.ToList();
        }

        public List<Reader> GetAllReaders()
        {
            return _context.Readers.ToList();
        }

        public void DeleteAuthor(int authorId)
        {
            var author = _context.Authors.Find(authorId);

            if (author != null)
            {
                _context.Authors.Remove(author);
                _context.SaveChanges();
            }
        }

        public void DeleteBook(int bookId)
        {
            var book = _context.Books.Find(bookId);

            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }

        public void DeleteIssue(int issueId)
        {
            var issue = _context.Issues.Find(issueId);

            if (issue != null)
            {
                _context.Issues.Remove(issue);
                _context.SaveChanges();
            }
        }

        public void DeleteIssueBook(int issueBookId)
        {
            var issueBook = _context.IssueBooks.Find(issueBookId);

            if (issueBook != null)
            {
                _context.IssueBooks.Remove(issueBook);
                _context.SaveChanges();
            }
        }

        public void DeletePublisher(int publisherId)
        {
            var publisher = _context.Publishers.Find(publisherId);

            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                _context.SaveChanges();
            }
        }

        public void DeleteReader(int readerId)
        {
            var reader = _context.Readers.Find(readerId);

            if (reader != null)
            {
                _context.Readers.Remove(reader);
                _context.SaveChanges();
            }
        }
    }
}
