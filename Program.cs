using ConsoleApp1;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


StudentDBStorage studentDBStorage = new StudentDBStorage(new StudentContext());
Direction d1 = new Direction { DirectionName = "FIIT" };
Group g1 = new Group { GroupName = "61", Direction = d1 };
Student s1 = new Student
{
    Name = "Tom",
    Age = "10",
    Group = g1,

};
Student s2 = new Student
{
    Name = "Tom2",
    Age = "11",
    Group = g1,
};
studentDBStorage.addStudent(s1);
studentDBStorage.addStudent(s2);

List<Student> studentList = studentDBStorage.GetAllStudents();
foreach (Student student in studentList)
{
    Console.WriteLine(student.Name);
}

studentDBStorage.removeStudent("Tom2");
studentList = studentDBStorage.GetAllStudents();
foreach (Student student in studentList)
{
    Console.WriteLine(student.Name);
}
studentList = studentDBStorage.GetAllStudents();
foreach (Student student in studentList)
{
    Console.WriteLine(student.Name);
    student.Name = "Bob";
    studentDBStorage.editStudent(student);
}

/*using (StudentContext db = new StudentContext())
{
    Direction d1 = new Direction { DirectionName = "FIIT"};
    Direction d2 = new Direction { DirectionName = "PMI"};

    Group g1 = new Group { GroupName = "61", Direction = d1};
    Group g2 = new Group { GroupName = "62", Direction = d2};

    Course c1 = new Course { CourseName = "мат анализ" };
    Course c2 = new Course { CourseName = "физика" };

    Student s1 = new Student { Name = "Tom", Age = "10", Group = g1, Courses = new List<Course> { c1,c2} };
    Student s2 = new Student { Name = "Alice", Age = "100", Group = g2, Courses = new List<Course> { c1 } };


    db.Students.AddRange(s1, s2);
    db.Groups.AddRange(g1, g2);
    db.Directions.AddRange(d1, d2);
    db.Courses.AddRange(c1, c2);
    db.SaveChanges();


}
*/
/*namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            
        }
    }
}*/
