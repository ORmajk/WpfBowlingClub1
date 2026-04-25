using Microsoft.Win32;
using System;
using System.Windows;
using WpfBowlingClub.AppData;
using WpfBowlingClub.Classes;

namespace WpfBowlingClub.Pages
{
    public partial class ProductDialogPage : Window
    {
        private DatabaseHelper db;
        private Products editingProduct;
        private bool isEdit;
        private string selectedImage = null;

        public ProductDialogPage(Products product = null)
        {
            InitializeComponent();
            db = new DatabaseHelper();

            // Загружаем списки
            cmbSupplier.ItemsSource = db.GetSuppliers();
            cmbSupplier.DisplayMemberPath = "Name";
            cmbSupplier.SelectedValuePath = "Id";

            cmbManufacturer.ItemsSource = db.GetManufacturers();
            cmbManufacturer.DisplayMemberPath = "Name";
            cmbManufacturer.SelectedValuePath = "Id";

            if (product != null)
            {
                isEdit = true;
                editingProduct = product;
                txtArticle.Text = product.Article;
                txtName.Text = product.Name;
                txtPrice.Text = product.Price.ToString();
                txtQuantity.Text = product.StockQuantity.ToString();
                txtImagePath.Text = product.ImageUrl;

                if (product.SupplierId.HasValue)
                    cmbSupplier.SelectedValue = product.SupplierId.Value;
                if (product.ManufacturerId.HasValue)
                    cmbManufacturer.SelectedValue = product.ManufacturerId.Value;

                Title = "Редактирование товара";
            }
            else
            {
                Title = "Добавление товара";
            }
        }

        private void BtnLoadImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Картинки|*.jpg;*.png;*.bmp";

            if (dialog.ShowDialog() == true)
            {
                selectedImage = dialog.FileName;
                txtImagePath.Text = selectedImage;
            }
        }

        private void BtnClearImage_Click(object sender, RoutedEventArgs e)
        {
            selectedImage = null;
            txtImagePath.Text = "";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка
                if (string.IsNullOrWhiteSpace(txtArticle.Text))
                { MessageBox.Show("Введите артикул"); return; }
                if (string.IsNullOrWhiteSpace(txtName.Text))
                { MessageBox.Show("Введите название"); return; }
                if (!decimal.TryParse(txtPrice.Text, out decimal price))
                { MessageBox.Show("Введите цену"); return; }

                var product = new Products
                {
                    Article = txtArticle.Text,
                    Name = txtName.Text,
                    Price = price,
                    StockQuantity = int.Parse(txtQuantity.Text)
                };

                if (cmbSupplier.SelectedItem != null)
                    product.SupplierId = ((Suppliers)cmbSupplier.SelectedItem).Id;
                if (cmbManufacturer.SelectedItem != null)
                    product.ManufacturerId = ((Manufacturers)cmbManufacturer.SelectedItem).Id;

                if (isEdit)
                {
                    product.Id = editingProduct.Id;

                    // Сохраняем картинку
                    if (!string.IsNullOrEmpty(selectedImage))
                    {
                        string oldImage = editingProduct.ImageUrl;
                        if (!string.IsNullOrEmpty(oldImage))
                            ImageHelper.DeleteImage(oldImage);
                        product.ImageUrl = ImageHelper.SaveImage(selectedImage, product.Id);
                    }
                    else
                    {
                        product.ImageUrl = editingProduct.ImageUrl;
                    }

                    db.UpdateProduct(product);
                    MessageBox.Show("Товар обновлен");
                }
                else
                {
                    db.AddProduct(product);

                    // Сохраняем картинку после получения ID
                    if (!string.IsNullOrEmpty(selectedImage))
                    {
                        product.ImageUrl = ImageHelper.SaveImage(selectedImage, product.Id);
                        db.UpdateProduct(product);
                    }
                    MessageBox.Show("Товар добавлен");
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}