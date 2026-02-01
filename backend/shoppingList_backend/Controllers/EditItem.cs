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
    [Route("edititem")]
    [ApiController]
    public class EditItem : ControllerBase
    {

        [HttpPost("{GroceryListId}/{TaskId}")]
        public async Task<IActionResult> AddItem(int GroceryListId, int TaskId)
        {

            Console.WriteLine("POST");

            var request = HttpContext.Request;

            using var reader = new StreamReader(request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();

            var data = JsonConvert.DeserializeObject<EditItemRequestData>(body);

            if (data == null)
            {
                return BadRequest("Request body is empty");
            }

            else
            {
                Ëdit_Item(data, GroceryListId, TaskId);
                ChangeRecomendations(data, data.Name, data.color, GroceryListId);
            }



            return Ok();

        }

        private void ChangeRecomendations(EditItemRequestData data, string name, string color, int GroceryListId)
        {
            using DBContext context = new DBContext();

            if (!context.Recomendations.Any(g => g.Name == name && g.ListId == GroceryListId))
            {
                var existingItem = context.Recomendations.FirstOrDefault(g => g.Name == name);
                if (existingItem != null)
                {
                    existingItem.color = color;

                    context.SaveChanges();
                }
            }
        }



        private void Ëdit_Item(EditItemRequestData data, int GroceryListId, int TaskId)
        {
            using DBContext context = new DBContext();

            var item = context.GroceryItem.FirstOrDefault(o => o.Id == TaskId && o.ListId == GroceryListId);
            if (item != null)
            {
                item.Name = data.Name;
                item.Quantity = data.Quantity;
                item.color = data.color;
            }

            context.SaveChanges();
        }


        private class EditItemRequestData
        {
            public required string Name { get; set; }
            public string? Quantity { get; set; }
            public required string color { get; set; }
        }
    }

}