using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence
{
    public sealed class TaskManagerDbContext : DbContext
    {
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options)
            : base(options) { }

        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskManagerDbContext).Assembly);
        }
    }
}
