namespace WebApplication1.Models;

public class Student
{
    public int StudentId { set; get; }
    public string? NameStudent { set; get; }
    public Group? Group { set; get; }
    
    public int GroupId { set; get; }
}