using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Npgsql;
using ElectroStoreApp.Data;
using ElectroStoreApp.Models;

namespace ElectroStoreApp
{
    public partial class ProductListWindow : Window
    {
        private List<Product> _allProducts = new List<Product>();
        private bool _isEditWindowOpen;

        public ProductListWindow()
        {
            InitializeComponent();
            ShowUserInfo();
            ApplyRolePermissions();
            LoadSuppliersFilter();
            LoadProducts();
        }

        private void ShowUserInfo()
        {
            if (Session.CurrentUser == null)
            {
                UserNameTextBlock.Text = "Гость";
                RoleTextBlock.Text = "Просмотр товаров";
                return;
            }
            UserNameTextBlock.Text = Session.CurrentUser.FullName;
            RoleTextBlock.Text = Session.CurrentUser.RoleName;
        }

        private void ApplyRolePermissions()
        {
            UserRole role = Session.CurrentRole;
            bool isAdministrator = role == UserRole.Administrator;
            bool isManager = role == UserRole.Manager;

            AddProductButton.Visibility = isAdministrator ? Visibility.Visible : Visibility.Collapsed;
            AdminHintTextBlock.Visibility = isAdministrator ? Visibility.Visible : Visibility.Collapsed;
            OrdersButton.Visibility = (isAdministrator || isManager) ? Visibility.Visible : Visibility.Collapsed;
            FilterPanel.Visibility = (isAdministrator || isManager) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LoadSuppliersFilter()
        {
            try
            {
                List<string> suppliers = new List<string> { "Все поставщики" };
                suppliers.AddRange(LookupRepository.GetSuppliers().Select(s => s.Name));
                SupplierFilterComboBox.ItemsSource = suppliers;
                SupplierFilterComboBox.SelectedIndex = 0;
            }
            catch
            {
                SupplierFilterComboBox.ItemsSource = new List<string> { "Все поставщики" };
                SupplierFilterComboBox.SelectedIndex = 0;
            }
        }

        private void LoadProducts()
        {
            try
            {
                _allProducts = ProductRepository.GetAll();
                ApplyFilters();
            }
            catch (NpgsqlException)
            {
                MessageBox.Show(
                    "Не удалось загрузить список товаров из базы данных. Проверьте подключение к серверу PostgreSQL.",
                    "Ошибка загрузки данных",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Непредвиденная ошибка при загрузке товаров: " + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ApplyFilters()
        {
            if (_allProducts == null || ProductsListBox == null) return;

            IEnumerable<Product> query = _allProducts;

            string search = (SearchTextBox?.Text ?? string.Empty).Trim();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    ContainsIgnoreCase(p.Article, search) ||
                    ContainsIgnoreCase(p.ProductName, search) ||
                    ContainsIgnoreCase(p.CategoryName, search) ||
                    ContainsIgnoreCase(p.ManufacturerName, search) ||
                    ContainsIgnoreCase(p.SupplierName, search) ||
                    ContainsIgnoreCase(p.Description, search));
            }

            string selectedSupplier = SupplierFilterComboBox?.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedSupplier) && selectedSupplier != "Все поставщики")
            {
                query = query.Where(p => string.Equals(p.SupplierName, selectedSupplier, StringComparison.OrdinalIgnoreCase));
            }

            int sortIndex = SortComboBox?.SelectedIndex ?? 0;
            if (sortIndex == 1)
            {
                query = query.OrderBy(p => p.StockQuantity);
            }
            else if (sortIndex == 2)
            {
                query = query.OrderByDescending(p => p.StockQuantity);
            }

            List<Product> filtered = query.ToList();
            ProductsListBox.ItemsSource = filtered;
            StatusTextBlock.Text = $"Показано товаров: {filtered.Count} из {_allProducts.Count} (время выборки из БД: {ProductRepository.LastQueryMilliseconds} мс)";
        }

        private static bool ContainsIgnoreCase(string source, string value)
        {
            return !string.IsNullOrEmpty(source) &&
                   source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilters();
        private void SupplierFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilters();
        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilters();

        private void OpenProductEditor(Product product)
        {
            if (Session.CurrentRole != UserRole.Administrator) return;

            if (_isEditWindowOpen)
            {
                MessageBox.Show(
                    "Окно добавления или редактирования товара уже открыто. Закройте его перед открытием нового.",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            _isEditWindowOpen = true;
            try
            {
                ProductEditWindow editor = new ProductEditWindow(product) { Owner = this };
                if (editor.ShowDialog() == true)
                {
                    LoadProducts();
                }
            }
            finally
            {
                _isEditWindowOpen = false;
            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            OpenProductEditor(null);
        }

        private void ProductsListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Session.CurrentRole != UserRole.Administrator) return;
            Product selected = ProductsListBox.SelectedItem as Product;
            if (selected != null)
            {
                OpenProductEditor(selected);
            }
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            OrdersWindow ordersWindow = new OrdersWindow { Owner = this };
            ordersWindow.ShowDialog();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Session.CurrentUser = null;
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}
