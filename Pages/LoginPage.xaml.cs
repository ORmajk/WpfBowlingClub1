using System;
using System.Windows;
using WpfBowlingClub.Classes;

namespace WpfBowlingClub.Pages
{
    public partial class LoginPage : Window
    {
        private DatabaseHelper db;
        public static AppData.Users CurrentUser { get; set; }

        public LoginPage()
        {
            InitializeComponent();
            db = new DatabaseHelper();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = txtLogin.Text.Trim();
                string pass = txtPassword.Password;

                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pass))
                {
                    ShowError("Введите логин и пароль");
                    return;
                }

                var user = db.Login(login, pass);

                if (user == null)
                {
                    ShowError("Неверный логин или пароль");
                    return;
                }

                if (user.Roles.Name != "Администратор" && user.Roles.Name != "Менеджер")
                {
                    ShowError("У вас нет доступа к этой программе");
                    return;
                }

                CurrentUser = user;

                MainPage main = new MainPage();
                main.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                ShowError("Ошибка: " + ex.Message);
            }
        }

        private void ShowError(string msg)
        {
            txtError.Text = msg;
            txtError.Visibility = Visibility.Visible;
        }
    }
}