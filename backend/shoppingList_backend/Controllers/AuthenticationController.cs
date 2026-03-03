using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shoppingList_backend.Controllers;
using shoppingList_backend.Database;
using shoppingList_backend.Database.DTOs;
using shoppingList_backend.Database.Models;
using System.Text;

namespace shoppingList_backend.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        [HttpPost("signup")]
        [Consumes("application/json")]
        public IActionResult Signup([FromBody] UserRequestData data)
        {

            if (data == null)
            {
                return BadRequest("Request body is empty");
            }

            int userId = HashPasswordAndCreateUser(data);

            if (userId == 0)
            {
                return BadRequest("Something went wrong. Please try again.");
            }


            return Ok(userId);
        }

        private int HashPasswordAndCreateUser(UserRequestData data)
        {            
            using DBContext context = new DBContext();

            User newUser = new User()
            {
                Name = data.Name,
                Email = data.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(data.Password, 12)
            };

            context.Users.Add(newUser);

            context.SaveChanges();

            var UserId = context.Users
                .Where(g => g.Name == data.Name && g.Email == data.Email)
                .Select(g => g.Id)
                .FirstOrDefault();

            if (UserId == default)
            {
                return 0;
            }

            return UserId;
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            using DBContext context = new DBContext();

            var request = HttpContext.Request;
            using var reader = new StreamReader(request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<UserRequestData>(body);

            if (data == null)
            {
                return BadRequest("Request body is empty");
            }

            var UserPassword = context.Users
                .Where(g => g.Name == data.Name && g.Email == data.Email)
                .Select(g => g.Password)
                .FirstOrDefault();

            
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(data.Password, UserPassword);

            if (isPasswordCorrect)
            {
                Console.WriteLine("Login successful!");
                return Ok("Login successful!");
            }
            else
            {
                Console.WriteLine("Invalid password!");
                return BadRequest("Invalid password or Username!"); 
            }

        }

        public class UserRequestData
        {
            public required string Name { get; set; }
            public required string Email { get; set; }
            public required string Password { get; set; }


        }
    }
}


