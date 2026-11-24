using System;
using System.Windows;
using Npgsql;
using ElectroStoreApp.Data;
using ElectroStoreApp.Models;

namespace ElectroStoreApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordInput.Password;

            if (login.Length == 0 || password.Length == 0)
            {
                MessageBox.Show(
                    "Введите логин и пароль, затем нажмите «Войти».",
                    "Не заполнены поля",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                AppUser user = UserRepository.Authenticate(login, password);
                if (user == null)
                {
                    MessageBox.Show(
                        "Неверный логин или пароль. Проверьте раскладку и регистр и повторите ввод.",
                        "Ошибка входа",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
                OpenProductList(user);
            }
            catch (NpgsqlException)
            {
                MessageBox.Show(
                    "Не удалось подключиться к базе данных. Убедитесь, что сервер PostgreSQL запущен, и проверьте строку подключения.",
                    "Ошибка подключения",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Непредвиденная ошибка: " + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            OpenProductList(null);
        }

        private void OpenProductList(AppUser user)
        {
            Session.CurrentUser = user;
            ProductListWindow productListWindow = new ProductListWindow();
            productListWindow.Show();
            Close();
        }
    }
}