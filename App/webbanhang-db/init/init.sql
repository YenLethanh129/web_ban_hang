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

-- 1) Tables with no foreign keys first
CREATE TABLE categories (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(255) NOT NULL
);

CREATE TABLE roles (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(100)
);

CREATE TABLE taxes (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(100) NOT NULL,          -- ví dụ: VAT 10%, VAT 5%, Miễn thuế
  tax_rate DECIMAL(5,2) NOT NULL,       -- % thuế (10.00 = 10%)
  description VARCHAR(255)
);

CREATE TABLE payment_methods (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50) NOT NULL
);

CREATE TABLE payment_statuses (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50) NOT NULL
);

CREATE TABLE order_statuses (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50) NOT NULL
);

CREATE TABLE delivery_statuses (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50) NOT NULL
);

CREATE TABLE suppliers (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  phone VARCHAR(20),
  email VARCHAR(100),
  address VARCHAR(255),
  note VARCHAR(255)
);

CREATE TABLE branches (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  address VARCHAR(255),
  phone VARCHAR(20),
  manager VARCHAR(100)
);

-- 2) Tables that reference the above tables
CREATE TABLE employees (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  branch_id BIGINT NOT NULL,        
  full_name NVARCHAR(100) NOT NULL,
  phone VARCHAR(20),
  email VARCHAR(100),
  position VARCHAR(50), 
  hire_date DATE NOT NULL,
  resign_date DATE NULL,           
  status VARCHAR(20) DEFAULT 'ACTIVE', 
  created_at DATETIME2(6) DEFAULT SYSDATETIME(),
  CONSTRAINT FK_employees_branch FOREIGN KEY (branch_id) REFERENCES branches(id)
);

