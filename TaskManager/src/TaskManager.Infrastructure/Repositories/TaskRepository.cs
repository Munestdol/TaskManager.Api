using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Repositories
{
    public sealed class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerDbContext _db;

        public TaskRepository(TaskManagerDbContext db) => _db = db;

        public Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            _db.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, ct);

        public async Task<IReadOnlyList<TaskItem>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default)
        {
            var query = _db.Tasks.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(t => t.Title.Contains(search));

            return await query
                .OrderByDescending(t => t.CreatedAtUtc)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task AddAsync(TaskItem task, CancellationToken ct = default)
        {
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(TaskItem task, CancellationToken ct = default)
        {
            _db.Tasks.Update(task);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(TaskItem task, CancellationToken ct = default)
        {
            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync(ct);
        }

        public Task<int> CountAsync(string? search, CancellationToken ct = default)
        {
            var query = _db.Tasks.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(t => t.Title.Contains(search));

            return query.CountAsync(ct);
        }
    }
}
