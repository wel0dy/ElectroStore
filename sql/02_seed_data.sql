-- База данных: Магазин электроники (electro_shop)
-- Модуль 1: Наполнение справочников, пунктов выдачи и пользователей

INSERT INTO user_role (role_name) VALUES 
('Администратор'), ('Менеджер'), ('Авторизированный клиент');

INSERT INTO unit (unit_name) VALUES ('шт.');

INSERT INTO supplier (supplier_name) VALUES ('Ситилинк'), ('Технопарк');

INSERT INTO manufacturer (manufacturer_name) VALUES 
('Samsung'), ('Xiaomi'), ('ASUS'), ('realme'), ('HUAWEI'), ('Logitech'), 
('Baseus'), ('MSI'), ('JBL'), ('TP-Link'), ('Kingston'), ('TCL'), ('WD'), 
('HONOR'), ('Palit'), ('Microsoft'), ('Fifine'), ('Bosch'), ('70mai'), 
('Кронштейн'), ('Razer'), ('Philips'), ('Duracell'), ('Deerma');

INSERT INTO category (category_name) VALUES 
('Мобильные телефоны'), ('Аудиодевайсы'), ('Компьютеры'), ('Смарт-гаджеты'),
('Компьютерная периферия'), ('Аксессуары'), ('Сетевое оборудование'),
('Компьютерные комплектующие'), ('Телевизоры и ТВ'), ('Планшеты'),
('Игровые консоли'), ('Мелкая бытовая техника'), ('Автоэлектроника'),
('Красота и здоровье');

INSERT INTO order_status (status_name) VALUES ('Завершен'), ('Новый');

INSERT INTO pickup_point (address) VALUES
('420151, г. Лесной, ул. Вишневая, 32'), ('125061, г. Лесной, ул. Подгорная, 8'),
('630370, г. Лесной, ул. Шоссейная, 24'), ('400562, г. Лесной, ул. Зеленая, 32'),
('614510, г. Лесной, ул. Маяковского, 47'), ('410542, г. Лесной, ул. Светлая, 46'),
('620839, г. Лесной, ул. Цветочная, 8'), ('443890, г. Лесной, ул. Коммунистическая, 1'),
('603379, г. Лесной, ул. Спортивная, 46'), ('603721, г. Лесной, ул. Гоголя, 41'),
('410172, г. Лесной, ул. Северная, 13'), ('614611, г. Лесной, ул. Молодежная, 50'),
('454311, г. Лесной, ул. Новая, 19'), ('660007, г. Лесной, ул. Октябрьская, 19'),
('603036, г. Лесной, ул. Садовая, 4'), ('394060, г. Лесной, ул. Фрунзе, 43'),
('410661, г. Лесной, ул. Школьная, 50'), ('625590, г. Лесной, ул. Коммунистическая, 20'),
('625683, г. Лесной, ул. 8 Марта'), ('450983, г. Лесной, ул. Комсомольская, 26'),
('394782, г. Лесной, ул. Чехова, 3'), ('603002, г. Лесной, ул. Дзержинского, 28'),
('450558, г. Лесной, ул. Набережная, 30'), ('344288, г. Лесной, ул. Чехова, 1'),
('614164, г. Лесной, ул. Степная, 30'), ('394242, г. Лесной, ул. Коммунистическая, 43'),
('660540, г. Лесной, ул. Солнечная, 25'), ('125837, г. Лесной, ул. Шоссейная, 40'),
('125703, г. Лесной, ул. Партизанская, 49'), ('625283, г. Лесной, ул. Победы, 46'),
('614753, г. Лесной, ул. Полевая, 35'), ('426030, г. Лесной, ул. Маяковского, 44'),
('450375, г. Лесной, ул. Клубная, 44'), ('625560, г. Лесной, ул. Некрасова, 12'),
('630201, г. Лесной, ул. Комсомольская, 17'), ('190949, г. Лесной, ул. Мичурина, 26');

