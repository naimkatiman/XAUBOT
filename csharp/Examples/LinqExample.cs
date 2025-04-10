using System;
using System.Collections.Generic;
using System.Linq;

namespace XaubotClone.Examples
{
    /// <summary>
    /// Represents a simple product entity for LINQ examples.
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int UnitsInStock { get; set; }
    }

    /// <summary>
    /// Demonstrates various Language-Integrated Query (LINQ) operations.
    /// NOTE: This code is illustrative and requires a .NET runtime to execute.
    /// </summary>
    public static class LinqExample
    {
        private static List<Product> GetSampleProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Category = "Electronics", Price = 1200.00m, UnitsInStock = 10 },
                new Product { Id = 2, Name = "Keyboard", Category = "Electronics", Price = 75.50m, UnitsInStock = 25 },
                new Product { Id = 3, Name = "Mouse", Category = "Electronics", Price = 25.99m, UnitsInStock = 50 },
                new Product { Id = 4, Name = "Desk Chair", Category = "Furniture", Price = 150.00m, UnitsInStock = 5 },
                new Product { Id = 5, Name = "Notebook", Category = "Stationery", Price = 3.50m, UnitsInStock = 100 },
                new Product { Id = 6, Name = "Pen Set", Category = "Stationery", Price = 12.00m, UnitsInStock = 80 },
                new Product { Id = 7, Name = "Monitor", Category = "Electronics", Price = 300.00m, UnitsInStock = 8 }
            };
        }

        /// <summary>
        /// Finds all products in a specific category.
        /// </summary>
        public static void FindProductsByCategory(string category)
        {
            List<Product> products = GetSampleProducts();

            // LINQ Query Syntax
            var querySyntaxResult = from p in products
                                    where p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)
                                    select p;

            // LINQ Method Syntax
            var methodSyntaxResult = products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

            Console.WriteLine($"Products in category '{category}':");
            foreach (var product in methodSyntaxResult) // Using method syntax result here
            {
                Console.WriteLine($"  - {product.Name} (Price: {product.Price:C})");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Orders products by price (descending) and takes the top N.
        /// </summary>
        public static void FindTopNMostExpensiveProducts(int n)
        {
            List<Product> products = GetSampleProducts();

            var topNProducts = products.OrderByDescending(p => p.Price).Take(n);

            Console.WriteLine($"Top {n} most expensive products:");
            foreach (var product in topNProducts)
            {
                 Console.WriteLine($"  - {product.Name} (Price: {product.Price:C})");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Groups products by category and counts them.
        /// </summary>
        public static void GroupProductsByCategory()
        {
            List<Product> products = GetSampleProducts();

            var groupedProducts = products.GroupBy(p => p.Category);

            Console.WriteLine("Product counts by category:");
            foreach (var group in groupedProducts)
            {
                Console.WriteLine($"  - Category: {group.Key}, Count: {group.Count()}");
                // Optionally iterate through products in the group
                // foreach (var product in group)
                // {
                //    Console.WriteLine($"      -> {product.Name}");
                // }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Calculates the average price of products in stock.
        /// </summary>
        public static void CalculateAveragePriceInStock()
        {
            List<Product> products = GetSampleProducts();

            var avgPrice = products.Where(p => p.UnitsInStock > 0).Average(p => p.Price);

            Console.WriteLine($"Average price of products in stock: {avgPrice:C}");
            Console.WriteLine();
        }
    }
} 