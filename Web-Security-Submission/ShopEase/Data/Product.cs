using System;
using System.ComponentModel.DataAnnotations;

public class Product
{
    public int ProductID { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; }

    [Range(0.01, 10000, ErrorMessage = "Price must be between $0.01 and $10,000.")]
    public decimal Price { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters.")]
    public string Category { get; set; }

    public void PrintDetails()
    {
        Console.WriteLine($"Product: {Name} | Price: ${Price:F2} | Category: {Category}");
    }
}