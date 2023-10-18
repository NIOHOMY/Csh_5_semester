// sqlserver, netcore, tools
// add-migration InitMigration
// add-migration "AddCourseTeacherName"
// Remove-Migration
// update-database
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
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
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public void RemoveStudent(Student student)
        {
            var studentToRemove = _context.Students.FirstOrDefault(s => s.StudentId == student.StudentId);
            if (studentToRemove != null)
            {
                _context.Students.Remove(studentToRemove);
                _context.SaveChanges();
            }
        }

        public void EditStudent(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public List<Student> GetAllStudents()
        {
            return _context.Students.ToList();
        }
        public List<Group> GetAllGroups()
        {
            return _context.Groups.ToList();
        }
        public List<Student> GetStudentsByGroup(int groupId)
        {
            return GetAllStudents().Where(s => s.GroupId == groupId).ToList();
        }
    }
}
