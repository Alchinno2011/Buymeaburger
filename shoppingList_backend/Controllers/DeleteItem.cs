using Microsoft.AspNetCore.Mvc;
using shoppingList_backend.Database;

namespace shoppingList_backend.Controllers
{
    [Route("api/Delete Item")]
    [ApiController]
    public class DeleteItem : Controller
    {
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteOrder(int Id)
        {


            Console.WriteLine(Id);

            using (var context = new DBContext())
            {
                var item = context.GroceryList.FirstOrDefault(o => o.Id == Id);
                if (item != null)
                {
                    context.GroceryList.Remove(item);
                    await context.SaveChangesAsync();
                }
            }

            return Ok(Id);


        }
    }
}
