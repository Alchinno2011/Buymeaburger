using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace shoppingList_backend.Database.Models
{
    [Table("Recomendation", Schema = "work")]
    public class Recomendation
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }

        [Required] public int ListId { get; set; }

        [ForeignKey(nameof(ListId))]
        public virtual GroceryList? GroceryList { get; set; }

        [Required] public required string Name { get; set; }

        [Required] public required string color { get; set; }
    }
}
