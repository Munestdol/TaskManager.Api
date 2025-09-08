namespace TaskManager.Application.Tasks.DTOs
{
    public sealed class UpdateTaskDto
    {
        public string Title { get; init; } = default!;
        public string? Description { get; init; }
        public int Status { get; init; }
        public DateTime? DueDateUtc { get; init; }
    }
}
