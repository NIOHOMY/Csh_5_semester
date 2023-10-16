using ConsoleApp1;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            using var context = new StudentContext();
            /*
            string[] groupNames = { "ФИИТ", "МОАИС", "ПМИ" };
            List<Group> groups = new List<Group>();

            for (int i = 0; i < groupNames.Length; i++)
            {
                Group group = new Group
                {
                    GroupName = groupNames[i]
                };
                groups.Add(group);
            }
            foreach (var group in groups)
            {
                context.Groups.Add(group);
            }
            context.SaveChanges();
            */
            var storage = new StudentDBStorage(context);
            var controller = new StudentController(storage);


            while (true)
            {
                controller.PrintStudentsByGroups();
                Console.WriteLine("Меню:");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Удалить студента");
                Console.WriteLine("3. Вывести список студентов по группам");
                Console.WriteLine("4. Выход");

                Console.Write("Введите номер операции: ");
                string choice = Console.ReadLine();

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
                        Console.WriteLine("Неверный выбор. Повторите ввод.");
                        break;
                }
            }
        }
    }
}

