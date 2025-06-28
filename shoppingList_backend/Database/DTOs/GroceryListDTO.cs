namespace shoppingList_backend.Database.DTOs
{
    public class GroceryListDTO
    {

        public int UserId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Quantity { get; set; }

        public bool IsBought { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
