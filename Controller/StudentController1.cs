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

        var NewStudentNodes = rootnodes.SelectMany(x => x.DescendantsOrSelf()).Where(x => x.ContentType.Alias == "student");


        var studentDetail = NewStudentNodes.Select(studentNode =>
        {
            var NewCourseNodes = studentNode.Value<IEnumerable<IPublishedContent>>("eduPicker");
            //var courseNode = courseNodes?.FirstOrDefault();

            return new Student
            {
                Id = studentNode.Key,
                Name = studentNode.Name ?? "",
                Age = studentNode.Value<int>("age"),
                Email = studentNode.Value<string>("email") ?? "",

                // FIX: Check if courseNode is NOT null first
                //course = courseNode != null ? new Courses
                EduPicker = NewCourseNodes?.Select(courseNode => new Courses
                {
                    Id = courseNode.Key,
                    Name = courseNode.Name ?? "",
                    creditHour = courseNode.Value<int>("creditHours"),
                    courseName = courseNode.Value<string>("courseName") ?? "",
                    dateTaken = courseNode.Value<DateOnly?>("dateTaken")
                }).ToList() ?? []// Return null if no course was picked in the eduPicker
            };
        }).ToList();


        return Ok(studentDetail);
    }


    [HttpGet]
    [Route("studentlist/{id}")]
    public IActionResult GetStudentID(Guid Id)
    {
        var studentNode = umbracoHelper.Content(Id);

        if (studentNode == null)
        {
            return NotFound("student missing");
        }

        

        var NewCourseNodes = studentNode.Value<IEnumerable<IPublishedContent>>("eduPicker");

        //  var courseNode = courseNodes?.FirstOrDefault();

        var studentDetail = new Student
        {
            Id = studentNode.Key,
            Name = studentNode.Name ?? "",
            Age = studentNode.Value<int>("age"),
            Email = studentNode.Value<string>("email") ?? "",

            // course = courseNode != null ? new Courses
            EduPicker = NewCourseNodes?.Select(courseNode => new Courses
            {
                Id = courseNode.Key,
                Name = courseNode.Name ?? "",
                creditHour = courseNode.Value<int>("creditHours"),
                courseName = courseNode.Value<string>("courseName") ?? "",


                dateTaken = courseNode.Value<DateOnly?>("dateTaken")
            }).ToList() ?? []

        };
        

        return Ok(studentDetail);
    }

    [Route("studentview")]

    public IActionResult GetStudentView()
    {
        IEnumerable<IPublishedContent> rootnodes = umbracoHelper.ContentAtRoot();
        var NewStudentNodes = rootnodes.SelectMany(x => x.DescendantsOrSelf()).Where(x => x.ContentType.Alias == "student");



        var studentDetail = NewStudentNodes.Select(studentNode =>
        {

            var NewCourseNode = studentNode.Value<IEnumerable<IPublishedContent>>("eduPicker");

            return new Student
            {
                Id = studentNode.Key,
                Name = studentNode.Name ?? "",
                Age = studentNode.Value<int>("age"),
                Email = studentNode.Value<string>("email") ?? "",

                EduPicker = NewCourseNode?.Select(courseNode => new Courses
                {
                    Id = courseNode.Key,
                    Name = courseNode.Name ?? "",
                    creditHour = courseNode.Value<int>("creditHours"),
                    courseName = courseNode.Value<string>("courseName") ?? "",
                    dateTaken = courseNode.Value<DateOnly?>("dateTaken")

                }).ToList() ?? []
            };
            
        }).ToList();
        return View("studentview", studentDetail);

    }



        [HttpPost("create")]
    public IActionResult createPostStudent([FromBody] StudentDTO studentCreation)
    {
        studentServices.studentCreation(studentCreation);
        return Ok(studentCreation);

    }

    [HttpPut("update/{id}")]
    public IActionResult updatePostStudent(Guid Id, [FromBody] StudentDTO studentUpdate)
    {
        studentServices.studentUpdate(Id, studentUpdate);
        return Ok(studentUpdate);
    }

    [HttpPatch("patch/{id}")]
    public IActionResult patchPostStudent(Guid Id, [FromBody] StudentPatchDTO studentPatch)
    {
        studentServices.studentPatch(Id, studentPatch);
        return Ok("patch success");
    }

    [HttpDelete("delete/{id}")]
    public IActionResult deletePostStudent(Guid Id)
    {
        studentServices.studentDelete(Id);
        return Ok("delete success");
    }


}