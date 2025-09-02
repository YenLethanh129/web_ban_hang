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

-- ====================================================================
-- LEVEL 1: TABLES WITHOUT DEPENDENCIES
-- ====================================================================

CREATE TABLE [dbo].[__EFMigrationsHistory] (
[MigrationId] nvarchar(150) NOT NULL,
[ProductVersion] nvarchar(32) NOT NULL,
PRIMARY KEY ([MigrationId])
);
GO

CREATE TABLE [dbo].[categories] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(255) NOT NULL UNIQUE,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[ingredient_categories] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(50) NOT NULL,
[description] nvarchar(255),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[taxes] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(100) NOT NULL,
[tax_rate] decimal(5,2) NOT NULL,
[description] nvarchar(255),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[roles] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(100) UNIQUE NOT NULL,
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[branches] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(255) NOT NULL,
[address] nvarchar(255),
[phone] nvarchar(20),
[manager] nvarchar(100),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[suppliers] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(255) NOT NULL,
[phone] nvarchar(20),
[email] nvarchar(100),
[address] nvarchar(255),
[note] nvarchar(255),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[shipping_providers] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(100) NOT NULL UNIQUE,
[contact_info] nvarchar(200),
[api_endpoint] nvarchar(200),
[created_at] datetime2(6) DEFAULT (sysdatetime()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[payment_methods] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(50) NOT NULL UNIQUE,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[payment_statuses] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(50) NOT NULL UNIQUE,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[order_statuses] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(50) NOT NULL UNIQUE,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[delivery_statuses] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(50) NOT NULL UNIQUE,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[purchase_order_statuses] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(50) NOT NULL UNIQUE,
[description] nvarchar(255),
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[invoice_statuses] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(50) NOT NULL UNIQUE,
[description] nvarchar(255),
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[goods_received_statuses] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(50) NOT NULL UNIQUE,
[description] nvarchar(255),
PRIMARY KEY ([id])
);
GO

-- ====================================================================
-- LEVEL 2: TABLES WITH LEVEL 1 DEPENDENCIES
-- ====================================================================

