USE [webbanhang]
GO

-- Enable Unicode support with proper collation
-- Use Vietnamese collation for better Unicode support
-- ALTER DATABASE [webbanhang] COLLATE Vietnamese_CI_AS;

-- Drop views first (views depend on tables)
IF OBJECT_ID('dbo.v_cogs_summary', 'V') IS NOT NULL DROP VIEW [dbo].[v_cogs_summary];
IF OBJECT_ID('dbo.v_profit_summary', 'V') IS NOT NULL DROP VIEW [dbo].[v_profit_summary];
IF OBJECT_ID('dbo.v_inventory_status', 'V') IS NOT NULL DROP VIEW [dbo].[v_inventory_status];
IF OBJECT_ID('dbo.v_employee_payroll', 'V') IS NOT NULL DROP VIEW [dbo].[v_employee_payroll];
IF OBJECT_ID('dbo.v_sales_summary', 'V') IS NOT NULL DROP VIEW [dbo].[v_sales_summary];

-- Drop tables in reverse order of dependencies
IF OBJECT_ID('dbo.supplier_performance', 'U') IS NOT NULL DROP TABLE [dbo].[supplier_performance];
IF OBJECT_ID('dbo.purchase_return_details', 'U') IS NOT NULL DROP TABLE [dbo].[purchase_return_details];
IF OBJECT_ID('dbo.purchase_returns', 'U') IS NOT NULL DROP TABLE [dbo].[purchase_returns];
IF OBJECT_ID('dbo.goods_received_details', 'U') IS NOT NULL DROP TABLE [dbo].[goods_received_details];
IF OBJECT_ID('dbo.goods_received_notes', 'U') IS NOT NULL DROP TABLE [dbo].[goods_received_notes];
IF OBJECT_ID('dbo.purchase_invoice_details', 'U') IS NOT NULL DROP TABLE [dbo].[purchase_invoice_details];
IF OBJECT_ID('dbo.purchase_invoices', 'U') IS NOT NULL DROP TABLE [dbo].[purchase_invoices];
IF OBJECT_ID('dbo.profit_summary', 'U') IS NOT NULL DROP TABLE [dbo].[profit_summary];
IF OBJECT_ID('dbo.cogs_summary', 'U') IS NOT NULL DROP TABLE [dbo].[cogs_summary];
IF OBJECT_ID('dbo.sales_summary', 'U') IS NOT NULL DROP TABLE [dbo].[sales_summary];
IF OBJECT_ID('dbo.order_delivery_tracking', 'U') IS NOT NULL DROP TABLE [dbo].[order_delivery_tracking];
IF OBJECT_ID('dbo.order_shipments', 'U') IS NOT NULL DROP TABLE [dbo].[order_shipments];
IF OBJECT_ID('dbo.order_payments', 'U') IS NOT NULL DROP TABLE [dbo].[order_payments];
IF OBJECT_ID('dbo.order_details', 'U') IS NOT NULL DROP TABLE [dbo].[order_details];
IF OBJECT_ID('dbo.orders', 'U') IS NOT NULL DROP TABLE [dbo].[orders];
IF OBJECT_ID('dbo.customers', 'U') IS NOT NULL DROP TABLE [dbo].[customers];
IF OBJECT_ID('dbo.branch_expenses', 'U') IS NOT NULL DROP TABLE [dbo].[branch_expenses];
IF OBJECT_ID('dbo.supplier_ingredient_prices', 'U') IS NOT NULL DROP TABLE [dbo].[supplier_ingredient_prices];
IF OBJECT_ID('dbo.ingredient_transfers', 'U') IS NOT NULL DROP TABLE [dbo].[ingredient_transfers];
IF OBJECT_ID('dbo.inventory_movements', 'U') IS NOT NULL DROP TABLE [dbo].[inventory_movements];
IF OBJECT_ID('dbo.ingredient_transfer_request_details', 'U') IS NOT NULL DROP TABLE [dbo].[ingredient_transfer_request_details];
IF OBJECT_ID('dbo.ingredient_transfer_requests', 'U') IS NOT NULL DROP TABLE [dbo].[ingredient_transfer_requests];
IF OBJECT_ID('dbo.inventory_thresholds', 'U') IS NOT NULL DROP TABLE [dbo].[inventory_thresholds];
IF OBJECT_ID('dbo.ingredient_warehouse', 'U') IS NOT NULL DROP TABLE [dbo].[ingredient_warehouse];
IF OBJECT_ID('dbo.branch_ingredient_inventory', 'U') IS NOT NULL DROP TABLE [dbo].[branch_ingredient_inventory];
IF OBJECT_ID('dbo.employee_shifts', 'U') IS NOT NULL DROP TABLE [dbo].[employee_shifts];
IF OBJECT_ID('dbo.employee_salaries', 'U') IS NOT NULL DROP TABLE [dbo].[employee_salaries];
IF OBJECT_ID('dbo.payrolls', 'U') IS NOT NULL DROP TABLE [dbo].[payrolls];
IF OBJECT_ID('dbo.recipe_ingredients', 'U') IS NOT NULL DROP TABLE [dbo].[recipe_ingredients];
IF OBJECT_ID('dbo.recipes', 'U') IS NOT NULL DROP TABLE [dbo].[recipes];
IF OBJECT_ID('dbo.ingredient_purchase_order_details', 'U') IS NOT NULL DROP TABLE [dbo].[ingredient_purchase_order_details];
IF OBJECT_ID('dbo.product_recipes', 'U') IS NOT NULL DROP TABLE [dbo].[product_recipes];
IF OBJECT_ID('dbo.product_images', 'U') IS NOT NULL DROP TABLE [dbo].[product_images];
IF OBJECT_ID('dbo.role_permissions', 'U') IS NOT NULL DROP TABLE [dbo].[role_permissions];
IF OBJECT_ID('dbo.social_accounts', 'U') IS NOT NULL DROP TABLE [dbo].[social_accounts];
IF OBJECT_ID('dbo.tokens', 'U') IS NOT NULL DROP TABLE [dbo].[tokens];
IF OBJECT_ID('dbo.users', 'U') IS NOT NULL DROP TABLE [dbo].[users];
IF OBJECT_ID('dbo.ingredient_purchase_orders', 'U') IS NOT NULL DROP TABLE [dbo].[ingredient_purchase_orders];
IF OBJECT_ID('dbo.ingredients', 'U') IS NOT NULL DROP TABLE [dbo].[ingredients];
IF OBJECT_ID('dbo.products', 'U') IS NOT NULL DROP TABLE [dbo].[products];
IF OBJECT_ID('dbo.employees', 'U') IS NOT NULL DROP TABLE [dbo].[employees];
IF OBJECT_ID('dbo.branches', 'U') IS NOT NULL DROP TABLE [dbo].[branches];
IF OBJECT_ID('dbo.suppliers', 'U') IS NOT NULL DROP TABLE [dbo].[suppliers];
IF OBJECT_ID('dbo.shipping_providers', 'U') IS NOT NULL DROP TABLE [dbo].[shipping_providers];
IF OBJECT_ID('dbo.delivery_statuses', 'U') IS NOT NULL DROP TABLE [dbo].[delivery_statuses];
IF OBJECT_ID('dbo.payment_statuses', 'U') IS NOT NULL DROP TABLE [dbo].[payment_statuses];
IF OBJECT_ID('dbo.payment_methods', 'U') IS NOT NULL DROP TABLE [dbo].[payment_methods];
IF OBJECT_ID('dbo.order_statuses', 'U') IS NOT NULL DROP TABLE [dbo].[order_statuses];
IF OBJECT_ID('dbo.goods_received_statuses', 'U') IS NOT NULL DROP TABLE [dbo].[goods_received_statuses];
IF OBJECT_ID('dbo.invoice_statuses', 'U') IS NOT NULL DROP TABLE [dbo].[invoice_statuses];
IF OBJECT_ID('dbo.purchase_order_statuses', 'U') IS NOT NULL DROP TABLE [dbo].[purchase_order_statuses];
IF OBJECT_ID('dbo.ingredient_categories', 'U') IS NOT NULL DROP TABLE [dbo].[ingredient_categories];
IF OBJECT_ID('dbo.categories', 'U') IS NOT NULL DROP TABLE [dbo].[categories];
IF OBJECT_ID('dbo.permissions', 'U') IS NOT NULL DROP TABLE [dbo].[permissions];
IF OBJECT_ID('dbo.taxes', 'U') IS NOT NULL DROP TABLE [dbo].[taxes];
IF OBJECT_ID('dbo.roles', 'U') IS NOT NULL DROP TABLE [dbo].[roles];
GO

