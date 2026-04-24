using System;
using System.Windows;
using System.Xml.Linq;
using WpfBowlingClub.AppData;
using WpfBowlingClub.Classes;

namespace WpfBowlingClub.Pages
{
    public partial class ManufacturerDialogPage : Window
    {
        private DatabaseHelper db;
        private Manufacturers editingItem;
        private bool isEdit;

        public ManufacturerDialogPage(Manufacturers item = null)
        {
            InitializeComponent();
            db = new DatabaseHelper();

            if (item != null)
            {
                isEdit = true;
                editingItem = item;
                txtName.Text = item.Name;
                txtCountry.Text = item.Country;
                Title = "Редактирование производителя";
            }
            else
            {
                Title = "Добавление производителя";
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

                var item = new Manufacturers
                {
                    Name = txtName.Text.Trim(),
                    Country = txtCountry.Text.Trim()
                };

                if (isEdit)
                {
                    item.Id = editingItem.Id;
                    db.UpdateManufacturer(item);
                    MessageBox.Show("Производитель обновлен", "Успех");
                }
                else
                {
                    db.AddManufacturer(item);
                    MessageBox.Show("Производитель добавлен", "Успех");
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}