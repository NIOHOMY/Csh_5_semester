using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class StudentMenuController
    {
        private readonly StudentDBStorage _storage;

        public StudentMenuController(StudentDBStorage storage)
        {
            _storage = storage;
        }

        public List<Student> GetStudentsByGroup(int groupId)
        {
            return _storage.GetAllStudents().Where(s => s.GroupId == groupId).ToList();
        }

        public void AddStudent()
        {
            int? groupId = ChooseGroup();
            if (groupId != null)
            {
                Console.WriteLine("Введите ФИО студента:");
                string? name = Console.ReadLine();
                if (name != null && name != "")
                {
                    var student = new Student
                    {
                        Name = name,
                        GroupId = groupId.Value
                    };
                    _storage.AddStudent(student);

                    Console.WriteLine(" * Студент успешно добавлен.");
                }
                else
                {
                    Console.WriteLine(" ! Имя студента не может быть пустым.");
                }
            }
        }

        public void RemoveStudent()
        {
            int? groupId = ChooseGroup();
            if (groupId != null)
            {
                Console.WriteLine("Выберите номер студента:");
                var students = GetStudentsByGroup(groupId.Value);
                foreach (var _student in students)
                {
                    Console.WriteLine($" {_student.StudentId} - {_student.Name}");
                }

                int? studentId = ConsoleGetNumberOf("студента");
                if (studentId == null)
                {
                    return;
                }

                var student = students.FirstOrDefault(s => s.StudentId == studentId);
                if (student == null)
                {
                    Console.WriteLine(" ! Студент не найден.");
                    return;
                }

                _storage.RemoveStudent(student);

                Console.WriteLine(" * Студент успешно удален.");
            }
        }

        public void PrintStudentsByGroups()
        {
            Console.WriteLine("\n - Списки студентов по группам -");
            var groups = _storage.GetAllGroups();
            foreach (var group in groups)
            {
                Console.WriteLine($" {group.GroupName} ({group.Students.Count})");
                List<Student> students = GetStudentsByGroup(group.GroupId);
                foreach (var student in students.OrderBy(s => s.Name))
                {
                    Console.WriteLine(' '+student.Name);
                }
            }
            Console.WriteLine(' ');
        }

        private int? ChooseGroup()
        {
            Console.WriteLine("Выберете номер группы:");
            var groups = _storage.GetAllGroups();
            foreach (var _group in groups)
            {
                Console.WriteLine($"{_group.GroupId} - {_group.GroupName}");
            }
            int? groupId = ConsoleGetNumberOf("группы");
            if (groupId==null)
            {
                return null;
            }
            var group = groups.FirstOrDefault(g => g.GroupId == groupId);
            if (group == null)
            {
                Console.WriteLine(" ! Группа не найдена.");
                return null;
            }
            return groupId;
        }
        private int? ConsoleGetNumberOf(string obj)
        {
            if (!int.TryParse(Console.ReadLine(), out int groupId))
            {
                Console.WriteLine($" ! Неверный номер {obj}.");
                Console.WriteLine($" ! Номер {obj} - целое положительное число из списка выше.");
                return null;
            }
            return groupId;
        }
    }

}