-- ====================================================================
-- CREATE TABLES - LEVEL 1: BASE TABLES WITHOUT DEPENDENCIES
-- ====================================================================

-- Roles table
CREATE TABLE [dbo].[roles] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(100) COLLATE Vietnamese_CI_AS NOT NULL UNIQUE,
    [description] nvarchar(255) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_roles] PRIMARY KEY ([id])
);
GO

-- Employee Position
-- Positions table
CREATE TABLE [dbo].[positions] (
    [id] BIGINT IDENTITY(1,1) NOT NULL,
    [name] NVARCHAR(100) COLLATE Vietnamese_CI_AS NOT NULL,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [need_schedule] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_positions] PRIMARY KEY ([id])
);
GO


-- Permissions table
CREATE TABLE [dbo].[permissions] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(100) COLLATE Vietnamese_CI_AS NOT NULL UNIQUE,
    [description] nvarchar(255) COLLATE Vietnamese_CI_AS,
    [resource] nvarchar(50) COLLATE Vietnamese_CI_AS NOT NULL,
    [action] nvarchar(50) COLLATE Vietnamese_CI_AS NOT NULL,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_permissions] PRIMARY KEY ([id])
);
GO

-- Taxes table
CREATE TABLE [dbo].[taxes] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(100) COLLATE Vietnamese_CI_AS NOT NULL,
    [tax_rate] decimal(5,2) NOT NULL,
    [description] nvarchar(255) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_taxes] PRIMARY KEY ([id])
);
GO

-- Categories table
CREATE TABLE [dbo].[categories] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(255) COLLATE Vietnamese_CI_AS NOT NULL UNIQUE,
    CONSTRAINT [PK_categories] PRIMARY KEY ([id])
);
GO

-- Ingredient Categories table
CREATE TABLE [dbo].[ingredient_categories] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(50) COLLATE Vietnamese_CI_AS NOT NULL,
    [description] nvarchar(255) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_ingredient_categories] PRIMARY KEY ([id])
);
GO

-- Purchase Order Statuses table
CREATE TABLE [dbo].[purchase_order_statuses] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(50) COLLATE Vietnamese_CI_AS NOT NULL,
    [description] nvarchar(255) COLLATE Vietnamese_CI_AS,
    CONSTRAINT [PK_purchase_order_statuses] PRIMARY KEY ([id])
);
GO

-- Invoice Statuses table
CREATE TABLE [dbo].[invoice_statuses] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(50) COLLATE Vietnamese_CI_AS NOT NULL,
    [description] nvarchar(255) COLLATE Vietnamese_CI_AS,
    CONSTRAINT [PK_invoice_statuses] PRIMARY KEY ([id])
);
GO

-- Goods Received Statuses table
CREATE TABLE [dbo].[goods_received_statuses] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(50) COLLATE Vietnamese_CI_AS NOT NULL,
    [description] nvarchar(255) COLLATE Vietnamese_CI_AS,
    CONSTRAINT [PK_goods_received_statuses] PRIMARY KEY ([id])
);
GO

-- Order Statuses table
CREATE TABLE [dbo].[order_statuses] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(50) NOT NULL UNIQUE,
    CONSTRAINT [PK_order_statuses] PRIMARY KEY ([id])
);
GO

-- Payment Methods table
CREATE TABLE [dbo].[payment_methods] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(50) NOT NULL UNIQUE,
    CONSTRAINT [PK_payment_methods] PRIMARY KEY ([id])
);
GO

-- Payment Statuses table
CREATE TABLE [dbo].[payment_statuses] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(50) NOT NULL UNIQUE,
    CONSTRAINT [PK_payment_statuses] PRIMARY KEY ([id])
);
GO

-- Delivery Statuses table
CREATE TABLE [dbo].[delivery_statuses] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(50) COLLATE Vietnamese_CI_AS NOT NULL,
    CONSTRAINT [PK_delivery_statuses] PRIMARY KEY ([id])
);
GO

-- Shipping Providers table
CREATE TABLE [dbo].[shipping_providers] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(100) COLLATE Vietnamese_CI_AS NOT NULL,
    [contact_info] nvarchar(255) COLLATE Vietnamese_CI_AS,
    [api_endpoint] nvarchar(500),
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_shipping_providers] PRIMARY KEY ([id])
);
GO

-- Suppliers table
CREATE TABLE [dbo].[suppliers] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(255) COLLATE Vietnamese_CI_AS NOT NULL,
    [phone] varchar(20),
    [email] varchar(100),
    [address] nvarchar(255) COLLATE Vietnamese_CI_AS,
    [note] nvarchar(255) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_suppliers] PRIMARY KEY ([id])
);
GO

-- Branches table
CREATE TABLE [dbo].[branches] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(255) COLLATE Vietnamese_CI_AS NOT NULL,
    [address] nvarchar(255) COLLATE Vietnamese_CI_AS,
    [phone] varchar(20),
    [manager] nvarchar(100) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_branches] PRIMARY KEY ([id])
);
GO

-- ====================================================================
-- CREATE TABLES - LEVEL 2: TABLES WITH SINGLE DEPENDENCIES
-- ====================================================================

-- Employees table
CREATE TABLE [dbo].[employees] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [branch_id] bigint NOT NULL,
    [full_name] nvarchar(255) COLLATE Vietnamese_CI_AS NOT NULL,
    [phone] varchar(20),
    [email] varchar(255),
    [position_id] bigint NOT NULL,
    [address] VARCHAR(255) COLLATE Vietnamese_CI_AS,
    [hire_date] datetime2,
    [resign_date] datetime2,
    [status] varchar(20) DEFAULT 'ACTIVE',
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_employees] PRIMARY KEY ([id]),
    CONSTRAINT [FK_employees_branches] FOREIGN KEY ([branch_id]) REFERENCES [dbo].[branches]([id]),
    CONSTRAINT [FK_employees_positions] FOREIGN KEY ([position_id]) REFERENCES [dbo].[positions]([id])
);

GO 
-- Products table
CREATE TABLE [dbo].[products] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [price] decimal(18,2) NOT NULL,
    [category_id] bigint NULL,
    [is_active] bit NOT NULL DEFAULT 1,
    [tax_id] bigint NULL,
    [description] nvarchar(255) COLLATE Vietnamese_CI_AS NOT NULL,
    [name] nvarchar(255) COLLATE Vietnamese_CI_AS NOT NULL,
    [thumbnail] nvarchar(255),
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_products] PRIMARY KEY ([id]),
    CONSTRAINT [FK_products_categories] FOREIGN KEY ([category_id]) REFERENCES [dbo].[categories]([id]),
    CONSTRAINT [FK_products_taxes] FOREIGN KEY ([tax_id]) REFERENCES [dbo].[taxes]([id])
);
GO

