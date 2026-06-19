using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleShop.Service;
using SimpleShop.Service.Interfaces;

namespace SimpleShop.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    // GET /api/products — Public
    [HttpGet]
    public async Task<IActionResult> GetActive()
        => Ok(await _service.GetAllActiveAsync());

    // GET /api/products/all — Admin only
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    // GET /api/products/{id} — Public
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound(new { message = $"Product {id} not found." });
        return Ok(result);
    }

    // GET /api/products/category/{categoryId} — Public
    [HttpGet("category/{categoryId:int}")]
    public async Task<IActionResult> GetByCategory(int categoryId)
        => Ok(await _service.GetByCategoryAsync(categoryId));

    // GET /api/products/search — Public
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? name,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int? categoryId)
    {
        var result = await _service.SearchAsync(name, minPrice, maxPrice, categoryId);
        return Ok(result);
    }

    // POST /api/products — Admin only
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.ProductID }, result);
    }

    // PUT /api/products/{id} — Admin only
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _service.UpdateAsync(id, dto);
        if (result == null) return NotFound(new { message = $"Product {id} not found." });
        return Ok(result);
    }

    // DELETE /api/products/{id} — Admin only (soft delete)
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.SoftDeleteAsync(id);
        if (!success) return NotFound(new { message = $"Product {id} not found." });
        return Ok(new { message = "Product deactivated successfully." });
    }
}
