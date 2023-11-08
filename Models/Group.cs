namespace WebApplication1.Models;

public class Group
{
    public int GroupId { set; get; }
    public string NameGroup { set; get; }
    public List<Student> Students { set; get; }

    public Group()
    {
        Students = new List<Student>();
    }
    

}