-- Ingredients table
CREATE TABLE [dbo].[ingredients] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [category_id] bigint NOT NULL,
    [name] nvarchar(255) COLLATE Vietnamese_CI_AS NOT NULL,
    [unit] nvarchar(50) COLLATE Vietnamese_CI_AS NOT NULL,
    [is_active] bit NOT NULL DEFAULT 1,
    [description] nvarchar(255) COLLATE Vietnamese_CI_AS,
    [tax_id] bigint NULL,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_ingredients] PRIMARY KEY ([id]),
    CONSTRAINT [FK_ingredients_categories] FOREIGN KEY ([category_id]) REFERENCES [dbo].[ingredient_categories]([id]),
    CONSTRAINT [FK_ingredients_taxes] FOREIGN KEY ([tax_id]) REFERENCES [dbo].[taxes]([id])
);
GO

-- Ingredient Purchase Orders table
CREATE TABLE [dbo].[ingredient_purchase_orders] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [purchase_order_code] nvarchar(100) COLLATE Vietnamese_CI_AS NOT NULL UNIQUE,
    [supplier_id] bigint NOT NULL,
    [branch_id] bigint,
    [employee_id] bigint,
    [order_date] datetime2(6) NOT NULL,
    [expected_delivery_date] datetime2(6),
    [status_id] bigint NOT NULL,
    [total_amount_before_tax] decimal(18,2),
    [total_tax_amount] decimal(18,2),
    [total_amount_after_tax] decimal(18,2),
    [discount_amount] decimal(18,2) DEFAULT 0,
    [final_amount] decimal(18,2),
    [note] nvarchar(1000) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_ingredient_purchase_orders] PRIMARY KEY ([id]),
    CONSTRAINT [FK_ingredient_purchase_orders_suppliers] FOREIGN KEY ([supplier_id]) REFERENCES [dbo].[suppliers]([id]),
    CONSTRAINT [FK_ingredient_purchase_orders_branches] FOREIGN KEY ([branch_id]) REFERENCES [dbo].[branches]([id]),
    CONSTRAINT [FK_ingredient_purchase_orders_employees] FOREIGN KEY ([employee_id]) REFERENCES [dbo].[employees]([id]),
    CONSTRAINT [FK_ingredient_purchase_orders_statuses] FOREIGN KEY ([status_id]) REFERENCES [dbo].[purchase_order_statuses]([id])
);
GO

-- TODO Migrate address and date of birth to employees table and customers table
-- Users table
CREATE TABLE [dbo].[users] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [employee_id] bigint,
    [date_of_birth] datetime2,
    [facebook_account_id] bigint,
    [google_account_id] bigint,
    [is_active] bit NOT NULL DEFAULT 1,
    [role_id] bigint NOT NULL,
    [phone_number] varchar(20) NOT NULL,
    [fullname] nvarchar(100) COLLATE Vietnamese_CI_AS,
    [address] nvarchar(200) COLLATE Vietnamese_CI_AS,
    [password] varchar(200) NOT NULL,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_users] PRIMARY KEY ([id]),
    CONSTRAINT [FK_users_roles] FOREIGN KEY ([role_id]) REFERENCES [dbo].[roles]([id])
);
GO

CREATE TABLE [dbo].[employee_users] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [username] varchar(100) NOT NULL, 
    [password] varchar(200) NOT NULL,
    [is_active] bit NOT NULL DEFAULT 1,
    [employee_id] bigint NOT NULL,
    [role_id] bigint NOT NULL,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_employee_users] PRIMARY KEY ([id]),
    CONSTRAINT [FK_employee_users_roles] FOREIGN KEY ([role_id]) REFERENCES [dbo].[roles]([id])
);
GO 


-- ====================================================================
-- CREATE TABLES - LEVEL 3: TABLES WITH MULTIPLE DEPENDENCIES
-- ====================================================================

-- Tokens table
CREATE TABLE [dbo].[tokens] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [expired] bit NOT NULL DEFAULT 0,
    [revoked] bit NOT NULL DEFAULT 0,
    [expiration_date] datetime2(6),
    [user_id] bigint NULL,
    [employee_user_id] bigint NULL,
    [token_type] varchar(50) NOT NULL,
    [token] nvarchar(MAX) NOT NULL,
    CONSTRAINT [PK_tokens] PRIMARY KEY ([id]),
    CONSTRAINT [FK_tokens_users] FOREIGN KEY ([user_id]) REFERENCES [dbo].[users]([id]),
    CONSTRAINT [FK_tokens_employees] FOREIGN KEY ([employee_user_id]) REFERENCES [dbo].[employee_users]([id])
);
GO

-- Social Accounts table
CREATE TABLE [dbo].[social_accounts] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [provider_id] bigint NOT NULL,
    [user_id] bigint NOT NULL,
    [provider] varchar(50) NOT NULL,
    [name] nvarchar(255) COLLATE Vietnamese_CI_AS,
    [email] varchar(255),
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_social_accounts] PRIMARY KEY ([id]),
    CONSTRAINT [FK_social_accounts_users] FOREIGN KEY ([user_id]) REFERENCES [dbo].[users]([id])
);
GO

-- Role Permissions table
CREATE TABLE [dbo].[role_permissions] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [role_id] bigint NOT NULL,
    [permission_id] bigint NOT NULL,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_role_permissions] PRIMARY KEY ([id]),
    CONSTRAINT [FK_role_permissions_roles] FOREIGN KEY ([role_id]) REFERENCES [dbo].[roles]([id]),
    CONSTRAINT [FK_role_permissions_permissions] FOREIGN KEY ([permission_id]) REFERENCES [dbo].[permissions]([id]),
    CONSTRAINT [UQ_role_permissions] UNIQUE ([role_id], [permission_id])
);
GO

-- Product Images table
CREATE TABLE [dbo].[product_images] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [product_id] bigint NOT NULL,
    [image_url] nvarchar(500) NOT NULL,
    CONSTRAINT [PK_product_images] PRIMARY KEY ([id]),
    CONSTRAINT [FK_product_images_products] FOREIGN KEY ([product_id]) REFERENCES [dbo].[products]([id])
);
GO

-- Product Recipes table
CREATE TABLE [dbo].[product_recipes] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [product_id] bigint NOT NULL,
    [ingredient_id] bigint NOT NULL,
    [quantity] decimal(10,3) NOT NULL,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_product_recipes] PRIMARY KEY ([id]),
    CONSTRAINT [FK_product_recipes_products] FOREIGN KEY ([product_id]) REFERENCES [dbo].[products]([id]),
    CONSTRAINT [FK_product_recipes_ingredients] FOREIGN KEY ([ingredient_id]) REFERENCES [dbo].[ingredients]([id])
);
GO

-- Recipes table
CREATE TABLE [dbo].[recipes] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(255) COLLATE Vietnamese_CI_AS NOT NULL,
    [description] nvarchar(500) COLLATE Vietnamese_CI_AS,
    [product_id] bigint NOT NULL,
    [serving_size] decimal(18,2) NOT NULL DEFAULT 1,
    [unit] nvarchar(50) COLLATE Vietnamese_CI_AS DEFAULT 'portion',
    [is_active] bit NOT NULL DEFAULT 1,
    [notes] nvarchar(500) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_recipes] PRIMARY KEY ([id]),
    CONSTRAINT [FK_recipes_products] FOREIGN KEY ([product_id]) REFERENCES [dbo].[products]([id])
);
GO

