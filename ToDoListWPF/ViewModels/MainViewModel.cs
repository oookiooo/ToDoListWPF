
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToDoListWPF.ApiServices;
using ToDoListWPF.Models;
using ToDoListWPF.Views;

namespace ToDoListWPF.ViewModels
{
    public interface INotificationManager
    {
        void Show(object content, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null, Action onClose = null);
    }
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly INotificationManager _manager;
        private DateTime _selectedDate;
        private TaskItem _selectedTask;

        public ObservableCollection<TaskItem> TaskItems { get; set; }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                    LoadTasksForSelectedDate();
                }
            }
        }

        public TaskItem SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (_selectedTask != value)
                {
                    _selectedTask = value;
                    OnPropertyChanged(nameof(SelectedTask));
                }
            }
        }

        public ICommand AddTaskCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }

        public MainViewModel()
        {
            _apiService = new ApiService();
            TaskItems = new ObservableCollection<TaskItem>();
            SelectedDate = DateTime.Today;
            AddTaskCommand = new RelayCommand(async _ => await AddTask());
            EditTaskCommand = new RelayCommand(async _ => await EditTask(), _ => SelectedTask != null);
            DeleteTaskCommand = new RelayCommand(async _ => await DeleteTask(), _ => SelectedTask != null);

            LoadTasksForSelectedDate();
            StartNotificationTimer();
        }

        public async void LoadTasksForSelectedDate()
        {
            var tasks = await _apiService.GetTaskItems();
            TaskItems.Clear();
            foreach (var task in tasks)
            {
                if (task.DueDate.Date == SelectedDate.Date)
                {
                    TaskItems.Add(task);
                }
            }
        }

        private async Task AddTask()
        {
            // Tworzenie nowego ViewModel dla dialogu
            var taskDialogVM = new TaskDialogViewModel
            {
                DueDate = SelectedDate
            };

            // Tworzenie i konfigurowanie okna dialogowego
            var taskDialog = new TaskDialog
            {
                DataContext = taskDialogVM,
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                WindowStyle = WindowStyle.ToolWindow,
                ShowInTaskbar = false,
                SizeToContent = SizeToContent.WidthAndHeight
            };

            // Subskrybowanie zdarzeń zamknięcia dialogu
            bool? result = taskDialog.ShowDialog();

            if (result == true)
            {
                var newTask = new TaskItem
                {
                    Title = taskDialogVM.Title,
                    DueDate = taskDialogVM.DueDate,
                    IsCompleted = taskDialogVM.IsCompleted
                };

                await _apiService.AddTaskItem(newTask);
                LoadTasksForSelectedDate();
            }
        }

        private async Task EditTask()
        {
            if (SelectedTask == null)
                return;

            // Tworzenie ViewModelu z istniejącymi danymi
            var taskDialogVM = new TaskDialogViewModel
            {
                Title = SelectedTask.Title,
                DueDate = SelectedTask.DueDate,
                IsCompleted = SelectedTask.IsCompleted
            };

            // Tworzenie i konfigurowanie okna dialogowego
            var taskDialog = new TaskDialog
            {
                DataContext = taskDialogVM,
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                WindowStyle = WindowStyle.ToolWindow,
                ShowInTaskbar = false,
                SizeToContent = SizeToContent.WidthAndHeight
            };

            // Subskrybowanie zdarzeń zamknięcia dialogu
            bool? result = taskDialog.ShowDialog();

            if (result == true)
            {
                SelectedTask.Title = taskDialogVM.Title;
                SelectedTask.DueDate = taskDialogVM.DueDate;
                SelectedTask.IsCompleted = taskDialogVM.IsCompleted;

                await _apiService.UpdateTaskItem(SelectedTask);
                LoadTasksForSelectedDate();
            }
        }

        private async Task DeleteTask()
        {
            if (SelectedTask == null)
                return;

            var confirmation = MessageBox.Show($"Czy na pewno chcesz usunąć zadanie: {SelectedTask.Title}?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmation == MessageBoxResult.Yes)
            {
                await _apiService.DeleteTaskItem(SelectedTask.Id);
                LoadTasksForSelectedDate();
            }
        }

        // Funkcjonalność powiadomień
        private void StartNotificationTimer()
        {
            var timer = new System.Timers.Timer(10000); // Sprawdzaj co minutę
            timer.Elapsed += async (sender, e) => await CheckForUpcomingTasks();
            timer.Start();
        }

        private async Task CheckForUpcomingTasks()
        {
            var tasks = await _apiService.GetTaskItems();
            var now = DateTime.Now;

            foreach (var task in tasks)
            {
                if (!task.IsCompleted && (task.DueDate - now).TotalMinutes <= 30 && (task.DueDate - now).TotalMinutes > 1)
                {
                    NotificationService.ShowNotification("Nadchodzące zadanie", task.Title);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
