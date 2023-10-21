using ConsoleApp1.Models;
using ConsoleApp1.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

// парсер/структуру студента            +
// начатльные данные
// функции работы с бд добавить в dbstorage     +
// выводить для удаления не пустые группы       +
// раскидать по файлам, добавить сообщение об ошибке в []   +
// Model - dataclasses DAL - для работы с бд    +
// добавить try catch

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            using var context = new StudentContext();
            var storage = new StudentDBStorage(context);
            var controller = new StudentMenuController(storage);

            try
            {
                while (true)
                {
                    controller.PrintStudentsByGroups();
                    Console.WriteLine(" Меню:");
                    Console.WriteLine(" 1. Добавить студента");
                    Console.WriteLine(" 2. Удалить студента");
                    Console.WriteLine(" 3. Вывести список студентов по группам");
                    Console.WriteLine(" 4. Выход");

                    Console.Write(" Введите номер операции: ");
                    string? choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            controller.AddStudent();
                            break;
                        case "2":
                            controller.RemoveStudent();
                            break;
                        case "3":
                            controller.PrintStudentsByGroups();
                            break;
                        case "4":
                            return;
                        default:
                            Console.WriteLine(" ! Неверный выбор. Повторите ввод.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" ! Error: {ex.Message}");
                Debug.WriteLine($" ! Error: {ex.Message}");
            }
        }
    }

}

