using System;
using System.Windows.Media;
using ElectroStoreApp.Helpers;

namespace ElectroStoreApp.Models
{
    public class Product
    {
        private const int BigDiscountThreshold = 15;
        private const int LowStockThreshold = 3;
        private ImageSource _photo;

        public int ProductId { get; set; }
        public string Article { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; } = string.Empty;
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public int Discount { get; set; }
        public string PhotoPath { get; set; }

        public bool HasDiscount => Discount > 0;
        public bool IsBigDiscount => Discount > BigDiscountThreshold;
        public bool IsOutOfStock => StockQuantity <= 0;
        public bool IsLowStock => StockQuantity > 0 && StockQuantity <= LowStockThreshold;
        public decimal FinalPrice => Math.Round(Price * (100 - Discount) / 100m, 2);

        public ImageSource Photo
        {
            get
            {
                if (_photo == null)
                {
                    _photo = ImageLoader.Load(PhotoPath);
                }
                return _photo;
            }
        }
    }
}
