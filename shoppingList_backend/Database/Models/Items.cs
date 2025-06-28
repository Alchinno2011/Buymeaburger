using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shoppingList_backend.Database.Models
{
    [Table("Items", Schema = "work")]
    public class Items
    {
        [Key] public int Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public int FrequencyUsed { get; set; }
    }
}
