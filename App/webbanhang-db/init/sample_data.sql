USE [${DB_NAME}];
GO


-- Insert into categories
INSERT INTO [dbo].[categories] ([name]) VALUES
(N'Cà phê'),
(N'Trà'),
(N'Nước ép'),
(N'Bánh ngọt'),
(N'Món ăn nhẹ');
GO

-- Insert into ingredient_categories
INSERT INTO [dbo].[ingredient_categories] ([name], [description], [created_at], [last_modified]) VALUES
(N'Nguyên liệu cà phê', N'Các loại hạt cà phê và nguyên liệu pha chế', GETDATE(), GETDATE()),
(N'Nguyên liệu trà', N'Các loại lá trà và phụ gia', GETDATE(), GETDATE()),
(N'Sữa và kem', N'Sữa tươi, kem tươi và các sản phẩm từ sữa', GETDATE(), GETDATE()),
(N'Đường và chất ngọt', N'Đường, mật ong và các chất tạo ngọt', GETDATE(), GETDATE()),
(N'Gia vị và phụ gia', N'Vanilla, quế, và các gia vị khác', GETDATE(), GETDATE());
GO

-- Insert into taxes
INSERT INTO [dbo].[taxes] ([name], [tax_rate], [description], [created_at], [last_modified]) VALUES
(N'VAT 10%', 10.00, N'Thuế giá trị gia tăng 10%', GETDATE(), GETDATE()),
(N'VAT 5%', 5.00, N'Thuế giá trị gia tăng 5%', GETDATE(), GETDATE()),
(N'VAT 0%', 0.00, N'Miễn thuế', GETDATE(), GETDATE()),
(N'VAT 8%', 8.00, N'Thuế giá trị gia tăng 8%', GETDATE(), GETDATE()),
(N'VAT 15%', 15.00, N'Thuế giá trị gia tăng 15%', GETDATE(), GETDATE());
GO

-- Insert into roles
INSERT INTO [dbo].[roles] ([name], [created_at], [last_modified]) VALUES
('ADMIN', GETDATE(), GETDATE()),
('MANAGER', GETDATE(), GETDATE()),
('EMPLOYEE', GETDATE(), GETDATE()),
('CUSTOMER', GETDATE(), GETDATE()),
('GUEST', GETDATE(), GETDATE());
GO

-- Insert into branches
INSERT INTO [dbo].[branches] ([name], [address], [phone], [manager], [created_at], [last_modified]) VALUES
(N'Chi nhánh Quận 1', N'123 Nguyễn Huệ, Quận 1, TP.HCM', '0901234567', N'Nguyễn Văn A', GETDATE(), GETDATE()),
(N'Chi nhánh Quận 3', N'456 Võ Thị Sáu, Quận 3, TP.HCM', '0901234568', N'Trần Thị B', GETDATE(), GETDATE()),
(N'Chi nhánh Quận 7', N'789 Nguyễn Thị Thập, Quận 7, TP.HCM', '0901234569', N'Lê Văn C', GETDATE(), GETDATE()),
(N'Chi nhánh Hà Nội', N'321 Hoàn Kiếm, Hà Nội', '0901234570', N'Phạm Thị D', GETDATE(), GETDATE()),
(N'Chi nhánh Đà Nẵng', N'654 Hải Châu, Đà Nẵng', '0901234571', N'Hoàng Văn E', GETDATE(), GETDATE());
GO

-- Insert into suppliers
INSERT INTO [dbo].[suppliers] ([name], [phone], [email], [address], [note], [created_at], [last_modified]) VALUES
(N'Công ty Cà phê Trung Nguyên', '0281234567', 'contact@trungnguyengroup.com', N'234 Đường ABC, Quận 1, TP.HCM', N'Nhà cung cấp cà phê chính', GETDATE(), GETDATE()),
(N'Vinamilk', '0281234568', 'supplier@vinamilk.com', N'345 Đường DEF, Quận 2, TP.HCM', N'Cung cấp sữa tươi', GETDATE(), GETDATE()),
(N'Công ty Đường Biên Hòa', '0281234569', 'sales@bienhoasugar.com', N'456 Đường GHI, Biên Hòa', N'Cung cấp đường', GETDATE(), GETDATE()),
(N'Tập đoàn Phúc Sinh', '0281234570', 'order@phucsinhgroup.com', N'567 Đường JKL, Quận 5, TP.HCM', N'Cung cấp trà', GETDATE(), GETDATE()),
(N'Công ty Thực phẩm ABC', '0281234571', 'info@abcfood.com', N'678 Đường MNO, Quận 6, TP.HCM', N'Cung cấp gia vị', GETDATE(), GETDATE());
GO

-- Insert into shipping_providers
INSERT INTO [dbo].[shipping_providers] ([name], [contact_info], [api_endpoint], [created_at], [last_modified]) VALUES
('Giao Hàng Nhanh', 'GHN - 1900 1900', 'https://api.ghn.vn', GETDATE(), GETDATE()),
('Viettel Post', 'VTP - 1900 8095', 'https://api.viettelpost.vn', GETDATE(), GETDATE()),
('J&T Express', 'J&T - 1900 1088', 'https://api.jtexpress.vn', GETDATE(), GETDATE()),
('Grab Express', 'Grab - 1900 1717', 'https://api.grab.com', GETDATE(), GETDATE()),
('Be Delivery', 'Be - 1900 2323', 'https://api.be.com.vn', GETDATE(), GETDATE());
GO


