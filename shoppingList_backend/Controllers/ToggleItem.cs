using Microsoft.AspNetCore.Mvc;
using shoppingList_backend.Database;

namespace shoppingList_backend.Controllers
{
    [Route("api/changeState")]
    [ApiController]
    public class ToggleItem : Controller
    {
        [HttpPut("{Id}")]
        public async Task<IActionResult> DeleteOrder(int Id)
        {


            Console.WriteLine(Id);

            using (var context = new DBContext())
            {
                var item = context.GroceryList.FirstOrDefault(o => o.Id == Id);
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

                    await context.SaveChangesAsync();
                }
            }

            return Ok(Id);


        }
    }
}
