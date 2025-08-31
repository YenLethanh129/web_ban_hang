CREATE TABLE [dbo].[profit_summary] (
[id] bigint NOT NULL IDENTITY(1,1),
[branch_id] bigint,
[period_type] varchar(20) NOT NULL,
[period_value] varchar(20) NOT NULL,
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

CREATE TABLE [dbo].[recipe_ingredients] (
[id] bigint NOT NULL IDENTITY(1,1),
[RecipeId] bigint NOT NULL,
[IngredientId] bigint NOT NULL,
[Quantity] decimal(18,4) NOT NULL,
[Unit] nvarchar(50) NOT NULL,
[WastePercentage] decimal(18,4),
[Notes] nvarchar(500),
[IsOptional] bit NOT NULL,
[SortOrder] int NOT NULL,
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[users] (
[id] bigint NOT NULL IDENTITY(1,1),
[employee_id] bigint,
[date_of_birth] date,
[facebook_account_id] bigint,
[google_account_id] bigint,
[is_active] bit DEFAULT (CONVERT([bit],(1))) NOT NULL,
[role_id] bigint NOT NULL,
[phone_number] varchar(20) NOT NULL,
[fullname] nvarchar(100),
[address] nvarchar(200),
[password] varchar(200) NOT NULL,
[created_at] datetime2(6) DEFAULT (sysdatetime()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[expenses_summary] (
[id] bigint NOT NULL IDENTITY(1,1),
[branch_id] bigint,
[period_type] varchar(20) NOT NULL,
[period_value] varchar(20) NOT NULL,
[total_purchase_orders] int NOT NULL,
[total_ingredients] int NOT NULL,
[expense_before_tax] decimal(18,2) NOT NULL,
[expense_after_tax] decimal(18,2) NOT NULL,
[tax_amount] decimal(18,2) NOT NULL,
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[shipping_providers] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] varchar(100) NOT NULL,
[contact_info] varchar(200),
[api_endpoint] varchar(200),
[created_at] datetime2(6) DEFAULT (sysdatetime()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[employee_shifts] (
[id] bigint NOT NULL IDENTITY(1,1),
[employee_id] bigint NOT NULL,
[shift_date] date NOT NULL,
[start_time] time(7) NOT NULL,
[end_time] time(7) NOT NULL,
[status] varchar(20) DEFAULT ('PRESENT'),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[supplier_performance] (
[id] bigint NOT NULL IDENTITY(1,1),
[supplier_id] bigint NOT NULL,
[evaluation_period] varchar(20) NOT NULL,
[period_value] varchar(20) NOT NULL,
[total_orders] int DEFAULT ((0)),
[total_amount] decimal(18,2) DEFAULT ((0.0)),
[on_time_deliveries] int DEFAULT ((0)),
[late_deliveries] int DEFAULT ((0)),
[quality_score] decimal(3,2) DEFAULT ((0.0)),
[service_score] decimal(3,2) DEFAULT ((0.0)),
[overall_rating] decimal(3,2) DEFAULT ((0.0)),
[total_returns] int DEFAULT ((0)),
[return_value] decimal(18,2) DEFAULT ((0.0)),
[comments] varchar(500),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[employee_salaries] (
[id] bigint NOT NULL IDENTITY(1,1),
[employee_id] bigint NOT NULL,
[base_salary] decimal(18,2) NOT NULL,
[salary_type] varchar(20) DEFAULT ('MONTHLY'),
[allowance] decimal(18,2) DEFAULT ((0.0)),
[bonus] decimal(18,2) DEFAULT ((0.0)),
[penalty] decimal(18,2) DEFAULT ((0.0)),
[tax_rate] decimal(18,2) DEFAULT ((0.1)),
[effective_date] date NOT NULL,
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[goods_received_notes] (
[id] bigint NOT NULL IDENTITY(1,1),
[grn_code] varchar(50) NOT NULL,
[purchase_order_id] bigint,
[invoice_id] bigint,
[supplier_id] bigint NOT NULL,
[branch_id] bigint NOT NULL,
[warehouse_staff_id] bigint,
[received_date] datetime2(6) DEFAULT (getdate()),
[status_id] bigint DEFAULT (CONVERT([bigint],(1))),
[total_quantity_ordered] decimal(18,2) DEFAULT ((0.0)),
[total_quantity_received] decimal(18,2) DEFAULT ((0.0)),
[total_quantity_rejected] decimal(18,2) DEFAULT ((0.0)),
[delivery_note_number] varchar(100),
[vehicle_number] varchar(20),
[driver_name] varchar(100),
[note] varchar(500),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[categories] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(255) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[employees] (
[id] bigint NOT NULL IDENTITY(1,1),
[branch_id] bigint NOT NULL,
[full_name] nvarchar(100) NOT NULL,
[phone] varchar(20),
[email] varchar(100),
[position] nvarchar(50),
[hire_date] date NOT NULL,
[resign_date] date,
[status] varchar(20) DEFAULT ('ACTIVE'),
[created_at] datetime2(6) DEFAULT (sysdatetime()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[payment_statuses] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] varchar(50) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[order_delivery_tracking] (
[id] bigint NOT NULL IDENTITY(1,1),
[order_id] bigint NOT NULL,
[tracking_number] varchar(100) NOT NULL,
[status_id] bigint NOT NULL,
[location] varchar(255),
[estimated_delivery] datetime2(6),
[delivery_person_id] bigint,
[shipping_provider_id] bigint,
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) DEFAULT (sysdatetime()) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[supplier_ingredient_prices] (
[id] bigint NOT NULL IDENTITY(1,1),
[supplier_id] bigint NOT NULL,
[ingredient_id] bigint NOT NULL,
[price] decimal(18,2) NOT NULL,
[unit] varchar(50) NOT NULL,
[effective_date] datetime2(6) DEFAULT (getdate()),
[expired_date] datetime2(6),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[ingredient_categories] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(50) NOT NULL,
[description] nvarchar(255),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[ingredients] (
[id] bigint NOT NULL IDENTITY(1,1),
[category_id] bigint NOT NULL,
[name] nvarchar(255) NOT NULL,
[unit] nvarchar(50) NOT NULL,
[is_active] bit DEFAULT (CONVERT([bit],(1))) NOT NULL,
[description] nvarchar(255),
[tax_id] bigint,
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) DEFAULT (getdate()) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[product_recipes] (
[id] bigint NOT NULL IDENTITY(1,1),
[product_id] bigint NOT NULL,
[ingredient_id] bigint NOT NULL,
[quantity] decimal(18,2) NOT NULL,
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[purchase_invoices] (
[id] bigint NOT NULL IDENTITY(1,1),
[invoice_code] varchar(50) NOT NULL,
[purchase_order_id] bigint,
[supplier_id] bigint NOT NULL,
[branch_id] bigint,
[invoice_date] datetime2(6) NOT NULL,
[due_date] datetime2(6),
[payment_date] datetime2(6),
[status_id] bigint DEFAULT (CONVERT([bigint],(1))),
[total_amount_before_tax] decimal(18,2) DEFAULT ((0.0)),
[total_tax_amount] decimal(18,2) DEFAULT ((0.0)),
[total_amount_after_tax] decimal(18,2) DEFAULT ((0.0)),
[paid_amount] decimal(18,2) DEFAULT ((0.0)),
[remaining_amount] decimal(18,2) DEFAULT ((0.0)),
[discount_amount] decimal(18,2) DEFAULT ((0.0)),
[payment_method] varchar(50),
[payment_reference] varchar(100),
[note] varchar(500),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[goods_received_statuses] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] varchar(50) NOT NULL,
[description] varchar(255),
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[purchase_invoice_details] (
[id] bigint NOT NULL IDENTITY(1,1),
[invoice_id] bigint NOT NULL,
[ingredient_id] bigint NOT NULL,
[quantity] decimal(18,2) NOT NULL,
[unit_price] decimal(18,2) NOT NULL,
[amount_before_tax] decimal(18,2) NOT NULL,
[tax_rate] decimal(5,2) DEFAULT ((0.0)),
[tax_amount] decimal(18,2) DEFAULT ((0.0)),
[amount_after_tax] decimal(18,2) NOT NULL,
[discount_rate] decimal(5,2) DEFAULT ((0.0)),
[discount_amount] decimal(18,2) DEFAULT ((0.0)),
[final_amount] decimal(18,2) NOT NULL,
[expiry_date] date,
[batch_number] varchar(50),
[note] varchar(255),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[order_shipments] (
[id] bigint NOT NULL IDENTITY(1,1),
[order_id] bigint NOT NULL,
[shipping_provider_id] bigint,
[shipping_address] nvarchar(500) NOT NULL,
[shipping_cost] decimal(18,2),
[shipping_method] varchar(50),
[estimated_delivery_date] datetime2(6),
[notes] varchar(255),
[created_at] datetime2(6) DEFAULT (sysdatetime()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[purchase_return_details] (
[id] bigint NOT NULL IDENTITY(1,1),
[return_id] bigint NOT NULL,
[ingredient_id] bigint NOT NULL,
[return_quantity] decimal(18,2) NOT NULL,
[unit_price] decimal(18,2),
[return_amount] decimal(18,2),
[return_reason] varchar(255),
[batch_number] varchar(50),
[expiry_date] date,
[quality_issue] varchar(255),
[note] varchar(255),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[v_employee_payroll] (
[employee_id] bigint NOT NULL,
[full_name] nvarchar(100) NOT NULL,
[branch_name] varchar(255) NOT NULL,
[position_name] varchar(100),
[base_salary] decimal(18,2),
[salary_type] varchar(20),
[total_allowances] decimal(18,2) NOT NULL,
[total_bonus] decimal(18,2) NOT NULL,
[total_deductions] decimal(18,2) NOT NULL,
[gross_salary] decimal(18,2) NOT NULL,
[effective_date] date,
[end_date] date
);

CREATE TABLE [dbo].[v_inventory_status] (
[ingredient_id] bigint NOT NULL,
[ingredient_name] varchar(255) NOT NULL,
[location_id] bigint NOT NULL,
[location_name] varchar(100) NOT NULL,
[branch_id] bigint NOT NULL,
[branch_name] varchar(255) NOT NULL,
[quantity_on_hand] decimal(18,2) NOT NULL,
[quantity_reserved] decimal(18,2) NOT NULL,
[available_quantity] decimal(18,2) NOT NULL,
[minimum_stock] decimal(18,2),
[stock_status] varchar(20) NOT NULL,
[unit_of_measure] varchar(50) NOT NULL,
[last_updated] datetime2(6) NOT NULL
);

CREATE TABLE [dbo].[branch_expenses] (
[id] bigint NOT NULL IDENTITY(1,1),
[branch_id] bigint NOT NULL,
[expense_type] nvarchar(100) NOT NULL,
[amount] decimal(18,2) NOT NULL,
[start_date] date NOT NULL,
[end_date] date,
[payment_cycle] varchar(50) DEFAULT ('MONTHLY'),
[note] nvarchar(255),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[orders] (
[id] bigint NOT NULL IDENTITY(1,1),
[order_uuid] char(36) NOT NULL,
[order_code] varchar(20) NOT NULL,
[customer_id] bigint NOT NULL,
[branch_id] bigint,
[total_money] decimal(18,2),
[status_id] bigint,
[notes] nvarchar(500),
[created_at] datetime2(6) DEFAULT (sysdatetime()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[__EFMigrationsHistory] (
[MigrationId] nvarchar(150) NOT NULL,
[ProductVersion] nvarchar(32) NOT NULL,
PRIMARY KEY ([MigrationId])
);

CREATE TABLE [dbo].[roles] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] varchar(100) NOT NULL,
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[ingredient_purchase_orders] (
[id] bigint NOT NULL IDENTITY(1,1),
[purchase_order_code] varchar(50) NOT NULL,
[supplier_id] bigint,
[branch_id] bigint,
[employee_id] bigint,
[order_date] datetime2(6) DEFAULT (getdate()),
[expected_delivery_date] datetime2(6),
[status_id] bigint DEFAULT (CONVERT([bigint],(1))),
[total_amount_before_tax] decimal(18,2) DEFAULT ((0.0)),
[total_tax_amount] decimal(18,2) DEFAULT ((0.0)),
[total_amount_after_tax] decimal(18,2) DEFAULT ((0.0)),
[discount_amount] decimal(18,2) DEFAULT ((0.0)),
[final_amount] decimal(18,2) DEFAULT ((0.0)),
[note] varchar(500),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[order_details] (
[id] bigint NOT NULL IDENTITY(1,1),
[quantity] int NOT NULL,
[order_id] bigint NOT NULL,
[product_id] bigint NOT NULL,
[color] varchar(255),
[note] varchar(255),
[total_amount] decimal(18,2) NOT NULL,
[unit_price] decimal(18,2) NOT NULL,
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

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

CREATE TABLE [dbo].[inventory_movements] (
[id] bigint NOT NULL IDENTITY(1,1),
[BranchId] bigint NOT NULL,
[IngredientId] bigint NOT NULL,
[MovementType] nvarchar(20) NOT NULL,
[Quantity] decimal(18,2) NOT NULL,
[Unit] nvarchar(50) NOT NULL,
[QuantityBefore] decimal(18,2) NOT NULL,
[QuantityAfter] decimal(18,2) NOT NULL,
[ReferenceType] nvarchar(100),
[ReferenceId] bigint,
[ReferenceCode] nvarchar(100),
[Notes] nvarchar(500),
[EmployeeId] bigint,
[MovementDate] datetime2(7) NOT NULL,
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[ingredient_transfer_request_details] (
[id] bigint NOT NULL IDENTITY(1,1),
[transfer_request_id] bigint NOT NULL,
[ingredient_id] bigint NOT NULL,
[requested_quantity] decimal(18,2) NOT NULL,
[approved_quantity] decimal(18,2),
[transferred_quantity] decimal(18,2) NOT NULL,
[status] varchar(20) NOT NULL,
[note] nvarchar(255),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[ingredient_transfer_requests] (
[id] bigint NOT NULL IDENTITY(1,1),
[branch_id] bigint NOT NULL,
[request_number] varchar(50) NOT NULL,
[request_date] datetime2(7) NOT NULL,
[required_date] datetime2(7) NOT NULL,
[status] varchar(20) NOT NULL,
[total_items] int NOT NULL,
[approved_date] datetime2(7),
[completed_date] datetime2(7),
[note] nvarchar(500),
[requested_by] nvarchar(100) NOT NULL,
[approved_by] nvarchar(100),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

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

CREATE TABLE [dbo].[taxes] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] varchar(100) NOT NULL,
[tax_rate] decimal(5,2) NOT NULL,
[description] varchar(255),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[product_images] (
[id] bigint NOT NULL IDENTITY(1,1),
[product_id] bigint,
[image_url] varchar(300),
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[payment_methods] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] varchar(50) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[delivery_statuses] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] varchar(50) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[recipes] (
[id] bigint NOT NULL IDENTITY(1,1),
[Name] nvarchar(255) NOT NULL,
[Description] nvarchar(500),
[ProductId] bigint NOT NULL,
[ServingSize] decimal(18,2) NOT NULL,
[Unit] nvarchar(50) NOT NULL,
[IsActive] bit NOT NULL,
[Notes] nvarchar(500),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[goods_received_details] (
[id] bigint NOT NULL IDENTITY(1,1),
[grn_id] bigint NOT NULL,
[ingredient_id] bigint NOT NULL,
[ordered_quantity] decimal(18,2) NOT NULL,
[received_quantity] decimal(18,2) NOT NULL,
[rejected_quantity] decimal(18,2) DEFAULT ((0.0)),
[quality_status] varchar(20) DEFAULT ('ACCEPTED'),
[rejection_reason] varchar(255),
[unit_price] decimal(18,2),
[expiry_date] date,
[batch_number] varchar(50),
[storage_location] varchar(100),
[note] varchar(255),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[products] (
[id] bigint NOT NULL IDENTITY(1,1),
[price] decimal(18,2) NOT NULL,
[category_id] bigint,
[is_active] bit DEFAULT (CONVERT([bit],(1))) NOT NULL,
[tax_id] bigint,
[description] nvarchar(255) NOT NULL,
[name] nvarchar(255) NOT NULL,
[thumbnail] varchar(255),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[branch_ingredient_inventory] (
[id] bigint NOT NULL IDENTITY(1,1),
[branch_id] bigint NOT NULL,
[ingredient_id] bigint NOT NULL,
[quantity] decimal(18,2) NOT NULL,
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_transfer_date] datetime2(7),
[location] nvarchar(100),
[minimum_stock] decimal(18,2) DEFAULT ((0.0)) NOT NULL,
[reserved_quantity] decimal(18,2) DEFAULT ((0.0)) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[ingredient_transfers] (
[id] bigint NOT NULL IDENTITY(1,1),
[ingredient_id] bigint NOT NULL,
[branch_id] bigint NOT NULL,
[quantity] decimal(18,2) NOT NULL,
[note] nvarchar(500),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
[approved_by] nvarchar(100),
[completed_date] datetime2(7),
[requested_by] nvarchar(100),
[status] varchar(20) DEFAULT ('') NOT NULL,
[transfer_date] datetime2(7) DEFAULT ('0001-01-01T00:00:00.0000000') NOT NULL,
[transfer_type] varchar(20) DEFAULT ('') NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[purchase_returns] (
[id] bigint NOT NULL IDENTITY(1,1),
[return_code] varchar(50) NOT NULL,
[grn_id] bigint,
[invoice_id] bigint,
[supplier_id] bigint NOT NULL,
[branch_id] bigint NOT NULL,
[return_date] datetime2(6) DEFAULT (getdate()),
[return_reason] varchar(255),
[status_id] bigint DEFAULT (CONVERT([bigint],(1))),
[total_return_amount] decimal(18,2) DEFAULT ((0.0)),
[refund_amount] decimal(18,2) DEFAULT ((0.0)),
[credit_note_number] varchar(100),
[approved_by] bigint,
[approval_date] datetime2(6),
[note] varchar(500),
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[ingredient_warehouse] (
[id] bigint NOT NULL IDENTITY(1,1),
[ingredient_id] bigint NOT NULL,
[quantity] decimal(18,2) NOT NULL,
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) DEFAULT (getdate()) NOT NULL,
[location] nvarchar(100),
[maximum_stock] decimal(18,2),
[minimum_stock] decimal(18,2) DEFAULT ((0.0)) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[v_expenses_summary] (
[branch_id] bigint,
[year] int NOT NULL,
[month] int NOT NULL,
[period] varchar(7) NOT NULL,
[total_purchase_orders] int NOT NULL,
[total_ingredients] decimal(18,2) NOT NULL,
[expense_before_tax] decimal(18,2) NOT NULL,
[expense_after_tax] decimal(18,2) NOT NULL,
[tax_amount] decimal(18,2) NOT NULL
);

CREATE TABLE [dbo].[v_profit_summary] (
[branch_id] bigint,
[year] int NOT NULL,
[month] int NOT NULL,
[period] varchar(7) NOT NULL,
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

CREATE TABLE [dbo].[suppliers] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(255) NOT NULL,
[phone] varchar(20),
[email] varchar(100),
[address] nvarchar(255),
[note] nvarchar(255),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[invoice_statuses] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] varchar(50) NOT NULL,
[description] varchar(255),
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[inventory_thresholds] (
[id] bigint NOT NULL IDENTITY(1,1),
[BranchId] bigint NOT NULL,
[IngredientId] bigint NOT NULL,
[MinimumStock] decimal(18,2) NOT NULL,
[ReorderPoint] decimal(18,2) NOT NULL,
[MaximumStock] decimal(18,2) NOT NULL,
[MinimumThreshold] decimal(18,2) NOT NULL,
[LeadTimeDays] int NOT NULL,
[AverageDailyConsumption] decimal(18,2) NOT NULL,
[LastCalculatedDate] datetime2(7),
[IsActive] bit NOT NULL,
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[v_sales_summary] (
[branch_id] bigint,
[year] int NOT NULL,
[month] int NOT NULL,
[period] varchar(7) NOT NULL,
[total_orders] int NOT NULL,
[total_products] int NOT NULL,
[revenue_before_tax] decimal(18,2) NOT NULL,
[revenue_after_tax] decimal(18,2) NOT NULL,
[tax_amount] decimal(18,2) NOT NULL
);

CREATE TABLE [dbo].[order_payments] (
[id] bigint NOT NULL IDENTITY(1,1),
[order_id] bigint NOT NULL,
[payment_method_id] bigint NOT NULL,
[payment_status_id] bigint NOT NULL,
[amount] decimal(18,2) NOT NULL,
[payment_date] datetime2(6),
[transaction_id] varchar(100),
[notes] varchar(255),
[created_at] datetime2(6) DEFAULT (sysdatetime()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[purchase_order_statuses] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] varchar(50) NOT NULL,
[description] varchar(255),
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[customers] (
[id] bigint NOT NULL,
[user_id] bigint,
[fullname] nvarchar(100) NOT NULL,
[phone_number] varchar(20),
[email] varchar(100),
[address] nvarchar(200),
[created_at] datetime2(6) DEFAULT (sysdatetime()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[branches] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] nvarchar(255) NOT NULL,
[address] nvarchar(255),
[phone] varchar(20),
[manager] nvarchar(100),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[social_accounts] (
[id] bigint NOT NULL IDENTITY(1,1),
[provider_id] bigint NOT NULL,
[user_id] bigint,
[provider] varchar(20) NOT NULL,
[name] varchar(100),
[email] varchar(150),
[created_at] datetime2(6) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[order_statuses] (
[id] bigint NOT NULL IDENTITY(1,1),
[name] varchar(50) NOT NULL,
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[tokens] (
[id] bigint NOT NULL IDENTITY(1,1),
[expired] bit NOT NULL,
[revoked] bit NOT NULL,
[expiration_date] datetime2(6),
[user_id] bigint,
[token_type] varchar(50),
[token] varchar(255),
PRIMARY KEY ([id])
);

CREATE TABLE [dbo].[sales_summary] (
[id] bigint NOT NULL IDENTITY(1,1),
[branch_id] bigint,
[period_type] varchar(20) NOT NULL,
[period_value] varchar(20) NOT NULL,
[total_orders] int NOT NULL,
[total_products] int NOT NULL,
[revenue_before_tax] decimal(18,2) NOT NULL,
[revenue_after_tax] decimal(18,2) NOT NULL,
[tax_amount] decimal(18,2) NOT NULL,
[created_at] datetime2(6) DEFAULT (getdate()) NOT NULL,
[last_modified] datetime2(6) NOT NULL,
PRIMARY KEY ([id])
);


ALTER TABLE [dbo].[profit_summary]
ADD CONSTRAINT [FK_profit_summary_branches]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[recipe_ingredients]
ADD CONSTRAINT [FK_recipe_ingredients_ingredients_IngredientId]
FOREIGN KEY ([IngredientId]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[recipe_ingredients]
ADD CONSTRAINT [FK_recipe_ingredients_recipes_RecipeId]
FOREIGN KEY ([RecipeId]) 
REFERENCES [dbo].[recipes]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[users]
ADD CONSTRAINT [FK_users_employees]
FOREIGN KEY ([employee_id]) 
REFERENCES [dbo].[employees]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[users]
ADD CONSTRAINT [FK_users_roles]
FOREIGN KEY ([role_id]) 
REFERENCES [dbo].[roles]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[expenses_summary]
ADD CONSTRAINT [FK_expenses_summary_branches]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[employee_shifts]
ADD CONSTRAINT [FK_shifts_employee]
FOREIGN KEY ([employee_id]) 
REFERENCES [dbo].[employees]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[supplier_performance]
ADD CONSTRAINT [FK_performance_supplier]
FOREIGN KEY ([supplier_id]) 
REFERENCES [dbo].[suppliers]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[employee_salaries]
ADD CONSTRAINT [FK_salaries_employee]
FOREIGN KEY ([employee_id]) 
REFERENCES [dbo].[employees]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[goods_received_notes]
ADD CONSTRAINT [FK_grn_invoice]
FOREIGN KEY ([invoice_id]) 
REFERENCES [dbo].[purchase_invoices]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[goods_received_notes]
ADD CONSTRAINT [FK_grn_purchase_order]
FOREIGN KEY ([purchase_order_id]) 
REFERENCES [dbo].[ingredient_purchase_orders]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[goods_received_notes]
ADD CONSTRAINT [FK_grn_branch]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[goods_received_notes]
ADD CONSTRAINT [FK_grn_warehouse_staff]
FOREIGN KEY ([warehouse_staff_id]) 
REFERENCES [dbo].[employees]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[goods_received_notes]
ADD CONSTRAINT [FK_grn_supplier]
FOREIGN KEY ([supplier_id]) 
REFERENCES [dbo].[suppliers]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[goods_received_notes]
ADD CONSTRAINT [FK_grn_status]
FOREIGN KEY ([status_id]) 
REFERENCES [dbo].[goods_received_statuses]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[employees]
ADD CONSTRAINT [FK_employees_branch]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[order_delivery_tracking]
ADD CONSTRAINT [FK_tracking_employees]
FOREIGN KEY ([delivery_person_id]) 
REFERENCES [dbo].[employees]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[order_delivery_tracking]
ADD CONSTRAINT [FK_tracking_status]
FOREIGN KEY ([status_id]) 
REFERENCES [dbo].[delivery_statuses]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[order_delivery_tracking]
ADD CONSTRAINT [FK_tracking_orders]
FOREIGN KEY ([order_id]) 
REFERENCES [dbo].[orders]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[order_delivery_tracking]
ADD CONSTRAINT [FK_tracking_providers]
FOREIGN KEY ([shipping_provider_id]) 
REFERENCES [dbo].[shipping_providers]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[supplier_ingredient_prices]
ADD CONSTRAINT [FK_sip_ingredient]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[supplier_ingredient_prices]
ADD CONSTRAINT [FK_sip_supplier]
FOREIGN KEY ([supplier_id]) 
REFERENCES [dbo].[suppliers]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[ingredients]
ADD CONSTRAINT [FK_ingredients_ingredient_categories]
FOREIGN KEY ([category_id]) 
REFERENCES [dbo].[ingredient_categories]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[ingredients]
ADD CONSTRAINT [FK_ingredients_taxes]
FOREIGN KEY ([tax_id]) 
REFERENCES [dbo].[taxes]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[product_recipes]
ADD CONSTRAINT [FK_recipe_ingredient]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[product_recipes]
ADD CONSTRAINT [FK_recipe_product]
FOREIGN KEY ([product_id]) 
REFERENCES [dbo].[products]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[purchase_invoices]
ADD CONSTRAINT [FK_invoice_purchase_order]
FOREIGN KEY ([purchase_order_id]) 
REFERENCES [dbo].[ingredient_purchase_orders]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[purchase_invoices]
ADD CONSTRAINT [FK_invoice_status]
FOREIGN KEY ([status_id]) 
REFERENCES [dbo].[invoice_statuses]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[purchase_invoices]
ADD CONSTRAINT [FK_invoice_supplier]
FOREIGN KEY ([supplier_id]) 
REFERENCES [dbo].[suppliers]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[purchase_invoices]
ADD CONSTRAINT [FK_invoice_branch]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[purchase_invoice_details]
ADD CONSTRAINT [FK_invoice_detail_invoice]
FOREIGN KEY ([invoice_id]) 
REFERENCES [dbo].[purchase_invoices]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[purchase_invoice_details]
ADD CONSTRAINT [FK_invoice_detail_ingredient]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[order_shipments]
ADD CONSTRAINT [FK_order_shipments_orders]
FOREIGN KEY ([order_id]) 
REFERENCES [dbo].[orders]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[order_shipments]
ADD CONSTRAINT [FK_order_shipments_shipping_providers]
FOREIGN KEY ([shipping_provider_id]) 
REFERENCES [dbo].[shipping_providers]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[purchase_return_details]
ADD CONSTRAINT [FK_return_detail_ingredient]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[purchase_return_details]
ADD CONSTRAINT [FK_return_detail_return]
FOREIGN KEY ([return_id]) 
REFERENCES [dbo].[purchase_returns]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[v_employee_payroll]
ADD CONSTRAINT [FK_v_employee_payroll_employees_employee_id]
FOREIGN KEY ([employee_id]) 
REFERENCES [dbo].[employees]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[v_inventory_status]
ADD CONSTRAINT [FK_v_inventory_status_branches_branch_id]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[v_inventory_status]
ADD CONSTRAINT [FK_v_inventory_status_ingredients_ingredient_id]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[branch_expenses]
ADD CONSTRAINT [FK_branch_expenses_branches]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[orders]
ADD CONSTRAINT [FK_orders_branches]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[orders]
ADD CONSTRAINT [FK_orders_order_status]
FOREIGN KEY ([status_id]) 
REFERENCES [dbo].[order_statuses]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[orders]
ADD CONSTRAINT [FK_orders_customers]
FOREIGN KEY ([customer_id]) 
REFERENCES [dbo].[customers]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[ingredient_purchase_orders]
ADD CONSTRAINT [FK_purchase_order_status]
FOREIGN KEY ([status_id]) 
REFERENCES [dbo].[purchase_order_statuses]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[ingredient_purchase_orders]
ADD CONSTRAINT [FK_purchase_order_branch]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[ingredient_purchase_orders]
ADD CONSTRAINT [FK_purchase_order_supplier]
FOREIGN KEY ([supplier_id]) 
REFERENCES [dbo].[suppliers]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[ingredient_purchase_orders]
ADD CONSTRAINT [FK_purchase_order_employee]
FOREIGN KEY ([employee_id]) 
REFERENCES [dbo].[employees]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[order_details]
ADD CONSTRAINT [FKjyu2qbqt8gnvno9oe9j2s2ldk]
FOREIGN KEY ([order_id]) 
REFERENCES [dbo].[orders]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[order_details]
ADD CONSTRAINT [FK4q98utpd73imf4yhttm3w0eax]
FOREIGN KEY ([product_id]) 
REFERENCES [dbo].[products]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[ingredient_purchase_order_details]
ADD CONSTRAINT [FK_ipod_ingredients]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[ingredient_purchase_order_details]
ADD CONSTRAINT [FK_ipod_purchase_orders]
FOREIGN KEY ([purchase_order_id]) 
REFERENCES [dbo].[ingredient_purchase_orders]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[inventory_movements]
ADD CONSTRAINT [FK_inventory_movements_branches_BranchId]
FOREIGN KEY ([BranchId]) 
REFERENCES [dbo].[branches]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[inventory_movements]
ADD CONSTRAINT [FK_inventory_movements_employees_EmployeeId]
FOREIGN KEY ([EmployeeId]) 
REFERENCES [dbo].[employees]([id])
ON DELETE SET NULL
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[inventory_movements]
ADD CONSTRAINT [FK_inventory_movements_ingredients_IngredientId]
FOREIGN KEY ([IngredientId]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[ingredient_transfer_request_details]
ADD CONSTRAINT [FK_ingredient_transfer_request_details_ingredients_ingredient_id]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[ingredient_transfer_request_details]
ADD CONSTRAINT [FK_ingredient_transfer_request_details_ingredient_transfer_requests_transfer_request_id]
FOREIGN KEY ([transfer_request_id]) 
REFERENCES [dbo].[ingredient_transfer_requests]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[ingredient_transfer_requests]
ADD CONSTRAINT [FK_ingredient_transfer_requests_branches_branch_id]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[payrolls]
ADD CONSTRAINT [FK_payroll_employee]
FOREIGN KEY ([employee_id]) 
REFERENCES [dbo].[employees]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[product_images]
ADD CONSTRAINT [FKqnq71xsohugpqwf3c9gxmsuy]
FOREIGN KEY ([product_id]) 
REFERENCES [dbo].[products]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[recipes]
ADD CONSTRAINT [FK_recipes_products_ProductId]
FOREIGN KEY ([ProductId]) 
REFERENCES [dbo].[products]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[goods_received_details]
ADD CONSTRAINT [FK_grn_detail_ingredient]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[goods_received_details]
ADD CONSTRAINT [FK_grn_detail_grn]
FOREIGN KEY ([grn_id]) 
REFERENCES [dbo].[goods_received_notes]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[products]
ADD CONSTRAINT [FK_products_taxes]
FOREIGN KEY ([tax_id]) 
REFERENCES [dbo].[taxes]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[products]
ADD CONSTRAINT [FK_products_categories]
FOREIGN KEY ([category_id]) 
REFERENCES [dbo].[categories]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[branch_ingredient_inventory]
ADD CONSTRAINT [FK_bii_ingredient]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[branch_ingredient_inventory]
ADD CONSTRAINT [FK_bii_branch]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[ingredient_transfers]
ADD CONSTRAINT [FK_transfer_branch]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[ingredient_transfers]
ADD CONSTRAINT [FK_transfer_ingredient]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[purchase_returns]
ADD CONSTRAINT [FK_return_approved_by]
FOREIGN KEY ([approved_by]) 
REFERENCES [dbo].[employees]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[purchase_returns]
ADD CONSTRAINT [FK_return_invoice]
FOREIGN KEY ([invoice_id]) 
REFERENCES [dbo].[purchase_invoices]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[purchase_returns]
ADD CONSTRAINT [FK_return_branch]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[purchase_returns]
ADD CONSTRAINT [FK_return_supplier]
FOREIGN KEY ([supplier_id]) 
REFERENCES [dbo].[suppliers]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[purchase_returns]
ADD CONSTRAINT [FK_return_grn]
FOREIGN KEY ([grn_id]) 
REFERENCES [dbo].[goods_received_notes]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[ingredient_warehouse]
ADD CONSTRAINT [FK_ingredient_warehouse]
FOREIGN KEY ([ingredient_id]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[v_expenses_summary]
ADD CONSTRAINT [FK_v_expenses_summary_branches_branch_id]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[v_profit_summary]
ADD CONSTRAINT [FK_v_profit_summary_branches_branch_id]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[inventory_thresholds]
ADD CONSTRAINT [FK_inventory_thresholds_branches_BranchId]
FOREIGN KEY ([BranchId]) 
REFERENCES [dbo].[branches]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[inventory_thresholds]
ADD CONSTRAINT [FK_inventory_thresholds_ingredients_IngredientId]
FOREIGN KEY ([IngredientId]) 
REFERENCES [dbo].[ingredients]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[v_sales_summary]
ADD CONSTRAINT [FK_v_sales_summary_branches_branch_id]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[order_payments]
ADD CONSTRAINT [FK_order_payments_payment_statuses]
FOREIGN KEY ([payment_status_id]) 
REFERENCES [dbo].[payment_statuses]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[order_payments]
ADD CONSTRAINT [FK_order_payments_orders]
FOREIGN KEY ([order_id]) 
REFERENCES [dbo].[orders]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[order_payments]
ADD CONSTRAINT [FK_order_payments_payment_methods]
FOREIGN KEY ([payment_method_id]) 
REFERENCES [dbo].[payment_methods]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[customers]
ADD CONSTRAINT [FK_customers_users]
FOREIGN KEY ([id]) 
REFERENCES [dbo].[users]([id])
ON DELETE CASCADE
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[social_accounts]
ADD CONSTRAINT [FK6rmxxiton5yuvu7ph2hcq2xn7]
FOREIGN KEY ([user_id]) 
REFERENCES [dbo].[users]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[tokens]
ADD CONSTRAINT [FK2dylsfo39lgjyqml2tbe0b0ss]
FOREIGN KEY ([user_id]) 
REFERENCES [dbo].[users]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[sales_summary]
ADD CONSTRAINT [FK_sales_summary_branches]
FOREIGN KEY ([branch_id]) 
REFERENCES [dbo].[branches]([id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



