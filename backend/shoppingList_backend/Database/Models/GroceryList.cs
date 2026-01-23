using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shoppingList_backend.Database.Models
{
    [Table("GroceryList", Schema = "work")]
    public class GroceryList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual User? Owner { get; set; }

        [InverseProperty(nameof(User.SharedGroceryLists))]

        public List<User>? SharedUsers { get; set; } = new List<User>();

        public virtual List<GroceryItem>? Items { get; set; }
    }
}