-- Recipe Ingredients table
CREATE TABLE [dbo].[recipe_ingredients] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [recipe_id] bigint NOT NULL,
    [ingredient_id] bigint NOT NULL,
    [quantity] decimal(18,4) NOT NULL,
    [unit] nvarchar(50) COLLATE Vietnamese_CI_AS NOT NULL,
    [waste_percentage] decimal(18,4) DEFAULT 0,
    [notes] nvarchar(500) COLLATE Vietnamese_CI_AS,
    [is_optional] bit NOT NULL DEFAULT 0,
    [sort_order] int NOT NULL DEFAULT 0,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_recipe_ingredients] PRIMARY KEY ([id]),
    CONSTRAINT [FK_recipe_ingredients_recipes] FOREIGN KEY ([recipe_id]) REFERENCES [dbo].[recipes]([id]),
    CONSTRAINT [FK_recipe_ingredients_ingredients] FOREIGN KEY ([ingredient_id]) REFERENCES [dbo].[ingredients]([id])
);
GO

-- Ingredient Purchase Order Details table
CREATE TABLE [dbo].[ingredient_purchase_order_details] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [purchase_order_id] bigint NOT NULL,
    [ingredient_id] bigint NOT NULL,
    [quantity] decimal(10,3) NOT NULL,
    [unit_price] decimal(18,2) NOT NULL,
    [tax_price] decimal(18,2),
    [total_price] decimal(18,2) NOT NULL,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_ingredient_purchase_order_details] PRIMARY KEY ([id]),
    CONSTRAINT [FK_ingredient_purchase_order_details_orders] FOREIGN KEY ([purchase_order_id]) REFERENCES [dbo].[ingredient_purchase_orders]([id]),
    CONSTRAINT [FK_ingredient_purchase_order_details_ingredients] FOREIGN KEY ([ingredient_id]) REFERENCES [dbo].[ingredients]([id])
);
GO

-- Payrolls table
CREATE TABLE [dbo].[payrolls] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [employee_id] bigint NOT NULL,
    [month] int NOT NULL,
    [year] int NOT NULL,
    [total_working_hours] decimal(8,2),
    [base_salary] decimal(18,2) NOT NULL,
    [allowance] decimal(18,2) DEFAULT 0,
    [bonus] decimal(18,2) DEFAULT 0,
    [penalty] decimal(18,2) DEFAULT 0,
    [gross_salary] decimal(18,2) NOT NULL,
    [tax_amount] decimal(18,2) DEFAULT 0,
    [net_salary] decimal(18,2) NOT NULL,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_payrolls] PRIMARY KEY ([id]),
    CONSTRAINT [FK_payrolls_employees] FOREIGN KEY ([employee_id]) REFERENCES [dbo].[employees]([id])
);
GO

-- Employee Salaries table
CREATE TABLE [dbo].[employee_salaries] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [employee_id] bigint NOT NULL,
    [base_salary] decimal(18,2) NOT NULL,
    [salary_type] varchar(20) NOT NULL DEFAULT 'MONTHLY',
    [allowance] decimal(18,2) DEFAULT 0,
    [bonus] decimal(18,2) DEFAULT 0,
    [penalty] decimal(18,2) DEFAULT 0,
    [tax_rate] decimal(5,2) DEFAULT 0,
    [effective_date] datetime2 NOT NULL,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_employee_salaries] PRIMARY KEY ([id]),
    CONSTRAINT [FK_employee_salaries_employees] FOREIGN KEY ([employee_id]) REFERENCES [dbo].[employees]([id])
);
GO

-- Employee Shifts table
CREATE TABLE [dbo].[employee_shifts] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [employee_id] bigint NOT NULL,
    [shift_date] datetime2 NOT NULL,
    [start_time] time NOT NULL,
    [end_time] time NOT NULL,
    [status] varchar(20) DEFAULT 'SCHEDULED',
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_employee_shifts] PRIMARY KEY ([id]),
    CONSTRAINT [FK_employee_shifts_employees] FOREIGN KEY ([employee_id]) REFERENCES [dbo].[employees]([id])
);
GO

-- Branch Ingredient Inventory table
CREATE TABLE [dbo].[branch_ingredient_inventory] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [branch_id] bigint NOT NULL,
    [ingredient_id] bigint NOT NULL,
    [quantity] decimal(10,3) NOT NULL DEFAULT 0,
    [reserved_quantity] decimal(10,3) NOT NULL DEFAULT 0,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_branch_ingredient_inventory] PRIMARY KEY ([id]),
    CONSTRAINT [FK_branch_ingredient_inventory_branches] FOREIGN KEY ([branch_id]) REFERENCES [dbo].[branches]([id]),
    CONSTRAINT [FK_branch_ingredient_inventory_ingredients] FOREIGN KEY ([ingredient_id]) REFERENCES [dbo].[ingredients]([id]),
    CONSTRAINT [UQ_branch_ingredient_inventory] UNIQUE ([branch_id], [ingredient_id])
);
GO

-- Ingredient Warehouse table
CREATE TABLE [dbo].[ingredient_warehouse] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [ingredient_id] bigint NOT NULL,
    [quantity] decimal(18,2) NOT NULL DEFAULT 0,
    [safety_stock] decimal(18,2) NOT NULL DEFAULT 0,
    [maximum_stock] decimal(18,2) NULL,
    [location] nvarchar(100) NULL,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_ingredient_warehouse] PRIMARY KEY ([id]),
    CONSTRAINT [FK_ingredient_warehouse_ingredients] FOREIGN KEY ([ingredient_id]) REFERENCES [dbo].[ingredients]([id]),
    CONSTRAINT [UQ_ingredient_warehouse] UNIQUE ([ingredient_id])
);
GO

-- Inventory Thresholds table
CREATE TABLE [dbo].[inventory_thresholds] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [ingredient_id] bigint NOT NULL,
    [branch_id] bigint,
    [safety_stock] decimal(10,3) NOT NULL,
    [reorder_point] decimal(10,3),
    [maximum_stock] decimal(10,3),
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_inventory_thresholds] PRIMARY KEY ([id]),
    CONSTRAINT [FK_inventory_thresholds_ingredients] FOREIGN KEY ([ingredient_id]) REFERENCES [dbo].[ingredients]([id]),
    CONSTRAINT [FK_inventory_thresholds_branches] FOREIGN KEY ([branch_id]) REFERENCES [dbo].[branches]([id])
);
GO

-- Inventory Movements table
CREATE TABLE [dbo].[inventory_movements] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [branch_id] bigint NOT NULL,
    [ingredient_id] bigint NOT NULL,
    [movement_type] nvarchar(20) COLLATE Vietnamese_CI_AS NOT NULL,
    [quantity] decimal(18,2) NOT NULL,
    [unit] nvarchar(50) COLLATE Vietnamese_CI_AS NOT NULL,
    [quantity_before] decimal(18,2) NOT NULL,
    [quantity_after] decimal(18,2) NOT NULL,
    [reference_type] nvarchar(100) COLLATE Vietnamese_CI_AS,
    [reference_id] bigint,
    [reference_code] nvarchar(100) COLLATE Vietnamese_CI_AS,
    [notes] nvarchar(500) COLLATE Vietnamese_CI_AS,
    [employee_id] bigint,
    [movement_date] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_inventory_movements] PRIMARY KEY ([id]),
    CONSTRAINT [FK_inventory_movements_branches] FOREIGN KEY ([branch_id]) REFERENCES [dbo].[branches]([id]),
    CONSTRAINT [FK_inventory_movements_ingredients] FOREIGN KEY ([ingredient_id]) REFERENCES [dbo].[ingredients]([id]),
    CONSTRAINT [FK_inventory_movements_employees] FOREIGN KEY ([employee_id]) REFERENCES [dbo].[employees]([id])
);
GO

