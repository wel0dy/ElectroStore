using System;
using System.Collections.Generic;
using System.Windows;
using Npgsql;
using ElectroStoreApp.Data;

namespace ElectroStoreApp
{
    public partial class OrdersWindow : Window
    {
        private class OrderRow
        {
            public int OrderId { get; set; }
            public string ArticlesSummary { get; set; }
            public string StatusName { get; set; }
            public string PickupAddress { get; set; }
            public DateTime OrderDate { get; set; }
            public DateTime DeliveryDate { get; set; }
        }

        public OrdersWindow()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            const string sql = @"
                SELECT o.order_id,
                       STRING_AGG(p.article || ', ' || oi.quantity, ', ' ORDER BY p.article) AS items,
                       s.status_name,
                       pp.address,
                       o.order_date,
                       o.delivery_date
                FROM customer_order o
                JOIN order_status s ON s.order_status_id = o.order_status_id
                JOIN pickup_point pp ON pp.pickup_point_id = o.pickup_point_id
                LEFT JOIN order_item oi ON oi.order_id = o.order_id
                LEFT JOIN product p ON p.product_id = oi.product_id
                GROUP BY o.order_id, s.status_name, pp.address, o.order_date, o.delivery_date
                ORDER BY o.order_id";

            try
            {
                List<OrderRow> rows = new List<OrderRow>();
                using (NpgsqlConnection connection = Database.CreateConnection())
                {
                    connection.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
                    using (NpgsqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            rows.Add(new OrderRow
                            {
                                OrderId = r.GetInt32(0),
                                ArticlesSummary = r.IsDBNull(1) ? "" : r.GetString(1),
                                StatusName = r.GetString(2),
                                PickupAddress = r.GetString(3),
                                OrderDate = r.GetDateTime(4),
                                DeliveryDate = r.GetDateTime(5)
                            });
                        }
                    }
                }
                OrdersDataGrid.ItemsSource = rows;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки заказов: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
    }
}
