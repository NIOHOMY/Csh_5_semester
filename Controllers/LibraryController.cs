using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibraryManagementSystem.DAL;
using LibraryManagementSystem.Models;

using Microsoft.EntityFrameworkCore;


/*
Menu:
1. Создание
    1. Создать издательство
    2. Создать автора
    3. Создать читателя
2. Управление издательствами
    (список издательств на выбор)
    1. Выпустить книгу
        (список авторов на выбор)
3. Управление читателями
    (список читателей на выбор)
    1. Запросить выдачу
        (добавляем книги в выдачу из списка доступных книг)

~ вывести/удалить выдачи

*/

namespace LibraryManagementSystem.Controllers
{
    public class LibraryController
    {
        private readonly DatabaseManager _databaseManager;
        private bool _exitRequested;

        public LibraryController(DatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
            _exitRequested = false;
        }

        public void Start()
        {
            while (!_exitRequested)
            {
                try
                {
                    MainMenu();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка: " + ex.Message);
                    Debug.WriteLine("Произошла ошибка: " + ex.Message);
                }
            }
        }

        public void MainMenu()
        {
            Console.WriteLine("Меню:");
            Console.WriteLine("1. Создание");
            Console.WriteLine("2. Удаление");
            Console.WriteLine("3. Управление издательствами");
            Console.WriteLine("4. Управление читателями");
            Console.WriteLine("5. Посмотреть все выдачи");
            Console.WriteLine("6. Удалить выдачу");
            Console.WriteLine("0. Выйти");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > 6)
            {
                Console.WriteLine("Неверный выбор. Пожалуйста, введите число от 0 до 3.");
            }

            switch (choice)
            {
                case 1:
                    CreateMenu();
                    break;
                case 2:
                    DeleteMenu();
                    break;
                case 3:
                    PublishersMenu();
                    break;
                case 4:
                    ReadersMenu();
                    break;
                case 5:
                    PrintAllIssues();
                    break;
                case 6:
                    DeleteIssue();
                    break;
                case 0:
                    _exitRequested = true;
                    break;
                default:
                    break;
            }
        }

        public void CreateMenu()
        {
            Console.WriteLine("Меню создания:");
            Console.WriteLine("1. Создать издательство");
            Console.WriteLine("2. Создать автора");
            Console.WriteLine("3. Создать читателя");
            Console.WriteLine("0. Назад");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > 3)
            {
                Console.WriteLine("Неверный выбор. Пожалуйста, введите число от 0 до 3.");
            }

