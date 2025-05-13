using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

public class Cart
{
    private List<Product> _products = new List<Product>();
    private string _connectionString = "Server=localhost;Database=Shop;Uid=root;Pwd=your_password;"; // Replace with your MySQL credentials

    public void AddProduct(Product product)
    {
        _products.Add(product);
        SaveToDatabase(product);
    }

    public void RemoveProduct(int productId)
    {
        var productToRemove = _products.FirstOrDefault(p => p.ProductID == productId);
        if (productToRemove != null)
        {
            _products.Remove(productToRemove);
            RemoveFromDatabase(productId);
        }
        else
        {
            Console.WriteLine($"Product with ID {productId} not found in the cart.");
        }
    }

    public void DisplayCartItems()
    {
        if (_products.Count == 0)
        {
            Console.WriteLine("Your cart is empty.");
            return;
        }
        Console.WriteLine("--- Cart Items ---");
        foreach (var product in _products)
        {
            product.PrintDetails();
        }
        Console.WriteLine("------------------");
    }

    public decimal CalculateTotal()
    {
        return _products.Sum(p => p.Price);
    }

    private void SaveToDatabase(Product product)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Products (ProductID, Name, Price, Category) VALUES (@ProductID, @Name, @Price, @Category)";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductID", product.ProductID);
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Category", product.Category);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine($"{product.Name} added to the database.");
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error saving to database: {ex.Message}");
        }
    }

    private void RemoveFromDatabase(int productId)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM Products WHERE ProductID = @ProductID";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Product with ID {productId} removed from the database.");
                    }
                    else
                    {
                        Console.WriteLine($"Product with ID {productId} not found in the database.");
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error removing from database: {ex.Message}");
        }
    }
}