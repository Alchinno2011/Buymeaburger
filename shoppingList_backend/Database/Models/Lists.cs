using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace shoppingList_backend.Database.Models
{
    public class Lists
    {
        [Table("Lists", Schema = "work")]
        public class List
        {
            [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }

            [Required] public string Name { get; set; }

            [Required] public int[] SharedUsers { get; set; }

            [Required] public int Owner { get; set; }
        }
    }
}
