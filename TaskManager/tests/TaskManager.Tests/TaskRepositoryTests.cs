using Microsoft.EntityFrameworkCore;
using System;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;
using TaskManager.Infrastructure.Repositories;
using Xunit;

namespace TaskManager.Tests
{
    public class TaskRepositoryTests
    {
        private TaskManagerDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<TaskManagerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TaskManagerDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddTask()
        {
            var db = GetDbContext();
            var repo = new TaskRepository(db);
            var task = new TaskItem("Test", "Desc", DateTime.UtcNow);

            await repo.AddAsync(task);

            var saved = await db.Tasks.FirstOrDefaultAsync();
            Assert.NotNull(saved);
            Assert.Equal("Test", saved!.Title);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectTask()
        {
            var db = GetDbContext();
            var repo = new TaskRepository(db);
            var task = new TaskItem("Test", "Desc", DateTime.UtcNow);
            await repo.AddAsync(task);

            var found = await repo.GetByIdAsync(task.Id);

            Assert.NotNull(found);
            Assert.Equal(task.Id, found!.Id);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldReturnFilteredTasks()
        {
            var db = GetDbContext();
            var repo = new TaskRepository(db);

            await repo.AddAsync(new TaskItem("Alpha", "Desc", DateTime.UtcNow));
            await repo.AddAsync(new TaskItem("Beta", "Desc", DateTime.UtcNow));

            var result = await repo.GetPagedAsync(1, 10, "Alpha");

            Assert.Single(result);
            Assert.Equal("Alpha", result.First().Title);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveTask()
        {
            var db = GetDbContext();
            var repo = new TaskRepository(db);
            var task = new TaskItem("Test", "Desc", DateTime.UtcNow);
            await repo.AddAsync(task);

            await repo.DeleteAsync(task);

            var exists = await db.Tasks.AnyAsync();
            Assert.False(exists);
        }
    }
}
