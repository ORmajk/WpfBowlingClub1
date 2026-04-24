using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfBowlingClub.AppData;
using WpfBowlingClub.Classes;

namespace WpfBowlingClub.Pages
{
    public partial class ProductsPage : Page
    {
        private DatabaseHelper db;
        private List<Products> products;

        public ProductsPage()
        {
            InitializeComponent();
            db = new DatabaseHelper();
            LoadProducts();
        }

        private void LoadProducts()
        {
            products = db.GetProducts();
            dgProducts.ItemsSource = products;
        }

        private void FilterProducts()
        {
            var search = txtSearch.Text.ToLower();
            if (string.IsNullOrEmpty(search))
                dgProducts.ItemsSource = products;
            else
                dgProducts.ItemsSource = products.Where(p => p.Name.ToLower().Contains(search) ||
                                                             p.Article.ToLower().Contains(search)).ToList();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e) => FilterProducts();

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ProductDialogPage(null);
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true) LoadProducts();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var product = dgProducts.SelectedItem as Products;
            if (product == null)
            {
                MessageBox.Show("Выберите товар", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var dialog = new ProductDialogPage(product);
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true) LoadProducts();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var product = dgProducts.SelectedItem as Products;
            if (product == null) return;

            if (MessageBox.Show($"Удалить \"{product.Name}\"?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                db.DeleteProduct(product.Id);
                LoadProducts();
            }
        }

        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            var product = dgProducts.SelectedItem as Products;
            if (product == null) return;

            var dialog = new PriceHistoryPage(product.Id, product.Name);
            dialog.Owner = Window.GetWindow(this);
            dialog.ShowDialog();
        }
    }
}