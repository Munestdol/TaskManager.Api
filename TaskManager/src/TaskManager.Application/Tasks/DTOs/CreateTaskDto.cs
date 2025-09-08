namespace TaskManager.Application.Tasks.DTOs
{
    public sealed class CreateTaskDto
    {
        public string Title { get; init; } = default!;
        public string? Description { get; init; }
        public DateTime? DueDateUtc { get; init; }
    }
}
