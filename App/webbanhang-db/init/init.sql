USE [master];
GO

-- 1) Tạo login ở cấp server (nếu chưa có)
IF NOT EXISTS (
    SELECT 1
    FROM sys.server_principals
    WHERE name = '${APP_USER}'
)
BEGIN
    PRINT 'Creating LOGIN ${APP_USER}';
    CREATE LOGIN [${APP_USER}] WITH PASSWORD = '${APP_USER_LOGIN_PASSWORD}';
END
GO

-- 2) Tạo database nếu chưa tồn tại
IF NOT EXISTS (
    SELECT 1
    FROM sys.databases
    WHERE name = '${DB_NAME}'
)
BEGIN
    PRINT 'Creating DATABASE ${DB_NAME}';
    CREATE DATABASE [${DB_NAME}];
END
GO

-- 3) Chuyển ngữ cảnh vào database mới
USE [${DB_NAME}];
GO

-- 4) Tạo user trong database, gán quyền db_owner (nếu chưa có)
IF NOT EXISTS (
    SELECT 1
    FROM sys.database_principals
    WHERE name = '${APP_USER}'
)
BEGIN
    PRINT 'Creating USER ${APP_USER} in DB ${DB_NAME}';
    CREATE USER [${APP_USER}] FOR LOGIN [${APP_USER}];
    ALTER ROLE [db_owner] ADD MEMBER [${APP_USER}];
END
GO

-- Table: categories
CREATE TABLE categories (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(255) NOT NULL
);

-- Table: roles
CREATE TABLE roles (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(100)
);

-- Table: users
CREATE TABLE users (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  is_active BIT NULL,
  created_at DATETIME2(6) NULL,
  date_of_birth DATETIME2(6) NULL,
  facebook_account_id BIGINT NULL,
  google_account_id BIGINT NULL,
  role_id BIGINT NULL,
  updated_at DATETIME2(6) NULL,
  phone_number VARCHAR(20) NOT NULL,
  fullname VARCHAR(100),
  address VARCHAR(200),
  password VARCHAR(200) NOT NULL,
  CONSTRAINT FK_users_roles FOREIGN KEY (role_id) REFERENCES roles(id)
);

-- Table: products
CREATE TABLE products (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  price FLOAT NOT NULL,
  category_id BIGINT NULL,
  created_at DATETIME2(6) NULL,
  updated_at DATETIME2(6) NULL,
  description VARCHAR(255) NOT NULL,
  name VARCHAR(255) NOT NULL,
  thumbnail VARCHAR(255),
  CONSTRAINT FK_products_categories FOREIGN KEY (category_id) REFERENCES categories(id)
);

-- Table: product_images
CREATE TABLE product_images (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  product_id BIGINT NULL,
  image_url VARCHAR(300),
  CONSTRAINT FK_product_images_products FOREIGN KEY (product_id) REFERENCES products(id)
);

-- Table: orders
CREATE TABLE orders (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  is_active BIT NULL,
  total_money FLOAT NULL,
  created_at DATETIME2(6) NULL,
  order_date DATETIME2(6) NULL,
  shipping_date DATETIME2(6) NULL,
  updated_at DATETIME2(6) NULL,
  user_id BIGINT NULL,
  phone_number VARCHAR(20) NOT NULL,
  email VARCHAR(100),
  fullname VARCHAR(100),
  address VARCHAR(200) NOT NULL,
  note VARCHAR(255),
  payment_method VARCHAR(255),
  payment_status VARCHAR(255),
  shipping_address VARCHAR(255),
  shipping_method VARCHAR(255),
  status VARCHAR(255),
  tracking_number VARCHAR(255),
  CONSTRAINT FK_orders_users FOREIGN KEY (user_id) REFERENCES users(id)
);

-- Table: order_details
CREATE TABLE order_details (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  number_of_products INT NOT NULL,
  price FLOAT NOT NULL,
  total_money FLOAT NOT NULL,
  order_id BIGINT NULL,
  product_id BIGINT NULL,
  color VARCHAR(255),
  CONSTRAINT FK_order_details_orders FOREIGN KEY (order_id) REFERENCES orders(id),
  CONSTRAINT FK_order_details_products FOREIGN KEY (product_id) REFERENCES products(id)
);

-- Table: social_accounts
CREATE TABLE social_accounts (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  provider_id BIGINT NOT NULL,
  user_id BIGINT NULL,
  provider VARCHAR(20) NOT NULL,
  name VARCHAR(100),
  email VARCHAR(150),
  CONSTRAINT FK_social_accounts_users FOREIGN KEY (user_id) REFERENCES users(id)
);

-- Table: tokens
CREATE TABLE tokens (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  expired BIT NOT NULL,
  revoked BIT NOT NULL,
  expiration_date DATETIME2(6) NULL,
  user_id BIGINT NULL,
  token_type VARCHAR(50),
  token VARCHAR(255),
  CONSTRAINT FK_tokens_users FOREIGN KEY (user_id) REFERENCES users(id)
);
GO


