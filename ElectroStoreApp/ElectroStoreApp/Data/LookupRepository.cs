using System.Collections.Generic;
using Npgsql;
using ElectroStoreApp.Models;

namespace ElectroStoreApp.Data
{
    public static class LookupRepository
    {
        public static List<LookupItem> GetCategories() =>
            LoadItems("SELECT category_id, category_name FROM category ORDER BY category_name");

        public static List<LookupItem> GetManufacturers() =>
            LoadItems("SELECT manufacturer_id, manufacturer_name FROM manufacturer ORDER BY manufacturer_name");

        public static List<LookupItem> GetSuppliers() =>
            LoadItems("SELECT supplier_id, supplier_name FROM supplier ORDER BY supplier_name");

        public static List<LookupItem> GetUnits() =>
            LoadItems("SELECT unit_id, unit_name FROM unit ORDER BY unit_name");

        public static List<LookupItem> GetPickupPoints() =>
            LoadItems("SELECT pickup_point_id, address FROM pickup_point ORDER BY pickup_point_id");

        public static List<LookupItem> GetOrderStatuses() =>
            LoadItems("SELECT order_status_id, status_name FROM order_status ORDER BY order_status_id");

        public static List<LookupItem> GetUsers() =>
            LoadItems("SELECT user_id, TRIM(last_name || ' ' || first_name || ' ' || COALESCE(patronymic, '')) FROM app_user ORDER BY last_name");

        private static List<LookupItem> LoadItems(string sql)
        {
            List<LookupItem> items = new List<LookupItem>();
            using (NpgsqlConnection connection = Database.CreateConnection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new LookupItem
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
            return items;
        }
    }
}

