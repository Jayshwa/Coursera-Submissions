using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ShopEase
{
    public class ApplicationDbContext
    {
        private readonly string _connectionString = "Server=localhost;Database=ShopEase;Uid=root;Pwd=;";

        public List<Product> GetProducts()
        {
            var products = new List<Product>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT ProductID, Name, Price, Category FROM Products";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ProductID = reader.GetInt32("ProductID"),
                            Name = reader.GetString("Name"),
                            Price = reader.GetDecimal("Price"),
                            Category = reader.GetString("Category")
                        });
                    }
                }
            }

            return products;
        }

        public void AddProduct(Product product)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO Products (ProductID, Name, Price, Category) VALUES (@ProductID, @Name, @Price, @Category)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", product.ProductID);
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Category", product.Category);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void RemoveProduct(int productId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Products WHERE ProductID = @ProductID";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}