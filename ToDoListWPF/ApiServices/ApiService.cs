using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ToDoListWPF.Models;

namespace ToDoListWPF.ApiServices
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private HttpClient httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7020/") };
        }

        public ApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<TaskItem>> GetTaskItems()
        {
            return await _httpClient.GetFromJsonAsync<List<TaskItem>>("api/TaskItems");
        }

        public async Task<TaskItem> GetTaskItem(int id)
        {
            return await _httpClient.GetFromJsonAsync<TaskItem>($"api/TaskItems/{id}");
        }

        public async Task AddTaskItem(TaskItem taskItem)
        {
            await _httpClient.PostAsJsonAsync("api/TaskItems", taskItem);
        }

        public async Task UpdateTaskItem(TaskItem taskItem)
        {
            await _httpClient.PutAsJsonAsync($"api/TaskItems/{taskItem.Id}", taskItem);
        }

        public async Task DeleteTaskItem(int id)
        {
            await _httpClient.DeleteAsync($"api/TaskItems/{id}");
        }
    }
}