INSERT INTO [dbo].[purchase_order_statuses] ([name], [description]) VALUES
    (N'PENDING', N'Đơn đặt hàng đang chờ xử lý'),
    (N'CONFIRMED', N'Đơn đặt hàng đã được xác nhận'),
    (N'PROCESSING', N'Đơn đặt hàng đang được xử lý'),
    (N'SHIPPED', N'Đơn đặt hàng đã được giao'),
    (N'DELIVERED', N'Đơn đặt hàng đã giao thành công'),
    (N'CANCELLED', N'Đơn đặt hàng đã bị hủy'),
    (N'RETURNED', N'Đơn đặt hàng đã được trả lại');
GO

INSERT INTO [dbo].[order_statuses] ([name]) VALUES 
    (N'PENDING'),
    (N'CONFIRMED'),
    (N'PROCESSING'),
    (N'SHIPPED'),
    (N'DELIVERED'),
    (N'CANCELLED'),
    (N'RETURNED');
GO

-- Insert into payment_statuses
INSERT INTO [dbo].[payment_statuses] ([name]) VALUES
(N'PENDING'),
(N'PAID'),
(N'REFUNDED'),
(N'VOIDED');
GO

-- Insert into delivery_statuses
INSERT INTO [dbo].[delivery_statuses] ([name]) VALUES
(N'PENDING'),
(N'IN_TRANSIT'),
(N'OUT_FOR_DELIVERY'),
(N'DELIVERED'),
(N'DELIVERY_FAILED'),
(N'RETURNED');
GO


-- Insert Invoice Statuses
INSERT INTO [dbo].[invoice_statuses] ([name], [description])
VALUES 
('PENDING', N'Hóa đơn chờ xử lý'),
('APPROVED', N'Hóa đơn đã duyệt'),
('PAID', N'Hóa đơn đã thanh toán'),
('PARTIALLY_PAID', N'Hóa đơn thanh toán một phần'),
('OVERDUE', N'Hóa đơn quá hạn'),
('CANCELLED', N'Hóa đơn đã hủy'),
('DISPUTED', N'Hóa đơn có tranh chấp');
GO

-- Insert Goods Received Statuses
INSERT INTO [dbo].[goods_received_statuses] ([name], [description])
VALUES 
('PENDING', N'Chờ nhận hàng'),
('IN_PROGRESS', N'Đang kiểm tra hàng'),
('COMPLETED', N'Đã nhận hàng hoàn tất'),
('PARTIALLY_RECEIVED', N'Nhận hàng một phần'),
('REJECTED', N'Từ chối nhận hàng'),
('ON_HOLD', N'Tạm dừng nhận hàng');
GO


-- Insert into payment_methods
INSERT INTO [dbo].[payment_methods] ([name]) VALUES
(N'CASH'),
(N'CARD'),
(N'BANK_TRANSFER'),
(N'E_WALLET'),
(N'COD');
GO

-- Insert into employees
INSERT INTO [dbo].[employees] ([branch_id], [full_name], [phone], [email], [position], [hire_date], [status], [created_at], [last_modified]) VALUES
(1, N'Nguyễn Văn A', '0901111111', 'nva@coffee.com', N'Quản lý chi nhánh', '2023-01-15', 'ACTIVE', GETDATE(), GETDATE()),
(1, N'Trần Thị B', '0901111112', 'ttb@coffee.com', N'Nhân viên pha chế', '2023-02-20', 'ACTIVE', GETDATE(), GETDATE()),
(2, N'Lê Văn C', '0901111113', 'lvc@coffee.com', N'Nhân viên phục vụ', '2023-03-10', 'ACTIVE', GETDATE(), GETDATE()),
(3, N'Phạm Thị D', '0901111114', 'ptd@coffee.com', N'Thu ngân', '2023-04-05', 'ACTIVE', GETDATE(), GETDATE()),
(2, N'Hoàng Văn E', '0901111115', 'hve@coffee.com', N'Nhân viên kho', '2023-05-01', 'ACTIVE', GETDATE(), GETDATE());
GO

-- Insert into products
INSERT INTO [dbo].[products] ([price], [category_id], [tax_id], [description], [name], [thumbnail], [created_at], [last_modified]) VALUES
(45000, 1, 1, N'Cà phê espresso đậm đà, hương vị mạnh mẽ', N'Espresso', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE()),
(55000, 1, 1, N'Cà phê cappuccino với lớp foam mịn màng', N'Cappuccino', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE()),
(35000, 2, 1, N'Trà xanh thơm mát, tốt cho sức khỏe', N'Trà xanh', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE()),
(40000, 3, 1, N'Nước ép cam tươi nguyên chất', N'Nước ép cam', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE()),
(25000, 4, 1, N'Bánh croissant bơ thơm ngon', N'Croissant', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE());
GO

-- Insert into ingredients
INSERT INTO [dbo].[ingredients] ([category_id], [name], [unit], [description], [tax_id], [created_at], [last_modified]) VALUES
(1, N'Hạt cà phê Arabica', 'kg', N'Hạt cà phê Arabica chất lượng cao', 1, GETDATE(), GETDATE()),
(1, N'Hạt cà phê Robusta', 'kg', N'Hạt cà phê Robusta đậm đà', 1, GETDATE(), GETDATE()),
(3, N'Sữa tươi', N'lít', N'Sữa tươi nguyên chất', 1, GETDATE(), GETDATE()),
(4, N'Đường trắng', 'kg', N'Đường trắng tinh luyện', 1, GETDATE(), GETDATE()),
(2, N'Lá trà xanh', 'kg', N'Lá trà xanh tươi', 1, GETDATE(), GETDATE());
GO

