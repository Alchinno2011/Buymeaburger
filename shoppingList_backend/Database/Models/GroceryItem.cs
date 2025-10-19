using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shoppingList_backend.Database.Models
{
    [Table("GroceryItem", Schema = "work")]
    public class GroceryItem
    {

        [Required] public int ListId { get; set; }

        [ForeignKey("ListId")]
        public virtual GroceryList? GroceryList { get; set; }

        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }

        [Required] public required string Name { get; set; }

        public string? Quantity { get; set; }

        [Required] public bool IsBought { get; set; }

        [Required] public DateTime CreatedAt { get; set; }
        
        public required string color { get; set; }



        //[ForeignKey("UserId")]
        //public virtual Users Users { get; set; }
    }
}
