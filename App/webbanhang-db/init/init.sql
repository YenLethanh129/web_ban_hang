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
