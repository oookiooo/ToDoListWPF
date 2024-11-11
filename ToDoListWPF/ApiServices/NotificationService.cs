using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Net.Http;
using System.Xml; // Keep this if needed elsewhere
using Windows.Data.Xml.Dom; // Keep this
using Windows.UI.Notifications;
using System.Windows;
using ToDoListWPF.ViewModels;
using Notifications.Wpf; // For MessageBox

namespace ToDoListWPF.ApiServices
{
    public static class NotificationService
    {
        private static TaskbarIcon _notifyIcon;


        public static void Initialize(TaskbarIcon notifyIcon)
        {
            _notifyIcon = notifyIcon;
        }

        public static void ShowNotification(string title, string message)
        {

            var notificationManager = new Notifications.Wpf.NotificationManager();

            notificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = NotificationType.Warning
            });



        }
    }
}