INSERT INTO app_user (user_role_id, last_name, first_name, patronymic, login, password) VALUES
((SELECT user_role_id FROM user_role WHERE role_name='Администратор'), 'Никифорова', 'Весения', 'Николаевна', '94d5ous@gmail.com', 'uzWC67'),
((SELECT user_role_id FROM user_role WHERE role_name='Администратор'), 'Сазонов', 'Руслан', 'Германович', 'uth4iz@mail.com', '2L6KZG'),
((SELECT user_role_id FROM user_role WHERE role_name='Администратор'), 'Одинцов', 'Серафим', 'Артёмович', 'yzls62@outlook.com', 'JlFRCZ'),
((SELECT user_role_id FROM user_role WHERE role_name='Менеджер'), 'Степанов', 'Михаил', 'Артёмович', '1diph5e@tutanota.com', '8ntwUp'),
((SELECT user_role_id FROM user_role WHERE role_name='Менеджер'), 'Ворсин', 'Петр', 'Евгеньевич', 'tjde7c@yahoo.com', 'YOyhfR'),
((SELECT user_role_id FROM user_role WHERE role_name='Менеджер'), 'Старикова', 'Елена', 'Павловна', 'wpmrc3do@tutanota.com', 'RSbvHv'),
((SELECT user_role_id FROM user_role WHERE role_name='Авторизированный клиент'), 'Михайлюк', 'Анна', 'Вячеславовна', '5d4zbu@tutanota.com', 'rwVDh9'),
((SELECT user_role_id FROM user_role WHERE role_name='Авторизированный клиент'), 'Ситдикова', 'Елена', 'Анатольевна', 'ptec8ym@yahoo.com', 'LdNyos'),
((SELECT user_role_id FROM user_role WHERE role_name='Авторизированный клиент'), 'Ворсин', 'Петр', 'Евгеньевич', '1qz4kw@mail.com', 'gynQMT'),
((SELECT user_role_id FROM user_role WHERE role_name='Авторизированный клиент'), 'Старикова', 'Елена', 'Павловна', '4np6se@mail.com', 'AtnDjr');
-- Модуль 1: Наполнение товаров (30 шт.), заказов (10 шт.) и позиций заказов (20 шт.)

INSERT INTO product (article, product_name, category_id, manufacturer_id, supplier_id, unit_id, price, discount, stock_quantity, description, photo_path)
SELECT v.art, v.pname, c.category_id, m.manufacturer_id, s.supplier_id, 1, v.price, v.disc, v.qty, v.descr, v.photo
FROM (VALUES
('A112T4','Смартфон',49990,'Ситилинк','Samsung','Мобильные телефоны',3,6,'Смартфон Samsung Galaxy A55 8/256GB, цвет черный','1.jpg'),
('F635R4','Наушники',3244,'Технопарк','Xiaomi','Аудиодевайсы',2,13,'Беспроводные наушники Xiaomi Redmi Buds 5, цвет белый','2.jpg'),
('H782T5','Ноутбук',64499,'Ситилинк','ASUS','Компьютеры',4,5,'Ноутбук ASUS Vivobook 15 X1504ZA-BQ574, Intel Core i3, 8GB, 512GB SSD','3.jpg'),
('G783F5','Смартфон',15900,'Ситилинк','realme','Мобильные телефоны',2,8,'Смартфон realme C67 8/256GB, цвет зеленый','4.jpg'),
('J384T6','Смарт-часы',13800,'Технопарк','HUAWEI','Смарт-гаджеты',2,16,'Смарт-часы HUAWEI Watch FIT 3, ремешок из фторэластомера, черный','5.jpg'),
('D572U8','Клавиатура',4100,'Технопарк','Logitech','Компьютерная периферия',3,6,'Беспроводная клавиатура Logitech Pebble Keys 2 K380s, графит',NULL),
('F572H7','Мышь',2700,'Ситилинк','Logitech','Компьютерная периферия',2,14,'Беспроводная мышь Logitech M650 L, леворукая, черный',NULL),
('D329H3','Повербанк',1890,'Технопарк','Baseus','Аксессуары',4,4,'Внешний аккумулятор Baseus Bipow 20W 10000mAh, черный',NULL),
('B320R5','Монитор',14300,'Ситилинк','MSI','Компьютеры',2,6,'Монитор MSI PRO MP2412 23.8", черный',NULL),
('G432E4','Колонки',2800,'Ситилинк','JBL','Аудиодевайсы',3,15,'Портативная колонка JBL GO 4, цвет синий',NULL),
('S213E3','Роутер',2156,'Технопарк','TP-Link','Сетевое оборудование',3,6,'Wi-Fi роутер TP-Link Archer C54 AC1200',NULL),
('E482R4','Накопитель',1800,'Ситилинк','Kingston','Компьютерные комплектующие',2,14,'Флешка Kingston DataTraveler Exodia M 128GB USB 3.2',NULL),
('S634B5','Телевизор',35500,'Технопарк','TCL','Телевизоры и ТВ',3,0,'Телевизор TCL 43P745 43" 4K Ultra HD, черный',NULL),
('K345R4','Жесткий диск',5100,'Технопарк','WD','Компьютерные комплектующие',2,3,'Внешний жесткий диск WD Elements Portable 1TB',NULL),
('O754F4','Планшет',25400,'Технопарк','HONOR','Планшеты',4,18,'Планшет HONOR Pad X9 4/128GB Wi-Fi, серый космос',NULL),
('G531F4','Видеокарта',46600,'Ситилинк','Palit','Компьютерные комплектующие',12,9,'Видеокарта Palit NVIDIA GeForce RTX 4060 Dual 8GB',NULL),
('J542F5','Кабель',500,'Ситилинк','Baseus','Аксессуары',13,0,'Кабель Baseus USB Type-C - USB Type-C 100W 1м',NULL),
('B431R5','Геймпад',5700,'Технопарк','Microsoft','Игровые консоли',2,5,'Геймпад Microsoft Xbox Wireless Controller, черный',NULL),
('P764G4','Микрофон',6800,'Ситилинк','Fifine','Аудиодевайсы',15,15,'Игровой микрофон Fifine AmpliGame A8 с RGB подсветкой',NULL),
('C436G5','Электрочайник',4200,'Ситилинк','Xiaomi','Мелкая бытовая техника',15,9,'Умный электрочайник Xiaomi Smart Kettle Pro, белый',NULL),
('F427R5','Кофемолка',3800,'Технопарк','Bosch','Мелкая бытовая техника',15,11,'Электрическая кофемолка Bosch TSM6A013B, черный',NULL),
('N457T5','Весы',2600,'Ситилинк','Xiaomi','Смарт-гаджеты',3,13,'Умные напольные весы Xiaomi Body Composition Scale 2',NULL),
('D364R4','Видеорегистратор',8400,'Ситилинк','70mai','Автоэлектроника',16,5,'Видеорегистратор 70mai Dash Cam M300, черный',NULL),
('S326R5','Кронштейн',2900,'Технопарк','Кронштейн','Аксессуары',17,15,'Наклонно-поворотный кронштейн для телевизора 32"-55"',NULL),
('L754R4','Зарядное устр.',1700,'Ситилинк','Xiaomi','Аксессуары',2,7,'Сетевое зарядное устройство Xiaomi Mi 33W Wall Charger',NULL),
('M542T5','Игровая мышь',2800,'Технопарк','Razer','Компьютерная периферия',18,3,'Игровая мышь Razer DeathAdder Essential, черный',NULL),
('D268G5','Эл. бритва',4399,'Технопарк','Philips','Красота и здоровье',3,12,'Электробритва Philips Series 1000 S1133/41',NULL),
('T324F5','Триммер',3699,'Ситилинк','Philips','Красота и здоровье',2,5,'Триммер для бороды и усов Philips BT3222/14',NULL),
('K358H6','Батарейки',599,'Ситилинк','Duracell','Аксессуары',20,2,'Алкалиновые батарейки Duracell Basic AA, 4 шт.',NULL),
('H535R5','Увлажнитель',2300,'Технопарк','Deerma','Мелкая бытовая техника',2,7,'Увлажнитель воздуха Deerma DEM-F325, белый',NULL)
) AS v(art, pname, price, sup, man, cat, disc, qty, descr, photo)
JOIN supplier s ON s.supplier_name = v.sup
JOIN manufacturer m ON m.manufacturer_name = v.man
JOIN category c ON c.category_name = v.cat;

