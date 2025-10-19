using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using shoppingList_backend.Database;
using shoppingList_backend.Database.DTOs;
using shoppingList_backend.Database.Models;

namespace shoppingList_backend.Controllers
{

    [Route("getGroceries")]
    [ApiController]
    public class GetGroceries : Controller
    {
        [HttpGet("{userId}/{GroceryListId}")]
        public IActionResult getGroceries(int GroceryListId, int userId)
        {
            using DBContext context = new DBContext();

            var GroceryItem = context.GroceryItem
                .Where(g => g.ListId == GroceryListId)

                //.ToList()
                .OrderByDescending(o => o.IsBought)
                .ThenByDescending(o => o.IsBought ? o.CreatedAt.ToString() : o.color)
                .Select(g => new GroceryItemDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    Quantity = g.Quantity,
                    IsBought = g.IsBought,
                    CreatedAt = g.CreatedAt,
                    color = g.color
                })

                .ToList();

            Console.WriteLine(JsonConvert.SerializeObject(GroceryItem.ToArray()));

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "dd/MM/yyyy hh:mm:ss";


            return Content(JsonConvert.SerializeObject(GroceryItem.ToArray(), jsonSettings), "application/json");
        }
    }
}