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

    [Route("getGroceryLists")]
    [ApiController]
    public class GetGroceryLists : Controller
    {
        [HttpGet("{userId}")]
        public IActionResult getGrocerylists(int userId)
        {
            using DBContext context = new DBContext();

            var user = context.Users.Find(userId);
            if (user == null) return BadRequest("User not found");

            var groceryLists = context.GroceryList
                .Where(g => g.OwnerId == userId || g.SharedUsers.Any(u => u.Id == userId))
                .OrderByDescending(g => g.Id)
                .Select(g => new ShareGroceryListsDTO { Id = g.Id, Name = g.Name })
                .ToList();

            return Content(JsonConvert.SerializeObject(groceryLists), "application/json");
        }
    }
}