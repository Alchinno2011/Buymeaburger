namespace shoppingList_backend.Database.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        public required virtual List<GroceryListDTO> OwnedGroceryLists { get; set; }

        public required virtual List<GroceryListDTO> SharedGroceryLists { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
