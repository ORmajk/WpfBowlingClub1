using System;
using System.Windows;
using WpfBowlingClub.Classes;

namespace WpfBowlingClub.Pages
{
    public partial class PriceHistoryPage : Window
    {
        private DatabaseHelper db;
        private int productId;

        public PriceHistoryPage(int productId, string productName)
        {
            InitializeComponent();
            db = new DatabaseHelper();
            this.productId = productId;

            lblProductName.Text = $"История цен: {productName}";
            LoadHistory();
        }

        private void LoadHistory()
        {
            try
            {
                var history = db.GetPriceHistory(productId);
                dgHistory.ItemsSource = history;
                
                if (history.Count == 0)
                {
                    MessageBox.Show("История изменения цен отсутствует", "Информация", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}