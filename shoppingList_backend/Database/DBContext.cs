using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using shoppingList_backend.Database.Models;
using System.Collections;
using System.Collections.Generic;

namespace shoppingList_backend.Database
{
    public class DBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<GroceryItem> GroceryItem { get; set; }
        public DbSet<GroceryList> GroceryList { get; set; }
        public DbSet<Recomendation> Recomendations { get; set; }


        // Add auto get time when order is placed

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Order>()
        //        .Property(b => b.TimePlaced)
        //        .HasDefaultValueSql("CURRENT_TIMESTAMP");
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                if (File.Exists("/src/.env"))
                    DotNetEnv.Env.Load("/src/.env");
                else if (File.Exists("../.env"))
                    DotNetEnv.Env.Load("../.env");


                foreach (DictionaryEntry env in Environment.GetEnvironmentVariables())
                {
                    Console.WriteLine($"{env.Key}={env.Value}");
                }

                Console.WriteLine("Environment variables loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading environment variables: " + ex.Message);
            }

            Console.WriteLine($"DB_HOST: {Environment.GetEnvironmentVariable("DB_HOST")}");

            Console.WriteLine($"DB_PASSWORD: {Environment.GetEnvironmentVariable("DB_PASSWORD")}");

            optionsBuilder.UseNpgsql($"Host={Environment.GetEnvironmentVariable("DB_HOST")};Port={Environment.GetEnvironmentVariable("DB_PORT")};Username={Environment.GetEnvironmentVariable("DB_USER")};Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};Database={Environment.GetEnvironmentVariable("DB_DATABASE")};");
        }

    }
}
