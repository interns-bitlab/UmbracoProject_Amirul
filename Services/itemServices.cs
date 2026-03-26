using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using MyCustomUmbracoProject.Models.Item;

namespace MyCustomUmbracoProject.Services;

public class itemServices(IContentService ContentServices)
{
    //CREATE 
    public void CreateItem(ItemDTO item)
    {
        var parentid = Guid.Parse("9f2854f2-6c1c-4a47-8d2b-f2d64d1b6530");
        var product = ContentServices.Create(item.Name, parentid, "itemName");

        product.SetValue("type", item.Type);
        product.SetValue("price",item.Price);


        ContentServices.Save(product);
       
        ContentServices.Publish(product, new[] { "*" });
    }

    //UPDATE
    public void UpdateItem(Guid Id, ItemUpdateDTO item)
    {
        var updateProduct = ContentServices.GetById(Id);

        if (updateProduct == null)
        {
            throw new ArgumentException("Content Not Found");
        }

        updateProduct.Name = item.Name;
        updateProduct.SetValue("type", item.Type);
        updateProduct.SetValue("price", item.Price);

        ContentServices.Save(updateProduct);
      
        ContentServices.Publish(updateProduct, new[] { "*" });

    }

    //PATCH
    public void UpdatePatchItem(Guid Id, ItemPatchDTO item)
    {
        var patchProduct = ContentServices.GetById(Id);

        if (patchProduct == null)
        {
            throw new ArgumentException("Content Not Found");
        }

        if (!string.IsNullOrEmpty(item.Name))
        {
            patchProduct.Name = item.Name;
        }
        if (!string.IsNullOrEmpty(item.Type))
        {
            patchProduct.SetValue("type", item.Type);
        }
        if (item.Price.HasValue)
        {
            patchProduct.SetValue("price", item.Price.Value);
        }

        
        ContentServices.Save(patchProduct);
       
        ContentServices.Publish(patchProduct, new[] { "*" });

    }
    //DELETE
    public void DeleteItem(Guid Id)
    {
        var deleteProduct = ContentServices.GetById(Id);
        if (deleteProduct == null)
        {
            throw new ArgumentException("Content Not Found");
        }

        ContentServices.Delete(deleteProduct);    
    }

}
