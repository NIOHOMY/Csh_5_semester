using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.DAL;
using ConsoleApp1.Models;

namespace ConsoleApp1
{
    public class StudentMenuController
    {
        private readonly StudentDBStorage _storage;

        public StudentMenuController(StudentDBStorage storage)
        {
            _storage = storage;
        }

        

        public void AddStudent()
        {
            try
            {
                int? groupId = ChooseGroup();
                if (groupId != null)
                {
                    Console.WriteLine("Введите ФИО студента:");
                    string? name = Console.ReadLine();
                    if (name != null && name != "")
                    {
                        string[] nameParts = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (nameParts.Length == 3)
                        {
                            string? _Firstname = nameParts[1];
                            string? _Lastname = nameParts[0];
                            string? _Surname = string.Join(" ", nameParts.Skip(2));
                            if (_Firstname != null && _Firstname != "" &&
                                _Lastname != null && _Lastname != "" &&
                                _Surname != null && _Surname != ""
                                )
                            {
                                var student = new Student
                                {
                                    Firstname = _Firstname,
                                    Lastname = _Lastname,
                                    Surname = _Surname,
                                    GroupId = groupId.Value
                                };
                                _storage.AddStudent(student);

                                Console.WriteLine(" * Студент успешно добавлен.");
                            }
                        }
                        else
                        {
                            Console.WriteLine(" ! Имя фамилия и отчество студента не могут быть пустым.");
                        }
                    }
                    else
                    {
                        Console.WriteLine(" ! ФИО студента не может быть пустым.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" ! Error: {ex.Message}");
                Debug.WriteLine($"Error: {ex.Message}");
            }
            
        }

        public void RemoveStudent()
        {
            try
            {
                int? groupId = ChooseGroup(true);
                if (groupId != null)
                {
                    Console.WriteLine("Выберите номер студента:");
                    var students = _storage.GetStudentsByGroup(groupId.Value);
                    if (students.Count == 0)
                    {
                        Console.WriteLine(" ! Список студентов пуст.");
                        return;
                    }
                    foreach (var _student in students)
                    {
                        Console.WriteLine($" {_student.StudentId} - {_student.Lastname} {_student.Firstname} {_student.Surname}");
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
            catch (Exception ex)
            {
                Console.WriteLine($" ! Error: {ex.Message}");
                Debug.WriteLine($"Error: {ex.Message}");
            }
            
        }

        public void PrintStudentsByGroups()
        {
            try
            {
                Console.WriteLine("\n - Списки студентов по группам -");
                var groups = _storage.GetAllGroups();
                foreach (var group in groups)
                {
                    Console.WriteLine($" {group.GroupName} ({group.Students.Count})");
                    List<Student> students = _storage.GetStudentsByGroup(group.GroupId);
                    foreach (var student in students.OrderBy(s => s.Lastname))
                    {
                        Console.WriteLine($" {student.Lastname} {student.Firstname} {student.Surname}");
                    }
                }
                Console.WriteLine(' ');
            }
            catch (Exception ex)
            {
                Console.WriteLine($" ! Error: {ex.Message}");
                Debug.WriteLine($"Error: {ex.Message}");
            }
            
        }

        private int? ChooseGroup(bool notEmpty = false)
        {
            try
            {
                var groups = _storage.GetAllGroups();
                bool checkNotAllGroupsEmpty = notEmpty;
                if (notEmpty == true)
                {
                    foreach (var _group in groups)
                    {
                        if (checkNotAllGroupsEmpty == true && _group.Students.Count != 0)
                        {
                            checkNotAllGroupsEmpty = false;
                        }
                    }
                }
                if (checkNotAllGroupsEmpty == false)
                {
                    Console.WriteLine("Выберете номер группы:");
                    foreach (var _group in groups)
                    {
                        if ((notEmpty == true && _group.Students.Count != 0) || (notEmpty == false))
                        {
                            Console.WriteLine($"{_group.GroupId} - {_group.GroupName}");
                        }
                    }
                    int? groupId = ConsoleGetNumberOf("группы");
                    if (groupId == null)
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
                else
                {
                    Console.WriteLine(" ! Все группы пусты.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" ! Error: {ex.Message}");
                Debug.WriteLine($"Error: {ex.Message}");
                return null;
            }
            
        }
        private int? ConsoleGetNumberOf(string obj)
        {
            try
            {
                if (!int.TryParse(Console.ReadLine(), out int groupId))
                {
                    Console.WriteLine($" ! Неверный номер {obj}.");
                    Console.WriteLine($" ! Номер {obj} - целое положительное число из списка выше.");
                    return null;
                }
                return groupId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" ! Error: {ex.Message}");
                Debug.WriteLine($"Error: {ex.Message}");
                return null;
            }
            
        }
    }

}
