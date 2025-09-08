using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<TaskItem>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
        Task AddAsync(TaskItem task, CancellationToken ct = default);
        Task UpdateAsync(TaskItem task, CancellationToken ct = default);
        Task DeleteAsync(TaskItem task, CancellationToken ct = default);
        Task<int> CountAsync(string? search, CancellationToken ct = default);
    }
}
