using System;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using WpfBowlingClub.AppData;
using WpfBowlingClub.Classes;

namespace WpfBowlingClub.Pages
{
    public partial class ProductDialogPage : Window
    {
        private DatabaseHelper db;
        private Products editingProduct;
        private bool isEdit;

        public ProductDialogPage(Products product = null)
        {
            InitializeComponent();
            db = new DatabaseHelper();

            cmbSupplier.ItemsSource = db.GetSuppliers();
            cmbManufacturer.ItemsSource = db.GetManufacturers();

            if (product != null)
            {
                isEdit = true;
                editingProduct = product;
                txtArticle.Text = product.Article;
                txtName.Text = product.Name;
                txtPrice.Text = product.Price.ToString();
                txtQuantity.Text = product.StockQuantity.ToString();
                if (product.SupplierId.HasValue) cmbSupplier.SelectedValue = product.SupplierId.Value;
                if (product.ManufacturerId.HasValue) cmbManufacturer.SelectedValue = product.ManufacturerId.Value;
                Title = "Редактирование товара";
            }
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(txtArticle.Text)) { ShowError("Введите артикул"); return false; }
            if (string.IsNullOrWhiteSpace(txtName.Text)) { ShowError("Введите название"); return false; }
            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            { ShowError("Введите корректную цену"); return false; }
            return true;
        }

        private void ShowError(string msg) => MessageBox.Show(msg, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;

            var product = new Products
            {
                Article = txtArticle.Text.Trim(),
                Name = txtName.Text.Trim(),
                Price = decimal.Parse(txtPrice.Text),
                StockQuantity = int.Parse(txtQuantity.Text),
                UnitOfMeasure = "шт."
            };

            if (cmbSupplier.SelectedItem != null) product.SupplierId = ((Suppliers)cmbSupplier.SelectedItem).Id;
            if (cmbManufacturer.SelectedItem != null) product.ManufacturerId = ((Manufacturers)cmbManufacturer.SelectedItem).Id;

            if (isEdit) { product.Id = editingProduct.Id; db.UpdateProduct(product); }
            else db.AddProduct(product);

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}