using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class StudentController
    {
        private readonly StudentDBStorage _storage;

        public StudentController(StudentDBStorage storage)
        {
            _storage = storage;
        }

        public List<Student> GetStudentsByGroup(int groupId)
        {
            return _storage.GetAllStudents().Where(s => s.GroupId == groupId).ToList();
        }


        public void AddStudent()
        {
            Console.WriteLine("Выберете номер группы:");
            var groups = _storage.GetAllGroups();
            foreach (var _group in groups)
            {
                Console.WriteLine($"{_group.GroupId} - {_group.GroupName}");
            }

            if (!int.TryParse(Console.ReadLine(), out int groupId))
            {
                Console.WriteLine("Неверный номер группы.");
                return;
            }

            var group = groups.FirstOrDefault(g => g.GroupId == groupId);
            if (group == null)
            {
                Console.WriteLine("Группа не найдена.");
                return;
            }

            Console.WriteLine("Введите ФИО студента:");
            var name = Console.ReadLine();

            var student = new Student
            {
                Name = name,
                GroupId = groupId
            };
            _storage.AddStudent(student);

            Console.WriteLine("Студент успешно добавлен.");
        }

        public void RemoveStudent()
        {
            Console.WriteLine("Выберете номер группы:");
            var groups = _storage.GetAllGroups();
            foreach (var _group in groups)
            {
                Console.WriteLine($"{_group.GroupId} - {_group.GroupName}");
            }

            if (!int.TryParse(Console.ReadLine(), out int groupId))
            {
                Console.WriteLine("Неверный номер группы.");
                return;
            }

            var group = groups.FirstOrDefault(g => g.GroupId == groupId);
            if (group == null)
            {
                Console.WriteLine("Группа не найдена.");
                return;
            }

            Console.WriteLine("Выберите номер студента:");
            var students = GetStudentsByGroup(groupId);
            foreach (var _student in students)
            {
                Console.WriteLine($"{_student.StudentId} - {_student.Name}");
            }

            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Неверный номер студента.");
                return;
            }

            var student = students.FirstOrDefault(s => s.StudentId == studentId);
            if (student == null)
            {
                Console.WriteLine("Студент не найден.");
                return;
            }

            _storage.RemoveStudent(student);

            Console.WriteLine("Студент успешно удален.");
        }

        public void PrintStudentsByGroups()
        {
            Console.WriteLine("Списки студентов по группам:");
            var groups = _storage.GetAllGroups();
            foreach (var group in groups)
            {
                Console.WriteLine($"{group.GroupName} ({group.Students.Count})");
                List<Student> students = GetStudentsByGroup(group.GroupId);
                foreach (var student in students.OrderBy(s => s.Name))
                {
                    Console.WriteLine(student.Name);
                }
            }
        }



    }

}
