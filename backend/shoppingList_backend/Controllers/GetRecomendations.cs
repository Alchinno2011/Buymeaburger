using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using shoppingList_backend.Database;
using shoppingList_backend.Database.DTOs;
using shoppingList_backend.Database.Models;
using System.Drawing;

namespace shoppingList_backend.Controllers
{

    [Route("GetRecomendations")]
    [ApiController]
    public class GetRecomendations : Controller
    {
        [HttpGet("{GroceryListId}")]
        public IActionResult getRecomendations(int GroceryListId)
        {
            using DBContext context = new DBContext();

            var GroceryItems = context.Recomendations
                .Where(g => g.ListId == GroceryListId)

                .OrderByDescending(o => o.Name)
                .Select(g => new RecomendationDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    color = g.color

                })

                .ToList();

            Console.WriteLine(JsonConvert.SerializeObject(GroceryItems.ToArray()));

            var jsonSettings = new JsonSerializerSettings();


            return Content(JsonConvert.SerializeObject(GroceryItems.ToArray(), jsonSettings), "application/json");
        }
    }
}