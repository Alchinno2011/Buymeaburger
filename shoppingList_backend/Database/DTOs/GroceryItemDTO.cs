namespace shoppingList_backend.Database.DTOs
{
    public class GroceryItemDTO
    {

        public int ListId { get; set; }

        public virtual GroceryListDTO? GroceryList { get; set; }

        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Quantity { get; set; }

        public bool IsBought { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public required string color { get; set; }
    }
}