-- Ingredient Transfers table
CREATE TABLE [dbo].[ingredient_transfers] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [ingredient_id] bigint NOT NULL,
    [branch_id] bigint NOT NULL,
    [quantity] decimal(10,3) NOT NULL,
    [transfer_type] varchar(50) NOT NULL DEFAULT 'IN',
    [note] nvarchar(1000) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_ingredient_transfers] PRIMARY KEY ([id]),
    CONSTRAINT [FK_ingredient_transfers_ingredients] FOREIGN KEY ([ingredient_id]) REFERENCES [dbo].[ingredients]([id]),
    CONSTRAINT [FK_ingredient_transfers_branches] FOREIGN KEY ([branch_id]) REFERENCES [dbo].[branches]([id])
);
GO

-- Ingredient Transfer Requests table
CREATE TABLE [dbo].[ingredient_transfer_requests] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [branch_id] bigint NOT NULL,
    [request_number] varchar(50) NOT NULL,
    [request_date] datetime2(6) NOT NULL,
    [required_date] datetime2(6) NOT NULL,
    [status] varchar(20) NOT NULL DEFAULT 'PENDING',
    [total_items] int NOT NULL,
    [approved_date] datetime2(6),
    [completed_date] datetime2(6),
    [note] nvarchar(500) COLLATE Vietnamese_CI_AS,
    [requested_by] nvarchar(100) COLLATE Vietnamese_CI_AS NOT NULL,
    [approved_by] nvarchar(100) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_ingredient_transfer_requests] PRIMARY KEY ([id]),
    CONSTRAINT [FK_ingredient_transfer_requests_branches] FOREIGN KEY ([branch_id]) REFERENCES [dbo].[branches]([id])
);
GO

-- Ingredient Transfer Request Details table
CREATE TABLE [dbo].[ingredient_transfer_request_details] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [transfer_request_id] bigint NOT NULL,
    [ingredient_id] bigint NOT NULL,
    [requested_quantity] decimal(18,2) NOT NULL,
    [approved_quantity] decimal(18,2),
    [transferred_quantity] decimal(18,2) NOT NULL DEFAULT 0,
    [status] varchar(20) NOT NULL DEFAULT 'PENDING',
    [note] nvarchar(255) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_ingredient_transfer_request_details] PRIMARY KEY ([id]),
    CONSTRAINT [FK_ingredient_transfer_request_details_requests] FOREIGN KEY ([transfer_request_id]) REFERENCES [dbo].[ingredient_transfer_requests]([id]),
    CONSTRAINT [FK_ingredient_transfer_request_details_ingredients] FOREIGN KEY ([ingredient_id]) REFERENCES [dbo].[ingredients]([id])
);
GO

-- Supplier Ingredient Prices table
CREATE TABLE [dbo].[supplier_ingredient_prices] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [supplier_id] bigint NOT NULL,
    [ingredient_id] bigint NOT NULL,
    [price] decimal(18,2) NOT NULL,
    [unit] nvarchar(20) COLLATE Vietnamese_CI_AS NOT NULL,
    [effective_date] datetime2 NOT NULL,
    [expired_date] datetime2,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_supplier_ingredient_prices] PRIMARY KEY ([id]),
    CONSTRAINT [FK_supplier_ingredient_prices_suppliers] FOREIGN KEY ([supplier_id]) REFERENCES [dbo].[suppliers]([id]),
    CONSTRAINT [FK_supplier_ingredient_prices_ingredients] FOREIGN KEY ([ingredient_id]) REFERENCES [dbo].[ingredients]([id])
);
GO

-- Branch Expenses table
CREATE TABLE [dbo].[branch_expenses] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [branch_id] bigint NOT NULL,
    [expense_type] nvarchar(100) COLLATE Vietnamese_CI_AS NOT NULL,
    [amount] decimal(18,2) NOT NULL,
    [start_date] datetime2 NOT NULL,
    [end_date] datetime2,
    [payment_cycle] varchar(20) DEFAULT 'MONTHLY',
    [note] nvarchar(1000) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_branch_expenses] PRIMARY KEY ([id]),
    CONSTRAINT [FK_branch_expenses_branches] FOREIGN KEY ([branch_id]) REFERENCES [dbo].[branches]([id])
);
GO

-- Customers table
CREATE TABLE [dbo].[customers] (
    [id] bigint NOT NULL,
    [user_id] bigint NOT NULL,
    [fullname] nvarchar(255) COLLATE Vietnamese_CI_AS NOT NULL,
    [phone_number] varchar(20),
    [email] varchar(255),
    [address] nvarchar(500) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_customers] PRIMARY KEY ([id]),
    CONSTRAINT [FK_customers_users] FOREIGN KEY ([user_id]) REFERENCES [dbo].[users]([id])
);
GO

-- Orders table
CREATE TABLE [dbo].[orders] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [order_uuid] varchar(36) NOT NULL,
    [order_code] varchar(20) NOT NULL,
    [customer_id] bigint NOT NULL,
    [branch_id] bigint,
    [total_money] decimal(18,2),
    [status_id] bigint,
    [notes] nvarchar(500) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_orders] PRIMARY KEY ([id]),
    CONSTRAINT [UQ_orders_uuid] UNIQUE ([order_uuid]),
    CONSTRAINT [UQ_orders_code] UNIQUE ([order_code]),
    CONSTRAINT [FK_orders_customers] FOREIGN KEY ([customer_id]) REFERENCES [dbo].[customers]([id]),
    CONSTRAINT [FK_orders_branches] FOREIGN KEY ([branch_id]) REFERENCES [dbo].[branches]([id]),
    CONSTRAINT [FK_orders_statuses] FOREIGN KEY ([status_id]) REFERENCES [dbo].[order_statuses]([id])
);
GO

-- Order Details table

-- Order Details table
CREATE TABLE [dbo].[order_details] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [quantity] int NOT NULL,
    [order_id] bigint NOT NULL,
    [product_id] bigint NOT NULL,
    [size] nvarchar(50) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [note] nvarchar(1000) COLLATE Vietnamese_CI_AS,
    [total_amount] decimal(18,2) NOT NULL,
    [unit_price] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_order_details] PRIMARY KEY ([id]),
    CONSTRAINT [FK_order_details_orders] FOREIGN KEY ([order_id]) REFERENCES [dbo].[orders]([id]),
    CONSTRAINT [FK_order_details_products] FOREIGN KEY ([product_id]) REFERENCES [dbo].[products]([id])
);
GO
-- Order Payments table
CREATE TABLE [dbo].[order_payments] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [order_id] bigint NOT NULL,
    [payment_method_id] bigint NOT NULL,
    [payment_status_id] bigint NOT NULL,
    [amount] decimal(18,2) NOT NULL,
    [payment_date] datetime2(6),
    [transaction_id] nvarchar(255),
    [notes] nvarchar(1000) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_order_payments] PRIMARY KEY ([id]),
    CONSTRAINT [FK_order_payments_orders] FOREIGN KEY ([order_id]) REFERENCES [dbo].[orders]([id]),
    CONSTRAINT [FK_order_payments_methods] FOREIGN KEY ([payment_method_id]) REFERENCES [dbo].[payment_methods]([id]),
    CONSTRAINT [FK_order_payments_statuses] FOREIGN KEY ([payment_status_id]) REFERENCES [dbo].[payment_statuses]([id])
);
GO

