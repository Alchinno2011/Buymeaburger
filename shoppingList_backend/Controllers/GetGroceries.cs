using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Azure.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using shoppingList_backend.Database;
using shoppingList_backend.Database.DTOs;

namespace shoppingList_backend.Controllers
{

    [Route("api/getGroceries")]
    [ApiController]
    public class GetGroceries : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using DBContext context = new DBContext();

            var GroceryList = (from Grocery in context.GroceryList
                select new GroceryListDTO
                {
                    UserId = Grocery.UserId,
                    Id = Grocery.Id,
                    Name = Grocery.Name,
                    Quantity = Grocery.Quantity,
                    IsBought = Grocery.IsBought,
                    CreatedAt = Grocery.CreatedAt,
                    color = Grocery.color

                })
                .OrderByDescending(o => o.color);

            Console.WriteLine(JsonConvert.SerializeObject(GroceryList.ToArray()));

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "dd/MM/yyyy hh:mm:ss";


            return Content(JsonConvert.SerializeObject(GroceryList.ToArray(), jsonSettings), "application/json");
        }
    }
}