using System;
using System.Collections.Generic;
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
                MainMenu();
            }
        }

        public void MainMenu()
        {
            Console.WriteLine("Меню:");
            Console.WriteLine("1. Создание");
            Console.WriteLine("2. Управление издательствами");
            Console.WriteLine("3. Управление читателями");
            Console.WriteLine("4. Посмотреть все выдачи");
            Console.WriteLine("5. Удалить выдачу");
            Console.WriteLine("0. Выйти");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > 5)
            {
                Console.WriteLine("Неверный выбор. Пожалуйста, введите число от 0 до 3.");
            }

            switch (choice)
            {
                case 1:
                    CreateMenu();
                    break;
                case 2:
                    PublishersMenu();
                    break;
                case 3:
                    ReadersMenu();
                    break;
                case 4:
                    PrintAllIssues();
                    break;
                case 5:
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
                    CreatePublisher();
                    break;
                case 2:
                    CreateAuthor();
                    break;
                case 3:
                    CreateReader();
                    break;
                case 0:
                    break;
                default:
                    break;
            }
        }

        public void PublishersMenu()
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

            var selectedPublisher = publishers[choice - 1];

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

        public void ReadersMenu()
        {
            Console.WriteLine("Меню управления читателями:");
            Console.WriteLine("0. Назад");

            var readers = _databaseManager.GetAllReaders();
            for (int i = 0; i < readers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {readers[i].FullName}");
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

        public void CreatePublisher()
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

        public void CreateAuthor()
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

        public void CreateReader()
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
                FullName = fullName,
                Address = address,
                PhoneNumber = phoneNumber
            };
            _databaseManager.AddReader(reader);

            Console.WriteLine("Читатель успешно создан!");
        }

        public void PublishBook(Publisher publisher)
        {
            Console.WriteLine("Список доступных авторов:");

            var authors = _databaseManager.GetAllAuthors();
            for (int i = 0; i < authors.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {authors[i].Name}");
            }

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > authors.Count)
            {
                Console.WriteLine($"Неверный выбор. Пожалуйста, введите число от 1 до {authors.Count}.");
            }

            var selectedAuthor = authors[choice - 1];

            Console.WriteLine("Введите название книги:");
            string title = Console.ReadLine();

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
            var book = new Book
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

        public void RequestIssue(Reader reader)
        {
            Console.WriteLine("Список доступных книг:");

            var books = _databaseManager.GetAllBooks();
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
                ReaderId = reader.ReaderId
            };
            _databaseManager.AddIssue(issue);

            foreach (var bookChoice in bookChoices)
            {
                var selectedBook = books[bookChoice - 1];

                // Создание объекта IssueBook и сохранение в базе данных
                var issueBook = new IssueBook
                {
                    BookId = selectedBook.BookId,
                    IssueId = issue.IssueId
                };
                _databaseManager.AddIssueBook(issueBook);
            }

            Console.WriteLine("Выдача успешно оформлена!");
        }

        public void PrintAllIssues()
        {
            var issues = _databaseManager.GetAllIssues();

            foreach (var issue in issues)
            {
                Console.WriteLine($"Выдача #{issue.IssueId}");
                Console.WriteLine($"Дата выдачи: {issue.IssueDate.ToString("dd.MM.yyyy")}");
                Console.WriteLine($"Дата возврата: {issue.ReturnDate.ToString("dd.MM.yyyy")}");
                Console.WriteLine("Список книг:");

                var issueBooks = _databaseManager.GetIssueBooksByIssueId(issue.IssueId);
                foreach (var issueBook in issueBooks)
                {
                    var book = _databaseManager.GetBookById(issueBook.BookId);
                    Console.WriteLine($"{book.Title}");
                }

                Console.WriteLine("--------------------");
            }
        }
        public void DeleteIssue()
        {
            Console.WriteLine("Введите ID выдачи, которую вы хотите удалить:");
            PrintAllIssues();
            int _issueId;
            if (!int.TryParse(Console.ReadLine(), out _issueId))
            {
                Console.WriteLine("Некорректный ID выдачи.");
            }

            _databaseManager.DeleteIssue(_issueId);
        }

    }
}
