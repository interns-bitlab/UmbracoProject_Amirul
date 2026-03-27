using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Core.Models.PublishedContent;
using MyCustomUmbracoProject.Services;
using MyCustomUmbracoProject.Models.Item;

namespace MyCustomUmbracoProject.Controllers;

[ApiController]
[Route("item")]
public class ItemController(UmbracoHelper umbracoHelper, PublishContentDemo publishContentDemo, itemServices ItemServices) : Controller
{
   

    // GET all items
    // URL: /item/itemlist
    [Route("itemlist")]
        public IActionResult GetItem()
        {
            IEnumerable<IPublishedContent> rootNodes = umbracoHelper.ContentAtRoot();

            var itemNodes = rootNodes
                .SelectMany(x => x.DescendantsOrSelf())
                .Where(x => x.ContentType.Alias == "itemName");

            var items = itemNodes.Select(node => new Item
            {
                Id = node.Key,
                Name = node.Name ?? "",
                Type = node.Value<string>("type") ?? "",
                Price = node.Value<int>("price")
            }).ToList();

            return Ok(items);
        }

    //GET ALL ITEMS WITH VIEW

    [Route("itemlistview")]
        public IActionResult GetItemView()  // ✅ different name
        {
            IEnumerable<IPublishedContent> rootNodes = umbracoHelper.ContentAtRoot();

            var itemNodes = rootNodes
                .SelectMany(x => x.DescendantsOrSelf())
                .Where(x => x.ContentType.Alias == "itemName");

            var items = itemNodes.Select(node => new Item
            {
                Id = node.Key,
                Name = node.Name ?? "",
                Type = node.Value<string>("type") ?? "",
                Price = node.Value<int>("price")
            }).ToList();

            return View("itemlistview", items);  // ✅ returns View
        }



    // GET single item by GUID
    // URL: /item/itemlist/b6fbbb31-a77f-4f9c-85f7-2dc4835c7f31
    [Route("itemlist/{id}")]
        public IActionResult GetAction(Guid id)
        {
            var content = umbracoHelper.Content(id);

            var item = new Item
            {
                Id = content.Key,
                Name = content.Name ?? "",
                Type = content.Value<string>("type") ?? "",
                Price = content.Value<int>("price")
            };

            return Ok(item);
        }
    // GET SINGLE ID WITH VIEW
    [Route("itemlistview/{id}")]
        public IActionResult GetActionView(Guid id)
        {
            var content = umbracoHelper.Content(id);

            var item = new Item
            {
                Id = content.Key,
                Name = content.Name ?? "",
                Type = content.Value<string>("type") ?? "",
                Price = content.Value<int>("price")
            };

            return View("itemlistviewid", item);  // ✅ returns View
        }

    //CREATE HARD CODED
    [HttpPost("create")]
        public IActionResult getCreate()  
        {

            publishContentDemo.Create();
            return Ok("item created");
        }

    //FUNCTION CREATE

    [HttpPost("createDTO")]
        public IActionResult getCreateDTO([FromBody] ItemDTO item)  // ✅ added [FromBody] + closing )
        {
            // ✅ use item.Name not name
            if (string.IsNullOrEmpty(item.Name))
                return BadRequest(new { Message = "Name cannot be empty!" });

            // ✅ just pass 'item' no type keyword
            publishContentDemo.CreateDTO(item);
            return Ok("item created");
        }

    
    //UPDATE HARD CODED
    [HttpPost("updatecoded")]
        public IActionResult getUpdateCoded([FromBody] ItemDTO item)
        {
            ItemServices.CreateItem(item);

            return Ok("item created");
        }

    //UPDATE
    [HttpPut("update/{id}")]
        public IActionResult getUpdate(Guid Id, [FromBody] ItemUpdateDTO item) {

            ItemServices.UpdateItem(Id, item);
            return Ok("Item Updated");
    
        }

    //PATCH
    [HttpPatch("patch/{id}")]
        public IActionResult getPatch(Guid Id, [FromBody] ItemPatchDTO item)
        {

            ItemServices.UpdatePatchItem(Id, item);
            return Ok("Item Patched Successfully");

        }

    //DELETE 
    [HttpDelete("delete/{id}")]
        public IActionResult getDelete(Guid Id)
        {
            ItemServices.DeleteItem(Id);
            return Ok("Item Deleted Successfully");
        }
}