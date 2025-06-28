using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace shoppingList_backend.Database.Models
{
    [Table("Users", Schema = "work")]
    public class Users
    {
        [Key] public int Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Email { get; set; }

        [Required] public string Password { get; set; }
    }
}
