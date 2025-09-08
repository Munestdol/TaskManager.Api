namespace TaskManager.Application.Tasks.DTOs
{
    public sealed class TaskDto
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public string? Description { get; init; }
        public int Status { get; init; }
        public DateTime CreatedAtUtc { get; init; }
        public DateTime? DueDateUtc { get; init; }
    }
}