-- Order Shipments table
CREATE TABLE [dbo].[order_shipments] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [order_id] bigint NOT NULL,
    [shipping_provider_id] bigint NOT NULL,
    [shipping_address] nvarchar(500) COLLATE Vietnamese_CI_AS NOT NULL,
    [shipping_cost] decimal(18,2),
    [shipping_method] nvarchar(100) COLLATE Vietnamese_CI_AS,
    [estimated_delivery_date] datetime2(6),
    [notes] nvarchar(1000) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_order_shipments] PRIMARY KEY ([id]),
    CONSTRAINT [FK_order_shipments_orders] FOREIGN KEY ([order_id]) REFERENCES [dbo].[orders]([id]),
    CONSTRAINT [FK_order_shipments_providers] FOREIGN KEY ([shipping_provider_id]) REFERENCES [dbo].[shipping_providers]([id])
);
GO

-- Order Delivery Tracking table
CREATE TABLE [dbo].[order_delivery_tracking] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [order_id] bigint NOT NULL,
    [tracking_number] nvarchar(100),
    [status_id] bigint NOT NULL,
    [location] nvarchar(255) COLLATE Vietnamese_CI_AS,
    [estimated_delivery] datetime2(6),
    [delivery_person_id] bigint,
    [shipping_provider_id] bigint NOT NULL,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_order_delivery_tracking] PRIMARY KEY ([id]),
    CONSTRAINT [FK_order_delivery_tracking_orders] FOREIGN KEY ([order_id]) REFERENCES [dbo].[orders]([id]),
    CONSTRAINT [FK_order_delivery_tracking_statuses] FOREIGN KEY ([status_id]) REFERENCES [dbo].[delivery_statuses]([id]),
    CONSTRAINT [FK_order_delivery_tracking_employees] FOREIGN KEY ([delivery_person_id]) REFERENCES [dbo].[employees]([id]),
    CONSTRAINT [FK_order_delivery_tracking_providers] FOREIGN KEY ([shipping_provider_id]) REFERENCES [dbo].[shipping_providers]([id])
);
GO

-- ====================================================================
-- CREATE TABLES - ADVANCED BUSINESS LOGIC TABLES
-- ====================================================================

-- Sales Summary table
CREATE TABLE [dbo].[sales_summary] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [branch_id] bigint NOT NULL,
    [period_type] varchar(20) NOT NULL,
    [period_value] nvarchar(50) COLLATE Vietnamese_CI_AS NOT NULL,
    [total_orders] int DEFAULT 0,
    [total_products] int DEFAULT 0,
    [revenue_before_tax] decimal(18,2) DEFAULT 0,
    [revenue_after_tax] decimal(18,2) DEFAULT 0,
    [tax_amount] decimal(18,2) DEFAULT 0,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_sales_summary] PRIMARY KEY ([id]),
    CONSTRAINT [FK_sales_summary_branches] FOREIGN KEY ([branch_id]) REFERENCES [dbo].[branches]([id])
);
GO

-- Expenses Summary table
CREATE TABLE [dbo].[cogs_summary] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [branch_id] bigint NOT NULL,
    [period_type] varchar(20) NOT NULL,
    [period_value] nvarchar(50) COLLATE Vietnamese_CI_AS NOT NULL,
    [total_purchase_orders] int DEFAULT 0,
    [total_ingredients] int DEFAULT 0,
    [expense_before_tax] decimal(18,2) DEFAULT 0,
    [expense_after_tax] decimal(18,2) DEFAULT 0,
    [tax_amount] decimal(18,2) DEFAULT 0,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_cogs_summary] PRIMARY KEY ([id]),
    CONSTRAINT [FK_cogs_summary_branches] FOREIGN KEY ([branch_id]) REFERENCES [dbo].[branches]([id])
);
GO

-- Profit Summary table
CREATE TABLE [dbo].[profit_summary] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [branch_id] bigint NOT NULL,
    [period_type] varchar(20) NOT NULL,
    [period_value] nvarchar(50) COLLATE Vietnamese_CI_AS NOT NULL,
    [revenue_before_tax] decimal(18,2) DEFAULT 0,
    [revenue_after_tax] decimal(18,2) DEFAULT 0,
    [expense_before_tax] decimal(18,2) DEFAULT 0,
    [expense_after_tax] decimal(18,2) DEFAULT 0,
    [output_tax] decimal(18,2) DEFAULT 0,
    [input_tax] decimal(18,2) DEFAULT 0,
    [vat_to_pay] decimal(18,2) DEFAULT 0,
    [profit_before_tax] decimal(18,2) DEFAULT 0,
    [profit_after_tax] decimal(18,2) DEFAULT 0,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_profit_summary] PRIMARY KEY ([id]),
    CONSTRAINT [FK_profit_summary_branches] FOREIGN KEY ([branch_id]) REFERENCES [dbo].[branches]([id])
);
GO

-- ====================================================================
-- CREATE TABLES - PURCHASE MANAGEMENT TABLES
-- ====================================================================

-- Purchase Invoices table
CREATE TABLE [dbo].[purchase_invoices] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [invoice_code] nvarchar(100) COLLATE Vietnamese_CI_AS NOT NULL UNIQUE,
    [purchase_order_id] bigint NOT NULL,
    [supplier_id] bigint NOT NULL,
    [branch_id] bigint,
    [invoice_date] datetime2(6) NOT NULL,
    [due_date] datetime2(6),
    [status_id] bigint NOT NULL,
    [total_amount_before_tax] decimal(18,2),
    [total_tax_amount] decimal(18,2),
    [total_amount_after_tax] decimal(18,2),
    [paid_amount] decimal(18,2) DEFAULT 0,
    [remaining_amount] decimal(18,2),
    [discount_amount] decimal(18,2) DEFAULT 0,
    [payment_method] nvarchar(50) COLLATE Vietnamese_CI_AS,
    [note] nvarchar(1000) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_purchase_invoices] PRIMARY KEY ([id]),
    CONSTRAINT [FK_purchase_invoices_orders] FOREIGN KEY ([purchase_order_id]) REFERENCES [dbo].[ingredient_purchase_orders]([id]),
    CONSTRAINT [FK_purchase_invoices_suppliers] FOREIGN KEY ([supplier_id]) REFERENCES [dbo].[suppliers]([id]),
    CONSTRAINT [FK_purchase_invoices_branches] FOREIGN KEY ([branch_id]) REFERENCES [dbo].[branches]([id]),
    CONSTRAINT [FK_purchase_invoices_statuses] FOREIGN KEY ([status_id]) REFERENCES [dbo].[invoice_statuses]([id])
);
GO

-- Purchase Invoice Details table
CREATE TABLE [dbo].[purchase_invoice_details] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [invoice_id] bigint NOT NULL,
    [ingredient_id] bigint NOT NULL,
    [quantity] decimal(10,3) NOT NULL,
    [unit_price] decimal(18,2) NOT NULL,
    [amount_before_tax] decimal(18,2) NOT NULL,
    [tax_rate] decimal(5,2) DEFAULT 0,
    [tax_amount] decimal(18,2) DEFAULT 0,
    [amount_after_tax] decimal(18,2) NOT NULL,
    [discount_rate] decimal(5,2) DEFAULT 0,
    [discount_amount] decimal(18,2) DEFAULT 0,
    [final_amount] decimal(18,2) NOT NULL,
    [expiry_date] datetime2,
    [batch_number] nvarchar(100),
    [note] nvarchar(1000) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_purchase_invoice_details] PRIMARY KEY ([id]),
    CONSTRAINT [FK_purchase_invoice_details_invoices] FOREIGN KEY ([invoice_id]) REFERENCES [dbo].[purchase_invoices]([id]),
    CONSTRAINT [FK_purchase_invoice_details_ingredients] FOREIGN KEY ([ingredient_id]) REFERENCES [dbo].[ingredients]([id])
);
GO

