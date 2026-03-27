using Microsoft.AspNetCore.Mvc;
using MyCustomUmbracoProject.Services.Interfaces;
namespace MyCustomUmbracoProject.Controller;

public class emailController
{
    [HttpGet]
    public IActionResult SendEmail()
    {
        return View();
    }
}
