using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleShop.Service;
using SimpleShop.Service.Interfaces;

namespace SimpleShop.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoriesController(ICategoryService service)
    {
        _service = service;
    }

    // GET /api/categories — Public: active categories only
    [HttpGet]
    public async Task<IActionResult> GetActive()
    {
        var result = await _service.GetAllActiveAsync();
        return Ok(result);
    }

    // GET /api/categories/all — Admin only
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    // GET /api/categories/{id} — Public
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound(new { message = $"Category {id} not found." });
        return Ok(result);
    }

    // GET /api/categories/search?name={keyword} — Admin only
    [HttpGet("search")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Search([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest(new { message = "Name keyword is required." });
        var result = await _service.SearchByNameAsync(name);
        return Ok(result);
    }

    // POST /api/categories — Admin only
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.CategoryID }, result);
    }

    // PUT /api/categories/{id} — Admin only
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _service.UpdateAsync(id, dto);
        if (result == null) return NotFound(new { message = $"Category {id} not found." });
        return Ok(result);
    }

    // DELETE /api/categories/{id} — Admin only
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, message) = await _service.DeleteAsync(id);
        if (!success) return BadRequest(new { message });
        return Ok(new { message });
    }
}
