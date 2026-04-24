using System.Windows;
using WpfBowlingClub.Classes;

namespace WpfBowlingClub.Pages
{
    public partial class MainPage : Window
    {
        public MainPage()
        {
            InitializeComponent();

            if (LoginPage.CurrentUser?.Roles?.Name == "Администратор")
            {
                btnUsers.Visibility = Visibility.Visible;
            }

            MainFrame.Navigate(new ProductsPage());
        }

        private void BtnProducts_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProductsPage());
        }

        private void BtnOrders_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OrdersPage());
        }

        private void BtnSuppliers_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SuppliersPage());
        }

        private void BtnManufacturers_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ManufacturersPage());
        }

        private void BtnUsers_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UsersPage());
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginPage();
            login.Show();
            this.Close();
        }
    }
}