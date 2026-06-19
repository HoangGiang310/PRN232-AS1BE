using System.ComponentModel.DataAnnotations;

namespace SimpleShop.Service;

public class CategoryDto
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryDescription { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int ProductCount { get; set; }
}

public class CreateCategoryDto
{
    [Required(ErrorMessage = "Category name is required")]
    [MaxLength(100, ErrorMessage = "Category name must not exceed 100 characters")]
    public string CategoryName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category description is required")]
    [MaxLength(250, ErrorMessage = "Description must not exceed 250 characters")]
    public string CategoryDescription { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

public class UpdateCategoryDto : CreateCategoryDto { }

public class ProductDto
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

public class CreateProductDto
{
    [Required(ErrorMessage = "Product name is required")]
    [MaxLength(200, ErrorMessage = "Product name must not exceed 200 characters")]
    public string ProductName { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be >= 0")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be >= 0")]
    public int StockQuantity { get; set; } = 0;

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Category is required")]
    public int CategoryID { get; set; }

    public bool IsActive { get; set; } = true;
}

public class UpdateProductDto : CreateProductDto { }

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
