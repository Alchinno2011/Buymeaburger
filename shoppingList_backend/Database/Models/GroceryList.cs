using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shoppingList_backend.Database.Models
{
    [Table("GroceryList", Schema = "work")]
    public class GroceryList
    {

        [Required] public int UserId { get; set; }

        [Key] public int Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Quantity { get; set; }

        [Required] public bool IsBought { get; set; }

        [Required] public DateTime CreatedAt { get; set; }



        [ForeignKey("UserId")]
        public virtual Users Users { get; set; }
    }
}
