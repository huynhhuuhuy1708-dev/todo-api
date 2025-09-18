using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoApi.Data;
using TodoApi.Models;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase {
    private readonly AppDbContext _db;
    public TasksController(AppDbContext db) { _db = db; }

    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var list = await _db.TodoItems.Where(t => t.UserId == CurrentUserId).ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) {
        var item = await _db.TodoItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == CurrentUserId);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TodoCreateDto dto) {
        var todo = new TodoItem { Title = dto.Title, Description = dto.Description, UserId = CurrentUserId };
        _db.TodoItems.Add(todo);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TodoUpdateDto dto) {
        var item = await _db.TodoItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == CurrentUserId);
        if (item == null) return NotFound();
        if (dto.Title != null) item.Title = dto.Title;
        if (dto.Description != null) item.Description = dto.Description;
        if (dto.IsCompleted.HasValue) item.IsCompleted = dto.IsCompleted.Value;
        await _db.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        var item = await _db.TodoItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == CurrentUserId);
        if (item == null) return NotFound();
        _db.TodoItems.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
