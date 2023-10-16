// sqlserver, netcore, tools
// add-migration InitMigration
// add-migration "AddCourseTeacherName"
// Remove-Migration
// update-database
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
        
        public void addStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }
        public void removeStudent(string studentName)
        {
            _context.Students.Remove(_context.Students
                             .Where(p=>p.Name
                             .Equals(studentName))
                             .FirstOrDefault());// от null 
            _context.SaveChanges();

        }
        public void editStudent(Student student)
        {
            _context.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
        /*public void removeStudent(Student student)
        {
            _context.Students.Remove(student);
        }*/
        public /*IEnumerable*/List<Student> GetAllStudents()
        {
            return _context.Students.ToList<Student>();
        }

    }
}
