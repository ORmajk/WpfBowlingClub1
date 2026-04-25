using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfBowlingClub.Classes;

namespace WpfBowlingClub.Pages
{
    public partial class UserDialogPage : Window
    {
        private DatabaseHelper db;
        private AppData.Users editingUser;
        private bool isEdit;

        public UserDialogPage(AppData.Users user = null)
        {
            InitializeComponent();
            db = new DatabaseHelper();

            LoadRoles();

            if (user != null)
            {
                isEdit = true;
                editingUser = user;
                LoadUserData();
                Title = "Редактирование пользователя";
            }
            else
            {
                Title = "Добавление пользователя";
            }
        }

        private void LoadRoles()
        {
            var roles = db.GetRoles();
            cmbRole.ItemsSource = roles;

            // Выбираем роль "Клиент" по умолчанию
            var defaultRole = roles.FirstOrDefault(r => r.Name == "Клиент");
            if (defaultRole != null)
                cmbRole.SelectedValue = defaultRole.Id;
        }

        private void LoadUserData()
        {
            txtLastName.Text = editingUser.LastName;
            txtFirstName.Text = editingUser.FirstName;
            txtMiddleName.Text = editingUser.MiddleName;
            txtPhone.Text = editingUser.Phone;
            txtEmail.Text = editingUser.Email;
            txtDiscount.Text = editingUser.Discount.ToString();

            if (editingUser.RoleId > 0)
                cmbRole.SelectedValue = editingUser.RoleId;

            // Выбираем статус
            foreach (ComboBoxItem item in cmbStatus.Items)
            {
                if (int.Parse(item.Tag.ToString()) == editingUser.Status)
                {
                    cmbStatus.SelectedItem = item;
                    break;
                }
            }

            // Пароль не загружаем (безопасность)
            txtPassword.Password = "";
            txtPassword.IsEnabled = false;
            txtPassword.ToolTip = "Оставьте пустым, чтобы не менять пароль";
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Введите фамилию", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Введите имя", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Введите телефон", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Введите Email", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!isEdit && string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Введите пароль", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(txtDiscount.Text, out int discount) || discount < 0 || discount > 100)
            {
                MessageBox.Show("Скидка должна быть от 0 до 100", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateFields()) return;

                var user = new AppData.Users
                {
                    LastName = txtLastName.Text.Trim(),
                    FirstName = txtFirstName.Text.Trim(),
                    MiddleName = txtMiddleName.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    RoleId = (int)cmbRole.SelectedValue,
                    Status = int.Parse(((ComboBoxItem)cmbStatus.SelectedItem).Tag.ToString()),
                    Discount = int.Parse(txtDiscount.Text),
                    CreatedAt = DateTime.Now
                };

                if (isEdit)
                {
                    user.Id = editingUser.Id;

                    // Если пароль введен - обновляем его
                    if (!string.IsNullOrWhiteSpace(txtPassword.Password))
                    {
                        user.Password = txtPassword.Password;
                    }
                    else
                    {
                        user.Password = editingUser.Password; // Оставляем старый пароль
                    }

                    db.UpdateUser(user);
                }
                else
                {
                    user.Password = txtPassword.Password;
                    db.AddUser(user);
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