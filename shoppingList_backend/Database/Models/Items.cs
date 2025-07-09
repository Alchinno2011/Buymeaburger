using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace shoppingList_backend.Database.Models
{
    [Table("Items", Schema = "work")]
    public class Items
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string color { get; set; }
    }
}
