using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SmartPantry.Api.Models;

namespace SmartPantry.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IMongoCollection<PantryItem> _collection;

    public ItemsController(IMongoCollection<PantryItem> collection)
    {
        _collection = collection;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = await _collection.Find(_ => true).ToListAsync();
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PantryItem item)
    {
        item.Id = Guid.NewGuid();
        await _collection.InsertOneAsync(item);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _collection.DeleteOneAsync(item => item.Id == id);
        
        if (result.DeletedCount == 0)
            return NotFound();

        return NoContent();
    }
}