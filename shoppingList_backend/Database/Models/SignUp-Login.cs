using Microsoft.AspNetCore.Mvc;

namespace shoppingList_backend.Database.Models
{
    public class SignUp_Login : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
