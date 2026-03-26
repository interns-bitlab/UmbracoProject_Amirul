using Microsoft.AspNetCore.Http.HttpResults;
using MyCustomUmbracoProject.Models.Course;
using MyCustomUmbracoProject.Models.Student;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;


namespace MyCustomUmbracoProject.Services;


public class StudentServices(IContentService contentServices)
{
    private static string? AllResults(List<Guid>? eduPicker)
    {
        if (eduPicker == null)
        {
            return null;
        }
        var pageUdi = eduPicker.Select(id => Udi.Create(Constants.UdiEntityType.Document, id).ToString());

        return string.Join(",", pageUdi);
    }
   
    public void studentCreation(StudentDTO students)
    {
        var parentId = Guid.Parse("dec5212e-a2c8-4dfd-8425-e393d8faaeb1");
        var student = contentServices.Create(students.Name, parentId, "student");
        var eduPickerValue = AllResults(students.eduPicker);


        student.SetValue("age", students.Age);
        student.SetValue("email", students.Email);
        if (students.eduPicker != null)
        {
            student.SetValue("eduPicker", eduPickerValue);
        }

        contentServices.Save(student);
        contentServices.Publish(student, ["*"], -1);
 

    }

    public void studentUpdate(Guid Id, StudentDTO students)
    {
        var student = contentServices.GetById(Id);
        var eduPickerValue = AllResults(students.eduPicker);

        if(student == null)
        {
            throw new ArgumentException("Student Missing");
        }

        student.Name = students.Name;
        student.SetValue("age", students.Age);
        if(students != null)
        {
            student.SetValue("eduPicker", eduPickerValue);
        }

        contentServices.Save(student);

        
        contentServices.Publish(student, new[] { "*" } );
    }


    public void studentPatch(Guid Id, StudentPatchDTO students)
    {
        var student = contentServices.GetById(Id);
        var eduPickerValue = AllResults(students.eduPicker);

        if(student == null)
        {
            throw new ArgumentException("student is missing");
        }

        if (!string.IsNullOrEmpty(students.Name))
        {
            student.Name = students.Name;
        }
        if (students.Age.HasValue)
        {
            student.SetValue("age", students.Age.Value);
        }
        if (students.eduPicker != null)
        {
            student.SetValue("eduPicker", eduPickerValue);
        }

        contentServices.Save(student);
        

        contentServices.Publish(student, new[] { "*" });

    }

    public void studentDelete(Guid Id)
    {
        var student = contentServices.GetById(Id);
        if (student == null)
        {
            throw new ArgumentException("Student is missing");
        }

        contentServices.Delete(student);

        contentServices.Save(student);


        contentServices.Publish(student, new[] { "*" });
    }
}