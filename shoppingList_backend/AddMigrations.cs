using Microsoft.EntityFrameworkCore;
using shoppingList_backend.Database;


namespace shoppingList_backend
{
    public class AddMigrations
    {
        public static void ApplyMigrations()
        {
            try
            {
                using DBContext context = new DBContext();

                var pendingMigrations = context.Database.GetPendingMigrations();

                if (pendingMigrations.Any())
                {
                    Console.WriteLine("Applying pending migrations...");
                    context.Database.Migrate();
                    Console.WriteLine("Migrations applied successfully.");
                }
                else
                {
                    Console.WriteLine("No pending migrations to apply.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Migration failed: " + ex.Message);
            }

        }
    }
}