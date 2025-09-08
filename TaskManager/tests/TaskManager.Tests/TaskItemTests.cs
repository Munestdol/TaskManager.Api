using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Tests
{
    public class TaskItemTests
    {
        [Fact]
        public void CreateTask_WithValidData_ShouldSetProperties()
        {
            var task = new TaskItem("Test title", "Test description", DateTime.UtcNow.AddDays(1));

            Assert.Equal("Test title", task.Title);
            Assert.Equal("Test description", task.Description);
            Assert.Equal(Domain.Enums.TaskStatus.Todo, task.Status);
            Assert.NotEqual(Guid.Empty, task.Id);
        }

        [Fact]
        public void CreateTask_WithEmptyTitle_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() => new TaskItem("", "desc", DateTime.UtcNow));
        }

        [Fact]
        public void ChangeStatus_ShouldUpdateStatus()
        {
            var task = new TaskItem("Test", "Desc", DateTime.UtcNow);
            task.ChangeStatus(Domain.Enums.TaskStatus.Done);

            Assert.Equal(Domain.Enums.TaskStatus.Done, task.Status);
        }

        [Fact]
        public void Update_ShouldChangeTitleDescriptionAndDueDate()
        {
            var task = new TaskItem("Old title", "Old desc", DateTime.UtcNow);
            var newDueDate = DateTime.UtcNow.AddDays(5);

            task.Update("New title", "New desc", newDueDate);

            Assert.Equal("New title", task.Title);
            Assert.Equal("New desc", task.Description);
            Assert.Equal(newDueDate, task.DueDateUtc);
        }
    }
}
