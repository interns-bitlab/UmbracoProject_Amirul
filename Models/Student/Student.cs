using MyCustomUmbracoProject.Models.Course;

namespace MyCustomUmbracoProject.Models.Student;

public class Student
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }

    public string Email { get; set; } = string.Empty;

    public List<Courses>? course { get; set; } = [];


}




