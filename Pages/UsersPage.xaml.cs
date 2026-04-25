using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfBowlingClub.Classes;

namespace WpfBowlingClub.Pages
{
    public partial class UsersPage : Page
    {
        private DatabaseHelper db;
        private List<AppData.Users> users;

        public UsersPage()
        {
            InitializeComponent();
            db = new DatabaseHelper();
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                users = db.GetUsers();
                dgUsers.ItemsSource = users;
                UpdateTotalCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateTotalCount()
        {
            int activeCount = users?.Count(u => u.Status == 1) ?? 0;
            int blockedCount = users?.Count(u => u.Status == 0) ?? 0;
            lblTotal.Text = $"Всего: {users?.Count ?? 0} | Активных: {activeCount} | Заблокировано: {blockedCount}";
        }

        private void FilterUsers()
        {
            var search = txtSearch.Text.ToLower();
            if (string.IsNullOrEmpty(search))
            {
                dgUsers.ItemsSource = users;
            }
            else
            {
                var filtered = users.Where(u => u.LastName.ToLower().Contains(search) ||
                                                u.FirstName.ToLower().Contains(search) ||
                                                u.Email.ToLower().Contains(search) ||
                                                u.Phone.Contains(search)).ToList();
                dgUsers.ItemsSource = filtered;
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterUsers();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new UserDialogPage(null);
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                LoadUsers();
                MessageBox.Show("Пользователь добавлен", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var user = dgUsers.SelectedItem as AppData.Users;
            if (user == null)
            {
                MessageBox.Show("Выберите пользователя для редактирования", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new UserDialogPage(user);
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                LoadUsers();
                MessageBox.Show("Пользователь обновлен", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var user = dgUsers.SelectedItem as AppData.Users;
            if (user == null)
            {
                MessageBox.Show("Выберите пользователя для удаления", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка - нельзя удалить самого себя
            if (LoginPage.CurrentUser != null && user.Id == LoginPage.CurrentUser.Id)
            {
                MessageBox.Show("Вы не можете удалить самого себя", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить пользователя \"{user.LastName} {user.FirstName}\"?\n\nЭто действие нельзя отменить!",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                db.DeleteUser(user.Id);
                LoadUsers();
                MessageBox.Show("Пользователь удален", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void UserStatusChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var combo = sender as ComboBox;
                var user = combo?.Tag as AppData.Users;
                var selectedItem = combo?.SelectedItem as ComboBoxItem;

                if (user != null && selectedItem != null)
                {
                    int newStatus = int.Parse(selectedItem.Tag.ToString());
                    if (newStatus != user.Status)
                    {
                        // Проверка - нельзя заблокировать самого себя
                        if (LoginPage.CurrentUser != null && user.Id == LoginPage.CurrentUser.Id)
                        {
                            MessageBox.Show("Вы не можете изменить статус самого себя", "Предупреждение",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                            LoadUsers(); // Сбросить статус обратно
                            return;
                        }

                        db.SetUserStatus(user.Id, newStatus);
                        LoadUsers();
                        string statusText = newStatus == 1 ? "активирован" : "заблокирован";
                        MessageBox.Show($"Пользователь {user.LastName} {user.FirstName} {statusText}",
                            "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}