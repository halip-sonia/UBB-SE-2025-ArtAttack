using Microsoft.UI.Xaml;
using ArtAttack.Domain;
using Microsoft.Data.SqlClient;

namespace ArtAttack
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Use your actual connection string
            string connectionString = "Data Source=KORRAL775\\SQLEXPRESS;Initial Catalog=PurchaseDataBase;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate = True; Application Intent = ReadWrite; Multi Subnet FailOver = False";
            //"DataSource =KORRAL775\SQLEXPRESS; Initial Catalog = PurchaseDataBase; Integrated Security = True; Connect Timeout = 30; Encript = true; Trust Server Certificate = true; Application Intent = ReadWrite; Multi Subnet FailOver = False;"
            // Test with product ID 6 (available) or 7 (unavailable)
            int productId = 6;

            var window = new BorrowProductWindow(connectionString, productId);
            window.Activate();
        }
    }
}