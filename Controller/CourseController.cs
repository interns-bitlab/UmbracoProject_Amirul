using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Core.Models.PublishedContent;
using MyCustomUmbracoProject.Models;
using MyCustomUmbracoProject.Models.Course;
using MyCustomUmbracoProject.Services;

namespace MyCustomUmbracoProject.Controllers;

[Route("course")]
public class CourseController(UmbracoHelper umbracoHelper, CourseServices courseServices) : Controller
{
    // GET all courses
    // URL: /course/courselist
    [HttpGet]
    [Route("courselist")]
    public IActionResult SubjectView()
    {
        IEnumerable<IPublishedContent> rootNodes = umbracoHelper.ContentAtRoot();
        var courseNodes = rootNodes
            .SelectMany(x => x.DescendantsOrSelf())
            .Where(x => x.ContentType.Alias == "subjectName");
        var courses = courseNodes.Select(node => new Courses
        {
            Id = node.Key,
            Name = node.Name ?? "",
            creditHour = node.Value<int>("creditHours"),
            courseName = node.Value<string>("courseName") ??"",
            dateTaken = node.Value<DateOnly?>("dateTaken")
        }).ToList();
        return Ok(courses);
    }
    [Route("courselist/{id}")]
    public IActionResult SubjectViewID(Guid Id)
    {
        IEnumerable<IPublishedContent> rootNodes = umbracoHelper.ContentAtRoot();
        var courseNode = rootNodes
            .SelectMany(x => x.DescendantsOrSelf())
            .FirstOrDefault(x => x.Key == Id && x.ContentType.Alias == "courseName");
        if (courseNode == null)
        {
            return NotFound();
        }
        var course = new Courses
        {
            Id = courseNode.Key,
            Name = courseNode.Name ?? "",
            creditHour = courseNode.Value<int>("creditHour"),
            courseName = courseNode.Value<string>("courseName") ?? "",
            dateTaken = courseNode.Value<DateOnly?>("dateTaken")
        };
        return Ok(course);
    }

    [HttpPost("create")]
    public IActionResult CreateSubject([FromBody] CourseDTO course)

    {

        Console.WriteLine($"Received Date: {course?.dateTaken}"); 

    if (course?.dateTaken == null) {
        // If it's null here, Postman sent the wrong format or the DTO can't read it.
    }
        courseServices.CreateCourse(course);
        return Ok();
    }

    [HttpPut("update/{id}")]
    public IActionResult UpdateSubject(Guid Id, [FromBody] CourseUpdateDTO course)
    {
        courseServices.UpdateCourse(Id, course);
        return Ok();
    }


    [HttpPatch("patch/{id}")]
    public IActionResult PatchSubject(Guid Id, [FromBody] CoursePatchDTO course)
    {
        courseServices.UpdatePatchCourse(Id, course);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    public IActionResult DeleteSubject(Guid Id)
    {
        courseServices.DeleteCourse(Id);
        return Ok();
    }



}