            switch (choice)
            {
                case 1:
                    try
                    {
                        CreatePublisher();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Произошла ошибка при создании издательства: " + ex.Message);
                        Debug.WriteLine("Произошла ошибка при создании издательства: " + ex.Message);
                    }
                    break;
                case 2:
                    try
                    {
                        CreateAuthor();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Произошла ошибка при создании автора: " + ex.Message);
                        Debug.WriteLine("Произошла ошибка при создании автора: " + ex.Message);
                    }
                    break;
                case 3:
                    try
                    {
                        CreateReader();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Произошла ошибка при создании читателя: " + ex.Message);
                        Debug.WriteLine("Произошла ошибка при создании читателя: " + ex.Message);
                    }
                    break;
                case 0:
                    break;
                default:
                    break;
            }
        }

        public void DeleteMenu()
        {
            Console.WriteLine("Меню удаления:");
            Console.WriteLine("1. Удалить издательство");
            Console.WriteLine("2. Удалить автора");
            Console.WriteLine("3. Удалить книгу");
            Console.WriteLine("4. Удалить читателя");
            Console.WriteLine("0. Назад");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > 4)
            {
                Console.WriteLine("Неверный выбор. Пожалуйста, введите число от 0 до 4.");
            }

            switch (choice)
            {
                case 1:
                    try
                    {
                        DeletePublisher();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Произошла ошибка при удалении издательства: " + ex.Message);
                        Debug.WriteLine("Произошла ошибка при удалении издательства: " + ex.Message);
                    }
                    break;
                case 2:
                    try
                    {
                        DeleteAuthor();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Произошла ошибка при удалении автора: " + ex.Message);
                        Debug.WriteLine("Произошла ошибка при удалении автора: " + ex.Message);
                    }
                    break;
                case 3:
                    try
                    {
                        DeleteBook();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Произошла ошибка при удалении книги: " + ex.Message);
                        Debug.WriteLine("Произошла ошибка при удалении книги: " + ex.Message);
                    }
                    break;
                case 4:
                    try
                    {
                        DeleteReader();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Произошла ошибка при удалении читателя: " + ex.Message);
                        Debug.WriteLine("Произошла ошибка при удалении читателя: " + ex.Message);
                    }
                    break;
                case 0:
                    break;
                default:
                    break;
            }
        }


        public void PublishersMenu()
        {
            try
            {
                Console.WriteLine("Меню управления издательствами:");
                Console.WriteLine("0. Назад");

                var publishers = _databaseManager.GetAllPublishers();
                for (int i = 0; i < publishers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {publishers[i].NameOfPublisher}");
                }

                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > publishers.Count)
                {
                    Console.WriteLine($"Неверный выбор. Пожалуйста, введите число от 0 до {publishers.Count}.");
                }

                if (choice == 0)
                {
                    return;
                }

                Publisher? selectedPublisher = publishers[choice - 1];

                Console.WriteLine("1. Выпустить книгу");
                Console.WriteLine("0. Назад");

                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > 1)
                {
                    Console.WriteLine("Неверный выбор. Пожалуйста, введите 0 или 1.");
                }

                if (choice == 0)
                {
                    return;
                }

                PublishBook(selectedPublisher);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при управлении издательствами: " + ex.Message);
                Debug.WriteLine("Произошла ошибка при управлении издательствами: " + ex.Message);
            }
        }

        public void ReadersMenu()
        {
            try
            {
                Console.WriteLine("Меню управления читателями:");
                Console.WriteLine("0. Назад");

                var readers = _databaseManager.GetAllReaders();
                for (int i = 0; i < readers.Count; i++)
                {
                    if (readers[i].Patronymic != null)
                        Console.WriteLine($"{i + 1}. {readers[i].FirstName} {readers[i].LastName} {readers[i].Patronymic}");
                    else
                        Console.WriteLine($"{i + 1}. {readers[i].FirstName} {readers[i].LastName}");
                }

                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > readers.Count)
                {
                    Console.WriteLine($"Неверный выбор. Пожалуйста, введите число от 0 до {readers.Count}.");
                }

                if (choice == 0)
                {
                    return;
                }

                var selectedReader = readers[choice - 1];

                Console.WriteLine("1. Запросить выдачу");
                Console.WriteLine("0. Назад");

                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > 1)
                {
                    Console.WriteLine("Неверный выбор. Пожалуйста, введите 0 или 1.");
                }

                if (choice == 0)
                {
                    return;
                }

                RequestIssue(selectedReader);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при управлении читателями: " + ex.Message);
                Debug.WriteLine("Произошла ошибка при управлении читателями: " + ex.Message);
            }
        }


        public void CreatePublisher()
        {
            try
            {
                Console.WriteLine("Введите название издательства:");
                string name = Console.ReadLine();

                Console.WriteLine("Введите город издательства:");
                string city = Console.ReadLine();

                // Создание объекта Publisher и сохранение в базе данных
                var publisher = new Publisher
                {
                    NameOfPublisher = name,
                    City = city
                };
                _databaseManager.AddPublisher(publisher);

                Console.WriteLine("Издательство успешно создано!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при создании издательства: " + ex.Message);
                Debug.WriteLine("Произошла ошибка при создании издательства: " + ex.Message);
            }
        }

        public void CreateAuthor()
        {
            try
            {
                Console.WriteLine("Введите имя автора:");
                string name = Console.ReadLine();

                // Создание объекта Author и сохранение в базе данных
                var author = new Author
                {
                    Name = name
                };
                _databaseManager.AddAuthor(author);

                Console.WriteLine("Автор успешно создан!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при создании автора: " + ex.Message);
                Debug.WriteLine("Произошла ошибка при создании автора: " + ex.Message);
            }
        }

        public void CreateReader()
        {
            try
            {
                Console.WriteLine("Введите полное имя читателя:");
                string fullName = Console.ReadLine();

                Console.WriteLine("Введите адрес читателя:");
                string address = Console.ReadLine();

                Console.WriteLine("Введите номер телефона читателя:");
                string phoneNumber = Console.ReadLine();

                // Создание объекта Reader и сохранение в базе данных
                var reader = new Reader
                {
                    Address = address,
                    PhoneNumber = phoneNumber
                };
                reader.SetFullName(fullName);
                _databaseManager.AddReader(reader);

                Console.WriteLine("Читатель успешно создан!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при создании читателя: " + ex.Message);
                Debug.WriteLine("Произошла ошибка при создании читателя: " + ex.Message);
            }
        }

        public void DeletePublisher()
        {
            try
            {
                Console.WriteLine("Список издательств:");
                foreach (var publisher in _databaseManager.GetAllPublishers())
                {
                    Console.WriteLine($"ID: {publisher.PublisherId}, Название: {publisher.NameOfPublisher}");
                }

                Console.WriteLine("Введите ID издательства для удаления:");
                int publisherId;
                while (!int.TryParse(Console.ReadLine(), out publisherId))
                {
                    Console.WriteLine("Неверный ID. Пожалуйста, введите целое число.");
                }

                if (_databaseManager.DeletePublisher(publisherId))
                    Console.WriteLine("Издательство успешно удалено.");
                else
                    Console.WriteLine("Ошибка при удалении.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при удалении издательства: " + ex.Message);
                Debug.WriteLine("Произошла ошибка при удалении издательства: " + ex.Message);
            }
        }

        public void DeleteAuthor()
        {
            try
            {
                Console.WriteLine("Список авторов:");
                foreach (var author in _databaseManager.GetAllAuthors())
                {
                    Console.WriteLine($"ID: {author.AuthorId}, Имя: {author.Name}");
                }

                Console.WriteLine("Введите ID автора для удаления:");
                int authorId;
                while (!int.TryParse(Console.ReadLine(), out authorId))
                {
                    Console.WriteLine("Неверный ID. Пожалуйста, введите целое число.");
                }

                if (_databaseManager.DeleteAuthor(authorId))
                    Console.WriteLine("Автор успешно удален.");
                else
                    Console.WriteLine("Ошибка при удалении.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при удалении автора: " + ex.Message);
                Debug.WriteLine("Произошла ошибка при удалении автора: " + ex.Message);
            }
        }

        public void DeleteBook()
        {
            try
            {
                Console.WriteLine("Список книг:");
                foreach (var book in _databaseManager.GetAllBooks())
                {
                    Console.WriteLine($"ID: {book.BookId}, Название: {book.Title}, Автор: {book.FirstAuthor.Name}, Издательство: {book.Publisher.NameOfPublisher}");
                }

                Console.WriteLine("Введите ID книги для удаления:");
                int bookId;
                while (!int.TryParse(Console.ReadLine(), out bookId))
                {
                    Console.WriteLine("Неверный ID. Пожалуйста, введите целое число.");
                }

                if (_databaseManager.DeleteBook(bookId))
                    Console.WriteLine("Книга успешно удалена.");
                else
                    Console.WriteLine("Ошибка при удалении.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при удалении книги: " + ex.Message);
                Debug.WriteLine("Произошла ошибка при удалении книги: " + ex.Message);
            }
        }

        public void DeleteReader()
        {
            try
            {
                Console.WriteLine("Список читателей:");
                foreach (var reader in _databaseManager.GetAllReaders())
                {
                    if (reader.Patronymic != null)
                        Console.WriteLine($"ID: {reader.ReaderId}, ФИО:  {reader.FirstName} {reader.LastName} {reader.Patronymic}");
                    else
                        Console.WriteLine($"ID: {reader.ReaderId}, ФИО:  {reader.FirstName} {reader.LastName}");
                }

                Console.WriteLine("Введите ID читателя для удаления:");
                int readerId;
                while (!int.TryParse(Console.ReadLine(), out readerId))
                {
                    Console.WriteLine("Неверный ID. Пожалуйста, введите целое число.");
                }

                if (_databaseManager.DeleteReader(readerId))
                    Console.WriteLine("Читатель успешно удален.");
                else
                    Console.WriteLine("Ошибка при удалении.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при удалении читателя: " + ex.Message);
                Debug.WriteLine("Произошла ошибка при удалении читателя: " + ex.Message);
            }
        }

        public void PublishBook(Publisher publisher)
        {
            try
            {
                Console.WriteLine("Список доступных авторов:");

                List<Author>? authors = _databaseManager.GetAllAuthors();
                for (int i = 0; i < authors.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {authors[i].Name}");
                }

                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > authors.Count)
                {
                    Console.WriteLine($"Неверный выбор. Пожалуйста, введите число от 1 до {authors.Count}.");
                }

                Author? selectedAuthor = authors[choice - 1];

                Console.WriteLine("Введите название книги:");
                string? title = Console.ReadLine();
                if (title != null)
                {
                    Console.WriteLine("Введите год публикации:");
                    int yearOfPublication;
                    while (!int.TryParse(Console.ReadLine(), out yearOfPublication))
                    {
                        Console.WriteLine("Некорректный ввод. Пожалуйста, введите корректный год публикации.");
                    }

                    Console.WriteLine("Введите цену книги:");
                    decimal price;
                    while (!decimal.TryParse(Console.ReadLine(), out price))
                    {
                        Console.WriteLine("Некорректный ввод. Пожалуйста, введите корректную цену книги.");
                    }

                    Console.WriteLine("Введите количество экземпляров книги:");
                    int numberOfExamples;
                    while (!int.TryParse(Console.ReadLine(), out numberOfExamples))
                    {
                        Console.WriteLine("Некорректный ввод. Пожалуйста, введите корректное количество экземпляров книги.");
                    }

                    // Создание объекта Book и сохранение в базе данных
                    Book? book = new Book
                    {
                        Title = title,
                        YearOfPublication = yearOfPublication,
                        Price = price,
                        NumberOfExamples = numberOfExamples,
                        PublisherId = publisher.PublisherId,
                        FirstAuthorId = selectedAuthor.AuthorId
                    };
                    _databaseManager.AddBook(book);

                    Console.WriteLine("Книга успешно выпущена!");
                }
                else
                {
                    Console.WriteLine("Некорректное название, публикация отменена.");

                }

               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при публикации книги:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Debug.WriteLine("Произошла ошибка при публикации книги:");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        public void RequestIssue(Reader reader)
        {
            try
            {
                Console.WriteLine("Список доступных книг:");

                var books = _databaseManager.GetAllAvailableBooks();
                for (int i = 0; i < books.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {books[i].Title}");
                }

                Console.WriteLine("Введите номера книг, которые вы хотите добавить в выдачу (разделенные запятой):");
                var bookChoices = new List<int>();
                string input = Console.ReadLine();
                foreach (var choice in input.Split(','))
                {
                    if (int.TryParse(choice.Trim(), out int bookChoice) && bookChoice >= 1 && bookChoice <= books.Count)
                    {
                        bookChoices.Add(bookChoice);
                    }
                }

                if (bookChoices.Count == 0)
                {
                    Console.WriteLine("Неверный выбор книг. Оформление выдачи отменено.");
                    return;
                }

                Console.WriteLine("Введите дату выдачи в формате dd.MM.yyyy:");
                DateTime issueDate;
                while (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null,
                    System.Globalization.DateTimeStyles.None, out issueDate))
                {
                    Console.WriteLine("Некорректный ввод. Пожалуйста, введите корректную дату выдачи в формате dd.MM.yyyy.");
                }

                Console.WriteLine("Введите дату возврата в формате dd.MM.yyyy:");
                DateTime returnDate;
                while (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null,
                    System.Globalization.DateTimeStyles.None, out returnDate))
                {
                    Console.WriteLine("Некорректный ввод. Пожалуйста, введите корректную дату возврата в формате dd.MM.yyyy.");
                }

                // Создание объекта Issue и сохранение в базе данных
                var issue = new Issue
                {
                    IssueDate = issueDate,
                    ReturnDate = returnDate,
                    ReaderId = reader.ReaderId,
                    Reader = _databaseManager.GetReaderById(reader.ReaderId),
                    Books = new List<Book>()
                };

                foreach (int bookChoice in bookChoices)
                {
                    Book? selectedBook = books[bookChoice - 1];
                    issue.Books.Add(selectedBook);
                    _databaseManager.DecreaseNumberOfExamples(selectedBook.BookId);
                }

                _databaseManager.AddIssue(issue);

                Console.WriteLine("Выдача успешно оформлена!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при оформлении выдачи:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Debug.WriteLine("Произошла ошибка при оформлении выдачи:");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        public void PrintAllIssues()
        {
            try
            {
                var issues = _databaseManager.GetAllIssues();

                foreach (var issue in issues)
                {
                    Console.WriteLine($"Выдача #{issue.IssueId}");
                    Console.WriteLine($"Дата выдачи: {issue.IssueDate.ToString("dd.MM.yyyy")}");
                    Console.WriteLine($"Дата возврата: {issue.ReturnDate.ToString("dd.MM.yyyy")}");
                    Reader reader = issue.Reader;
                    if (reader.Patronymic != null)
                        Console.WriteLine($"Читатель:  {reader.FirstName} {reader.LastName} {reader.Patronymic}");
                    else
                        Console.WriteLine($"Читатель:  {reader.FirstName} {reader.LastName}");
                    Console.WriteLine("Список книг:");

                    List<Book>? Books = _databaseManager.GetIssueBooksByIssueId(issue.IssueId);
                    foreach (var Book in Books)
                    {
                        Console.WriteLine($"{Book.Title}");
                    }

                    Console.WriteLine("--------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при выводе списка выдач:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Debug.WriteLine("Произошла ошибка при выводе списка выдач:");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteIssue()
        {
            try
            {
                Console.WriteLine("Введите ID выдачи, которую вы хотите удалить:");
                PrintAllIssues();
                int _issueId;
                if (!int.TryParse(Console.ReadLine(), out _issueId))
                {
                    Console.WriteLine("Некорректный ID выдачи.");
                    return;
                }

                _databaseManager.DeleteIssue(_issueId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при удалении выдачи:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Debug.WriteLine("Произошла ошибка при удалении выдачи:");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

    }
}
