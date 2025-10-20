using Microsoft.AspNetCore.Mvc;
using shoppingList_backend.Database;

namespace shoppingList_backend.Controllers
{
    [Route("changeState")]
    [ApiController]
    public class ToggleItem : Controller
    {
        [HttpPut("{GroceryListId}/{Id}")]
        public async Task<IActionResult> DeleteOrder(int Id, int GroceryListId)
        {


            Console.WriteLine(Id);

            using (var context = new DBContext())
            {
                var item = context.GroceryItem.FirstOrDefault(o => o.Id == Id && o.ListId == GroceryListId);
                if (item != null)
                {
                    if (item.IsBought == false)
                    {
                        item.IsBought = true;
                    }

                    else if (item.IsBought == true)
                    {
                        item.IsBought = false;
                    }

                    item.Quantity = null;

                    await context.SaveChangesAsync();
                }
            }

            return Ok(Id);


        }
    }
}
