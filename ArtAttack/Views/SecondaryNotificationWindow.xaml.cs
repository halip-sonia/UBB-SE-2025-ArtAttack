using ArtAttack;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ArtAttack.Domain;

namespace ArtAttack
{
    public sealed partial class SecondaryNotificationWindow : Window
    {
        public Notification SelectedNotification { get; }

        public SecondaryNotificationWindow(Notification notification)
        {
            this.InitializeComponent();
            this.AppWindow.MoveAndResize(new Windows.Graphics.RectInt32(400, 200, 1080, 600));
            this.SelectedNotification = notification;
            this.Populate();
        }

        private void Populate()
        {
            selectedNotificationTitle.Text = this.SelectedNotification.Title;
            selectedNotificationContent.Text = this.SelectedNotification.Content;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainNotificationWindow();
            mainWindow.Activate();
            this.Close();
        }
    }
}