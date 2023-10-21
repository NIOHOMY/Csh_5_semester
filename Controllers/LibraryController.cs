using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibraryManagementSystem.DAL;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{

    public class LibraryController
    {
        private readonly DatabaseManager _databaseManager;

        public LibraryController(DatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
        }

        public void Run()
        {
            bool exit = false;

            while (!exit)
            {
                PrintMenu();
                int choice = GetChoice();

                switch (choice)
                {
                    case 1:
                        AddAuthor();
                        break;
                    case 2:
                        AddBook();
                        break;
                    case 3:
                        AddIssue();
                        break;
                    case 4:
                        AddIssueBook();
                        break;
                    case 5:
                        AddPublisher();
                        break;
                    case 6:
                        AddReader();
                        break;
                    case 7:
                        GetAllAuthors();
                        break;
                    case 8:
                        GetAllBooks();
                        break;
                    case 9:
                        GetAllIssues();
                        break;
                    case 10:
                        GetAllIssueBooks();
                        break;
                    case 11:
                        GetAllPublishers();
                        break;
                    case 12:
                        GetAllReaders();
                        break;
                    case 13:
                        DeleteAuthor();
                        break;
                    case 14:
                        DeleteBook();
                        break;
                    case 15:
                        DeleteIssue();
                        break;
                    case 16:
                        DeleteIssueBook();
                        break;
                    case 17:
                        DeletePublisher();
                        break;
                    case 18:
                        DeleteReader();
                        break;
                    case 19:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void PrintMenu()
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Добавить автора");
            Console.WriteLine("2. Добавить книгу");
            Console.WriteLine("3. Добавить выдачу");
            Console.WriteLine("4. Добавить экземпляр книги в выдачу");
            Console.WriteLine("5. Добавить издателя");
            Console.WriteLine("6. Добавить читателя");
            Console.WriteLine("7. Просмотреть всех авторов");
            Console.WriteLine("8. Просмотреть все книги");
            Console.WriteLine("9. Просмотреть все выдачи");
            Console.WriteLine("10. Просмотреть все экземпляры книг в выдаче");
            Console.WriteLine("11. Просмотреть всех издателей");
            Console.WriteLine("12. Просмотреть всех читателей");
            Console.WriteLine("13. Удалить автора");
            Console.WriteLine("14. Удалить книгу");
            Console.WriteLine("15. Удалить выдачу");
            Console.WriteLine("16. Удалить экземпляр книги в выдаче");
            Console.WriteLine("17. Удалить издателя");
            Console.WriteLine("18. Удалить читателя");
            Console.WriteLine("19. Выход");
        }

        private int GetChoice()
        {
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
            return choice;
        }

        private void AddAuthor()
        {
            Console.WriteLine("Введите имя автора:");
            string name = Console.ReadLine();

            Author author = new Author
            {
                Name = name
            };

            _databaseManager.AddAuthor(author);
            Console.WriteLine("Автор успешно добавлен.");
        }

        private void AddBook()
        {
            Console.WriteLine("Введите название книги:");
            string title = Console.ReadLine();

            Console.WriteLine("Введите ID первого автора:");
            int firstAuthorId = GetChoice();

            Console.WriteLine("Введите год публикации:");
            int yearOfPublication = GetChoice();

            Console.WriteLine("Введите цену:");
            decimal price = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Введите количество экземпляров:");
            int numberOfExamples = GetChoice();

            Console.WriteLine("Введите ID издателя:");
            int publisherId = GetChoice();

            Book book = new Book
            {
                Title = title,
                FirstAuthorId = firstAuthorId,
                YearOfPublication = yearOfPublication,
                Price = price,
                NumberOfExamples = numberOfExamples,
                PublisherId = publisherId
            };

            _databaseManager.AddBook(book);
            Console.WriteLine("Книга успешно добавлена.");
        }

        private void AddIssue()
        {
            Console.WriteLine("Введите дату выдачи (yyyy-MM-dd):");
            DateTime issueDate = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Введите дату возврата (yyyy-MM-dd):");
            DateTime returnDate = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Введите ID читателя:");
            int readerId = GetChoice();

            Issue issue = new Issue
            {
                IssueDate = issueDate,
                ReturnDate = returnDate,
                ReaderId = readerId
            };

            _databaseManager.AddIssue(issue);
            Console.WriteLine("Выдача успешно добавлена.");
        }

        private void AddIssueBook()
        {
            Console.WriteLine("Введите ID книги:");
            int bookId = GetChoice();

            Console.WriteLine("Введите ID выдачи:");
            int issueId = GetChoice();

            IssueBook issueBook = new IssueBook
            {
                BookId = bookId,
                IssueId = issueId
            };

            _databaseManager.AddIssueBook(issueBook);
            Console.WriteLine("Экземпляр книги успешно добавлен в выдачу.");
        }

        private void AddPublisher()
        {
            Console.WriteLine("Введите название издателя:");
            string nameOfPublisher = Console.ReadLine();

            Console.WriteLine("Введите город издателя:");
            string city = Console.ReadLine();

            Publisher publisher = new Publisher
            {
                NameOfPublisher = nameOfPublisher,
                City = city
            };

            _databaseManager.AddPublisher(publisher);
            Console.WriteLine("Издатель успешно добавлен.");
        }

        private void AddReader()
        {
            Console.WriteLine("Введите ФИО читателя:");
            string fullName = Console.ReadLine();

            Console.WriteLine("Введите адрес:");
            string address = Console.ReadLine();

            Console.WriteLine("Введите номер телефона:");
            string phoneNumber = Console.ReadLine();

            Reader reader = new Reader
            {
                FullName = fullName,
                Address = address,
                PhoneNumber = phoneNumber
            };

            _databaseManager.AddReader(reader);
            Console.WriteLine("Читатель успешно добавлен.");
        }

        private void GetAllAuthors()
        {
            var authors = _databaseManager.GetAllAuthors();
            Console.WriteLine("Список всех авторов:");

            foreach (var author in authors)
            {
                Console.WriteLine($"ID: {author.AuthorId}, Имя: {author.Name}");
            }
        }

        private void GetAllBooks()
        {
            var books = _databaseManager.GetAllBooks();
            Console.WriteLine("Список всех книг:");

            foreach (var book in books)
            {
                Console.WriteLine($"ID: {book.BookId}, Название: {book.Title}, " +
                    $"Первый автор: {book.FirstAuthor.Name}, Год публикации: {book.YearOfPublication}, " +
                    $"Цена: {book.Price}, Количество экземпляров: {book.NumberOfExamples}, " +
                    $"Издатель: {book.Publisher.NameOfPublisher}");
            }
        }

        private void GetAllIssues()
        {
            var issues = _databaseManager.GetAllIssues();
            Console.WriteLine("Список всех выдач:");

            foreach (var issue in issues)
            {
                Console.WriteLine($"ID: {issue.IssueId}, Дата выдачи: {issue.IssueDate}, " +
                    $"Дата возврата: {issue.ReturnDate}, Читатель: {issue.Reader.FullName}");
            }
        }

        private void GetAllIssueBooks()
        {
            var issueBooks = _databaseManager.GetAllIssueBooks();
            Console.WriteLine("Список всех экземпляров книг в выдаче:");

            foreach (var issueBook in issueBooks)
            {
                Console.WriteLine($"ID: {issueBook.Id}, Книга: {issueBook.Book.Title}, Выдача: {issueBook.Issue.IssueDate}");
            }
        }

        private void GetAllPublishers()
        {
            var publishers = _databaseManager.GetAllPublishers();
            Console.WriteLine("Список всех издателей:");

            foreach (var publisher in publishers)
            {
                Console.WriteLine($"ID: {publisher.PublisherId}, Название: {publisher.NameOfPublisher}, Город: {publisher.City}");
            }
        }

        private void GetAllReaders()
        {
            var readers = _databaseManager.GetAllReaders();
            Console.WriteLine("Список всех читателей:");

            foreach (var reader in readers)
            {
                Console.WriteLine($"ID: {reader.ReaderId}, ФИО: {reader.FullName}, Адрес: {reader.Address}, " +
                    $"Номер телефона: {reader.PhoneNumber}");
            }
        }

        private void DeleteAuthor()
        {
            Console.WriteLine("Введите ID автора, которого хотите удалить:");
            int authorId = GetChoice();

            _databaseManager.DeleteAuthor(authorId);
            Console.WriteLine("Автор успешно удален.");
        }

        private void DeleteBook()
        {
            Console.WriteLine("Введите ID книги, которую хотите удалить:");
            int bookId = GetChoice();

            _databaseManager.DeleteBook(bookId);
            Console.WriteLine("Книга успешно удалена.");
        }

        private void DeleteIssue()
        {
            Console.WriteLine("Введите ID выдачи, которую хотите удалить:");
            int issueId = GetChoice();

            _databaseManager.DeleteIssue(issueId);
            Console.WriteLine("Выдача успешно удалена.");
        }

        private void DeleteIssueBook()
        {
            Console.WriteLine("Введите ID экземпляра книги в выдаче, который хотите удалить:");
            int issueBookId = GetChoice();

            _databaseManager.DeleteIssueBook(issueBookId);
            Console.WriteLine("Экземпляр книги успешно удален из выдачи.");
        }

        private void DeletePublisher()
        {
            Console.WriteLine("Введите ID издателя, которого хотите удалить:");
            int publisherId = GetChoice();

            _databaseManager.DeletePublisher(publisherId);
            Console.WriteLine("Издатель успешно удален.");
        }

        private void DeleteReader()
        {
            Console.WriteLine("Введите ID читателя, которого хотите удалить:");
            int readerId = GetChoice();

            _databaseManager.DeleteReader(readerId);
            Console.WriteLine("Читатель успешно удален.");
        }
    }

}
