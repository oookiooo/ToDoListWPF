using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListAPI.Models;
using ToDoListAPI.Services;
using Microsoft.EntityFrameworkCore.InMemory;
namespace NUnitTest
{
    [TestFixture]
    public class TaskItemServiceTests
    {
        private TodoContext _context;
        private TaskItemService _service;


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: "TestTodoDb")
                .Options;

            _context = new TodoContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Seed data
            _context.TaskItems.AddRange(
                new TaskItem { Id = 1, Title = "Task 1", DueDate = System.DateTime.Now.AddDays(1), IsCompleted = false },
                new TaskItem { Id = 2, Title = "Task 2", DueDate = System.DateTime.Now.AddDays(2), IsCompleted = true }
            );
            _context.SaveChanges();

            _service = new TaskItemService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetTaskItemsAsync_ReturnsAllTaskItems()
        {
            // Act
            var result = await _service.GetTaskItemsAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetTaskItemAsync_ExistingId_ReturnsTaskItem()
        {
            // Act
            var result = await _service.GetTaskItemAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public async Task GetTaskItemAsync_NonExistingId_ReturnsNull()
        {
            // Act
            var result = await _service.GetTaskItemAsync(99);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddTaskItemAsync_AddsTaskItem()
        {
            // Arrange
            var newTask = new TaskItem
            {
                Title = "Task 3",
                DueDate = System.DateTime.Now.AddDays(3),
                IsCompleted = false
            };

            // Act
            var createdTask = await _service.AddTaskItemAsync(newTask);
            var allTasks = await _service.GetTaskItemsAsync();

            // Assert
            Assert.IsNotNull(createdTask);
            Assert.AreEqual(3, allTasks.Count);
            Assert.AreEqual("Task 3", createdTask.Title);
        }

        [Test]
        public async Task UpdateTaskItemAsync_ExistingTask_ReturnsTrue()
        {
            // Arrange
            var task = await _service.GetTaskItemAsync(1);
            task.Title = "Updated Task 1";

            // Act
            var result = await _service.UpdateTaskItemAsync(task);
            var updatedTask = await _service.GetTaskItemAsync(1);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("Updated Task 1", updatedTask.Title);
        }

        [Test]
        public async Task UpdateTaskItemAsync_NonExistingTask_ReturnsFalse()
        {
            // Arrange
            var task = new TaskItem
            {
                Id = 99,
                Title = "Non-Existing Task",
                DueDate = System.DateTime.Now,
                IsCompleted = false
            };

            // Act
            var result = await _service.UpdateTaskItemAsync(task);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteTaskItemAsync_ExistingId_ReturnsTrue()
        {
            // Act
            var result = await _service.DeleteTaskItemAsync(1);
            var allTasks = await _service.GetTaskItemsAsync();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, allTasks.Count);
        }

        [Test]
        public async Task DeleteTaskItemAsync_NonExistingId_ReturnsFalse()
        {
            // Act
            var result = await _service.DeleteTaskItemAsync(99);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
