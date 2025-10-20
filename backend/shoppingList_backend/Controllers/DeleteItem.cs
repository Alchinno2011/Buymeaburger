using Microsoft.AspNetCore.Mvc;
using shoppingList_backend.Database;

namespace shoppingList_backend.Controllers
{
    [Route("DeleteItem")]
    [ApiController]
    public class DeleteItem : Controller
    {
        [HttpDelete("{GroceryListId}/{Id}")]
        public async Task<IActionResult> DeleteOrder(int Id, int GroceryListId)
        {


            Console.WriteLine(Id);

            using (var context = new DBContext())
            {
                var item = context.GroceryItem.FirstOrDefault(o => o.Id == Id && o.ListId == GroceryListId);
                if (item != null)
                {
                    context.GroceryItem.Remove(item);
                    await context.SaveChangesAsync();
                }
            }

            return Ok(Id);


        }
    }
}
