using System;
using System.Collections.Generic;
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
            users = db.GetUsers();
            dgUsers.ItemsSource = users;
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
                        db.SetUserStatus(user.Id, newStatus);
                        LoadUsers();
                        MessageBox.Show($"Статус пользователя {user.LastName} {user.FirstName} изменен",
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