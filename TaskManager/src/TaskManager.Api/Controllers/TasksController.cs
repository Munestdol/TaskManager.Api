using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Tasks.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _repo;
        private readonly IMapper _mapper;

        public TasksController(ITaskRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll(
        int page = 1, int pageSize = 10, string? search = null, CancellationToken ct = default)
        {
            var tasks = await _repo.GetPagedAsync(page, pageSize, search, ct);
            return Ok(_mapper.Map<IEnumerable<TaskDto>>(tasks));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskDto>> GetById(Guid id, CancellationToken ct)
        {
            var task = await _repo.GetByIdAsync(id, ct);
            if (task == null) return NotFound();
            return Ok(_mapper.Map<TaskDto>(task));
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateTaskDto dto, CancellationToken ct)
        {
            var task = _mapper.Map<TaskItem>(dto);
            await _repo.AddAsync(task, ct);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task.Id);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskDto dto, CancellationToken ct)
        {
            var existing = await _repo.GetByIdAsync(id, ct);
            if (existing == null) return NotFound();

            existing.Update(dto.Title, dto.Description, dto.DueDateUtc);
            existing.ChangeStatus((Domain.Enums.TaskStatus)dto.Status);

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
