namespace shoppingList_backend.Database.DTOs
{
    public class GroceryListDTO
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public int OwnerId { get; set; }

        public required virtual UserDTO Owner { get; set; }

        public required List<UserDTO> SharedUsers { get; set; }

        public required virtual List<GroceryItemDTO> Items { get; set; }
    }
}
