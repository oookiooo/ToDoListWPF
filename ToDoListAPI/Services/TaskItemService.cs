using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Interfaces;
using ToDoListAPI.Models;

namespace ToDoListAPI.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly TodoContext _context;

        public TaskItemService(TodoContext context)
        {
            _context = context;
        }

        public async Task<List<TaskItem>> GetTaskItemsAsync()
        {
            return await _context.TaskItems.ToListAsync();
        }

        public async Task<TaskItem> GetTaskItemAsync(int id)
        {
            return await _context.TaskItems.FindAsync(id);
        }

        public async Task<TaskItem> AddTaskItemAsync(TaskItem taskItem)
        {
            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }

        public async Task<bool> UpdateTaskItemAsync(TaskItem taskItem)
        {
            _context.Entry(taskItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(taskItem.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteTaskItemAsync(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return false;
            }

            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();
            return true;
        }

        private bool TaskItemExists(int id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }
    }
}
