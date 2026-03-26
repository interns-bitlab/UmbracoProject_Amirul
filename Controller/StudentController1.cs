using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Core.Models.PublishedContent;
using MyCustomUmbracoProject.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using MyCustomUmbracoProject.Models.Student;
using MyCustomUmbracoProject.Models.Course;

namespace MyCustomUmbracoProject.Controllers;


[Route("student")]
public class StudentController1(UmbracoHelper umbracoHelper, StudentServices studentServices) : Controller
{
    [HttpGet]
    [Route("studentlist")]
    public IActionResult GetStudent()
    {
        IEnumerable<IPublishedContent> rootnodes = umbracoHelper.ContentAtRoot();

        var studentnodes = rootnodes.SelectMany(x => x.DescendantsOrSelf()).Where(x => x.ContentType.Alias == "student");


        var student = studentnodes.Select(node =>
        {
            var courseNodes = node.Value<IEnumerable<IPublishedContent>>("eduPicker");
            //var courseNode = courseNodes?.FirstOrDefault();

            return new Student
            {
                Id = node.Key,
                Name = node.Name ?? "",
                Age = node.Value<int>("age"),
                Email = node.Value<string>("email") ?? "",

                // FIX: Check if courseNode is NOT null first
                //course = courseNode != null ? new Courses
                course = courseNodes?.Select(courseNode => new Courses
                {
                    Id = courseNode.Key,
                    Name = courseNode.Name ?? "",
                    creditHour = courseNode.Value<int>("creditHours"),
                    courseName = courseNode.Value<string>("courseName") ?? "",
                    dateTaken = courseNode.Value<DateOnly?>("dateTaken")
                }).ToList() ?? []// Return null if no course was picked in the eduPicker
            };
        }).ToList();


        return Ok(student);
    }


    [HttpGet]
    [Route("studentlist/{id}")]
    public IActionResult GetStudentID(Guid Id)
    {
        var studentnodes = umbracoHelper.Content(Id);

        if (studentnodes == null)
        {
            return NotFound("student missing");
        }

        

        var courseNodes = studentnodes.Value<IEnumerable<IPublishedContent>>("eduPicker");

        //  var courseNode = courseNodes?.FirstOrDefault();

        var student = new Student
        {
            Id = studentnodes.Key,
            Name = studentnodes.Name ?? "",
            Age = studentnodes.Value<int>("age"),
            Email = studentnodes.Value<string>("email") ?? "",

            // course = courseNode != null ? new Courses
            course = courseNodes?.Select(courseNode => new Courses
            {
                Id = courseNode.Key,
                Name = courseNode.Name ?? "",
                creditHour = courseNode.Value<int>("creditHours"),
                courseName = courseNode.Value<string>("courseName") ?? "",


                dateTaken = courseNode.Value<DateOnly?>("dateTaken")
            }).ToList() ?? []

        };
        

        return Ok(student);
    }

    [Route("studentview")]

    public IActionResult GetStudentView()
    {
        IEnumerable<IPublishedContent> rootnodes = umbracoHelper.ContentAtRoot();
        var studentnodes = rootnodes.SelectMany(x => x.DescendantsOrSelf()).Where(x => x.ContentType.Alias == "student");



        var student = studentnodes.Select(node =>
        {

            var courseNodes = node.Value<IEnumerable<IPublishedContent>>("eduPicker");

            return new Student
            {
                Id = node.Key,
                Name = node.Name ?? "",
                Age = node.Value<int>("age"),
                Email = node.Value<string>("email") ?? "",

                course = courseNodes?.Select(courseNode => new Courses
                {
                    Id = courseNode.Key,
                    Name = courseNode.Name ?? "",
                    creditHour = courseNode.Value<int>("creditHours"),
                    courseName = courseNode.Value<string>("courseName") ?? "",
                    dateTaken = courseNode.Value<DateOnly?>("dateTaken")

                }).ToList() ?? []
            };
            
        }).ToList();
        return View("studentview", student);

    }



        [HttpPost("create")]
    public IActionResult createPostStudent([FromBody] StudentDTO students)
    {
        studentServices.studentCreation(students);
        return Ok(students);

    }

    [HttpPut("update/{id}")]
    public IActionResult updatePostStudent(Guid Id, [FromBody] StudentDTO students)
    {
        studentServices.studentUpdate(Id, students);
        return Ok(students);
    }

    [HttpPatch("patch/{id}")]
    public IActionResult patchPostStudent(Guid Id, [FromBody] StudentPatchDTO students)
    {
        studentServices.studentPatch(Id, students);
        return Ok("patch success");
    }

    [HttpDelete("delete/{id}")]
    public IActionResult deletePostStudent(Guid Id)
    {
        studentServices.studentDelete(Id);
        return Ok("delete success");
    }


}