// sqlserver, netcore, tools
// add-migration InitMigration
// add-migration "AddCourseTeacherName"
// Remove-Migration
// update-database
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.DAL
{
    public class StudentDBStorage
    {
        private readonly StudentContext _context;

        public StudentDBStorage(StudentContext context)
        {
            _context = context;
        }

        public void AddStudent(Student student)
        {
            try
            {
                _context.Students.Add(student);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while adding a student: {ex.Message}");
                Console.WriteLine($"An error occurred while adding a student: {ex.Message}");
            }
        }

        public void RemoveStudent(Student student)
        {
            try
            {
                var studentToRemove = _context.Students.FirstOrDefault(s => s.StudentId == student.StudentId);
                if (studentToRemove != null)
                {
                    _context.Students.Remove(studentToRemove);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while removing a student: {ex.Message}");
                Console.WriteLine($"An error occurred while removing a student: {ex.Message}");
            }
        }

        public void EditStudent(Student student)
        {
            try
            {
                _context.Entry(student).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while editing a student: {ex.Message}");
                Console.WriteLine($"An error occurred while editing a student: {ex.Message}");
            }
        }

        public List<Student> GetAllStudents()
        {
            try
            {
                return _context.Students.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while getting all students: {ex.Message}");
                Console.WriteLine($"An error occurred while getting all students: {ex.Message}");
                return new List<Student>();
            }
        }

        public List<Group> GetAllGroups()
        {
            try
            {
                return _context.Groups.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while getting all groups: {ex.Message}");
                Console.WriteLine($"An error occurred while getting all groups: {ex.Message}");
                return new List<Group>();
            }
        }

        public List<Student> GetStudentsByGroup(int groupId)
        {
            try
            {
                return GetAllStudents().Where(s => s.GroupId == groupId).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while getting students by group: {ex.Message}");
                Console.WriteLine($"An error occurred while getting students by group: {ex.Message}");
                return new List<Student>();
            }
        }
    }

}
