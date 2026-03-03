using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using shoppingList_backend.Database;
using shoppingList_backend.Database.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static shoppingList_backend.Controllers.AuthenticationController;

namespace shoppingList_backend.Controllers
{
    [Route("sharelist")]
    [ApiController]
    public class ShareListController : Controller
    {

        [HttpPost("{groceryListId}")]


        public IActionResult ShareListWithUser(int groceryListId, [FromBody] ShareListRequest req)
        {
            using DBContext context = new DBContext();

            ShareGroceryListWithUser(groceryListId, req.ShareUserId);

            return Ok("User added");
        }

        public void ShareGroceryListWithUser(int groceryListId, int userId)
        {
            using DBContext context = new DBContext();

            // Find the grocery list and user
            var groceryList = context.GroceryList
                .Include(g => g.SharedUsers)
                .FirstOrDefault(g => g.Id == groceryListId);

            var user = context.Users.Find(userId);

            if (groceryList == null || user == null)
            {
                BadRequest("Grocery list or user not found");
            }

            // Check if the user is already in the shared list
            if (groceryList.SharedUsers.Any(u => u.Id == userId))
            {
                return; // User is already in the shared list
            }

            // Add the user to the shared list
            groceryList.SharedUsers.Add(user);

            context.SaveChanges();
        }


    }

    public class ShareListRequest
    {
        public int ShareUserId { get; set; }
    }

}