CREATE TABLE [dbo].[employees] (
[id] bigint NOT NULL IDENTITY(1,1),
[branch_id] bigint NOT NULL,
[full_name] nvarchar(100) NOT NULL,
[phone] nvarchar(20),
[email] nvarchar(100),
[position] nvarchar(50),
[hire_date] date NOT NULL,
[resign_date] date,
[status] nvarchar(20) DEFAULT ('ACTIVE'),
[created_at] datetime2(6) DEFAULT (sysdatetime()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[products] (
[id] bigint NOT NULL IDENTITY(1,1),
[price] decimal(18,2) NOT NULL,
[category_id] bigint,
[is_active] BIT DEFAULT (CONVERT([bit],(1))) NOT NULL,
[tax_id] bigint,
[description] nvarchar(255) NOT NULL,
[name] nvarchar(255) NOT NULL,
[thumbnail] nvarchar(255),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[ingredients] (
[id] bigint NOT NULL IDENTITY(1,1),
[category_id] bigint NOT NULL,
[name] nvarchar(255) NOT NULL,
[unit] nvarchar(50) NOT NULL,
[is_active] BIT DEFAULT (CONVERT([bit],(1))) NOT NULL,
[description] nvarchar(255),
[tax_id] bigint,
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) DEFAULT (getdate()) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[ingredient_purchase_orders] (
[id] bigint NOT NULL IDENTITY(1,1),
[purchase_order_code] nvarchar(50) NOT NULL UNIQUE,
[supplier_id] bigint,
[branch_id] bigint,
[employee_id] bigint,
[order_date] datetime2(6) DEFAULT (getdate()),
[expected_delivery_date] datetime2(6),
[status_id] bigint DEFAULT (1),
[total_amount_before_tax] decimal(18,2) DEFAULT (0),
[total_tax_amount] decimal(18,2) DEFAULT (0),
[total_amount_after_tax] decimal(18,2) DEFAULT (0),
[discount_amount] decimal(18,2) DEFAULT (0),
[final_amount] decimal(18,2) DEFAULT (0),
[note] nvarchar(500),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

-- ====================================================================
-- LEVEL 3: TABLES WITH LEVEL 2 DEPENDENCIES
-- ====================================================================

CREATE TABLE [dbo].[users] (
[id] bigint NOT NULL IDENTITY(1,1),
[employee_id] bigint,
[date_of_birth] date,
[facebook_account_id] bigint,
[google_account_id] bigint,
[is_active] BIT DEFAULT (CONVERT([bit],(1))) NOT NULL,
[role_id] bigint NOT NULL,
[phone_number] nvarchar(20) NOT NULL,
[fullname] nvarchar(100),
[address] nvarchar(200),
[password] nvarchar(200) NOT NULL,
[created_at] datetime2(6) DEFAULT (sysdatetime()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[product_images] (
[id] bigint NOT NULL IDENTITY(1,1),
[product_id] bigint,
[image_url] nvarchar(300),
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[product_recipes] (
[id] bigint NOT NULL IDENTITY(1,1),
[product_id] bigint NOT NULL,
[ingredient_id] bigint NOT NULL,
[quantity] decimal(18,2) NOT NULL,
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[ingredient_purchase_order_details] (
[id] bigint NOT NULL IDENTITY(1,1),
[purchase_order_id] bigint NOT NULL,
[ingredient_id] bigint NOT NULL,
[quantity] decimal(18,2) NOT NULL,
[unit_price] decimal(18,2) NOT NULL,
[tax_price] decimal(18,2) NOT NULL,
[total_price] decimal(18,2) NOT NULL,
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[payrolls] (
[id] bigint NOT NULL IDENTITY(1,1),
[employee_id] bigint NOT NULL,
[month] int NOT NULL,
[year] int NOT NULL,
[total_working_hours] decimal(18,2),
[base_salary] decimal(18,2),
[allowance] decimal(18,2),
[bonus] decimal(18,2),
[penalty] decimal(18,2),
[gross_salary] decimal(18,2),
[tax_amount] decimal(18,2),
[net_salary] decimal(18,2),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[employee_salaries] (
[id] bigint NOT NULL IDENTITY(1,1),
[employee_id] bigint NOT NULL,
[base_salary] decimal(18,2) NOT NULL,
[salary_type] nvarchar(20) DEFAULT ('MONTHLY'),
[allowance] decimal(18,2) DEFAULT ((0.0)),
[bonus] decimal(18,2) DEFAULT ((0.0)),
[penalty] decimal(18,2) DEFAULT ((0.0)),
[tax_rate] decimal(18,2) DEFAULT ((0.1)),
[effective_date] date NOT NULL,
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[employee_shifts] (
[id] bigint NOT NULL IDENTITY(1,1),
[employee_id] bigint NOT NULL,
[shift_date] date NOT NULL,
[start_time] time(7) NOT NULL,
[end_time] time(7) NOT NULL,
[status] nvarchar(20) DEFAULT ('PRESENT'),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[branch_ingredient_inventory] (
[id] bigint NOT NULL IDENTITY(1,1),
[branch_id] bigint NOT NULL,
[ingredient_id] bigint NOT NULL,
[quantity] decimal(18,2) NOT NULL,
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) DEFAULT (getdate()) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[ingredient_warehouse] (
[id] bigint NOT NULL IDENTITY(1,1),
[ingredient_id] bigint NOT NULL,
[quantity] decimal(18,2) NOT NULL,
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) DEFAULT (getdate()) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[ingredient_transfers] (
[id] bigint NOT NULL IDENTITY(1,1),
[ingredient_id] bigint NOT NULL,
[branch_id] bigint NOT NULL,
[quantity] decimal(18,2) NOT NULL,
[note] nvarchar(255),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[supplier_ingredient_prices] (
[id] bigint NOT NULL IDENTITY(1,1),
[supplier_id] bigint NOT NULL,
[ingredient_id] bigint NOT NULL,
[price] decimal(18,2) NOT NULL,
[unit] nvarchar(50) NOT NULL,
[effective_date] datetime2(6) DEFAULT (getdate()),
[expired_date] datetime2(6),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[branch_expenses] (
[id] bigint NOT NULL IDENTITY(1,1),
[branch_id] bigint NOT NULL,
[expense_type] nvarchar(100) NOT NULL,
[amount] decimal(18,2) NOT NULL,
[start_date] date NOT NULL,
[end_date] date,
[payment_cycle] nvarchar(50) DEFAULT ('MONTHLY'),
[note] nvarchar(255),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

-- Purchase Invoices from Suppliers
CREATE TABLE [dbo].[purchase_invoices] (
[id] bigint NOT NULL IDENTITY(1,1),
[invoice_code] nvarchar(50) NOT NULL UNIQUE,
[purchase_order_id] bigint,
[supplier_id] bigint NOT NULL,
[branch_id] bigint,
[invoice_date] datetime2(6) NOT NULL,
[due_date] datetime2(6),
[payment_date] datetime2(6),
[status_id] bigint DEFAULT (1),
[total_amount_before_tax] decimal(18,2) DEFAULT (0),
[total_tax_amount] decimal(18,2) DEFAULT (0),
[total_amount_after_tax] decimal(18,2) DEFAULT (0),
[paid_amount] decimal(18,2) DEFAULT (0),
[remaining_amount] decimal(18,2) DEFAULT (0),
[discount_amount] decimal(18,2) DEFAULT (0),
[payment_method] nvarchar(50),
[payment_reference] nvarchar(100),
[note] nvarchar(500),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

-- Purchase Invoice Details
CREATE TABLE [dbo].[purchase_invoice_details] (
[id] bigint NOT NULL IDENTITY(1,1),
[invoice_id] bigint NOT NULL,
[ingredient_id] bigint NOT NULL,
[quantity] decimal(18,2) NOT NULL,
[unit_price] decimal(18,2) NOT NULL,
[amount_before_tax] decimal(18,2) NOT NULL,
[tax_rate] decimal(5,2) DEFAULT (0),
[tax_amount] decimal(18,2) DEFAULT (0),
[amount_after_tax] decimal(18,2) NOT NULL,
[discount_rate] decimal(5,2) DEFAULT (0),
[discount_amount] decimal(18,2) DEFAULT (0),
[final_amount] decimal(18,2) NOT NULL,
[expiry_date] date,
[batch_number] nvarchar(50),
[note] nvarchar(255),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

-- Goods Received Notes (Phiếu nhập kho)
CREATE TABLE [dbo].[goods_received_notes] (
[id] bigint NOT NULL IDENTITY(1,1),
[grn_code] nvarchar(50) NOT NULL UNIQUE,
[purchase_order_id] bigint,
[invoice_id] bigint,
[supplier_id] bigint NOT NULL,
[branch_id] bigint NOT NULL,
[warehouse_staff_id] bigint,
[received_date] datetime2(6) DEFAULT (getdate()),
[status_id] bigint DEFAULT (1),
[total_quantity_ordered] decimal(18,2) DEFAULT (0),
[total_quantity_received] decimal(18,2) DEFAULT (0),
[total_quantity_rejected] decimal(18,2) DEFAULT (0),
[delivery_note_number] nvarchar(100),
[vehicle_number] nvarchar(20),
[driver_name] nvarchar(100),
[note] nvarchar(500),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

-- Goods Received Details
CREATE TABLE [dbo].[goods_received_details] (
[id] bigint NOT NULL IDENTITY(1,1),
[grn_id] bigint NOT NULL,
[ingredient_id] bigint NOT NULL,
[ordered_quantity] decimal(18,2) NOT NULL,
[received_quantity] decimal(18,2) NOT NULL,
[rejected_quantity] decimal(18,2) DEFAULT (0),
[quality_status] nvarchar(20) DEFAULT ('ACCEPTED'),
[rejection_reason] nvarchar(255),
[unit_price] decimal(18,2),
[expiry_date] date,
[batch_number] nvarchar(50),
[storage_location] nvarchar(100),
[note] nvarchar(255),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

-- Purchase Returns (Phiếu trả hàng)
CREATE TABLE [dbo].[purchase_returns] (
[id] bigint NOT NULL IDENTITY(1,1),
[return_code] nvarchar(50) NOT NULL UNIQUE,
[grn_id] bigint,
[invoice_id] bigint,
[supplier_id] bigint NOT NULL,
[branch_id] bigint NOT NULL,
[return_date] datetime2(6) DEFAULT (getdate()),
[return_reason] nvarchar(255),
[status_id] bigint DEFAULT (1),
[total_return_amount] decimal(18,2) DEFAULT (0),
[refund_amount] decimal(18,2) DEFAULT (0),
[credit_note_number] nvarchar(100),
[approved_by] bigint,
[approval_date] datetime2(6),
[note] nvarchar(500),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

-- Purchase Return Details
CREATE TABLE [dbo].[purchase_return_details] (
[id] bigint NOT NULL IDENTITY(1,1),
[return_id] bigint NOT NULL,
[ingredient_id] bigint NOT NULL,
[return_quantity] decimal(18,2) NOT NULL,
[unit_price] decimal(18,2),
[return_amount] decimal(18,2),
[return_reason] nvarchar(255),
[batch_number] nvarchar(50),
[expiry_date] date,
[quality_issue] nvarchar(255),
[note] nvarchar(255),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

-- Supplier Performance Tracking
CREATE TABLE [dbo].[supplier_performance] (
[id] bigint NOT NULL IDENTITY(1,1),
[supplier_id] bigint NOT NULL,
[evaluation_period] nvarchar(20) NOT NULL, -- MONTHLY, QUARTERLY, YEARLY
[period_value] nvarchar(20) NOT NULL, -- 2024-01, 2024-Q1, 2024
[total_orders] int DEFAULT (0),
[total_amount] decimal(18,2) DEFAULT (0),
[on_time_deliveries] int DEFAULT (0),
[late_deliveries] int DEFAULT (0),
[quality_score] decimal(3,2) DEFAULT (0), -- 0.00 to 5.00
[service_score] decimal(3,2) DEFAULT (0), -- 0.00 to 5.00
[overall_rating] decimal(3,2) DEFAULT (0), -- 0.00 to 5.00
[total_returns] int DEFAULT (0),
[return_value] decimal(18,2) DEFAULT (0),
[comments] nvarchar(500),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

-- ====================================================================
-- LEVEL 4: TABLES WITH LEVEL 3 DEPENDENCIES
-- ====================================================================

CREATE TABLE [dbo].[customers] (
[id] bigint NOT NULL,
[user_id] bigint,
[fullname] nvarchar(100) NOT NULL,
[phone_number] nvarchar(20),
[email] nvarchar(100),
[address] nvarchar(200),
[created_at] datetime2(6) DEFAULT (sysdatetime()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[tokens] (
[id] bigint NOT NULL IDENTITY(1,1),
[expired] bit NOT NULL,
[revoked] bit NOT NULL,
[expiration_date] datetime2(6),
[user_id] bigint,
[token_type] nvarchar(50),
[token] nvarchar(255),
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[social_accounts] (
[id] bigint NOT NULL IDENTITY(1,1),
[provider_id] bigint NOT NULL,
[user_id] bigint,
[provider] nvarchar(20) NOT NULL,
[name] nvarchar(100),
[email] nvarchar(150),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

-- ====================================================================
-- LEVEL 5: TABLES WITH LEVEL 4 DEPENDENCIES
-- ====================================================================

CREATE TABLE [dbo].[orders] (
[id] bigint NOT NULL IDENTITY(1,1),
[order_uuid] CHAR(36) NOT NULL UNIQUE,   
[order_code] nvarchar(20) NOT NULL UNIQUE,
[customer_id] bigint NOT NULL,
[branch_id] bigint,
[total_money] decimal(18,2),
[status_id] bigint,
[created_at] datetime2(6) DEFAULT (sysdatetime()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
[notes] nvarchar(500),
PRIMARY KEY ([id])
);
GO

-- ====================================================================
-- LEVEL 6: TABLES WITH LEVEL 5 DEPENDENCIES
-- ====================================================================

CREATE TABLE [dbo].[order_details] (
[id] bigint NOT NULL IDENTITY(1,1),
[quantity] int NOT NULL,
[order_id] bigint DEFAULT (CONVERT([bigint],(0))) NOT NULL,
[product_id] bigint DEFAULT (CONVERT([bigint],(0))) NOT NULL,
[color] nvarchar(255),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
[note] nvarchar(255),
[total_amount] decimal(18,2) DEFAULT ((0.0)) NOT NULL,
[unit_price] decimal(18,2) DEFAULT ((0.0)) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[order_payments] (
[id] bigint NOT NULL IDENTITY(1,1),
[order_id] bigint NOT NULL,
[payment_method_id] bigint NOT NULL,
[payment_status_id] bigint NOT NULL,
[amount] decimal(18,2) NOT NULL,
[payment_date] datetime2(6),
[transaction_id] nvarchar(100),
[notes] nvarchar(255),
[created_at] datetime2(6) DEFAULT (sysdatetime()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[order_shipments] (
[id] bigint NOT NULL IDENTITY(1,1),
[order_id] bigint NOT NULL,
[shipping_provider_id] bigint,
[shipping_address] nvarchar(500) NOT NULL,
[shipping_cost] decimal(18,2),
[shipping_method] nvarchar(50),
[estimated_delivery_date] datetime2(6),
[notes] nvarchar(255),
[created_at] datetime2(6) DEFAULT (sysdatetime()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[order_delivery_tracking] (
[id] bigint NOT NULL IDENTITY(1,1),
[order_id] bigint NOT NULL,
[tracking_number] nvarchar(100) NOT NULL,
[status_id] bigint NOT NULL,
[location] nvarchar(255),
[estimated_delivery] datetime2(6),
[delivery_person_id] bigint,
[shipping_provider_id] bigint,
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) DEFAULT (sysdatetime()) NOT NULL,
PRIMARY KEY ([id])
);
GO

-- ====================================================================
-- SUMMARY TABLES (Views as Tables)
-- ====================================================================

CREATE TABLE [dbo].[sales_summary] (
[id] bigint NOT NULL IDENTITY(1,1),
[branch_id] bigint,
[period_type] nvarchar(20) NOT NULL,
[period_value] nvarchar(20) NOT NULL,
[total_orders] int NOT NULL,
[total_products] int NOT NULL,
[revenue_before_tax] decimal(18,2) NOT NULL,
[revenue_after_tax] decimal(18,2) NOT NULL,
[tax_amount] decimal(18,2) NOT NULL,
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[expenses_summary] (
[id] bigint NOT NULL IDENTITY(1,1),
[branch_id] bigint,
[period_type] nvarchar(20) NOT NULL,
[period_value] nvarchar(20) NOT NULL,
[total_purchase_orders] int NOT NULL,
[total_ingredients] int NOT NULL,
[expense_before_tax] decimal(18,2) NOT NULL,
[expense_after_tax] decimal(18,2) NOT NULL,
[tax_amount] decimal(18,2) NOT NULL,
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

CREATE TABLE [dbo].[profit_summary] (
[id] bigint NOT NULL IDENTITY(1,1),
[branch_id] bigint,
[period_type] nvarchar(20) NOT NULL,
[period_value] nvarchar(20) NOT NULL,
[revenue_before_tax] decimal(18,2) NOT NULL,
[revenue_after_tax] decimal(18,2) NOT NULL,
[expense_before_tax] decimal(18,2) NOT NULL,
[expense_after_tax] decimal(18,2) NOT NULL,
[output_tax] decimal(18,2) NOT NULL,
[input_tax] decimal(18,2) NOT NULL,
[vat_to_pay] decimal(18,2) NOT NULL,
[profit_before_tax] decimal(18,2) NOT NULL,
[profit_after_tax] decimal(18,2) NOT NULL,
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);
GO

-- ====================================================================
-- VIEW TABLES (These were originally views but defined as tables)
-- ====================================================================

CREATE TABLE [dbo].[v_sales_summary] (
[branch_id] bigint,
[year] int NOT NULL,
[month] int NOT NULL,
[period] nvarchar(7) NOT NULL,
[total_orders] int NOT NULL,
[total_products] int NOT NULL,
[revenue_before_tax] decimal(18,2) NOT NULL,
[revenue_after_tax] decimal(18,2) NOT NULL,
[tax_amount] decimal(18,2) NOT NULL
);
GO

CREATE TABLE [dbo].[v_employee_payroll] (
[employee_id] bigint NOT NULL,
[full_name] nvarchar(100) NOT NULL,
[branch_name] nvarchar(255) NOT NULL,
[position_name] nvarchar(100),
[base_salary] decimal(18,2),
[salary_type] nvarchar(20),
[total_allowances] decimal(18,2) NOT NULL,
[total_bonus] decimal(18,2) NOT NULL,
[total_deductions] decimal(18,2) NOT NULL,
[gross_salary] decimal(18,2) NOT NULL,
[effective_date] date,
[end_date] date
);
GO

CREATE TABLE [dbo].[v_inventory_status] (
[ingredient_id] bigint NOT NULL,
[ingredient_name] nvarchar(255) NOT NULL,
[location_id] bigint NOT NULL,
[location_name] nvarchar(100) NOT NULL,
[branch_id] bigint NOT NULL,
[branch_name] nvarchar(255) NOT NULL,
[quantity_on_hand] decimal(18,2) NOT NULL,
[quantity_reserved] decimal(18,2) NOT NULL,
[available_quantity] decimal(18,2) NOT NULL,
[minimum_stock] decimal(18,2),
[stock_status] nvarchar(20) NOT NULL,
[unit_of_measure] nvarchar(50) NOT NULL,
[last_updated] datetime2(6) NOT NULL
);
GO

CREATE TABLE [dbo].[v_profit_summary] (
[branch_id] bigint,
[year] int NOT NULL,
[month] int NOT NULL,
[period] nvarchar(7) NOT NULL,
[revenue_before_tax] decimal(18,2) NOT NULL,
[revenue_after_tax] decimal(18,2) NOT NULL,
[expense_before_tax] decimal(18,2) NOT NULL,
[expense_after_tax] decimal(18,2) NOT NULL,
[output_tax] decimal(18,2) NOT NULL,
[input_tax] decimal(18,2) NOT NULL,
[vat_to_pay] decimal(18,2) NOT NULL,
[profit_before_tax] decimal(18,2) NOT NULL,
[profit_after_tax] decimal(18,2) NOT NULL
);
GO

CREATE TABLE [dbo].[v_expenses_summary] (
[branch_id] bigint,
[year] int NOT NULL,
[month] int NOT NULL,
[period] nvarchar(7) NOT NULL,
[total_purchase_orders] int NOT NULL,
[total_ingredients] decimal(18,2) NOT NULL,
[expense_before_tax] decimal(18,2) NOT NULL,
[expense_after_tax] decimal(18,2) NOT NULL,
[tax_amount] decimal(18,2) NOT NULL
);
GO

CREATE TABLE [dbo].[v_products_with_prices] (
[id] bigint NOT NULL,
[name] nvarchar(255) NOT NULL,
[description] nvarchar(500),
[sku] nvarchar(100),
[category_name] nvarchar(255),
[current_price] decimal(18,2),
[price_type] nvarchar(20),
[tax_name] nvarchar(100),
[tax_rate] decimal(5,2),
[unit_of_measure] nvarchar(50) NOT NULL,
[weight] decimal(10,3),
[dimensions] nvarchar(100),
[is_active] bit NOT NULL,
[created_at] datetime2(6),
[updated_at] datetime2(6)
);
GO

-- ====================================================================
-- FOREIGN KEY CONSTRAINTS
-- ====================================================================

-- Level 2 table constraints
ALTER TABLE [dbo].[employees]
ADD CONSTRAINT [FK_employees_branch]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[products]
ADD CONSTRAINT [FK_products_categories]
FOREIGN KEY ([category_id]) 
REFERENCES [dbo].[categories]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[products]
ADD CONSTRAINT [FK_products_taxes]
FOREIGN KEY ([tax_id]) 
REFERENCES [dbo].[taxes]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[ingredients]
ADD CONSTRAINT [FK_ingredients_ingredient_categories]
FOREIGN KEY ([category_id]) 
REFERENCES [dbo].[ingredient_categories]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[ingredients]
ADD CONSTRAINT [FK_ingredients_taxes]
FOREIGN KEY ([tax_id]) 
REFERENCES [dbo].[taxes]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[ingredient_purchase_orders]
ADD CONSTRAINT [FK_purchase_order_supplier]
FOREIGN KEY ([supplier_id]) 
REFERENCES [dbo].[suppliers]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[ingredient_purchase_orders]
ADD CONSTRAINT [FK_purchase_order_branch]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[ingredient_purchase_orders]
ADD CONSTRAINT [FK_purchase_order_employee]
FOREIGN KEY ([employee_id]) 
REFERENCES [dbo].[employees]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[ingredient_purchase_orders]
ADD CONSTRAINT [FK_purchase_order_status]
FOREIGN KEY ([status_id]) 
REFERENCES [dbo].[purchase_order_statuses]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

-- Level 3 table constraints
ALTER TABLE [dbo].[users]
ADD CONSTRAINT [FK_users_employees]
FOREIGN KEY ([employee_id]) 
REFERENCES [dbo].[employees]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[users]
ADD CONSTRAINT [FK_users_roles]
FOREIGN KEY ([role_id]) 
REFERENCES [dbo].[roles]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[product_images]
ADD CONSTRAINT [FKqnq71xsohugpqwf3c9gxmsuy]
FOREIGN KEY ([product_id]) 
REFERENCES [dbo].[products]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[product_recipes]
ADD CONSTRAINT [FK_recipe_product]
FOREIGN KEY ([product_id]) 
REFERENCES [dbo].[products]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[product_recipes]
ADD CONSTRAINT [FK_recipe_ingredient]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[ingredient_purchase_order_details]
ADD CONSTRAINT [FK_ipod_purchase_orders]
FOREIGN KEY ([purchase_order_id]) 
REFERENCES [dbo].[ingredient_purchase_orders]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[ingredient_purchase_order_details]
ADD CONSTRAINT [FK_ipod_ingredients]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[payrolls]
ADD CONSTRAINT [FK_payroll_employee]
FOREIGN KEY ([employee_id]) 
REFERENCES [dbo].[employees]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[employee_salaries]
ADD CONSTRAINT [FK_salaries_employee]
FOREIGN KEY ([employee_id]) 
REFERENCES [dbo].[employees]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[employee_shifts]
ADD CONSTRAINT [FK_shifts_employee]
FOREIGN KEY ([employee_id]) 
REFERENCES [dbo].[employees]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[branch_ingredient_inventory]
ADD CONSTRAINT [FK_bii_branch]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[branch_ingredient_inventory]
ADD CONSTRAINT [FK_bii_ingredient]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[ingredient_warehouse]
ADD CONSTRAINT [FK_ingredient_warehouse]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[ingredient_transfers]
ADD CONSTRAINT [FK_transfer_branch]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[ingredient_transfers]
ADD CONSTRAINT [FK_transfer_ingredient]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[supplier_ingredient_prices]
ADD CONSTRAINT [FK_sip_supplier]
FOREIGN KEY ([supplier_id]) 
REFERENCES [dbo].[suppliers]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[supplier_ingredient_prices]
ADD CONSTRAINT [FK_sip_ingredient]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[branch_expenses]
ADD CONSTRAINT [FK_branch_expenses_branches]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

-- Purchase Invoice constraints
ALTER TABLE [dbo].[purchase_invoices]
ADD CONSTRAINT [FK_invoice_purchase_order]
FOREIGN KEY ([purchase_order_id]) 
REFERENCES [dbo].[ingredient_purchase_orders]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[purchase_invoices]
ADD CONSTRAINT [FK_invoice_supplier]
FOREIGN KEY ([supplier_id]) 
REFERENCES [dbo].[suppliers]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[purchase_invoices]
ADD CONSTRAINT [FK_invoice_branch]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[purchase_invoices]
ADD CONSTRAINT [FK_invoice_status]
FOREIGN KEY ([status_id]) 
REFERENCES [dbo].[invoice_statuses]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

-- Purchase Invoice Details constraints
ALTER TABLE [dbo].[purchase_invoice_details]
ADD CONSTRAINT [FK_invoice_detail_invoice]
FOREIGN KEY ([invoice_id]) 
REFERENCES [dbo].[purchase_invoices]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[purchase_invoice_details]
ADD CONSTRAINT [FK_invoice_detail_ingredient]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

-- Goods Received Notes constraints
ALTER TABLE [dbo].[goods_received_notes]
ADD CONSTRAINT [FK_grn_purchase_order]
FOREIGN KEY ([purchase_order_id]) 
REFERENCES [dbo].[ingredient_purchase_orders]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[goods_received_notes]
ADD CONSTRAINT [FK_grn_invoice]
FOREIGN KEY ([invoice_id]) 
REFERENCES [dbo].[purchase_invoices]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[goods_received_notes]
ADD CONSTRAINT [FK_grn_supplier]
FOREIGN KEY ([supplier_id]) 
REFERENCES [dbo].[suppliers]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[goods_received_notes]
ADD CONSTRAINT [FK_grn_branch]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[goods_received_notes]
ADD CONSTRAINT [FK_grn_warehouse_staff]
FOREIGN KEY ([warehouse_staff_id]) 
REFERENCES [dbo].[employees]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[goods_received_notes]
ADD CONSTRAINT [FK_grn_status]
FOREIGN KEY ([status_id]) 
REFERENCES [dbo].[goods_received_statuses]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

-- Goods Received Details constraints
ALTER TABLE [dbo].[goods_received_details]
ADD CONSTRAINT [FK_grn_detail_grn]
FOREIGN KEY ([grn_id]) 
REFERENCES [dbo].[goods_received_notes]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[goods_received_details]
ADD CONSTRAINT [FK_grn_detail_ingredient]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

-- Purchase Returns constraints
ALTER TABLE [dbo].[purchase_returns]
ADD CONSTRAINT [FK_return_grn]
FOREIGN KEY ([grn_id]) 
REFERENCES [dbo].[goods_received_notes]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[purchase_returns]
ADD CONSTRAINT [FK_return_invoice]
FOREIGN KEY ([invoice_id]) 
REFERENCES [dbo].[purchase_invoices]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[purchase_returns]
ADD CONSTRAINT [FK_return_supplier]
FOREIGN KEY ([supplier_id]) 
REFERENCES [dbo].[suppliers]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[purchase_returns]
ADD CONSTRAINT [FK_return_branch]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[purchase_returns]
ADD CONSTRAINT [FK_return_approved_by]
FOREIGN KEY ([approved_by]) 
REFERENCES [dbo].[employees]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

-- Purchase Return Details constraints
ALTER TABLE [dbo].[purchase_return_details]
ADD CONSTRAINT [FK_return_detail_return]
FOREIGN KEY ([return_id]) 
REFERENCES [dbo].[purchase_returns]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[purchase_return_details]
ADD CONSTRAINT [FK_return_detail_ingredient]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

-- Supplier Performance constraints
ALTER TABLE [dbo].[supplier_performance]
ADD CONSTRAINT [FK_performance_supplier]
FOREIGN KEY ([supplier_id]) 
REFERENCES [dbo].[suppliers]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;
GO

-- Level 4 table constraints
ALTER TABLE [dbo].[customers]
ADD CONSTRAINT [FK_customers_users]
FOREIGN KEY ([id]) 
REFERENCES [dbo].[users]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[tokens]
ADD CONSTRAINT [FK2dylsfo39lgjyqml2tbe0b0ss]
FOREIGN KEY ([user_id]) 
REFERENCES [dbo].[users]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[social_accounts]
ADD CONSTRAINT [FK6rmxxiton5yuvu7ph2hcq2xn7]
FOREIGN KEY ([user_id]) 
REFERENCES [dbo].[users]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

-- Level 5 table constraints
ALTER TABLE [dbo].[orders]
ADD CONSTRAINT [FK_orders_customers]
FOREIGN KEY ([customer_id]) 
REFERENCES [dbo].[customers]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[orders]
ADD CONSTRAINT [FK_orders_branches]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[orders]
ADD CONSTRAINT [FK_orders_order_status]
FOREIGN KEY ([status_id]) 
REFERENCES [dbo].[order_statuses]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

-- Level 6 table constraints
ALTER TABLE [dbo].[order_details]
ADD CONSTRAINT [FKjyu2qbqt8gnvno9oe9j2s2ldk]
FOREIGN KEY ([order_id]) 
REFERENCES [dbo].[orders]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[order_details]
ADD CONSTRAINT [FK4q98utpd73imf4yhttm3w0eax]
FOREIGN KEY ([product_id]) 
REFERENCES [dbo].[products]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[order_payments]
ADD CONSTRAINT [FK_order_payments_orders]
FOREIGN KEY ([order_id]) 
REFERENCES [dbo].[orders]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[order_payments]
ADD CONSTRAINT [FK_order_payments_payment_methods]
FOREIGN KEY ([payment_method_id]) 
REFERENCES [dbo].[payment_methods]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[order_payments]
ADD CONSTRAINT [FK_order_payments_payment_statuses]
FOREIGN KEY ([payment_status_id]) 
REFERENCES [dbo].[payment_statuses]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[order_shipments]
ADD CONSTRAINT [FK_order_shipments_orders]
FOREIGN KEY ([order_id]) 
REFERENCES [dbo].[orders]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[order_shipments]
ADD CONSTRAINT [FK_order_shipments_shipping_providers]
FOREIGN KEY ([shipping_provider_id]) 
REFERENCES [dbo].[shipping_providers]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[order_delivery_tracking]
ADD CONSTRAINT [FK_tracking_orders]
FOREIGN KEY ([order_id]) 
REFERENCES [dbo].[orders]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[order_delivery_tracking]
ADD CONSTRAINT [FK_tracking_status]
FOREIGN KEY ([status_id]) 
REFERENCES [dbo].[delivery_statuses]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[order_delivery_tracking]
ADD CONSTRAINT [FK_tracking_employees]
FOREIGN KEY ([delivery_person_id]) 
REFERENCES [dbo].[employees]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[order_delivery_tracking]
ADD CONSTRAINT [FK_tracking_providers]
FOREIGN KEY ([shipping_provider_id]) 
REFERENCES [dbo].[shipping_providers]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

-- Summary table constraints
ALTER TABLE [dbo].[sales_summary]
ADD CONSTRAINT [FK_sales_summary_branches]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[expenses_summary]
ADD CONSTRAINT [FK_expenses_summary_branches]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[profit_summary]
ADD CONSTRAINT [FK_profit_summary_branches]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

-- View table constraints (these may be problematic as they reference tables, making them more like materialized views)
ALTER TABLE [dbo].[v_sales_summary]
ADD CONSTRAINT [FK_v_sales_summary_branches_branch_id]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[v_employee_payroll]
ADD CONSTRAINT [FK_v_employee_payroll_employees_employee_id]
FOREIGN KEY ([employee_id]) 
REFERENCES [dbo].[employees]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[v_inventory_status]
ADD CONSTRAINT [FK_v_inventory_status_ingredients_ingredient_id]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[v_inventory_status]
ADD CONSTRAINT [FK_v_inventory_status_branches_branch_id]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[v_profit_summary]
ADD CONSTRAINT [FK_v_profit_summary_branches_branch_id]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[v_expenses_summary]
ADD CONSTRAINT [FK_v_expenses_summary_branches_branch_id]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;
GO

PRINT 'Database schema created successfully.';