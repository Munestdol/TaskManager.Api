using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Models;
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
        public async Task<ActionResult<ApiResponse<IEnumerable<TaskDto>>>> GetAll(
        int page = 1, int pageSize = 10, string? search = null, CancellationToken ct = default)
        {
            var tasks = await _repo.GetPagedAsync(page, pageSize, search, ct);
            return Ok(ApiResponse<IEnumerable<TaskDto>>.Success(
                _mapper.Map<IEnumerable<TaskDto>>(tasks),
                traceId: HttpContext.TraceIdentifier
            ));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<TaskDto>>> GetById(Guid id, CancellationToken ct)
        {
            var task = await _repo.GetByIdAsync(id, ct);
            if (task == null)
                return NotFound(ApiResponse<TaskDto>.Fail("Task not found", 404, HttpContext.TraceIdentifier));

            return Ok(ApiResponse<TaskDto>.Success(
                _mapper.Map<TaskDto>(task),
                traceId: HttpContext.TraceIdentifier
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Guid>>> Create([FromBody] CreateTaskDto dto, CancellationToken ct)
        {
            var task = _mapper.Map<TaskItem>(dto);
            await _repo.AddAsync(task, ct);

            return CreatedAtAction(nameof(GetById), new { id = task.Id },
                ApiResponse<Guid>.Success(task.Id, "Task created", 201, HttpContext.TraceIdentifier));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskDto dto, CancellationToken ct)
        {
            var existing = await _repo.GetByIdAsync(id, ct);
            if (existing == null)
                return NotFound(ApiResponse<string>.Fail("Task not found", 404, HttpContext.TraceIdentifier));

            existing.Update(dto.Title, dto.Description, dto.DueDateUtc);
            existing.ChangeStatus((Domain.Enums.TaskStatus)dto.Status);

            await _repo.UpdateAsync(existing, ct);
            return Ok(ApiResponse<string>.Success("Task updated", traceId: HttpContext.TraceIdentifier));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var existing = await _repo.GetByIdAsync(id, ct);
            if (existing == null)
                return NotFound(ApiResponse<string>.Fail("Task not found", 404, HttpContext.TraceIdentifier));

            await _repo.DeleteAsync(existing, ct);
            return Ok(ApiResponse<string>.Success("Task deleted", traceId: HttpContext.TraceIdentifier));
        }

    }
}