-- Goods Received Notes table
CREATE TABLE [dbo].[goods_received_notes] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [grn_code] nvarchar(100) COLLATE Vietnamese_CI_AS NOT NULL UNIQUE,
    [purchase_order_id] bigint NOT NULL,
    [invoice_id] bigint,
    [supplier_id] bigint NOT NULL,
    [branch_id] bigint,
    [warehouse_staff_id] bigint,
    [received_date] datetime2(6) NOT NULL,
    [status_id] bigint NOT NULL,
    [total_quantity_ordered] decimal(10,3),
    [total_quantity_received] decimal(10,3),
    [total_quantity_rejected] decimal(10,3) DEFAULT 0,
    [delivery_note_number] nvarchar(100),
    [vehicle_number] nvarchar(50),
    [driver_name] nvarchar(255) COLLATE Vietnamese_CI_AS,
    [note] nvarchar(1000) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_goods_received_notes] PRIMARY KEY ([id]),
    CONSTRAINT [FK_goods_received_notes_orders] FOREIGN KEY ([purchase_order_id]) REFERENCES [dbo].[ingredient_purchase_orders]([id]),
    CONSTRAINT [FK_goods_received_notes_invoices] FOREIGN KEY ([invoice_id]) REFERENCES [dbo].[purchase_invoices]([id]),
    CONSTRAINT [FK_goods_received_notes_suppliers] FOREIGN KEY ([supplier_id]) REFERENCES [dbo].[suppliers]([id]),
    CONSTRAINT [FK_goods_received_notes_branches] FOREIGN KEY ([branch_id]) REFERENCES [dbo].[branches]([id]),
    CONSTRAINT [FK_goods_received_notes_employees] FOREIGN KEY ([warehouse_staff_id]) REFERENCES [dbo].[employees]([id]),
    CONSTRAINT [FK_goods_received_notes_statuses] FOREIGN KEY ([status_id]) REFERENCES [dbo].[goods_received_statuses]([id])
);
GO

-- Goods Received Details table
CREATE TABLE [dbo].[goods_received_details] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [grn_id] bigint NOT NULL,
    [ingredient_id] bigint NOT NULL,
    [ordered_quantity] decimal(10,3) NOT NULL,
    [received_quantity] decimal(10,3) NOT NULL,
    [rejected_quantity] decimal(10,3) DEFAULT 0,
    [quality_status] varchar(50) DEFAULT 'PENDING',
    [rejection_reason] nvarchar(500) COLLATE Vietnamese_CI_AS,
    [unit_price] decimal(18,2),
    [expiry_date] datetime2,
    [batch_number] nvarchar(100),
    [storage_location] nvarchar(100),
    [note] nvarchar(1000) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_goods_received_details] PRIMARY KEY ([id]),
    CONSTRAINT [FK_goods_received_details_grn] FOREIGN KEY ([grn_id]) REFERENCES [dbo].[goods_received_notes]([id]),
    CONSTRAINT [FK_goods_received_details_ingredients] FOREIGN KEY ([ingredient_id]) REFERENCES [dbo].[ingredients]([id])
);
GO

-- Purchase Returns table
CREATE TABLE [dbo].[purchase_returns] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [return_code] nvarchar(100) COLLATE Vietnamese_CI_AS NOT NULL UNIQUE,
    [grn_id] bigint,
    [invoice_id] bigint,
    [supplier_id] bigint NOT NULL,
    [branch_id] bigint,
    [return_date] datetime2(6) NOT NULL,
    [return_reason] nvarchar(500) COLLATE Vietnamese_CI_AS NOT NULL,
    [status_id] bigint NOT NULL,
    [total_return_amount] decimal(18,2),
    [refund_amount] decimal(18,2) DEFAULT 0,
    [credit_note_number] nvarchar(100),
    [approved_by] bigint,
    [approval_date] datetime2(6),
    [note] nvarchar(1000) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_purchase_returns] PRIMARY KEY ([id]),
    CONSTRAINT [FK_purchase_returns_grn] FOREIGN KEY ([grn_id]) REFERENCES [dbo].[goods_received_notes]([id]),
    CONSTRAINT [FK_purchase_returns_invoices] FOREIGN KEY ([invoice_id]) REFERENCES [dbo].[purchase_invoices]([id]),
    CONSTRAINT [FK_purchase_returns_suppliers] FOREIGN KEY ([supplier_id]) REFERENCES [dbo].[suppliers]([id]),
    CONSTRAINT [FK_purchase_returns_branches] FOREIGN KEY ([branch_id]) REFERENCES [dbo].[branches]([id]),
    CONSTRAINT [FK_purchase_returns_statuses] FOREIGN KEY ([status_id]) REFERENCES [dbo].[purchase_order_statuses]([id]),
    CONSTRAINT [FK_purchase_returns_employees] FOREIGN KEY ([approved_by]) REFERENCES [dbo].[employees]([id])
);
GO

-- Purchase Return Details table
CREATE TABLE [dbo].[purchase_return_details] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [return_id] bigint NOT NULL,
    [ingredient_id] bigint NOT NULL,
    [return_quantity] decimal(10,3) NOT NULL,
    [unit_price] decimal(18,2),
    [return_amount] decimal(18,2),
    [return_reason] nvarchar(500) COLLATE Vietnamese_CI_AS,
    [batch_number] nvarchar(100),
    [expiry_date] datetime2,
    [quality_issue] nvarchar(500) COLLATE Vietnamese_CI_AS,
    [note] nvarchar(1000) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_purchase_return_details] PRIMARY KEY ([id]),
    CONSTRAINT [FK_purchase_return_details_returns] FOREIGN KEY ([return_id]) REFERENCES [dbo].[purchase_returns]([id]),
    CONSTRAINT [FK_purchase_return_details_ingredients] FOREIGN KEY ([ingredient_id]) REFERENCES [dbo].[ingredients]([id])
);
GO

-- Supplier Performance table
CREATE TABLE [dbo].[supplier_performance] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [supplier_id] bigint NOT NULL,
    [evaluation_period] varchar(20) NOT NULL,
    [period_value] nvarchar(50) COLLATE Vietnamese_CI_AS NOT NULL,
    [total_orders] int DEFAULT 0,
    [total_amount] decimal(18,2) DEFAULT 0,
    [on_time_deliveries] int DEFAULT 0,
    [late_deliveries] int DEFAULT 0,
    [quality_score] decimal(3,1) DEFAULT 0,
    [service_score] decimal(3,1) DEFAULT 0,
    [overall_rating] decimal(3,1) DEFAULT 0,
    [total_returns] int DEFAULT 0,
    [return_value] decimal(18,2) DEFAULT 0,
    [comments] nvarchar(1000) COLLATE Vietnamese_CI_AS,
    [created_at] datetime2(6) NOT NULL DEFAULT GETDATE(),
    [last_modified] datetime2(6) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_supplier_performance] PRIMARY KEY ([id]),
    CONSTRAINT [FK_supplier_performance_suppliers] FOREIGN KEY ([supplier_id]) REFERENCES [dbo].[suppliers]([id])
);
GO

