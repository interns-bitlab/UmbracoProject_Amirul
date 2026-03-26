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
   
    public void studentCreation(StudentDTO studentCreation)
    {
        var parentId = Guid.Parse("dec5212e-a2c8-4dfd-8425-e393d8faaeb1");
        var student = contentServices.Create(studentCreation.Name, parentId, "student");
        var eduPickerValue = AllResults(studentCreation.EduPicker);


        student.SetValue("age", studentCreation.Age);
        student.SetValue("email", studentCreation.Email);
        if (studentCreation.EduPicker != null)
        {
            student.SetValue("eduPicker", eduPickerValue);
        }

        contentServices.Save(student);
        contentServices.Publish(student, ["*"], -1);
 

    }

    public void studentUpdate(Guid Id, StudentDTO studentUpdate)
    {
        var student = contentServices.GetById(Id);
        var eduPickerValue = AllResults(studentUpdate.EduPicker);

        if(student == null)
        {
            throw new ArgumentException("Student Missing");
        }

        student.Name = studentUpdate.Name;
        student.SetValue("age", studentUpdate.Age);
        if(studentUpdate != null)
        {
            student.SetValue("eduPicker", eduPickerValue);
        }

        contentServices.Save(student);

        
        contentServices.Publish(student, new[] { "*" } );
    }


    public void studentPatch(Guid Id, StudentPatchDTO studentPatch)
    {
        var student = contentServices.GetById(Id);
        var eduPickerValue = AllResults(studentPatch.EduPicker);

        if(student == null)
        {
            throw new ArgumentException("student is missing");
        }

        if (!string.IsNullOrEmpty(studentPatch.Name))
        {
            student.Name = studentPatch.Name;
        }
        if (studentPatch.Age.HasValue)
        {
            student.SetValue("age", studentPatch.Age.Value);
        }
        if (studentPatch.EduPicker != null)
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