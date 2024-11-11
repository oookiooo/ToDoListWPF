// ToDoListAPI/Controllers/TaskItemsController.cs
using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Models;
using ToDoListAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoListAPI.Interfaces;

namespace ToDoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly ITaskItemService _taskItemService;

        public TaskItemsController(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        // GET: api/TaskItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTaskItems()
        {
            var taskItems = await _taskItemService.GetTaskItemsAsync();
            return Ok(taskItems);
        }

        // GET: api/TaskItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskItem(int id)
        {
            var taskItem = await _taskItemService.GetTaskItemAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            return Ok(taskItem);
        }

        // POST: api/TaskItems
        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTaskItem(TaskItem taskItem)
        {
            var createdTaskItem = await _taskItemService.AddTaskItemAsync(taskItem);
            return CreatedAtAction(nameof(GetTaskItem), new { id = createdTaskItem.Id }, createdTaskItem);
        }

        // PUT: api/TaskItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskItem(int id, TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return BadRequest();
            }

            var result = await _taskItemService.UpdateTaskItemAsync(taskItem);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/TaskItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            var result = await _taskItemService.DeleteTaskItemAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