-- Insert into ingredient_purchase_orders
INSERT INTO [dbo].[ingredient_purchase_orders] ([purchase_order_code], [supplier_id], [order_date], [final_amount], [note], [status_id], [last_modified]) VALUES
('PO001', 1, GETDATE(), 5000000, N'Đơn hàng hạt cà phê tháng 12', 1, GETDATE()),
('PO002', 2, GETDATE(), 2000000, N'Đơn hàng sữa tươi tháng 12', 1, GETDATE()),
('PO003', 3, GETDATE(), 1500000, N'Đơn hàng đường tháng 12', 1, GETDATE()),
('PO004', 4, GETDATE(), 1000000, N'Đơn hàng trà tháng 12', 1, GETDATE()),
('PO005', 5, GETDATE(), 800000, N'Đơn hàng gia vị tháng 12', 1, GETDATE());
GO

-- Insert into users
INSERT INTO [dbo].[users] ([employee_id], [is_active], [date_of_birth], [role_id], [phone_number], [fullname], [address], [password], [created_at], [last_modified]) VALUES
(1, 1, '1990-01-15', 1, '0901111111', N'Nguyễn Văn A', N'123 Nguyễn Huệ, Quận 1, TP.HCM', 'hashed_password_1', GETDATE(), GETDATE()),
(2, 1, '1992-05-20', 3, '0901111112', N'Trần Thị B', N'456 Lê Lợi, Quận 1, TP.HCM', 'hashed_password_2', GETDATE(), GETDATE()),
(NULL, 1, '1995-03-10', 4, '0912345678', N'Khách hàng VIP 1', N'789 Hai Bà Trưng, Quận 3, TP.HCM', 'hashed_password_3', GETDATE(), GETDATE()),
(NULL, 1, '1988-12-25', 4, '0912345679', N'Khách hàng VIP 2', N'321 Pasteur, Quận 1, TP.HCM', 'hashed_password_4', GETDATE(), GETDATE()),
(3, 1, '1993-07-18', 3, '0901111113', N'Lê Văn C', N'654 Nguyễn Thị Minh Khai, Quận 3, TP.HCM', 'hashed_password_5', GETDATE(), GETDATE());
GO

