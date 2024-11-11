// ToDoListAPI.Tests/Controllers/TaskItemsControllerTests.cs
using NUnit.Framework;
using Moq;
using ToDoListAPI.Services;
using ToDoListAPI.Controllers;
using ToDoListAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoListAPI.Interfaces;

namespace ToDoListAPI.Tests.Controllers
{
    public class TaskItemsControllerTests
    {
        private Mock<ITaskItemService> _mockService;
        private TaskItemsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<ITaskItemService>();
            _controller = new TaskItemsController(_mockService.Object);
        }

        [Test]
        public async Task GetTaskItems_ReturnsOkResult_WithListOfTaskItems()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Task 1", DueDate = System.DateTime.Now.AddDays(1), IsCompleted = false },
                new TaskItem { Id = 2, Title = "Task 2", DueDate = System.DateTime.Now.AddDays(2), IsCompleted = true }
            };
            _mockService.Setup(s => s.GetTaskItemsAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetTaskItems();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(tasks, okResult.Value);
        }

        [Test]
        public async Task GetTaskItem_ExistingId_ReturnsOkResult_WithTaskItem()
        {
            // Arrange
            var task = new TaskItem { Id = 1, Title = "Task 1", DueDate = System.DateTime.Now.AddDays(1), IsCompleted = false };
            _mockService.Setup(s => s.GetTaskItemAsync(1)).ReturnsAsync(task);

            // Act
            var result = await _controller.GetTaskItem(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(task, okResult.Value);
        }

        [Test]
        public async Task GetTaskItem_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetTaskItemAsync(99)).ReturnsAsync((TaskItem)null);

            // Act
            var result = await _controller.GetTaskItem(99);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task PostTaskItem_ValidTaskItem_ReturnsCreatedAtAction()
        {
            // Arrange
            var newTask = new TaskItem { Id = 3, Title = "Task 3", DueDate = System.DateTime.Now.AddDays(3), IsCompleted = false };
            _mockService.Setup(s => s.AddTaskItemAsync(newTask)).ReturnsAsync(newTask);

            // Act
            var result = await _controller.PostTaskItem(newTask);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.AreEqual("GetTaskItem", createdAtActionResult.ActionName);
            Assert.AreEqual(newTask, createdAtActionResult.Value);
        }

        [Test]
        public async Task PutTaskItem_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            var task = new TaskItem { Id = 1, Title = "Task 1", DueDate = System.DateTime.Now.AddDays(1), IsCompleted = false };

            // Act
            var result = await _controller.PutTaskItem(2, task);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task PutTaskItem_ServiceReturnsFalse_ReturnsNotFound()
        {
            // Arrange
            var task = new TaskItem { Id = 99, Title = "Non-Existing Task", DueDate = System.DateTime.Now, IsCompleted = false };
            _mockService.Setup(s => s.UpdateTaskItemAsync(task)).ReturnsAsync(false);

            // Act
            var result = await _controller.PutTaskItem(99, task);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task PutTaskItem_ServiceReturnsTrue_ReturnsNoContent()
        {
            // Arrange
            var task = new TaskItem { Id = 1, Title = "Updated Task", DueDate = System.DateTime.Now.AddDays(1), IsCompleted = false };
            _mockService.Setup(s => s.UpdateTaskItemAsync(task)).ReturnsAsync(true);

            // Act
            var result = await _controller.PutTaskItem(1, task);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteTaskItem_ServiceReturnsFalse_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteTaskItemAsync(99)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteTaskItem(99);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteTaskItem_ServiceReturnsTrue_ReturnsNoContent()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteTaskItemAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteTaskItem(1);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
}
