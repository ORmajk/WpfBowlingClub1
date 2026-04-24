using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfBowlingClub.AppData;
using WpfBowlingClub.Classes;

namespace WpfBowlingClub.Pages
{
    public partial class SuppliersPage : Page
    {
        private DatabaseHelper db;
        private List<Suppliers> suppliers;

        public SuppliersPage()
        {
            InitializeComponent();
            db = new DatabaseHelper();
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            suppliers = db.GetSuppliers();
            dgSuppliers.ItemsSource = suppliers;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SupplierDialogPage(null);
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                LoadSuppliers();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var supplier = dgSuppliers.SelectedItem as Suppliers;
            if (supplier == null)
            {
                MessageBox.Show("Выберите поставщика для редактирования", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new SupplierDialogPage(supplier);
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                LoadSuppliers();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var supplier = dgSuppliers.SelectedItem as Suppliers;
            if (supplier == null)
            {
                MessageBox.Show("Выберите поставщика для удаления", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить поставщика \"{supplier.Name}\"?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                db.DeleteSupplier(supplier.Id);
                LoadSuppliers();
            }
        }
    }
}