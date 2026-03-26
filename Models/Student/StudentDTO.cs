using MyCustomUmbracoProject.Models.Course; 

namespace MyCustomUmbracoProject.Models.Student;

public class StudentDTO
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }

    public string Email { get; set; } = string.Empty;

    public List<Guid>? eduPicker { get; set; } = [];
}

