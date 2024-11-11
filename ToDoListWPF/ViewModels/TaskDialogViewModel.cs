using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ToDoListWPF.ViewModels
{
    public class TaskDialogViewModel : INotifyPropertyChanged
    {
        private string _title;
        private DateTime _dueDate;
        private bool _isCompleted;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(nameof(DueDate)); }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set { _isCompleted = value; OnPropertyChanged(nameof(IsCompleted)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public TaskDialogViewModel()
        {
            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());
            DueDate = DateTime.Today;
        }

        private void Save()
        {
            // Ustawienie DialogResult na true
            Close?.Invoke(this, true);
        }

        private void Cancel()
        {
            // Ustawienie DialogResult na false
            Close?.Invoke(this, false);

        }

        // Zdarzenie do zamknięcia okna
        public event EventHandler<bool> Close;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}