-- Insert into product_images
INSERT INTO [dbo].[product_images] ([product_id], [image_url]) VALUES
(1, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE='),
(2, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE='),
(3, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE='),
(4, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE='),
(5, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=');
GO

-- Insert into product_recipes
INSERT INTO [dbo].[product_recipes] ([product_id], [ingredient_id], [quantity], [created_at], [last_modified]) VALUES
(1, 1, 0.02, GETDATE(), GETDATE()), -- Espresso cần 20g cà phê Arabica
(2, 1, 0.015, GETDATE(), GETDATE()), -- Cappuccino cần 15g cà phê Arabica
(2, 3, 0.1, GETDATE(), GETDATE()), -- Cappuccino cần 100ml sữa tươi
(3, 5, 0.005, GETDATE(), GETDATE()), -- Trà xanh cần 5g lá trà
(4, 4, 0.01, GETDATE(), GETDATE()); -- Nước ép cam cần 10g đường
GO

-- Insert into ingredient_purchase_order_details
INSERT INTO [dbo].[ingredient_purchase_order_details] ([purchase_order_id], [ingredient_id], [quantity], [unit_price], [tax_price], [total_price], [created_at], [last_modified]) VALUES
(1, 1, 100, 200000, 20000, 2200000, GETDATE(), GETDATE()),
(1, 2, 50, 150000, 15000, 825000, GETDATE(), GETDATE()),
(2, 3, 200, 25000, 2500, 550000, GETDATE(), GETDATE()),
(3, 4, 100, 15000, 1500, 165000, GETDATE(), GETDATE()),
(4, 5, 20, 50000, 5000, 110000, GETDATE(), GETDATE());
GO

-- Insert into payrolls
INSERT INTO [dbo].[payrolls] ([employee_id], [month], [year], [total_working_hours], [base_salary], [allowance], [bonus], [penalty], [gross_salary], [tax_amount], [net_salary], [created_at], [last_modified]) VALUES
(1, 11, 2024, 176, 15000000, 2000000, 1000000, 0, 18000000, 1800000, 16200000, GETDATE(), GETDATE()),
(2, 11, 2024, 176, 8000000, 500000, 300000, 0, 8800000, 880000, 7920000, GETDATE(), GETDATE()),
(3, 11, 2024, 160, 6000000, 300000, 200000, 100000, 6400000, 640000, 5760000, GETDATE(), GETDATE()),
(4, 11, 2024, 168, 7000000, 400000, 250000, 0, 7650000, 765000, 6885000, GETDATE(), GETDATE()),
(5, 11, 2024, 172, 6500000, 350000, 150000, 50000, 6950000, 695000, 6255000, GETDATE(), GETDATE());
GO

-- Insert into employee_salaries
INSERT INTO [dbo].[employee_salaries] ([employee_id], [base_salary], [salary_type], [allowance], [bonus], [penalty], [tax_rate], [effective_date], [created_at], [last_modified]) VALUES
(1, 15000000, 'MONTHLY', 2000000, 1000000, 0, 0.10, '2024-01-01', GETDATE(), GETDATE()),
(2, 8000000, 'MONTHLY', 500000, 300000, 0, 0.10, '2024-02-01', GETDATE(), GETDATE()),
(3, 6000000, 'MONTHLY', 300000, 200000, 0, 0.10, '2024-03-01', GETDATE(), GETDATE()),
(4, 7000000, 'MONTHLY', 400000, 250000, 0, 0.10, '2024-04-01', GETDATE(), GETDATE()),
(5, 6500000, 'MONTHLY', 350000, 150000, 0, 0.10, '2024-05-01', GETDATE(), GETDATE());
GO

-- Insert into employee_shifts
INSERT INTO [dbo].[employee_shifts] ([employee_id], [shift_date], [start_time], [end_time], [status], [created_at], [last_modified]) VALUES
(1, '2024-12-01', '08:00:00', '17:00:00', 'PRESENT', GETDATE(), GETDATE()),
(2, '2024-12-01', '09:00:00', '18:00:00', 'PRESENT', GETDATE(), GETDATE()),
(3, '2024-12-01', '10:00:00', '19:00:00', 'PRESENT', GETDATE(), GETDATE()),
(4, '2024-12-01', '08:30:00', '17:30:00', 'PRESENT', GETDATE(), GETDATE()),
(5, '2024-12-01', '07:00:00', '16:00:00', 'PRESENT', GETDATE(), GETDATE());
GO

-- Insert into branch_ingredient_inventory
INSERT INTO [dbo].[branch_ingredient_inventory] ([branch_id], [ingredient_id], [quantity], [created_at], [last_modified]) VALUES
(1, 1, 50, GETDATE(), GETDATE()),
(1, 2, 30, GETDATE(), GETDATE()),
(1, 3, 100, GETDATE(), GETDATE()),
(2, 1, 40, GETDATE(), GETDATE()),
(3, 4, 80, GETDATE(), GETDATE());
GO

-- Insert into ingredient_warehouse
INSERT INTO [dbo].[ingredient_warehouse] ([ingredient_id], [quantity], [created_at], [last_modified]) VALUES
(1, 500, GETDATE(), GETDATE()),
(2, 300, GETDATE(), GETDATE()),
(3, 1000, GETDATE(), GETDATE()),
(4, 800, GETDATE(), GETDATE()),
(5, 200, GETDATE(), GETDATE());
GO

-- Insert into ingredient_transfers
INSERT INTO [dbo].[ingredient_transfers] ([ingredient_id], [branch_id], [quantity], [note], [created_at], [last_modified]) VALUES
(1, 1, 20, N'Chuyển hạt cà phê cho chi nhánh Q1', GETDATE(), GETDATE()),
(2, 2, 15, N'Chuyển hạt cà phê cho chi nhánh Q3', GETDATE(), GETDATE()),
(3, 3, 50, N'Chuyển sữa tươi cho chi nhánh Q7', GETDATE(), GETDATE()),
(4, 1, 30, N'Chuyển đường cho chi nhánh Q1', GETDATE(), GETDATE()),
(5, 2, 10, N'Chuyển trà xanh cho chi nhánh Q3', GETDATE(), GETDATE());
GO

-- Insert into supplier_ingredient_prices
INSERT INTO [dbo].[supplier_ingredient_prices] ([supplier_id], [ingredient_id], [price], [unit], [effective_date], [expired_date], [created_at], [last_modified]) VALUES
(1, 1, 200000, 'kg', GETDATE(), '2025-12-31', GETDATE(), GETDATE()),
(1, 2, 150000, 'kg', GETDATE(), '2025-12-31', GETDATE(), GETDATE()),
(2, 3, 25000, 'lít', GETDATE(), '2025-06-30', GETDATE(), GETDATE()),
(3, 4, 15000, 'kg', GETDATE(), '2025-12-31', GETDATE(), GETDATE()),
(4, 5, 50000, 'kg', GETDATE(), '2025-12-31', GETDATE(), GETDATE());
GO

-- Insert into branch_expenses
INSERT INTO [dbo].[branch_expenses] ([branch_id], [expense_type], [amount], [start_date], [end_date], [payment_cycle], [note], [created_at], [last_modified]) VALUES
(1, N'Tiền thuê mặt bằng', 30000000, '2024-01-01', '2024-12-31', 'MONTHLY', N'Thuê mặt bằng Quận 1', GETDATE(), GETDATE()),
(1, N'Tiền điện', 2000000, '2024-12-01', '2024-12-31', 'MONTHLY', N'Hóa đơn điện tháng 12', GETDATE(), GETDATE()),
(2, N'Tiền thuê mặt bằng', 25000000, '2024-01-01', '2024-12-31', 'MONTHLY', N'Thuê mặt bằng Quận 3', GETDATE(), GETDATE()),
(3, N'Tiền nước', 500000, '2024-12-01', '2024-12-31', 'MONTHLY', N'Hóa đơn nước tháng 12', GETDATE(), GETDATE()),
(1, N'Chi phí marketing', 5000000, '2024-12-01', '2024-12-31', 'MONTHLY', N'Quảng cáo Facebook', GETDATE(), GETDATE());
GO

-- Insert into customers (uses user_id as primary key)
INSERT INTO [dbo].[customers] ([id], [user_id], [fullname], [phone_number], [email], [address], [created_at], [last_modified]) VALUES
(3, 3, N'Khách hàng VIP 1', '0912345678', 'vip1@email.com', N'789 Hai Bà Trưng, Quận 3, TP.HCM', GETDATE(), GETDATE()),
(4, 4, N'Khách hàng VIP 2', '0912345679', 'vip2@email.com', N'321 Pasteur, Quận 1, TP.HCM', GETDATE(), GETDATE()),
(5, 5, N'Khách hàng VIP 3', '0912345680', 'vip3@email.com', N'456 Điện Biên Phủ, Quận 3, TP.HCM', GETDATE(), GETDATE());
GO

-- Insert additional users first for customers without user accounts
INSERT INTO [dbo].[users] ([employee_id], [is_active], [date_of_birth], [role_id], [phone_number], [fullname], [address], [password], [created_at], [last_modified]) VALUES
(NULL, 1, '1990-03-15', 4, '0923456789', N'Nguyễn Thị F', N'111 Nguyễn Du, Quận 1, TP.HCM', 'hashed_password_6', GETDATE(), GETDATE()),
(NULL, 1, '1985-07-20', 4, '0934567890', N'Trần Văn G', N'222 Lý Tự Trọng, Quận 1, TP.HCM', 'hashed_password_7', GETDATE(), GETDATE());
GO

-- Insert additional customers without user accounts
INSERT INTO [dbo].[customers] ([id], [user_id], [fullname], [phone_number], [email], [address], [created_at], [last_modified]) VALUES
(6, 6, N'Nguyễn Thị F', '0923456789', 'nthif@email.com', N'111 Nguyễn Du, Quận 1, TP.HCM', GETDATE(), GETDATE()),
(7, 7, N'Trần Văn G', '0934567890', 'tvg@email.com', N'222 Lý Tự Trọng, Quận 1, TP.HCM', GETDATE(), GETDATE());
GO

-- Insert into tokens
INSERT INTO [dbo].[tokens] ([expired], [revoked], [expiration_date], [user_id], [token_type], [token]) VALUES
(0, 0, DATEADD(day, 30, GETDATE()), 1, 'ACCESS_TOKEN', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...1'),
(0, 0, DATEADD(day, 30, GETDATE()), 2, 'ACCESS_TOKEN', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...2'),
(0, 0, DATEADD(day, 30, GETDATE()), 3, 'ACCESS_TOKEN', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...3'),
(1, 0, DATEADD(day, -1, GETDATE()), 4, 'ACCESS_TOKEN', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...4'),
(0, 1, DATEADD(day, 15, GETDATE()), 5, 'REFRESH_TOKEN', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...5');
GO

-- Insert into social_accounts
INSERT INTO [dbo].[social_accounts] ([provider_id], [user_id], [provider], [name], [email], [created_at], [last_modified]) VALUES
(1234567890, 3, 'GOOGLE', N'Khách hàng VIP 1', 'vip1@gmail.com', GETDATE(), GETDATE()),
(2345678901, 4, 'FACEBOOK', N'Khách hàng VIP 2', 'vip2@facebook.com', GETDATE(), GETDATE()),
(3456789012, 1, 'GOOGLE', N'Nguyễn Văn A', 'nva@gmail.com', GETDATE(), GETDATE()),
(4567890123, 2, 'FACEBOOK', N'Trần Thị B', 'ttb@facebook.com', GETDATE(), GETDATE()),
(5678901234, 5, 'GOOGLE', N'Lê Văn C', 'lvc@gmail.com', GETDATE(), GETDATE());
GO

-- Insert into orders
INSERT INTO [dbo].[orders] ([order_uuid], [order_code], [customer_id], [branch_id], [total_money], [status_id], [created_at], [last_modified], [notes]) VALUES
(NEWID(), 'ORD001', 3, 1, 100000, 1, GETDATE(), GETDATE(), N'Đơn hàng giao tận nơi'),
(NEWID(), 'ORD002', 4, 1, 75000, 2, DATEADD(day, -1, GETDATE()), DATEADD(day, -1, GETDATE()), N'Đơn hàng tại quầy'),
(NEWID(), 'ORD003', 5, 2, 120000, 1, GETDATE(), GETDATE(), N'Đơn hàng online'),
(NEWID(), 'ORD004', 6, 3, 95000, 2, DATEADD(hour, -2, GETDATE()), DATEADD(hour, -2, GETDATE()), N'Đơn hàng takeaway'),
(NEWID(), 'ORD005', 7, 1, 85000, 3, DATEADD(hour, -1, GETDATE()), DATEADD(hour, -1, GETDATE()), N'Đơn hàng giao nhanh');
GO

-- Insert into order_details
INSERT INTO [dbo].[order_details] ([quantity], [order_id], [product_id], [color], [created_at], [last_modified], [note], [total_amount], [unit_price]) VALUES
(2, 1, 1, NULL, GETDATE(), GETDATE(), N'Không đường', 90000, 45000),
(1, 1, 5, NULL, GETDATE(), GETDATE(), N'Thêm bơ', 25000, 25000),
(1, 2, 2, NULL, DATEADD(day, -1, GETDATE()), DATEADD(day, -1, GETDATE()), N'Ít đá', 55000, 55000),
(3, 3, 3, NULL, GETDATE(), GETDATE(), N'Nóng', 105000, 35000),
(2, 4, 4, NULL, DATEADD(hour, -2, GETDATE()), DATEADD(hour, -2, GETDATE()), N'Không đường', 80000, 40000);
GO

-- Insert into order_payments
INSERT INTO [dbo].[order_payments] ([order_id], [payment_method_id], [payment_status_id], [amount], [payment_date], [transaction_id], [notes], [created_at], [last_modified]) VALUES
(1, 4, 2, 100000, GETDATE(), 'MOMO123456789', N'Thanh toán qua MoMo', GETDATE(), GETDATE()),
(2, 1, 2, 75000, DATEADD(day, -1, GETDATE()), NULL, N'Thanh toán tiền mặt', DATEADD(day, -1, GETDATE()), DATEADD(day, -1, GETDATE())),
(3, 5, 1, 120000, NULL, 'ZALO987654321', N'Đang chờ thanh toán ZaloPay', GETDATE(), GETDATE()),
(4, 2, 2, 95000, DATEADD(hour, -2, GETDATE()), 'CARD123789456', N'Thanh toán thẻ tín dụng', DATEADD(hour, -2, GETDATE()), DATEADD(hour, -2, GETDATE())),
(5, 3, 2, 85000, DATEADD(hour, -1, GETDATE()), 'BANK456123789', N'Chuyển khoản ngân hàng', DATEADD(hour, -1, GETDATE()), DATEADD(hour, -1, GETDATE()));
GO

-- Insert into order_shipments
INSERT INTO [dbo].[order_shipments] ([order_id], [shipping_provider_id], [shipping_address], [shipping_cost], [shipping_method], [estimated_delivery_date], [notes], [created_at], [last_modified]) VALUES
(1, 1, N'789 Hai Bà Trưng, Quận 3, TP.HCM', 25000, N'Giao hàng tiêu chuẩn', DATEADD(day, 1, GETDATE()), N'Giao trong giờ hành chính', GETDATE(), GETDATE()),
(3, 2, N'111 Nguyễn Du, Quận 1, TP.HCM', 30000, N'Giao hàng nhanh', DATEADD(hour, 4, GETDATE()), N'Giao trong 4 giờ', GETDATE(), GETDATE()),
(5, 4, N'333 Cách Mạng Tháng 8, Quận 3, TP.HCM', 35000, N'Giao hàng siêu tốc', DATEADD(hour, 1, GETDATE()), N'Giao trong 1 giờ', DATEADD(hour, -1, GETDATE()), DATEADD(hour, -1, GETDATE())),
(1, 3, N'789 Hai Bà Trưng, Quận 3, TP.HCM', 20000, N'Giao hàng tiết kiệm', DATEADD(day, 2, GETDATE()), N'Không gấp', GETDATE(), GETDATE()),
(3, 5, N'111 Nguyễn Du, Quận 1, TP.HCM', 40000, N'Giao hàng cao cấp', DATEADD(hour, 2, GETDATE()), N'Dịch vụ VIP', GETDATE(), GETDATE());
GO

-- Insert into order_delivery_tracking
INSERT INTO [dbo].[order_delivery_tracking] ([order_id], [tracking_number], [status_id], [location], [estimated_delivery], [delivery_person_id], [shipping_provider_id], [created_at], [last_modified]) VALUES
(1, 'GHN001234567', 2, N'Kho Quận 1', DATEADD(day, 1, GETDATE()), 3, 1, GETDATE(), GETDATE()),
(3, 'VTP987654321', 1, N'Trung tâm phân loại', DATEADD(hour, 4, GETDATE()), 4, 2, GETDATE(), GETDATE()),
(5, 'GRAB555666777', 3, N'Đang trên đường giao', DATEADD(hour, 1, GETDATE()), 5, 4, DATEADD(hour, -1, GETDATE()), DATEADD(hour, -1, GETDATE())),
(1, 'JT888999000', 2, N'Kho Quận 3', DATEADD(day, 2, GETDATE()), 2, 3, GETDATE(), GETDATE()),
(3, 'BE111222333', 4, N'Đã giao thành công', NULL, 1, 5, GETDATE(), GETDATE());
GO

-- Insert into sales_summary
INSERT INTO [dbo].[sales_summary] ([branch_id], [period_type], [period_value], [total_orders], [total_products], [revenue_before_tax], [revenue_after_tax], [tax_amount], [created_at], [last_modified]) VALUES
(1, 'MONTH', '2024-12', 150, 300, 15000000, 16500000, 1500000, GETDATE(), GETDATE()),
(2, 'MONTH', '2024-12', 120, 250, 12000000, 13200000, 1200000, GETDATE(), GETDATE()),
(3, 'MONTH', '2024-12', 100, 200, 10000000, 11000000, 1000000, GETDATE(), GETDATE()),
(1, 'MONTH', '2024-11', 140, 280, 14000000, 15400000, 1400000, DATEADD(month, -1, GETDATE()), DATEADD(month, -1, GETDATE())),
(2, 'MONTH', '2024-11', 110, 220, 11000000, 12100000, 1100000, DATEADD(month, -1, GETDATE()), DATEADD(month, -1, GETDATE()));
GO

-- Insert into expenses_summary
INSERT INTO [dbo].[expenses_summary] ([branch_id], [period_type], [period_value], [total_purchase_orders], [total_ingredients], [expense_before_tax], [expense_after_tax], [tax_amount], [created_at], [last_modified]) VALUES
(1, 'MONTH', '2024-12', 20, 50, 8000000, 8800000, 800000, GETDATE(), GETDATE()),
(2, 'MONTH', '2024-12', 15, 40, 6000000, 6600000, 600000, GETDATE(), GETDATE()),
(3, 'MONTH', '2024-12', 18, 45, 7000000, 7700000, 700000, GETDATE(), GETDATE()),
(1, 'MONTH', '2024-11', 18, 48, 7500000, 8250000, 750000, DATEADD(month, -1, GETDATE()), DATEADD(month, -1, GETDATE())),
(2, 'MONTH', '2024-11', 16, 42, 6500000, 7150000, 650000, DATEADD(month, -1, GETDATE()), DATEADD(month, -1, GETDATE()));
GO

-- Insert into profit_summary
INSERT INTO [dbo].[profit_summary] ([branch_id], [period_type], [period_value], [revenue_before_tax], [revenue_after_tax], [expense_before_tax], [expense_after_tax], [output_tax], [input_tax], [vat_to_pay], [profit_before_tax], [profit_after_tax], [created_at], [last_modified]) VALUES
(1, 'MONTH', '2024-12', 15000000, 16500000, 8000000, 8800000, 1500000, 800000, 700000, 7000000, 7000000, GETDATE(), GETDATE()),
(2, 'MONTH', '2024-12', 12000000, 13200000, 6000000, 6600000, 1200000, 600000, 600000, 6000000, 6000000, GETDATE(), GETDATE()),
(3, 'MONTH', '2024-12', 10000000, 11000000, 7000000, 7700000, 1000000, 700000, 300000, 3000000, 3000000, GETDATE(), GETDATE()),
(1, 'MONTH', '2024-11', 14000000, 15400000, 7500000, 8250000, 1400000, 750000, 650000, 6500000, 6500000, DATEADD(month, -1, GETDATE()), DATEADD(month, -1, GETDATE())),
(2, 'MONTH', '2024-11', 11000000, 12100000, 6500000, 7150000, 1100000, 650000, 450000, 4500000, 4500000, DATEADD(month, -1, GETDATE()), DATEADD(month, -1, GETDATE()));
GO

-- Insert into view tables (these are treated as regular tables for data insertion)

-- Insert into v_sales_summary
INSERT INTO [dbo].[v_sales_summary] ([branch_id], [year], [month], [period], [total_orders], [total_products], [revenue_before_tax], [revenue_after_tax], [tax_amount]) VALUES
(1, 2024, 12, '2024-12', 150, 300, 15000000, 16500000, 1500000),
(2, 2024, 12, '2024-12', 120, 250, 12000000, 13200000, 1200000),
(3, 2024, 12, '2024-12', 100, 200, 10000000, 11000000, 1000000),
(1, 2024, 11, '2024-11', 140, 280, 14000000, 15400000, 1400000),
(2, 2024, 11, '2024-11', 110, 220, 11000000, 12100000, 1100000);
GO

-- Insert into v_employee_payroll
INSERT INTO [dbo].[v_employee_payroll] ([employee_id], [full_name], [branch_name], [position_name], [base_salary], [salary_type], [total_allowances], [total_bonus], [total_deductions], [gross_salary], [effective_date], [end_date]) VALUES
(1, N'Nguyễn Văn A', N'Chi nhánh Quận 1', N'Quản lý chi nhánh', 15000000, 'MONTHLY', 2000000, 1000000, 0, 18000000, '2024-01-01', NULL),
(2, N'Trần Thị B', N'Chi nhánh Quận 1', N'Nhân viên pha chế', 8000000, 'MONTHLY', 500000, 300000, 0, 8800000, '2024-02-01', NULL),
(3, N'Lê Văn C', N'Chi nhánh Quận 3', N'Nhân viên phục vụ', 6000000, 'MONTHLY', 300000, 200000, 100000, 6400000, '2024-03-01', NULL),
(4, N'Phạm Thị D', N'Chi nhánh Quận 7', N'Thu ngân', 7000000, 'MONTHLY', 400000, 250000, 0, 7650000, '2024-04-01', NULL),
(5, N'Hoàng Văn E', N'Chi nhánh Quận 3', N'Nhân viên kho', 6500000, 'MONTHLY', 350000, 150000, 50000, 6950000, '2024-05-01', NULL);
GO

-- Insert into v_inventory_status
INSERT INTO [dbo].[v_inventory_status] ([ingredient_id], [ingredient_name], [location_id], [location_name], [branch_id], [branch_name], [quantity_on_hand], [quantity_reserved], [available_quantity], [minimum_stock], [stock_status], [unit_of_measure], [last_updated]) VALUES
(1, N'Hạt cà phê Arabica', 1, N'Kho chính', 1, N'Chi nhánh Quận 1', 50, 5, 45, 20, 'IN_STOCK', 'kg', GETDATE()),
(2, N'Hạt cà phê Robusta', 1, N'Kho chính', 1, N'Chi nhánh Quận 1', 30, 3, 27, 15, 'IN_STOCK', 'kg', GETDATE()),
(3, N'Sữa tươi', 2, N'Kho lạnh', 1, N'Chi nhánh Quận 1', 100, 10, 90, 50, 'IN_STOCK', N'lít', GETDATE()),
(1, N'Hạt cà phê Arabica', 2, N'Kho chính', 2, N'Chi nhánh Quận 3', 40, 4, 36, 20, 'IN_STOCK', 'kg', GETDATE()),
(4, N'Đường trắng', 3, N'Kho khô', 3, N'Chi nhánh Quận 7', 80, 8, 72, 30, 'IN_STOCK', 'kg', GETDATE());
GO

-- Insert into v_profit_summary
INSERT INTO [dbo].[v_profit_summary] ([branch_id], [year], [month], [period], [revenue_before_tax], [revenue_after_tax], [expense_before_tax], [expense_after_tax], [output_tax], [input_tax], [vat_to_pay], [profit_before_tax], [profit_after_tax]) VALUES
(1, 2024, 12, '2024-12', 15000000, 16500000, 8000000, 8800000, 1500000, 800000, 700000, 7000000, 7000000),
(2, 2024, 12, '2024-12', 12000000, 13200000, 6000000, 6600000, 1200000, 600000, 600000, 6000000, 6000000),
(3, 2024, 12, '2024-12', 10000000, 11000000, 7000000, 7700000, 1000000, 700000, 300000, 3000000, 3000000),
(1, 2024, 11, '2024-11', 14000000, 15400000, 7500000, 8250000, 1400000, 750000, 650000, 6500000, 6500000),
(2, 2024, 11, '2024-11', 11000000, 12100000, 6500000, 7150000, 1100000, 650000, 450000, 4500000, 4500000);
GO

-- Insert into v_expenses_summary
INSERT INTO [dbo].[v_expenses_summary] ([branch_id], [year], [month], [period], [total_purchase_orders], [total_ingredients], [expense_before_tax], [expense_after_tax], [tax_amount]) VALUES
(1, 2024, 12, '2024-12', 20, 50, 8000000, 8800000, 800000),
(2, 2024, 12, '2024-12', 15, 40, 6000000, 6600000, 600000),
(3, 2024, 12, '2024-12', 18, 45, 7000000, 7700000, 700000),
(1, 2024, 11, '2024-11', 18, 48, 7500000, 8250000, 750000),
(2, 2024, 11, '2024-11', 16, 42, 6500000, 7150000, 650000);
GO

-- Insert sample Purchase Orders
INSERT INTO [dbo].[ingredient_purchase_orders] 
([purchase_order_code], [supplier_id], [branch_id], [employee_id], [order_date], [expected_delivery_date], [status_id], [total_amount_before_tax], [total_tax_amount], [total_amount_after_tax], [discount_amount], [final_amount], [note], [created_at], [last_modified])
VALUES 
('PO202501001', 1, 1, 1, '2025-01-15', '2025-01-22', 2, 5000000, 500000, 5500000, 0, 5500000, N'Đơn hàng nguyên liệu tháng 1', GETDATE(), GETDATE()),
('PO202501002', 2, 1, 1, '2025-01-16', '2025-01-23', 1, 3000000, 300000, 3300000, 100000, 3200000, N'Đơn hàng khẩn cấp', GETDATE(), GETDATE()),
('PO202501003', 1, 2, 2, '2025-01-17', '2025-01-24', 3, 8000000, 800000, 8800000, 200000, 8600000, N'Đơn hàng chi nhánh 2', GETDATE(), GETDATE());
GO

-- Insert sample Purchase Invoices
INSERT INTO [dbo].[purchase_invoices] 
([invoice_code], [purchase_order_id], [supplier_id], [branch_id], [invoice_date], [due_date], [status_id], [total_amount_before_tax], [total_tax_amount], [total_amount_after_tax], [paid_amount], [remaining_amount], [discount_amount], [payment_method], [note], [created_at], [last_modified])
VALUES 
('INV202501001', 1, 1, 1, '2025-01-22', '2025-02-22', 3, 5000000, 500000, 5500000, 5500000, 0, 0, 'BANK_TRANSFER', N'Thanh toán đầy đủ', GETDATE(), GETDATE()),
('INV202501002', 2, 2, 1, '2025-01-23', '2025-02-23', 4, 3000000, 300000, 3300000, 1650000, 1650000, 100000, 'CASH', N'Thanh toán một phần', GETDATE(), GETDATE()),
('INV202501003', 3, 1, 2, '2025-01-24', '2025-02-24', 2, 8000000, 800000, 8800000, 0, 8800000, 200000, '', N'Chờ duyệt thanh toán', GETDATE(), GETDATE());
GO

-- Insert sample Goods Received Notes
INSERT INTO [dbo].[goods_received_notes] 
([grn_code], [purchase_order_id], [invoice_id], [supplier_id], [branch_id], [warehouse_staff_id], [received_date], [status_id], [total_quantity_ordered], [total_quantity_received], [total_quantity_rejected], [delivery_note_number], [vehicle_number], [driver_name], [note], [created_at], [last_modified])
VALUES 
('GRN202501001', 1, 1, 1, 1, 1, '2025-01-22', 3, 100, 100, 0, 'DN202501001', '29A-12345', 'Nguyễn Văn A', N'Nhận hàng đầy đủ, chất lượng tốt', GETDATE(), GETDATE()),
('GRN202501002', 2, 2, 2, 1, 1, '2025-01-23', 4, 50, 40, 10, 'DN202501002', '30B-67890', 'Trần Văn B', N'Một phần hàng không đạt chất lượng', GETDATE(), GETDATE()),
('GRN202501003', 3, 3, 1, 2, 2, '2025-01-24', 2, 200, 150, 0, 'DN202501003', '31C-11111', 'Lê Văn C', N'Đang kiểm tra chất lượng', GETDATE(), GETDATE());
GO

-- Insert sample Purchase Returns
INSERT INTO [dbo].[purchase_returns] 
([return_code], [grn_id], [invoice_id], [supplier_id], [branch_id], [return_date], [return_reason], [status_id], [total_return_amount], [refund_amount], [credit_note_number], [approved_by], [approval_date], [note], [created_at], [last_modified])
VALUES 
('PR202501001', 2, 2, 2, 1, '2025-01-24', 'Hàng không đạt chất lượng', 1, 600000, 0, '', NULL, NULL, N'Chờ duyệt trả hàng', GETDATE(), GETDATE()),
('PR202501002', 1, 1, 1, 1, '2025-01-25', 'Giao nhầm sản phẩm', 1, 200000, 200000, 'CN202501001', 1, '2025-01-25', N'Đã hoàn tiền', GETDATE(), GETDATE());
GO

-- Insert sample Supplier Performance data
INSERT INTO [dbo].[supplier_performance] 
([supplier_id], [evaluation_period], [period_value], [total_orders], [total_amount], [on_time_deliveries], [late_deliveries], [quality_score], [service_score], [overall_rating], [total_returns], [return_value], [comments], [created_at], [last_modified])
VALUES 
(1, 'MONTHLY', '2025-01', 2, 13500000, 2, 0, 4.5, 4.8, 4.6, 1, 200000, N'Nhà cung cấp uy tín, giao hàng đúng hạn', GETDATE(), GETDATE()),
(2, 'MONTHLY', '2025-01', 1, 3200000, 0, 1, 3.2, 3.5, 3.4, 1, 600000, N'Cần cải thiện chất lượng hàng hóa', GETDATE(), GETDATE()),
(1, 'QUARTERLY', '2025-Q1', 2, 13500000, 2, 0, 4.5, 4.8, 4.6, 1, 200000, N'Đánh giá quý 1', GETDATE(), GETDATE()),
(2, 'QUARTERLY', '2025-Q1', 1, 3200000, 0, 1, 3.2, 3.5, 3.4, 1, 600000, N'Đánh giá quý 1', GETDATE(), GETDATE());
GO


PRINT N'Sample data insertion completed successfully!';
GO
