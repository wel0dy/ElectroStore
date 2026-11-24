using ElectroStoreApp.Data;
using ElectroStoreApp.Helpers;
using ElectroStoreApp.Models;
using Microsoft.Win32;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ElectroStoreApp
{
    public partial class ProductEditWindow : Window
    {
        private readonly Product _editingProduct;
        private readonly bool _isEditMode;
        private string _selectedLocalImageFile;
        private bool _photoCleared;

        public ProductEditWindow(Product product)
        {
            InitializeComponent();
            _editingProduct = product;
            _isEditMode = product != null;
            LoadLookups();

            if (_isEditMode)
            {
                Title = "Редактирование товара";
                DeleteButton.Visibility = Visibility.Visible;
                FillFields(product);
            }
            else
            {
                Title = "Добавление товара";
                IdTextBox.Text = "Автоматически";
                DeleteButton.Visibility = Visibility.Collapsed;
                DiscountTextBox.Text = "0";
                StockTextBox.Text = "0";
                PreviewImage.Source = ImageLoader.Load(null);
            }
        }

        private void LoadLookups()
        {
            CategoryComboBox.ItemsSource = LookupRepository.GetCategories();
            ManufacturerComboBox.ItemsSource = LookupRepository.GetManufacturers();
            SupplierComboBox.ItemsSource = LookupRepository.GetSuppliers();
            UnitComboBox.ItemsSource = LookupRepository.GetUnits();

            if (CategoryComboBox.Items.Count > 0) CategoryComboBox.SelectedIndex = 0;
            if (ManufacturerComboBox.Items.Count > 0) ManufacturerComboBox.SelectedIndex = 0;
            if (SupplierComboBox.Items.Count > 0) SupplierComboBox.SelectedIndex = 0;
            if (UnitComboBox.Items.Count > 0) UnitComboBox.SelectedIndex = 0;
        }

        private void FillFields(Product p)
        {
            IdTextBox.Text = p.ProductId.ToString();
            ArticleTextBox.Text = p.Article;
            NameTextBox.Text = p.ProductName;
            PriceTextBox.Text = p.Price.ToString("0.00", CultureInfo.InvariantCulture);
            DiscountTextBox.Text = p.Discount.ToString();
            StockTextBox.Text = p.StockQuantity.ToString();
            DescriptionTextBox.Text = p.Description;

            CategoryComboBox.SelectedItem = CategoryComboBox.Items.Cast<LookupItem>().FirstOrDefault(x => x.Id == p.CategoryId);
            ManufacturerComboBox.SelectedItem = ManufacturerComboBox.Items.Cast<LookupItem>().FirstOrDefault(x => x.Id == p.ManufacturerId);
            SupplierComboBox.SelectedItem = SupplierComboBox.Items.Cast<LookupItem>().FirstOrDefault(x => x.Id == p.SupplierId);
            UnitComboBox.SelectedItem = UnitComboBox.Items.Cast<LookupItem>().FirstOrDefault(x => x.Id == p.UnitId);
            PreviewImage.Source = ImageLoader.Load(p.PhotoPath);
        }

        private void SelectPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Выберите изображение товара",
                Filter = "Изображения (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp"
            };
            if (dialog.ShowDialog() == true)
            {
                _selectedLocalImageFile = dialog.FileName;
                _photoCleared = false;
                var bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bmp.UriSource = new Uri(_selectedLocalImageFile, UriKind.Absolute);
                bmp.EndInit();
                PreviewImage.Source = bmp;
            }
        }

        private void ClearPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedLocalImageFile = null;
            _photoCleared = true;
            PreviewImage.Source = ImageLoader.Load(null);
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string article = ArticleTextBox.Text.Trim();
            string name = NameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(article) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Заполните обязательные поля: Артикул и Наименование.", "Ошибка проверки", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (CategoryComboBox.SelectedItem == null || ManufacturerComboBox.SelectedItem == null ||
                SupplierComboBox.SelectedItem == null || UnitComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите значения из всех справочников.", "Ошибка проверки", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string priceRaw = PriceTextBox.Text.Trim().Replace(',', '.');
            if (!decimal.TryParse(priceRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price) || price < 0)
            {
                MessageBox.Show("Цена должна быть неотрицательным числом.", "Ошибка проверки", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(DiscountTextBox.Text.Trim(), out int discount) || discount < 0 || discount > 100)
            {
                MessageBox.Show("Скидка должна быть целым числом от 0 до 100.", "Ошибка проверки", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(StockTextBox.Text.Trim(), out int stock) || stock < 0)
            {
                MessageBox.Show("Количество на складе должно быть целым неотрицательным числом.", "Ошибка проверки", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int? excludeId = _isEditMode ? (int?)_editingProduct.ProductId : null;
                if (ProductRepository.ArticleExists(article, excludeId))
                {
                    MessageBox.Show("Товар с таким артикулом уже существует в базе данных.", "Дубликат артикула", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string oldPhoto = _isEditMode ? _editingProduct.PhotoPath : null;
                string finalPhoto = oldPhoto;
                if (!string.IsNullOrEmpty(_selectedLocalImageFile))
                {
                    finalPhoto = ImageService.SaveResizedProductImage(_selectedLocalImageFile);
                    if (!string.IsNullOrEmpty(oldPhoto)) ImageService.DeleteProductImage(oldPhoto);
                }
                else if (_photoCleared)
                {
                    finalPhoto = null;
                    if (!string.IsNullOrEmpty(oldPhoto)) ImageService.DeleteProductImage(oldPhoto);
                }

                Product model = new Product
                {
                    ProductId = _isEditMode ? _editingProduct.ProductId : 0,
                    Article = article,
                    ProductName = name,
                    CategoryId = ((LookupItem)CategoryComboBox.SelectedItem).Id,
                    ManufacturerId = ((LookupItem)ManufacturerComboBox.SelectedItem).Id,
                    SupplierId = ((LookupItem)SupplierComboBox.SelectedItem).Id,
                    UnitId = ((LookupItem)UnitComboBox.SelectedItem).Id,
                    Price = price,
                    Discount = discount,
                    StockQuantity = stock,
                    Description = DescriptionTextBox.Text,
                    PhotoPath = finalPhoto
                };

                if (_isEditMode) ProductRepository.Update(model);
                else ProductRepository.Add(model);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении товара: " + ex.Message, "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isEditMode) return;
            try
            {
                if (ProductRepository.IsInOrders(_editingProduct.ProductId))
                {
                    MessageBox.Show("Невозможно удалить товар, так как он присутствует в оформленных заказах.", "Удаление запрещено", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (MessageBox.Show($"Удалить товар «{_editingProduct.ProductName}» (артикул {_editingProduct.Article})?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string oldPhoto = _editingProduct.PhotoPath;
                    ProductRepository.Delete(_editingProduct.ProductId);
                    ImageService.DeleteProductImage(oldPhoto);
                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении товара: " + ex.Message, "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
