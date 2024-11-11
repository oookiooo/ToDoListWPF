using System.Windows;
using ToDoListWPF.ViewModels;

namespace ToDoListWPF.Views
{
    /// <summary>
    /// Logika interakcji dla klasy TaskDialog.xaml
    /// </summary>
    public partial class TaskDialog : Window
    {
        public TaskDialog()
        {
            InitializeComponent();
            this.DataContextChanged += TaskDialog_DataContextChanged;
        }
        private void TaskDialog_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is TaskDialogViewModel vm)
            {
                vm.Close += (s, result) =>
                {
                    this.DialogResult = result;
                    this.Close();
                };
            }
        }
    }
}