-- ====================================================================
-- CREATE VIEWS
-- ====================================================================

-- Sales Summary View
CREATE VIEW [dbo].[v_sales_summary] AS
SELECT 
    [branch_id],
    YEAR([created_at]) as [year],
    MONTH([created_at]) as [month],
    [period_value] as [period],
    [total_orders],
    [total_products],
    [revenue_before_tax],
    [revenue_after_tax],
    [tax_amount]
FROM [dbo].[sales_summary]
WHERE [period_type] = 'MONTH';
GO

-- Employee Payroll View
CREATE VIEW [dbo].[v_employee_payroll] AS
SELECT 
    e.[id] as [employee_id],
    e.[full_name],
    b.[name] as [branch_name],
    e.[position_id],
    s.[base_salary],
    s.[salary_type],
    s.[allowance] as [total_allowances],
    s.[bonus] as [total_bonus],
    s.[penalty] as [total_deductions],
    (s.[base_salary] + s.[allowance] + s.[bonus] - s.[penalty]) as [gross_salary],
    s.[effective_date],
    NULL as [end_date]
FROM [dbo].[employees] e
INNER JOIN [dbo].[branches] b ON e.[branch_id] = b.[id]
LEFT JOIN [dbo].[employee_salaries] s ON e.[id] = s.[employee_id];
GO

-- Inventory Status View
CREATE VIEW [dbo].[v_inventory_status] AS
SELECT 
    i.[id] as [ingredient_id],
    i.[name] as [ingredient_name],
    COALESCE(bi.[branch_id], wh.[id]) as [location_id],
    CASE 
        WHEN bi.[branch_id] IS NOT NULL THEN CONCAT('Chi nhánh ', b.[name])
        ELSE 'Kho chính'
    END as [location_name],
    bi.[branch_id],
    b.[name] as [branch_name],
    COALESCE(bi.[quantity], wh.[quantity], 0) as [quantity_on_hand],
    0 as [quantity_reserved],
    COALESCE(bi.[quantity], wh.[quantity], 0) as [available_quantity],
    COALESCE(t.[safety_stock], 0) as [safety_stock],
    CASE 
        WHEN COALESCE(bi.[quantity], wh.[quantity], 0) <= COALESCE(t.[safety_stock], 0) THEN 'LOW_STOCK'
        WHEN COALESCE(bi.[quantity], wh.[quantity], 0) = 0 THEN 'OUT_OF_STOCK'
        ELSE 'IN_STOCK'
    END as [stock_status],
    i.[unit] as [unit_of_measure],
    COALESCE(bi.[last_modified], wh.[last_modified]) as [last_updated]
FROM [dbo].[ingredients] i
LEFT JOIN [dbo].[branch_ingredient_inventory] bi ON i.[id] = bi.[ingredient_id]
LEFT JOIN [dbo].[ingredient_warehouse] wh ON i.[id] = wh.[ingredient_id] AND bi.[ingredient_id] IS NULL
LEFT JOIN [dbo].[branches] b ON bi.[branch_id] = b.[id]
LEFT JOIN [dbo].[inventory_thresholds] t ON i.[id] = t.[ingredient_id] AND (t.[branch_id] = bi.[branch_id] OR t.[branch_id] IS NULL);
GO

-- Profit Summary View
CREATE VIEW [dbo].[v_profit_summary] AS
SELECT 
    [branch_id],
    YEAR([created_at]) as [year],
    MONTH([created_at]) as [month],
    [period_value] as [period],
    [revenue_before_tax],
    [revenue_after_tax],
    [expense_before_tax],
    [expense_after_tax],
    [output_tax],
    [input_tax],
    [vat_to_pay],
    [profit_before_tax],
    [profit_after_tax]
FROM [dbo].[profit_summary]
WHERE [period_type] = 'MONTH';
GO

-- Expenses Summary View
CREATE VIEW [dbo].[v_cogs_summary] AS
SELECT 
    [branch_id],
    YEAR([created_at]) as [year],
    MONTH([created_at]) as [month],
    [period_value] as [period],
    [total_purchase_orders],
    [total_ingredients],
    [expense_before_tax],
    [expense_after_tax],
    [tax_amount]
FROM [dbo].[cogs_summary]
WHERE [period_type] = 'MONTH';
GO

-- ====================================================================
-- CREATE INDEXES FOR PERFORMANCE
-- ====================================================================

-- Index for frequently queried columns
CREATE INDEX IX_orders_branch_id ON [dbo].[orders]([branch_id]);
CREATE INDEX IX_orders_customer_id ON [dbo].[orders]([customer_id]);
CREATE INDEX IX_orders_created_at ON [dbo].[orders]([created_at]);
CREATE INDEX IX_ingredient_purchase_orders_supplier_id ON [dbo].[ingredient_purchase_orders]([supplier_id]);
CREATE INDEX IX_ingredient_purchase_orders_branch_id ON [dbo].[ingredient_purchase_orders]([branch_id]);
CREATE INDEX IX_ingredient_purchase_orders_order_date ON [dbo].[ingredient_purchase_orders]([order_date]);
CREATE INDEX IX_users_phone_number ON [dbo].[users]([phone_number]);
CREATE INDEX IX_users_fullname ON [dbo].[users]([fullname]);
CREATE INDEX IX_employees_branch_id ON [dbo].[employees]([branch_id]);
CREATE INDEX IX_products_category_id ON [dbo].[products]([category_id]);
CREATE INDEX IX_ingredients_category_id ON [dbo].[ingredients]([category_id]);

-- Indexes for new tables
CREATE INDEX IX_permissions_name ON [dbo].[permissions]([name]);
CREATE INDEX IX_permissions_resource_action ON [dbo].[permissions]([resource], [action]);
CREATE INDEX IX_role_permissions_role_id ON [dbo].[role_permissions]([role_id]);
CREATE INDEX IX_role_permissions_permission_id ON [dbo].[role_permissions]([permission_id]);
CREATE INDEX IX_recipes_product_id ON [dbo].[recipes]([product_id]);
CREATE INDEX IX_recipes_is_active ON [dbo].[recipes]([is_active]);
CREATE INDEX IX_recipe_ingredients_recipe_id ON [dbo].[recipe_ingredients]([recipe_id]);
CREATE INDEX IX_recipe_ingredients_ingredient_id ON [dbo].[recipe_ingredients]([ingredient_id]);
CREATE INDEX IX_inventory_movements_branch_id ON [dbo].[inventory_movements]([branch_id]);
CREATE INDEX IX_inventory_movements_ingredient_id ON [dbo].[inventory_movements]([ingredient_id]);
CREATE INDEX IX_inventory_movements_movement_date ON [dbo].[inventory_movements]([movement_date]);
CREATE INDEX IX_inventory_movements_movement_type ON [dbo].[inventory_movements]([movement_type]);
CREATE INDEX IX_ingredient_transfer_requests_branch_id ON [dbo].[ingredient_transfer_requests]([branch_id]);
CREATE INDEX IX_ingredient_transfer_requests_status ON [dbo].[ingredient_transfer_requests]([status]);
CREATE INDEX IX_ingredient_transfer_requests_request_date ON [dbo].[ingredient_transfer_requests]([request_date]);
CREATE INDEX IX_ingredient_transfer_request_details_transfer_request_id ON [dbo].[ingredient_transfer_request_details]([transfer_request_id]);
CREATE INDEX IX_ingredient_transfer_request_details_ingredient_id ON [dbo].[ingredient_transfer_request_details]([ingredient_id]);

PRINT N'Database schema created successfully with Unicode support!';
GO