INSERT INTO customer_order (order_date, delivery_date, pickup_point_id, user_id, receive_code, order_status_id) VALUES
('2025-02-27', '2025-04-20', 1,  (SELECT user_id FROM app_user WHERE last_name='Степанов' LIMIT 1), 901, 1),
('2022-09-28', '2025-04-21', 11, (SELECT user_id FROM app_user WHERE last_name='Никифорова' LIMIT 1), 902, 1),
('2025-03-21', '2025-04-22', 2,  (SELECT user_id FROM app_user WHERE last_name='Сазонов' LIMIT 1), 903, 1),
('2025-02-20', '2025-04-23', 11, (SELECT user_id FROM app_user WHERE last_name='Одинцов' LIMIT 1), 904, 1),
('2025-03-17', '2025-04-24', 2,  (SELECT user_id FROM app_user WHERE last_name='Степанов' LIMIT 1), 905, 1),
('2025-03-01', '2025-04-25', 15, (SELECT user_id FROM app_user WHERE last_name='Никифорова' LIMIT 1), 906, 1),
('2025-02-28', '2025-04-26', 3,  (SELECT user_id FROM app_user WHERE last_name='Сазонов' LIMIT 1), 907, 1),
('2025-03-31', '2025-04-27', 19, (SELECT user_id FROM app_user WHERE last_name='Одинцов' LIMIT 1), 908, 2),
('2025-04-02', '2025-04-28', 5,  (SELECT user_id FROM app_user WHERE last_name='Степанов' LIMIT 1), 909, 2),
('2025-04-03', '2025-04-29', 19, (SELECT user_id FROM app_user WHERE last_name='Степанов' LIMIT 1), 910, 2);

INSERT INTO order_item (order_id, product_id, quantity)
SELECT v.oid, p.product_id, v.qty
FROM (VALUES
(1,'A112T4',2),(1,'F635R4',2),
(2,'H782T5',1),(2,'G783F5',1),
(3,'J384T6',10),(3,'D572U8',10),
(4,'F572H7',5),(4,'D329H3',4),
(5,'A112T4',2),(5,'F635R4',2),
(6,'H782T5',1),(6,'G783F5',1),
(7,'J384T6',10),(7,'D572U8',10),
(8,'F572H7',5),(8,'D329H3',4),
(9,'B320R5',5),(9,'G432E4',1),
(10,'S213E3',5),(10,'E482R4',5)
) AS v(oid, art, qty)
JOIN product p ON p.article = v.art;
