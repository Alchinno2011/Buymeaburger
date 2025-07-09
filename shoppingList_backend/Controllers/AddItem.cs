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
    [Route("api/newitem")]
    [ApiController]
    public class NewOrderController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Login()
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
                CreateOrder(data);
            }

            AddToItems(data.Name, data.color);

            return Ok();

        }

    private void AddToItems(string name, string color)
    {
        using DBContext context = new DBContext();

        if (!context.Items.Any(g => g.Name == name))
        {
            Console.WriteLine(!context.Items.Any(g => g.Name == name));

            Items newItem = new Items()
            {
                Name = name,
                color = color
            };

            context.Items.Add(newItem);
            context.SaveChanges();
        }
        else
        {
            var existingItem = context.Items.FirstOrDefault(g => g.Name == name);
            if (existingItem != null)
            {
                existingItem.color = color;
                context.SaveChanges();
            }
        }
    }



        private void CreateOrder(NewItemRequestData data)
        {
            using DBContext context = new DBContext();

            GroceryList newGrocery = new GroceryList()
            {
                UserId = data.UserId,
                Id = data.Id,
                Name = data.Name,
                Quantity = data.Quantity,
                IsBought = data.IsBought,
                CreatedAt = data.CreatedAt,
                color = data.color
            };

            context.GroceryList.Add(newGrocery);

            context.SaveChanges();
        }


        private class NewItemRequestData
        {
            public int UserId { get; set; }
            public int Id { get; set; }
            public string Name { get; set; }
            public string Quantity { get; set; }
            public bool IsBought { get; set; }
            public DateTime CreatedAt { get; set; }

            public string color { get; set; }
        }
    }

}