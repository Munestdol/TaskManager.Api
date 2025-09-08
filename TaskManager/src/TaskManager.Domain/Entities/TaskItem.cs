namespace TaskManager.Domain.Entities
{
    public sealed class TaskItem
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public Enums.TaskStatus Status { get; private set; } = Enums.TaskStatus.Todo;
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
        public DateTime? DueDateUtc { get; private set; }
        public Guid? AssigneeId { get; private set; }

        private TaskItem() { }

        public TaskItem(string title, string? description = null, DateTime? dueDateUtc = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required", nameof(title));

            Title = title.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            DueDateUtc = dueDateUtc;
        }

        public void Update(string title, string? description, DateTime? dueDateUtc)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required", nameof(title));

            Title = title.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            DueDateUtc = dueDateUtc;
        }

        public void ChangeStatus(Enums.TaskStatus status) => Status = status;

        public void AssignTo(Guid userId) => AssigneeId = userId;
    }
}
