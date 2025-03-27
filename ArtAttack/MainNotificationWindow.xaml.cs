using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ArtAttack.Domain;
using ArtAttack.Model;
using System.Collections.ObjectModel;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ArtAttack
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainNotificationWindow : Window
    {

        public ObservableCollection<Domain.Notification> Itemsource { get; } = new ObservableCollection<Notification>();

        public MainNotificationWindow()
        {
            this.InitializeComponent();
            this.AppWindow.MoveAndResize(new Windows.Graphics.RectInt32(400, 200, 1080, 600));
            //ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            SetData();
            UpdateNotificationCount();
        }
        private void SetData()
        {
            var dataAdapter = new NotificationDataAdapter("Server=IUSTINS_LAPTOP\\SQLEXPRESS;Database=Notifications;Integrated Security=True;Encrypt=False;");
            var notifications = dataAdapter.GetNotificationsForUser(1005);

            foreach (var notification in notifications)
            {
                Itemsource.Add(notification);
            }
        }
        private void UpdateNotificationCount()
        {
            var numberOfUnreadNotifications = 0;
            foreach (var notification in Itemsource)
            {
                if (!notification.getIsRead())
                {
                    numberOfUnreadNotifications += 1;
                }
            }
            notificationCountText.Text = $"You've got {numberOfUnreadNotifications} Unread Notifications";
        }

        private void notificationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (notificationList.SelectedItem is Notification selectedNotification)
            {
                var secondaryWindow = new SecondaryNotificationWindow(selectedNotification);
                secondaryWindow.Activate();

                notificationList.SelectedItem = null;
            }
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //Create main Window and activate it;
            this.Close();
        }
    }
}
