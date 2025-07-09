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

    [Route("api/GetRecomendations")]
    [ApiController]
    public class GetRecomendations : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using DBContext context = new DBContext();

            var Items = (from Item in context.Items
                         select new ItemsDTO
                               {
                                   Id = Item.Id,
                                   Name = Item.Name,
                                   color = Item.color

                               })
                .OrderByDescending(o => o.Name);

            Console.WriteLine(JsonConvert.SerializeObject(Items.ToArray()));

            var jsonSettings = new JsonSerializerSettings();


            return Content(JsonConvert.SerializeObject(Items.ToArray(), jsonSettings), "application/json");
        }
    }
}