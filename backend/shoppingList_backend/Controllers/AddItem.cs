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
    [Route("newitem")]
    [ApiController]
    public class NewOrderController : ControllerBase
    {

        [HttpPost("GroceryListId")]
        public async Task<IActionResult> AddItem(int GroceryListId)
        {

            Console.WriteLine("POST");

            var request = HttpContext.Request;

            using var reader = new StreamReader(request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();

            var data = JsonConvert.DeserializeObject<NewItemRequestData>(body);

            if (data == null)
            {
                return BadRequest("Request body is empty");
            }

            else
            {
                CreateItem(data, GroceryListId);
            }

            AddToRecomendations(data.Name, data.color);

            return Ok();

        }

    private void AddToRecomendations(string name, string color)
    {
        using DBContext context = new DBContext();

        if (!context.Recomendations.Any(g => g.Name == name))
        {
            Console.WriteLine(!context.Recomendations.Any(g => g.Name == name));

            var newItem = new Recomendation
            {
                Name = name,
                color = color
            };

            context.Recomendations.Add(newItem);
            context.SaveChanges();

            }
            else
        {
            var existingItem = context.Recomendations.FirstOrDefault(g => g.Name == name);
            if (existingItem != null)
            {
                existingItem.color = color;
                context.SaveChanges();
            }
        }
    }



        private void CreateItem(NewItemRequestData data, int GroceryListId)
        {
            using DBContext context = new DBContext();

            var newGrocery = new GroceryItem()
            {
                ListId = GroceryListId,
                Name = data.Name,
                Quantity = data.Quantity,
                IsBought = data.IsBought,
                CreatedAt = data.CreatedAt,
                color = data.color
            };

            context.GroceryItem.Add(newGrocery);

            context.SaveChanges();
        }


        private class NewItemRequestData {
            public required string Name { get; set; }
            public string? Quantity { get; set; }
            public bool IsBought { get; set; }
            public DateTime CreatedAt { get; set; }
            public required string color { get; set; }
        }
    }

}