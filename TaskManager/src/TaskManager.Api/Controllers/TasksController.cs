using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _repo;

        public TasksController(ITaskRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll(
            int page = 1, int pageSize = 10, string? search = null, CancellationToken ct = default)
        {
            var tasks = await _repo.GetPagedAsync(page, pageSize, search, ct);
            return Ok(tasks);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskItem>> GetById(Guid id, CancellationToken ct)
        {
            var task = await _repo.GetByIdAsync(id, ct);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] TaskItem task, CancellationToken ct)
        {
            await _repo.AddAsync(task, ct);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task.Id);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TaskItem updatedTask, CancellationToken ct)
        {
            var existing = await _repo.GetByIdAsync(id, ct);
            if (existing == null) return NotFound();

            existing.Update(updatedTask.Title, updatedTask.Description, updatedTask.DueDateUtc);
            existing.ChangeStatus(updatedTask.Status);

            await _repo.UpdateAsync(existing, ct);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var existing = await _repo.GetByIdAsync(id, ct);
            if (existing == null) return NotFound();

            await _repo.DeleteAsync(existing, ct);
            return NoContent();
        }
    }
}
