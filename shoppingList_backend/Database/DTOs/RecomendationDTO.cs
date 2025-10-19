namespace shoppingList_backend.Database.DTOs
{
    public class RecomendationDTO
    {
        public int Id { get; set; }

        public int ListId { get; set; }

        public virtual GroceryListDTO? GroceryList { get; set; }

        public required string Name { get; set; }

        public required string color { get; set; }
    }
}
