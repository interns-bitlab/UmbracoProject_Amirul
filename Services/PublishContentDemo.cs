using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using MyCustomUmbracoProject.Models.Item;

namespace MyCustomUmbracoProject.Services;

public class PublishContentDemo
{
    //1.declate the private local variable to store the IContentService package   
    private readonly IContentService _contentService;

    //2.package from IContentService stores in the new variable inside the parameter
    public PublishContentDemo(IContentService contentService) =>_contentService = contentService; 

    //3.make a function create with parameter class DTO 
    public void CreateDTO(ItemDTO item)
    {
        //4. Create a variable for the GUID of the parent page - the folder of variable.
        var parentId = Guid.Parse("9f2854f2-6c1c-4a47-8d2b-f2d64d1b6530");

        //5.Create a variable content item with name,parentId and content type alias.
        var demoProduct = _contentService.Create(item.Name, parentId, "itemName");
        //6.Set the Value of the content 
        demoProduct.SetValue("type", item.Type);
        demoProduct.SetValue("price", item.Price);

        // 7.Save content first
        _contentService.Save(demoProduct);

        // 8.Publish content
        var userId = -1; // 0 = system user
        _contentService.Publish(demoProduct, new[] { "*" }, userId); // use "*" for invariant content
    }

    public void Create()
    {
        var parentId = Guid.Parse("9f2854f2-6c1c-4a47-8d2b-f2d64d1b6530");

        // ✅ Use _contentService (your private field) not ContentService (the class)
        var demoProduct = _contentService.Create("Microphone", parentId, "itemName");

        demoProduct.SetValue("type", "audio");
        demoProduct.SetValue("price", "1500");

        _contentService.Save(demoProduct);

        var userId = -1;
        _contentService.Publish(demoProduct, new[] { "*" }, userId);


    }
}