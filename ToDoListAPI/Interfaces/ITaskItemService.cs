using ToDoListAPI.Models;

namespace ToDoListAPI.Interfaces
{
    public interface ITaskItemService
    {
        Task<List<TaskItem>> GetTaskItemsAsync();
        Task<TaskItem> GetTaskItemAsync(int id);
        Task<TaskItem> AddTaskItemAsync(TaskItem taskItem);
        Task<bool> UpdateTaskItemAsync(TaskItem taskItem);
        Task<bool> DeleteTaskItemAsync(int id);
    }
}
