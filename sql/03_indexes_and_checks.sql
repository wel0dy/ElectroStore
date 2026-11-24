-- Модуль 3: Индексы для ускорения соединений (JOIN), фильтрации и сортировки
CREATE INDEX IF NOT EXISTS idx_product_category_id ON product(category_id);
CREATE INDEX IF NOT EXISTS idx_product_manufacturer_id ON product(manufacturer_id);
CREATE INDEX IF NOT EXISTS idx_product_supplier_id ON product(supplier_id);
CREATE INDEX IF NOT EXISTS idx_product_stock_quantity ON product(stock_quantity);
CREATE INDEX IF NOT EXISTS idx_order_item_product_id ON order_item(product_id);
CREATE INDEX IF NOT EXISTS idx_customer_order_user_id ON customer_order(user_id);

-- Контрольная проверка количества записей во всех 11 таблицах
SELECT 'user_role' AS table_name, count(*) FROM user_role
UNION ALL SELECT 'app_user', count(*) FROM app_user
UNION ALL SELECT 'category', count(*) FROM category
UNION ALL SELECT 'manufacturer', count(*) FROM manufacturer
UNION ALL SELECT 'supplier', count(*) FROM supplier
UNION ALL SELECT 'unit', count(*) FROM unit
UNION ALL SELECT 'product', count(*) FROM product
UNION ALL SELECT 'pickup_point', count(*) FROM pickup_point
UNION ALL SELECT 'order_status', count(*) FROM order_status
UNION ALL SELECT 'customer_order', count(*) FROM customer_order
UNION ALL SELECT 'order_item', count(*) FROM order_item;

-- Анализ плана выполнения оптимизированного запроса выборки товаров
EXPLAIN ANALYZE
SELECT p.article, p.product_name, c.category_name, m.manufacturer_name, s.supplier_name, p.price, p.stock_quantity
FROM product p
JOIN category c ON c.category_id = p.category_id
JOIN manufacturer m ON m.manufacturer_id = p.manufacturer_id
JOIN supplier s ON s.supplier_id = p.supplier_id
WHERE s.supplier_name = 'Ситилинк'
ORDER BY p.stock_quantity ASC;
