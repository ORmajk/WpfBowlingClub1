using System;
using System.Windows;
using WpfBowlingClub.AppData;
using WpfBowlingClub.Classes;

namespace WpfBowlingClub.Pages
{
    public partial class SupplierDialogPage : Window
    {
        private DatabaseHelper db;
        private Suppliers editingSupplier;
        private bool isEdit;

        public SupplierDialogPage( Suppliers supplier = null)
        {
            InitializeComponent();
            db = new DatabaseHelper();

            if (supplier != null)
            {
                isEdit = true;
                editingSupplier = supplier;
                txtName.Text = supplier.Name;
                txtContact.Text = supplier.ContactInfo;
                Title = "Редактирование поставщика";
            }
            else
            {
                Title = "Добавление поставщика";
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Введите название", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var supplier = new Suppliers
                {
                    Name = txtName.Text.Trim(),
                    ContactInfo = txtContact.Text.Trim()
                };

                if (isEdit)
                {
                    supplier.Id = editingSupplier.Id;
                    db.UpdateSupplier(supplier);
                    MessageBox.Show("Поставщик обновлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    db.AddSupplier(supplier);
                    MessageBox.Show("Поставщик добавлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}