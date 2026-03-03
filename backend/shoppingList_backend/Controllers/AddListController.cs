using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Cors;
using shoppingList_backend.Database;
using shoppingList_backend.Database.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using shoppingList_backend.Database.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace shoppingList_backend.Controllers
{
    [Route("newlist")]
    [ApiController]
    public class AddListController : Controller
    {

        public async Task<IActionResult> AddItem()
        {

            var request = HttpContext.Request;

            using var reader = new StreamReader(request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();

            var data = JsonConvert.DeserializeObject<NewListRequestData>(body);

            if (data == null)
            {
                return BadRequest("Request body is empty");
            }
            else
            {
                int GroceryListId = CreateItem(data);
                return Ok(GroceryListId);
            }

        }



        private int CreateItem(NewListRequestData data)
        {
            using DBContext context = new DBContext();

            var newList = new GroceryList()
            {
                Name = data.Name,
                OwnerId = data.OwnerId
            };

            context.GroceryList.Add(newList);

            context.SaveChanges();

            var GroceryListId = context.GroceryList
                .Where(g => g.Name == data.Name && g.OwnerId == data.OwnerId)
                .Select(g => g.Id)
                .FirstOrDefault();

            return GroceryListId;
        }


        private class NewListRequestData
        {
            public required string Name { get; set; }
            public required int OwnerId { get; set; }
        }
    }

}