using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class StudentDbContext:DbContext
{
    
    
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;
    
    public StudentDbContext(DbContextOptions<StudentDbContext> options)
        : base(options)
    {
        //Database.EnsureCreated();
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>().HasData(
            new Group { GroupId = 1, NameGroup = "FIIT" },
            new Group { GroupId = 2, NameGroup = "PMM" },
            new Group { GroupId = 3, NameGroup = "MOAIS" }
        );
        
        modelBuilder.Entity<Student>().HasData(
            new Student { StudentId = 1, NameStudent = "Fill Jhons", GroupId = 2},
            new Student { StudentId = 2, NameStudent = "Fill Jhons", GroupId = 3}
        );
    }
    
}