INSERT INTO `categories` (`id`, `name`) VALUES
(1, 'CÀ PHÊ'),
(2, 'TRÀ'),
(3, 'SINH TỐ'),
(4, 'NƯỚC ÉP'),
(5, 'NƯỚC NGỌT'),
(6, 'YAOURT'),
(7, 'SODA');

INSERT INTO `products` (`id`, `name`, `price`, `thumbnail`, `description`, `created_at`, `updated_at`, `category_id`) VALUES
(1, 'Cà phê đen', 19000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(2, 'Cà phê sữa', 19000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(3, 'Cà phê ý', 19000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(4, 'Bạc xỉu', 22000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(5, 'Cà phê trứng', 32000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(6, 'Cà phê dừa', 30000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(7, 'Latte', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(8, 'Capucchino', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(9, 'Ice café mocha', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(10, 'Cà phê sữa đá xay', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(11, 'Cacao Latte', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(12, 'Matcha Latte', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(13, 'Cacao sữa', 23000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(14, 'Cà phê Muối', 24000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(15, 'Emericano', 19000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(16, 'Cacao muối', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(17, 'Cà phê Cacao', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 1),
(18, 'Trà đào', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 2),
(19, 'Trà vải', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 2),
(20, 'Trà dâu', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 2),
(21, 'Trà đào cam sả', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 2),
(22, 'Lipton chanh', 20000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 2),
(23, 'Trà Mãn Cầu', 27000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 2),
(24, 'Trà gừng', 20000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 2),
(25, 'Đá me', 22000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 2),
(26, 'Trà Lạc Me', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 2),
(27, 'Trà ổi hồng', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 2),
(28, 'Trà Atiso', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 2),
(29, 'Trà tắc xí muội', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 2),
(30, 'Trà dâu tằm', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 2),
(31, 'Trà Việt quất Nha đam', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 2),
(32, 'Sinh tố Dâu', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 3),
(33, 'Sinh tố Bơ', 35000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 3),
(34, 'Sinh tố Việt Quất', 35000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 3),
(35, 'Sinh tố phúc bồn tử', 35000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 3),
(36, 'Sinh tố Xoài', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 3),
(37, 'Sinh tố Dừa', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 3),
(38, 'Sinh tố Mãng Cầu', 30000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 3),
(39, 'Ép Cóc', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 4),
(40, 'Ép Cà rốt', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 4),
(41, 'Ép Dưa hấu', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 4),
(42, 'Ép Cam', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 4),
(43, 'Ép Thơm', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 4),
(44, 'Ép Táo', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 4),
(45, 'Chanh tươi', 20000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 4),
(46, 'Ép Ổi', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 4),
(47, 'Chanh dây', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 4),
(48, 'Chanh dây Xí Muội', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 4),
(49, 'Sơ ri', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 4),
(50, 'Pepsi', 17000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 5),
(51, 'Redbull', 20000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 5),
(52, 'Olong Tea', 17000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 5),
(53, '7Up', 17000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 5),
(54, 'Dừa tươi', 18000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 5),
(55, 'Sting', 17000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 5),
(56, 'Nước suối', 12000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 5),
(57, '0 Độ', 17000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 5),
(58, 'Coca Cola', 17000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 5),
(59, 'Revive', 17000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 5),
(60, 'Yaourt Đá', 22000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 6),
(61, 'Yaourt Dâu', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 6),
(62, 'Yaourt Việt Quất', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 6),
(63, 'Yaourt Xoài', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 6),
(64, 'Yaourt Đào', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 6),
(65, 'Yaourt Trái Cây', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 6),
(66, 'Yaourt', 6000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 6),
(67, 'Soda Chanh', 20000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 7),
(68, 'Soda Blue Ocean', 22000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 7),
(69, 'Soda Dâu', 22000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 7),
(70, 'Soda Việt Quất', 22000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', NULL, NULL, 7);


INSERT INTO `roles` (`id`, `name`) VALUES
(1, 'USER'),
(2, 'ADMIN');

INSERT INTO `users` (`id`, `fullname`, `phone_number`, `address`, `password`, `created_at`, `updated_at`, `is_active`, `date_of_birth`, `facebook_account_id`, `google_account_id`, `role_id`) VALUES
(2, 'Lê Thanh Yên', '0375440580', '17A Cong Hoa', '$2a$10$PATMEvH86k45V0/z1vQS.u4qBumnzNVXjSt4B2u9uzpA5IC/Um68q', '2025-05-28 18:20:59', '2025-05-28 18:20:59', 0, '2004-09-16', 0, 0, 1),
(3, 'Lê Thanh Yên', '0375440581', '17A Cong Hoa', '$2a$10$FABWgZoRz4yZSgouTmS/8..WUZT1xlrfye6/5BJoe0U1NARHAwcWC', '2025-05-28 19:21:04', '2025-05-28 19:21:04', 0, '2004-09-16', 0, 0, 1),
(4, 'John HHHHHHHHHH', '0375440588', '123 Elm Street, Springfield', '$2a$10$fI6nLgsGpjGbilsz1vnGcub8WBxDmW0j6JjVtWaHy4XJEN9oaADai', '2025-07-05 19:00:38', '2025-07-05 19:00:38', 0, '1985-10-25', 0, 0, 1);
