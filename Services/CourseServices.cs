using MyCustomUmbracoProject.Models;
using MyCustomUmbracoProject.Models.Course;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace MyCustomUmbracoProject.Services;


public class CourseServices(IContentService contentService, IJsonSerializer _jsonSerializer)
{
    public void CreateCourse(CourseDTO courseCreation)
    {
        var parentId = Guid.Parse("a9c2448a-a55f-4987-a588-e035b7a54be9");
        var newCourse = contentService.Create(courseCreation.Name, parentId, "subjectName");

        newCourse.SetValue("creditHours", courseCreation.creditHour);
        newCourse.SetValue("courseName", courseCreation.courseName);

        if (courseCreation.dateTaken.HasValue)
        {
            // 1. Convert your DTO date to DateTimeOffset at midnight
            // Note: If your DTO is already DateTime, use: DateOnly.FromDateTime(course.dateTaken.Value)

            DateTimeOffset dateTimeOffset = courseCreation.dateTaken.Value.ToDateTime(TimeOnly.MinValue);//dia tukarkan jadi midnight

            // 2. Wrap it in the special Umbraco JSON object
            var dateObject = new DateOnlyValue { Date = dateTimeOffset };//dia simpan dalam satu variable yang ada object model 

            // 3. Serialize to JSON string
            string jsonValue = _jsonSerializer.Serialize(dateObject);//dia tukarkan kepada format JSON string

            // 4. Save the JSON string to the property
            newCourse.SetValue("dateTaken", jsonValue);//dia update dalam umbraco node
        }
        // 1. Save to the database
        contentService.Save(newCourse);

        // 2. Publish the saved version to the website
        contentService.Publish(newCourse, new[] { "*" });
    }

    public void UpdateCourse(Guid Id, CourseUpdateDTO courseUpdate)
    {
        var existingCourse = contentService.GetById(Id);
        if (existingCourse == null)
        {
            throw new ArgumentException("Course Not Found");
        }
        existingCourse.Name = courseUpdate.Name;
        existingCourse.SetValue("creditHours", courseUpdate.creditHour);
        existingCourse.SetValue("courseName", courseUpdate.courseName);

        if (courseUpdate.dateTaken.HasValue)
        {
            DateTimeOffset dateTimeOffset = courseUpdate.dateTaken.Value.ToDateTime(TimeOnly.MinValue);

            var dateObject = new DateOnlyValue{Date = dateTimeOffset};

            string jsonValue = _jsonSerializer.Serialize(dateObject);

            existingCourse.SetValue("dateTaken", jsonValue);

        }
        else
        {
            existingCourse.SetValue("dateTaken", null);
        }
        
        contentService.Save(existingCourse);
        contentService.Publish(existingCourse, new[] { "*" });
    }

    public void UpdatePatchCourse(Guid Id, CoursePatchDTO coursePatch)
    {
        var existingCourse = contentService.GetById(Id);
        if (existingCourse == null)
        {
            throw new ArgumentException("Course Not Found");
        }
        
        if (!string.IsNullOrEmpty(coursePatch.Name))
        {
            existingCourse.Name = coursePatch.Name;
         
        }
        if (coursePatch.creditHour.HasValue)
        {
            existingCourse.SetValue("creditHours", coursePatch.creditHour.Value);
        }
        if (!string.IsNullOrEmpty(coursePatch.courseName))
        {
            existingCourse.SetValue("courseName", coursePatch.courseName);
        }

        // ✅ CORRECT - simple DateTime conversion
        if (coursePatch.dateTaken.HasValue)
        {
            DateTime dateTime = coursePatch.dateTaken.Value.ToDateTime(TimeOnly.MinValue);

            var dateObject = new DateOnlyValue { Date = dateTime };

            string jsonValue = _jsonSerializer.Serialize(dateObject);
            existingCourse.SetValue("dateTaken", jsonValue);
        }

        contentService.Save(existingCourse);
        contentService.Publish(existingCourse, new[] { "*" });
    }

    public void DeleteCourse(Guid Id)
    {
        var courseToDelete = contentService.GetById(Id);
        if (courseToDelete == null)
        {
            throw new ArgumentException("Course Not Found");
        }
        contentService.Delete(courseToDelete);
    }
}
