using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfBowlingClub.AppData;
using WpfBowlingClub.Classes;

namespace WpfBowlingClub.Pages
{
    public partial class ManufacturersPage : Page
    {
        private DatabaseHelper db;
        private List<Manufacturers> manufacturers;

        public ManufacturersPage()
        {
            InitializeComponent();
            db = new DatabaseHelper();
            LoadManufacturers();
        }

        private void LoadManufacturers()
        {
            manufacturers = db.GetManufacturers();
            dgManufacturers.ItemsSource = manufacturers;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ManufacturerDialogPage(null);
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                LoadManufacturers();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var item = dgManufacturers.SelectedItem as Manufacturers;
            if (item == null)
            {
                MessageBox.Show("Выберите производителя", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new ManufacturerDialogPage(item);
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                LoadManufacturers();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var item = dgManufacturers.SelectedItem as Manufacturers;
            if (item == null)
            {
                MessageBox.Show("Выберите производителя", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить производителя \"{item.Name}\"?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                db.DeleteManufacturer(item.Id);
                LoadManufacturers();
            }
        }
    }
}