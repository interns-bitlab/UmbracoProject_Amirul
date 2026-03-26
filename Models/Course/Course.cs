    namespace MyCustomUmbracoProject.Models.Course;

    public class Courses
    {
        public Guid Id { get; set; }    
        public string Name { get; set; }
        public int creditHour { get; set; }
        public string courseName { get; set; } = string.Empty;

        public DateOnly? dateTaken { get; set; }
    }
