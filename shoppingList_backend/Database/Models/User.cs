using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace shoppingList_backend.Database.Models
{
    [Table("Users", Schema = "work")]
    public class User
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }

        [InverseProperty("Owner")]
        public virtual List<GroceryList> OwnedGroceryLists { get; set; }

        [InverseProperty("SharedUsers")]
        public virtual List<GroceryList> SharedGroceryLists { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Email { get; set; }

        [Required] public string Password { get; set; }
    }
}
