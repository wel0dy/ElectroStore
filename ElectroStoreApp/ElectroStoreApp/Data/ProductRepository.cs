using System.Collections.Generic;
using System.Diagnostics;
using Npgsql;
using ElectroStoreApp.Models;

namespace ElectroStoreApp.Data
{
    public static class ProductRepository
    {
        public static long LastQueryMilliseconds { get; private set; }

        private const string SelectAllSql = @"
            SELECT p.product_id, p.article, p.product_name,
                   p.category_id, c.category_name, p.description,
                   p.manufacturer_id, m.manufacturer_name,
                   p.supplier_id, s.supplier_name,
                   p.price, p.unit_id, u.unit_name,
                   p.stock_quantity, p.discount, p.photo_path
            FROM product p
            JOIN category c ON c.category_id = p.category_id
            JOIN manufacturer m ON m.manufacturer_id = p.manufacturer_id
            JOIN supplier s ON s.supplier_id = p.supplier_id
            JOIN unit u ON u.unit_id = p.unit_id
            ORDER BY p.product_id";

        public static List<Product> GetAll()
        {
            Stopwatch sw = Stopwatch.StartNew();
            List<Product> products = new List<Product>();
            using (NpgsqlConnection connection = Database.CreateConnection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(SelectAllSql, connection))
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ProductId = reader.GetInt32(0),
                            Article = reader.GetString(1),
                            ProductName = reader.GetString(2),
                            CategoryId = reader.GetInt32(3),
                            CategoryName = reader.GetString(4),
                            Description = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            ManufacturerId = reader.GetInt32(6),
                            ManufacturerName = reader.GetString(7),
                            SupplierId = reader.GetInt32(8),
                            SupplierName = reader.GetString(9),
                            Price = reader.GetDecimal(10),
                            UnitId = reader.GetInt32(11),
                            UnitName = reader.GetString(12),
                            StockQuantity = reader.GetInt32(13),
                            Discount = reader.GetInt16(14),
                            PhotoPath = reader.IsDBNull(15) ? null : reader.GetString(15)
                        });
                    }
                }
            }
            sw.Stop();
            LastQueryMilliseconds = sw.ElapsedMilliseconds;
            return products;
        }

        public static bool ArticleExists(string article, int? excludeProductId = null)
        {
            const string sql = @"
                SELECT COUNT(*) FROM product
                WHERE LOWER(article) = LOWER(@article)
                  AND (@excludeId IS NULL OR product_id <> @excludeId)";

            using (NpgsqlConnection connection = Database.CreateConnection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("article", NpgsqlTypes.NpgsqlDbType.Varchar, article.Trim());
                    command.Parameters.AddWithValue("excludeId", NpgsqlTypes.NpgsqlDbType.Integer,
                        (object)excludeProductId ?? System.DBNull.Value);
                    long count = (long)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        public static void Add(Product product)
        {
            const string sql = @"
                INSERT INTO product
                    (article, product_name, category_id, manufacturer_id, supplier_id,
                     unit_id, price, discount, stock_quantity, description, photo_path)
                VALUES
                    (@article, @name, @categoryId, @manufacturerId, @supplierId,
                     @unitId, @price, @discount, @stock, @description, @photoPath)";

            using (NpgsqlConnection connection = Database.CreateConnection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    FillParameters(command, product);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Update(Product product)
        {
            const string sql = @"
                UPDATE product
                SET article = @article,
                    product_name = @name,
                    category_id = @categoryId,
                    manufacturer_id = @manufacturerId,
                    supplier_id = @supplierId,
                    unit_id = @unitId,
                    price = @price,
                    discount = @discount,
                    stock_quantity = @stock,
                    description = @description,
                    photo_path = @photoPath
                WHERE product_id = @id";

            using (NpgsqlConnection connection = Database.CreateConnection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("id", product.ProductId);
                    FillParameters(command, product);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static bool IsInOrders(int productId)
        {
            const string sql = "SELECT COUNT(*) FROM order_item WHERE product_id = @id";
            using (NpgsqlConnection connection = Database.CreateConnection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("id", productId);
                    long count = (long)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public static void Delete(int productId)
        {
            const string sql = "DELETE FROM product WHERE product_id = @id";
            using (NpgsqlConnection connection = Database.CreateConnection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("id", productId);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void FillParameters(NpgsqlCommand command, Product p)
        {
            command.Parameters.AddWithValue("article", NpgsqlTypes.NpgsqlDbType.Varchar, p.Article.Trim());
            command.Parameters.AddWithValue("name", NpgsqlTypes.NpgsqlDbType.Varchar, p.ProductName.Trim());
            command.Parameters.AddWithValue("categoryId", NpgsqlTypes.NpgsqlDbType.Integer, p.CategoryId);
            command.Parameters.AddWithValue("manufacturerId", NpgsqlTypes.NpgsqlDbType.Integer, p.ManufacturerId);
            command.Parameters.AddWithValue("supplierId", NpgsqlTypes.NpgsqlDbType.Integer, p.SupplierId);
            command.Parameters.AddWithValue("unitId", NpgsqlTypes.NpgsqlDbType.Integer, p.UnitId);
            command.Parameters.AddWithValue("price", NpgsqlTypes.NpgsqlDbType.Numeric, p.Price);
            command.Parameters.AddWithValue("discount", NpgsqlTypes.NpgsqlDbType.Smallint, (short)p.Discount);
            command.Parameters.AddWithValue("stock", NpgsqlTypes.NpgsqlDbType.Integer, p.StockQuantity);
            command.Parameters.AddWithValue("description", NpgsqlTypes.NpgsqlDbType.Text,
                string.IsNullOrWhiteSpace(p.Description) ? (object)System.DBNull.Value : p.Description.Trim());
            command.Parameters.AddWithValue("photoPath", NpgsqlTypes.NpgsqlDbType.Varchar,
                string.IsNullOrWhiteSpace(p.PhotoPath) ? (object)System.DBNull.Value : p.PhotoPath);
        }
    }
}