CREATE TABLE users (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  employee_id BIGINT NULL, -- nếu user là nhân viên
  is_active BIT DEFAULT 1,
  created_at DATETIME2(6) DEFAULT SYSDATETIME(),
  updated_at DATETIME2(6) ,
  date_of_birth DATE NULL,
  facebook_account_id BIGINT NULL,
  google_account_id BIGINT NULL,
  role_id BIGINT NOT NULL,
  phone_number VARCHAR(20) NOT NULL,
  fullname VARCHAR(100),
  address VARCHAR(200),
  password VARCHAR(200) NOT NULL,
  CONSTRAINT FK_users_roles FOREIGN KEY (role_id) REFERENCES roles(id),
  CONSTRAINT FK_users_employees FOREIGN KEY (employee_id) REFERENCES employees(id)
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

-- Table: customers 
-- Khách hàng có thể có tài khoản hoặc không
-- Nếu có tài khoản thì liên kết với bảng users
CREATE TABLE customers (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  user_id BIGINT NULL,  
  fullname VARCHAR(100) NOT NULL,
  phone_number VARCHAR(20),
  email VARCHAR(100),
  address VARCHAR(200),
  created_at DATETIME2(6) DEFAULT SYSDATETIME(),
  CONSTRAINT FK_customers_users FOREIGN KEY (user_id) REFERENCES users(id)
);

-- Table: products
CREATE TABLE products (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  price DECIMAL(18,2) NOT NULL,
  category_id BIGINT NULL,
  tax_id BIGINT NULL,
  created_at DATETIME2(6) NULL,
  updated_at DATETIME2(6) NULL,
  description VARCHAR(255) NOT NULL,
  name VARCHAR(255) NOT NULL,
  thumbnail VARCHAR(255),
  CONSTRAINT FK_products_categories FOREIGN KEY (category_id) REFERENCES categories(id),
  CONSTRAINT FK_products_taxes FOREIGN KEY (tax_id) REFERENCES taxes(id)
);

-- Table: product_images
CREATE TABLE product_images (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  product_id BIGINT NULL,
  image_url VARCHAR(300),
  CONSTRAINT FK_product_images_products FOREIGN KEY (product_id) REFERENCES products(id)
);

-- Table: shipping_providers
-- Quản lý các nhà cung cấp dịch vụ vận chuyển
-- Ví dụ: Be, Grab, Shopee...
CREATE TABLE shipping_providers (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(100) NOT NULL,
  contact_info VARCHAR(200) NULL,
  api_endpoint VARCHAR(200) NULL,    -- để cho tích hợp API
  created_at DATETIME2(6) DEFAULT SYSDATETIME()
);

-- Table: orders
CREATE TABLE orders (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  customer_id BIGINT NOT NULL,
  branch_id BIGINT NULL,
  total_money DECIMAL(18,2) NULL,
  created_at DATETIME2(6) DEFAULT SYSDATETIME(),
  order_date DATETIME2(6) NULL,
  shipping_date DATETIME2(6) NULL,
  updated_at DATETIME2(6) NULL,
  note VARCHAR(255),
  payment_method_id BIGINT NULL,
  payment_status_id BIGINT NULL,
  status_id BIGINT NULL,
  shipping_address VARCHAR(255),
  shipping_method VARCHAR(50),
  CONSTRAINT FK_orders_customers FOREIGN KEY (customer_id) REFERENCES customers(id),
  CONSTRAINT FK_orders_branches FOREIGN KEY (branch_id) REFERENCES branches(id)
);

CREATE TABLE order_delivery_tracking (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  order_id BIGINT NOT NULL,
  tracking_number VARCHAR(100) NOT NULL,
  status_id BIGINT NOT NULL,      
  location VARCHAR(255) NULL,       
  estimated_delivery DATETIME2(6) NULL,
  updated_at DATETIME2(6) DEFAULT SYSDATETIME(),
  delivery_person_id BIGINT NULL,        -- shipper nội bộ
  shipping_provider_id BIGINT NULL,      -- đơn vị vận chuyển ngoài
  CONSTRAINT FK_tracking_orders FOREIGN KEY (order_id) REFERENCES orders(id),
  CONSTRAINT FK_tracking_employees FOREIGN KEY (delivery_person_id) REFERENCES employees(id),
  CONSTRAINT FK_tracking_providers FOREIGN KEY (shipping_provider_id) REFERENCES shipping_providers(id),
  CONSTRAINT FK_tracking_status FOREIGN KEY (status_id) REFERENCES delivery_statuses(id)
);

-- Table: order_details
CREATE TABLE order_details (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  number_of_products INT NOT NULL,
  price DECIMAL(18,2) NOT NULL,
  tax_price DECIMAL(18,2) NOT NULL,
  total_money DECIMAL(18,2) NOT NULL,
  order_id BIGINT NULL,
  product_id BIGINT NULL,
  color VARCHAR(255),
  CONSTRAINT FK_order_details_orders FOREIGN KEY (order_id) REFERENCES orders(id),
  CONSTRAINT FK_order_details_products FOREIGN KEY (product_id) REFERENCES products(id)
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

-- Inventory data 
CREATE TABLE ingredients (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  unit VARCHAR(50) NOT NULL,
  description VARCHAR(255),
  tax_id BIGINT NULL,
  created_at DATETIME2(6) DEFAULT GETDATE(),
  updated_at DATETIME2(6) DEFAULT GETDATE(),
  CONSTRAINT FK_ingredients_taxes FOREIGN KEY (tax_id) REFERENCES taxes(id)
);

CREATE TABLE ingredient_warehouse (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  ingredient_id BIGINT NOT NULL,
  quantity DECIMAL(18,2) NOT NULL DEFAULT 0,
  updated_at DATETIME2(6) DEFAULT GETDATE(),
  CONSTRAINT FK_ingredient_warehouse FOREIGN KEY (ingredient_id) REFERENCES ingredients(id)
);

CREATE TABLE supplier_ingredient_prices (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  supplier_id BIGINT NOT NULL,
  ingredient_id BIGINT NOT NULL,
  price DECIMAL(18,2) NOT NULL,         
  unit VARCHAR(50) NOT NULL,          
  effective_date DATETIME2(6) DEFAULT GETDATE(),
  expired_date DATETIME2(6) NULL,   
  CONSTRAINT FK_sip_supplier FOREIGN KEY (supplier_id) REFERENCES suppliers(id),
  CONSTRAINT FK_sip_ingredient FOREIGN KEY (ingredient_id) REFERENCES ingredients(id)
);

CREATE TABLE ingredient_purchase_orders (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  supplier_id BIGINT NULL,
  order_date DATETIME2(6) DEFAULT GETDATE(),
  total_money DECIMAL(18,2) NULL,
  note VARCHAR(255),
  CONSTRAINT FK_purchase_order_supplier FOREIGN KEY (supplier_id) REFERENCES suppliers(id)
);

CREATE TABLE ingredient_purchase_order_details (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  purchase_order_id BIGINT NOT NULL,
  ingredient_id BIGINT NOT NULL,
  quantity DECIMAL(18,2) NOT NULL,
  unit_price DECIMAL(18,2) NOT NULL,
  tax_price DECIMAL(18,2) NOT NULL,
  total_price DECIMAL(18,2) NOT NULL,
  CONSTRAINT FK_ipod_purchase_orders FOREIGN KEY (purchase_order_id) REFERENCES ingredient_purchase_orders(id),
  CONSTRAINT FK_ipod_ingredients FOREIGN KEY (ingredient_id) REFERENCES ingredients(id)
);

CREATE TABLE branch_ingredient_inventory (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  branch_id BIGINT NOT NULL,
  ingredient_id BIGINT NOT NULL,
  quantity DECIMAL(18,2) NOT NULL DEFAULT 0,
  updated_at DATETIME2(6) DEFAULT GETDATE(),
  CONSTRAINT FK_bii_branch FOREIGN KEY (branch_id) REFERENCES branches(id),
  CONSTRAINT FK_bii_ingredient FOREIGN KEY (ingredient_id) REFERENCES ingredients(id)
);

CREATE TABLE ingredient_transfers (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  ingredient_id BIGINT NOT NULL,
  branch_id BIGINT NOT NULL,
  quantity DECIMAL(18,2) NOT NULL,
  transfer_date DATETIME2(6) DEFAULT GETDATE(),
  note VARCHAR(255),
  CONSTRAINT FK_transfer_ingredient FOREIGN KEY (ingredient_id) REFERENCES ingredients(id),
  CONSTRAINT FK_transfer_branch FOREIGN KEY (branch_id) REFERENCES branches(id)
);

CREATE TABLE product_recipes (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  product_id BIGINT NOT NULL,
  ingredient_id BIGINT NOT NULL,
  quantity DECIMAL(18,2) NOT NULL,
  CONSTRAINT FK_recipe_product FOREIGN KEY (product_id) REFERENCES products(id),
  CONSTRAINT FK_recipe_ingredient FOREIGN KEY (ingredient_id) REFERENCES ingredients(id)
);

-- Revenue data
CREATE TABLE sales_summary (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  branch_id BIGINT NULL,                   -- nếu muốn phân tách theo chi nhánh
  period_type VARCHAR(20) NOT NULL,        -- 'WEEK' hoặc 'MONTH'
  period_value VARCHAR(20) NOT NULL,       -- ví dụ: '2025-W33' hoặc '2025-08'
  total_orders INT NOT NULL DEFAULT 0,     -- số đơn hàng
  total_products INT NOT NULL DEFAULT 0,   -- số lượng sản phẩm bán ra
  revenue_before_tax DECIMAL(18,2) NOT NULL,       -- doanh thu chưa thuế
  revenue_after_tax DECIMAL(18,2) NOT NULL,        -- doanh thu sau thuế
  tax_amount DECIMAL(18,2) NOT NULL,               -- tổng thuế đầu ra
  created_at DATETIME2(6) DEFAULT GETDATE()
);

CREATE TABLE expenses_summary (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  branch_id BIGINT NULL,                     -- nếu nguyên liệu nhập riêng cho chi nhánh
  period_type VARCHAR(20) NOT NULL,          -- 'WEEK' hoặc 'MONTH'
  period_value VARCHAR(20) NOT NULL,         -- ví dụ: '2025-W33' hoặc '2025-08'
  total_purchase_orders INT NOT NULL DEFAULT 0,
  total_ingredients INT NOT NULL DEFAULT 0,  -- tổng số lượng nguyên liệu nhập
  expense_before_tax DECIMAL(18,2) NOT NULL,         -- chi phí chưa thuế
  expense_after_tax DECIMAL(18,2) NOT NULL,          -- chi phí sau thuế
  tax_amount DECIMAL(18,2) NOT NULL,                 -- thuế đầu vào
  created_at DATETIME2(6) DEFAULT GETDATE()
);

CREATE TABLE profit_summary (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  branch_id BIGINT NULL,
  period_type VARCHAR(20) NOT NULL,        -- 'WEEK' hoặc 'MONTH'
  period_value VARCHAR(20) NOT NULL,       -- ví dụ: '2025-W33' hoặc '2025-08'
  revenue_before_tax DECIMAL(18,2) NOT NULL,
  revenue_after_tax DECIMAL(18,2) NOT NULL,
  expense_before_tax DECIMAL(18,2) NOT NULL,
  expense_after_tax DECIMAL(18,2) NOT NULL,
  output_tax DECIMAL(18,2) NOT NULL,
  input_tax DECIMAL(18,2) NOT NULL,
  vat_to_pay DECIMAL(18,2) NOT NULL,               -- thuế phải nộp = output_tax - input_tax
  profit_before_tax DECIMAL(18,2) NOT NULL,        -- doanh thu - chi phí (chưa thuế)
  profit_after_tax DECIMAL(18,2) NOT NULL,         -- lợi nhuận sau thuế
  created_at DATETIME2(6) DEFAULT GETDATE()
);

-- Branch expenses
-- Chi phí định kỳ của từng chi nhánh (ví dụ: tiền thuê mặt bằng, tiền điện, nước, bảo trì)
-- Có thể lặp lại hàng tháng, hàng quý, hàng năm
CREATE TABLE branch_expenses (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  branch_id BIGINT NOT NULL,
  expense_type VARCHAR(100) NOT NULL, -- VD: RENT, ELECTRICITY, WATER, MAINTENANCE
  amount DECIMAL(18,2) NOT NULL,
  start_date DATE NOT NULL,
  end_date DATE NULL, -- null = chi phí lặp định kỳ chưa hết hạn
  payment_cycle VARCHAR(50) DEFAULT 'MONTHLY', -- Hàng tháng, quý, năm
  note VARCHAR(255),
  created_at DATETIME2(6) DEFAULT GETDATE(),
  CONSTRAINT FK_branch_expenses_branches FOREIGN KEY (branch_id) REFERENCES branches(id)
);

-- Employee salaries
CREATE TABLE employee_salaries (
  id BIGINT IDENTITY(1,1) PRIMARY KEY,
  employee_id BIGINT NOT NULL,
  base_salary DECIMAL(18,2) NOT NULL,       -- lương cơ bản (theo tháng/giờ)
  salary_type VARCHAR(20) DEFAULT 'MONTHLY', -- MONTHLY/HOURLY
  allowance DECIMAL(18,2) DEFAULT 0,        -- phụ cấp
  bonus DECIMAL(18,2) DEFAULT 0,            -- thưởng
  penalty DECIMAL(18,2) DEFAULT 0,          -- phạt
  tax_rate DECIMAL(18,2) DEFAULT 0.1,       -- % thuế thu nhập (có thể tùy theo quy định)
  effective_date DATE NOT NULL,     -- ngày bắt đầu áp dụng
  created_at DATETIME2(6) DEFAULT GETDATE(),
  CONSTRAINT FK_salaries_employee FOREIGN KEY (employee_id) REFERENCES employees(id)
);

-- Employee shifts
-- Quản lý ca làm việc của nhân viên
CREATE TABLE employee_shifts (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    employee_id BIGINT NOT NULL,
    shift_date DATE NOT NULL,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    status VARCHAR(20) DEFAULT 'PRESENT', -- PRESENT, ABSENT, LATE
    created_at DATETIME2(6) DEFAULT GETDATE(),
    CONSTRAINT FK_shifts_employee FOREIGN KEY (employee_id) REFERENCES employees(id)
);

-- Payrolls
-- Bảng lương của nhân viên, tính toán hàng tháng
CREATE TABLE payrolls (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    employee_id BIGINT NOT NULL,
    month INT NOT NULL,
    year INT NOT NULL,
    total_working_hours DECIMAL(18,2),
    base_salary DECIMAL(18,2),
    allowance DECIMAL(18,2),
    bonus DECIMAL(18,2),
    penalty DECIMAL(18,2),
    gross_salary DECIMAL(18,2),   -- lương trước thuế
    tax_amount DECIMAL(18,2),
    net_salary DECIMAL(18,2),     -- lương thực nhận sau thuế
    created_at DATETIME2(6) DEFAULT GETDATE(),
    CONSTRAINT FK_payroll_employee FOREIGN KEY (employee_id) REFERENCES employees(id)
);

-- Seed data with SQL Server syntax (no backticks)
INSERT INTO categories (name) VALUES
('CÀ PHÊ'),
('TRÀ'),
('SINH TỐ'),
('NƯỚC ÉP'),
('NƯỚC NGỌT'),
('YAOURT'),
('SODA');

INSERT INTO roles (name) VALUES
('USER'),
('ADMIN');

-- Note: Products table inserts should come after these basic lookups
-- Sample product data (using proper SQL Server INSERT syntax)
INSERT INTO products (name, price, thumbnail, description, category_id) VALUES
('Cà phê đen', 19000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', 1),
('Cà phê sữa', 19000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', 1),
('Cà phê ý', 19000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', 1),
('Bạc xỉu', 22000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', 1),
('Cà phê trứng', 32000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', 1),
('Trà đào', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', 2),
('Trà vải', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', 2),
('Sinh tố Dâu', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', 3),
('Sinh tố Bơ', 35000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', 3),
('Ép Cam', 25000, 'https://vuicoffee.com/wp-content/uploads/2024/11/ca-fe-sua-da.png', '', 4);

-- Sample user data (fixing syntax)
INSERT INTO users (fullname, phone_number, address, password, created_at, updated_at, is_active, date_of_birth, facebook_account_id, google_account_id, role_id) VALUES
('Lê Thanh Yên', '0375440580', '17A Cong Hoa', '$2a$10$PATMEvH86k45V0/z1vQS.u4qBumnzNVXjSt4B2u9uzpA5IC/Um68q', '2025-05-28 18:20:59', '2025-05-28 18:20:59', 0, '2004-09-16', 0, 0, 1),
('Admin User', '0375440581', '17A Cong Hoa', '$2a$10$FABWgZoRz4yZSgouTmS/8..WUZT1xlrfye6/5BJoe0U1NARHAwcWC', '2025-05-28 19:21:04', '2025-05-28 19:21:04', 1, '2004-09-16', 0, 0, 2);
