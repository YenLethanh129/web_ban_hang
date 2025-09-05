USE [${DB_NAME}];
GO


-- Insert into categories
INSERT INTO [dbo].[categories] ([name]) VALUES
(N'CÀ PHÊ'),
(N'TRÀ'),
(N'NƯỚC ÉP'),
(N'BÁNH NGỌT'),
(N'MÓN ĂN NHẸ'),
(N'ĐỒ UỐNG LẠNH'),
(N'COMBO SET'),
(N'NƯỚC NGỌT'),
(N'YAOURT'),
(N'SODA');
GO

-- Insert into ingredient_categories
INSERT INTO [dbo].[ingredient_categories] ([name], [description], [created_at], [last_modified]) VALUES
(N'Nguyên liệu cà phê', N'Các loại hạt cà phê và nguyên liệu pha chế', GETDATE(), GETDATE()),
(N'Nguyên liệu trà', N'Các loại lá trà và phụ gia', GETDATE(), GETDATE()),
(N'Sữa và kem', N'Sữa tươi, kem tươi và các sản phẩm từ sữa', GETDATE(), GETDATE()),
(N'Đường và chất ngọt', N'Đường, mật ong và các chất tạo ngọt', GETDATE(), GETDATE()),
(N'Gia vị và phụ gia', N'Vanilla, quế, và các gia vị khác', GETDATE(), GETDATE()),
(N'Trái cây tươi', N'Cam, chanh, dâu và các loại trái cây', GETDATE(), GETDATE()),
(N'Bánh và ngũ cốc', N'Bột mì, bơ, trứng cho làm bánh', GETDATE(), GETDATE());
GO

-- Insert into taxes
INSERT INTO [dbo].[taxes] ([name], [tax_rate], [description], [created_at], [last_modified]) VALUES
(N'VAT 10%', 10.00, N'Thuế giá trị gia tăng 10%', GETDATE(), GETDATE()),
(N'VAT 5%', 5.00, N'Thuế giá trị gia tăng 5%', GETDATE(), GETDATE()),
(N'VAT 0%', 0.00, N'Miễn thuế', GETDATE(), GETDATE()),
(N'VAT 8%', 8.00, N'Thuế giá trị gia tăng 8%', GETDATE(), GETDATE()),
(N'VAT 15%', 15.00, N'Thuế giá trị gia tăng 15%', GETDATE(), GETDATE()),
(N'VAT 3%', 3.00, N'Thuế giá trị gia tăng 3%', GETDATE(), GETDATE()),
(N'VAT 12%', 12.00, N'Thuế giá trị gia tăng 12%', GETDATE(), GETDATE());
GO

-- Insert into roles
INSERT INTO [dbo].[roles] ([name], [description], [created_at], [last_modified]) VALUES
('ADMIN', N'Quản trị viên hệ thống', GETDATE(), GETDATE()),
('MANAGER', N'Quản lý chi nhánh', GETDATE(), GETDATE()),
('EMPLOYEE', N'Nhân viên', GETDATE(), GETDATE()),
('CUSTOMER', N'Khách hàng', GETDATE(), GETDATE()),
('GUEST', N'Khách vãng lai', GETDATE(), GETDATE()),
('SUPERVISOR', N'Giám sát viên', GETDATE(), GETDATE()),
('CASHIER', N'Thu ngân', GETDATE(), GETDATE());
GO

-- Insert into branches
INSERT INTO [dbo].[branches] ([name], [address], [phone], [manager], [created_at], [last_modified]) VALUES
(N'Chi nhánh Quận 1', N'123 Nguyễn Huệ, Quận 1, TP.HCM', '0901234567', N'Nguyễn Văn A', GETDATE(), GETDATE()),
(N'Chi nhánh Quận 3', N'456 Võ Thị Sáu, Quận 3, TP.HCM', '0901234568', N'Trần Thị B', GETDATE(), GETDATE()),
(N'Chi nhánh Quận 7', N'789 Nguyễn Thị Thập, Quận 7, TP.HCM', '0901234569', N'Lê Văn C', GETDATE(), GETDATE()),
(N'Chi nhánh Hà Nội', N'321 Hoàn Kiếm, Hà Nội', '0901234570', N'Phạm Thị D', GETDATE(), GETDATE()),
(N'Chi nhánh Đà Nẵng', N'654 Hải Châu, Đà Nẵng', '0901234571', N'Hoàng Văn E', GETDATE(), GETDATE()),
(N'Chi nhánh Cần Thơ', N'987 Ninh Kiều, Cần Thơ', '0901234572', N'Ngô Thị F', GETDATE(), GETDATE()),
(N'Chi nhánh Quận 10', N'147 3 Tháng 2, Quận 10, TP.HCM', '0901234573', N'Võ Văn G', GETDATE(), GETDATE());
GO

-- Insert into suppliers
INSERT INTO [dbo].[suppliers] ([name], [phone], [email], [address], [note], [created_at], [last_modified]) VALUES
(N'Công ty Cà phê Trung Nguyên', '0281234567', 'contact@trungnguyengroup.com', N'234 Đường ABC, Quận 1, TP.HCM', N'Nhà cung cấp cà phê chính', GETDATE(), GETDATE()),
(N'Vinamilk', '0281234568', 'supplier@vinamilk.com', N'345 Đường DEF, Quận 2, TP.HCM', N'Cung cấp sữa tươi', GETDATE(), GETDATE()),
(N'Công ty Đường Biên Hòa', '0281234569', 'sales@bienhoasugar.com', N'456 Đường GHI, Biên Hòa', N'Cung cấp đường', GETDATE(), GETDATE()),
(N'Tập đoàn Phúc Sinh', '0281234570', 'order@phucsinhgroup.com', N'567 Đường JKL, Quận 5, TP.HCM', N'Cung cấp trà', GETDATE(), GETDATE()),
(N'Công ty Thực phẩm ABC', '0281234571', 'info@abcfood.com', N'678 Đường MNO, Quận 6, TP.HCM', N'Cung cấp gia vị', GETDATE(), GETDATE()),
(N'Công ty Highlands Coffee', '0281234572', 'supplier@highlands.com.vn', N'789 Đường PQR, Quận 7, TP.HCM', N'Cung cấp cà phê rang xay', GETDATE(), GETDATE()),
(N'Dairy Farm Vietnam', '0281234573', 'contact@dairyfarm.vn', N'890 Đường STU, Quận 8, TP.HCM', N'Cung cấp sản phẩm từ sữa', GETDATE(), GETDATE());
GO

-- Insert into shipping_providers
INSERT INTO [dbo].[shipping_providers] ([name], [contact_info], [api_endpoint], [created_at], [last_modified]) VALUES
('Giao Hàng Nhanh', 'GHN - 1900 1900', 'https://api.ghn.vn', GETDATE(), GETDATE()),
('Viettel Post', 'VTP - 1900 8095', 'https://api.viettelpost.vn', GETDATE(), GETDATE()),
('J&T Express', 'J&T - 1900 1088', 'https://api.jtexpress.vn', GETDATE(), GETDATE()),
('Grab Express', 'Grab - 1900 1717', 'https://api.grab.com', GETDATE(), GETDATE()),
('Be Delivery', 'Be - 1900 2323', 'https://api.be.com.vn', GETDATE(), GETDATE()),
('Now Ship', 'Now - 1900 1515', 'https://api.foody.vn', GETDATE(), GETDATE()),
('AhaMove', 'Aha - 1900 6886', 'https://api.ahamove.com', GETDATE(), GETDATE());
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
(2, N'Hoàng Văn E', '0901111115', 'hve@coffee.com', N'Nhân viên kho', '2023-05-01', 'ACTIVE', GETDATE(), GETDATE()),
(4, N'Ngô Thị F', '0901111116', 'ntf@coffee.com', N'Nhân viên marketing', '2023-06-15', 'ACTIVE', GETDATE(), GETDATE()),
(5, N'Võ Văn G', '0901111117', 'vvg@coffee.com', N'Nhân viên bảo trì', '2023-07-20', 'ACTIVE', GETDATE(), GETDATE());
GO

-- Insert into products
INSERT INTO [dbo].[products] ([price], [category_id], [tax_id], [description], [name], [thumbnail], [created_at], [last_modified]) VALUES
(45000, 1, 1, N'Cà phê espresso đậm đà, hương vị mạnh mẽ', N'Espresso', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE()),
(55000, 1, 1, N'Cà phê cappuccino với lớp foam mịn màng', N'Cappuccino', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE()),
(35000, 2, 1, N'Trà xanh thơm mát, tốt cho sức khỏe', N'Trà xanh', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE()),
(40000, 3, 1, N'Nước ép cam tươi nguyên chất', N'Nước ép cam', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE()),
(25000, 4, 1, N'Bánh croissant bơ thơm ngon', N'Croissant', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE()),
(35000, 1, 1, N'Cà phê latte thơm ngon với sữa tươi', N'Latte', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE()),
(20000, 4, 1, N'Bánh muffin chocolate chip', N'Muffin Chocolate', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE());
GO

-- Insert into ingredients
INSERT INTO [dbo].[ingredients] ([category_id], [name], [unit], [description], [tax_id], [created_at], [last_modified]) VALUES
(1, N'Hạt cà phê Arabica', 'kg', N'Hạt cà phê Arabica chất lượng cao', 1, GETDATE(), GETDATE()),
(1, N'Hạt cà phê Robusta', 'kg', N'Hạt cà phê Robusta đậm đà', 1, GETDATE(), GETDATE()),
(3, N'Sữa tươi', N'L', N'Sữa tươi nguyên chất', 1, GETDATE(), GETDATE()),
(4, N'Đường trắng', 'kg', N'Đường trắng tinh luyện', 1, GETDATE(), GETDATE()),
(2, N'Lá trà xanh', 'kg', N'Lá trà xanh tươi', 1, GETDATE(), GETDATE()),
(5, N'Bột cacao', 'kg', N'Bột cacao nguyên chất', 1, GETDATE(), GETDATE()),
(6, N'Kem tươi', N'L', N'Kem tươi để trang trí', 1, GETDATE(), GETDATE());
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
(3, 1, '1993-07-18', 3, '0901111113', N'Lê Văn C', N'654 Nguyễn Thị Minh Khai, Quận 3, TP.HCM', 'hashed_password_5', GETDATE(), GETDATE()),
(NULL, 1, '1991-09-05', 4, '0912345680', N'Phạm Thị D', N'987 Võ Văn Tần, Quận 3, TP.HCM', 'hashed_password_6', GETDATE(), GETDATE()),
(4, 1, '1994-04-12', 3, '0901111114', N'Hoàng Văn E', N'147 Cách Mạng Tháng 8, Quận 10, TP.HCM', 'hashed_password_7', GETDATE(), GETDATE());
GO

-- Insert into product_images
INSERT INTO [dbo].[product_images] ([product_id], [image_url]) VALUES
(1, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE='),
(2, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE='),
(3, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE='),
(4, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE='),
(5, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE='),
(6, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE='),
(7, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=');
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

-- Insert into ingredient_transfers
INSERT INTO [dbo].[ingredient_transfers] ([ingredient_id], [branch_id], [quantity], [transfer_type],  [note], [created_at], [last_modified]) VALUES
(1, 1, 20, 'IN', N'Chuyển hạt cà phê cho chi nhánh Q1', GETDATE(), GETDATE()),
(2, 2, 15, 'IN', N'Chuyển hạt cà phê cho chi nhánh Q3', GETDATE(), GETDATE()),
(3, 3, 50, 'IN', N'Chuyển sữa tươi cho chi nhánh Q7', GETDATE(), GETDATE()),
(4, 1, 30, 'IN', N'Chuyển đường cho chi nhánh Q1', GETDATE(), GETDATE()),
(5, 2, 10, 'IN', N'Chuyển trà xanh cho chi nhánh Q3', GETDATE(), GETDATE());
GO

-- Insert into supplier_ingredient_prices
INSERT INTO [dbo].[supplier_ingredient_prices] ([supplier_id], [ingredient_id], [price], [unit], [effective_date], [expired_date], [created_at], [last_modified]) VALUES
(1, 1, 200000, 'kg', GETDATE(), '2025-12-31', GETDATE(), GETDATE()),
(1, 2, 150000, 'kg', GETDATE(), '2025-12-31', GETDATE(), GETDATE()),
(2, 3, 25000, 'L', GETDATE(), '2025-06-30', GETDATE(), GETDATE()),
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

-- Insert additional users first for more customers
INSERT INTO [dbo].[users] ([employee_id], [is_active], [date_of_birth], [role_id], [phone_number], [fullname], [address], [password], [created_at], [last_modified]) VALUES
(NULL, 1, '1990-03-15', 4, '0923456789', N'Nguyễn Thị F', N'111 Nguyễn Du, Quận 1, TP.HCM', 'hashed_password_8', GETDATE(), GETDATE()),
(NULL, 1, '1985-07-20', 4, '0934567890', N'Trần Văn G', N'222 Lý Tự Trọng, Quận 1, TP.HCM', 'hashed_password_9', GETDATE(), GETDATE()),
(NULL, 1, '1992-11-12', 4, '0945678901', N'Lê Thị H', N'333 Đồng Khởi, Quận 1, TP.HCM', 'hashed_password_10', GETDATE(), GETDATE()),
(NULL, 1, '1987-08-25', 4, '0956789012', N'Phạm Văn I', N'444 Cống Quỳnh, Quận 1, TP.HCM', 'hashed_password_11', GETDATE(), GETDATE());
GO

-- Insert into customers (uses manual id to match user_id)
INSERT INTO [dbo].[customers] ([id], [user_id], [fullname], [phone_number], [email], [address], [created_at], [last_modified]) VALUES
(3, 3, N'Khách hàng VIP 1', '0912345678', 'vip1@email.com', N'789 Hai Bà Trưng, Quận 3, TP.HCM', GETDATE(), GETDATE()),
(4, 4, N'Khách hàng VIP 2', '0912345679', 'vip2@email.com', N'321 Pasteur, Quận 1, TP.HCM', GETDATE(), GETDATE()),
(6, 6, N'Phạm Thị D', '0912345680', 'ptd@email.com', N'987 Võ Văn Tần, Quận 3, TP.HCM', GETDATE(), GETDATE()),
(7, 7, N'Hoàng Văn E', '0901111114', 'hve@email.com', N'147 Cách Mạng Tháng 8, Quận 10, TP.HCM', GETDATE(), GETDATE());
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
(NEWID(), 'ORD003', 6, 2, 120000, 1, GETDATE(), GETDATE(), N'Đơn hàng online'),
(NEWID(), 'ORD004', 7, 3, 95000, 2, DATEADD(hour, -2, GETDATE()), DATEADD(hour, -2, GETDATE()), N'Đơn hàng takeaway'),
(NEWID(), 'ORD005', 3, 1, 85000, 1, DATEADD(hour, -1, GETDATE()), DATEADD(hour, -1, GETDATE()), N'Đơn hàng giao nhanh');
GO

-- Insert into order_details
INSERT INTO [dbo].[order_details] ([quantity], [order_id], [product_id], [color], [created_at], [last_modified], [note], [total_amount], [unit_price]) VALUES
(2, 1, 1, NULL, GETDATE(), GETDATE(), N'Không đường', 90000, 45000),
(1, 1, 5, NULL, GETDATE(), GETDATE(), N'Thêm bơ', 25000, 25000),
(1, 2, 2, NULL, DATEADD(day, -1, GETDATE()), DATEADD(day, -1, GETDATE()), N'Ít đá', 55000, 55000),
(3, 3, 3, NULL, GETDATE(), GETDATE(), N'Nóng', 105000, 35000),
(2, 4, 4, NULL, DATEADD(hour, -2, GETDATE()), DATEADD(hour, -2, GETDATE()), N'Không đường', 80000, 40000),
(1, 5, 6, NULL, DATEADD(hour, -1, GETDATE()), DATEADD(hour, -1, GETDATE()), N'Size L', 35000, 35000);
GO

-- Insert into order_payments
INSERT INTO [dbo].[order_payments] ([order_id], [payment_method_id], [payment_status_id], [amount], [payment_date], [transaction_id], [notes], [created_at], [last_modified]) VALUES
(1, 4, 2, 100000, GETDATE(), 'MOMO123456789', N'Thanh toán qua MoMo', GETDATE(), GETDATE()),
(2, 1, 2, 75000, DATEADD(day, -1, GETDATE()), NULL, N'Thanh toán tiền mặt', DATEADD(day, -1, GETDATE()), DATEADD(day, -1, GETDATE())),
(3, 4, 1, 120000, NULL, 'ZALO987654321', N'Đang chờ thanh toán ZaloPay', GETDATE(), GETDATE()),
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

-- Views are automatically populated from base tables - no manual INSERT needed
-- v_sales_summary, v_employee_payroll, v_inventory_status, v_profit_summary, v_expenses_summary
-- will display data based on underlying tables (orders, employees, inventory, financial_reports, etc.)

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


-- Insert missing data for tables that need seed data

-- Insert additional Purchase Invoice Details
INSERT INTO [dbo].[purchase_invoice_details] 
([invoice_id], [ingredient_id], [quantity], [unit_price], [amount_before_tax], [tax_rate], [tax_amount], [amount_after_tax], [discount_rate], [discount_amount], [final_amount], [expiry_date], [batch_number], [note], [created_at], [last_modified])
VALUES 
(1, 1, 100, 200000, 20000000, 10.00, 2000000, 22000000, 0, 0, 22000000, '2025-12-31', 'BATCH001', N'Hạt cà phê Arabica chất lượng cao', GETDATE(), GETDATE()),
(1, 2, 50, 150000, 7500000, 10.00, 750000, 8250000, 0, 0, 8250000, '2025-12-31', 'BATCH002', N'Hạt cà phê Robusta', GETDATE(), GETDATE()),
(2, 3, 200, 25000, 5000000, 10.00, 500000, 5500000, 5.00, 250000, 5250000, '2025-06-30', 'BATCH003', N'Sữa tươi nguyên chất', GETDATE(), GETDATE()),
(2, 4, 100, 15000, 1500000, 10.00, 150000, 1650000, 0, 0, 1650000, '2026-12-31', 'BATCH004', N'Đường trắng tinh luyện', GETDATE(), GETDATE()),
(3, 5, 20, 50000, 1000000, 10.00, 100000, 1100000, 0, 0, 1100000, '2025-12-31', 'BATCH005', N'Lá trà xanh tươi', GETDATE(), GETDATE());
GO

-- Insert Goods Received Details
INSERT INTO [dbo].[goods_received_details] 
([grn_id], [ingredient_id], [ordered_quantity], [received_quantity], [rejected_quantity], [quality_status], [rejection_reason], [unit_price], [expiry_date], [batch_number], [storage_location], [note], [created_at], [last_modified])
VALUES 
(1, 1, 100, 100, 0, 'ACCEPTED', '', 200000, '2025-12-31', 'BATCH001', 'A01', N'Chất lượng tốt', GETDATE(), GETDATE()),
(1, 2, 50, 50, 0, 'ACCEPTED', '', 150000, '2025-12-31', 'BATCH002', 'A02', N'Đạt tiêu chuẩn', GETDATE(), GETDATE()),
(2, 3, 200, 190, 10, 'PARTIALLY_ACCEPTED', N'10 lít bị hỏng bao bì', 25000, '2025-06-30', 'BATCH003', 'B01', N'Một phần hàng có vấn đề', GETDATE(), GETDATE()),
(2, 4, 100, 100, 0, 'ACCEPTED', '', 15000, '2026-12-31', 'BATCH004', 'C01', N'Hàng tốt', GETDATE(), GETDATE()),
(3, 5, 20, 15, 0, 'ACCEPTED', '', 50000, '2025-12-31', 'BATCH005', 'D01', N'Đang kiểm tra', GETDATE(), GETDATE());
GO

-- Insert Purchase Return Details
INSERT INTO [dbo].[purchase_return_details] 
([return_id], [ingredient_id], [return_quantity], [unit_price], [return_amount], [return_reason], [batch_number], [expiry_date], [quality_issue], [note], [created_at], [last_modified])
VALUES 
(1, 3, 10, 25000, 250000, N'Bao bì bị hỏng', 'BATCH003', '2025-06-30', N'Rò rỉ, không đảm bảo chất lượng', N'Trả hàng do vận chuyển', GETDATE(), GETDATE()),
(1, 4, 20, 15000, 300000, N'Không đúng quy cách', 'BATCH004', '2026-12-31', N'Hạt to không đều', N'Không đạt tiêu chuẩn', GETDATE(), GETDATE()),
(2, 1, 10, 200000, 2000000, N'Giao nhầm loại', 'BATCH001', '2025-12-31', N'Giao Robusta thay vì Arabica', N'Lỗi giao hàng', GETDATE(), GETDATE()),
(2, 2, 5, 150000, 750000, N'Hạt bị ẩm mốc', 'BATCH002', '2025-12-31', N'Bảo quản không tốt', N'Chất lượng kém', GETDATE(), GETDATE());
GO

-- Insert more data into existing tables that need more records

-- Additional branches
INSERT INTO [dbo].[branches] ([name], [address], [phone], [manager], [created_at], [last_modified]) VALUES
(N'Chi nhánh Bình Thạnh', N'890 Điện Biên Phủ, Bình Thạnh, TP.HCM', '0901234572', N'Nguyễn Thị F', GETDATE(), GETDATE()),
(N'Chi nhánh Thủ Đức', N'123 Võ Văn Ngân, Thủ Đức, TP.HCM', '0901234573', N'Trần Văn G', GETDATE(), GETDATE()),
(N'Chi nhánh Gò Vấp', N'456 Quang Trung, Gò Vấp, TP.HCM', '0901234574', N'Lê Thị H', GETDATE(), GETDATE()),
(N'Chi nhánh Tân Bình', N'789 Cộng Hòa, Tân Bình, TP.HCM', '0901234575', N'Phạm Văn I', GETDATE(), GETDATE()),
(N'Chi nhánh Phú Nhuận', N'321 Phan Xích Long, Phú Nhuận, TP.HCM', '0901234576', N'Hoàng Thị J', GETDATE(), GETDATE());
GO

-- Additional suppliers
INSERT INTO [dbo].[suppliers] ([name], [phone], [email], [address], [note], [created_at], [last_modified]) VALUES
(N'Công ty Bánh kẹo Kinh Đô', '0281234572', 'contact@kinhdo.com', N'101 Đường PQR, Quận 8, TP.HCM', N'Cung cấp bánh kẹo', GETDATE(), GETDATE()),
(N'Tập đoàn Masan', '0281234573', 'supplier@masan.com', N'202 Đường STU, Quận 4, TP.HCM', N'Cung cấp gia vị và thực phẩm', GETDATE(), GETDATE()),
(N'Công ty Cà phê Highlands', '0281234574', 'order@highlands.com', N'303 Đường VWX, Quận 2, TP.HCM', N'Cung cấp cà phê chuyên nghiệp', GETDATE(), GETDATE()),
(N'Vinacafe Biên Hòa', '0281234575', 'sales@vinacafe.com', N'404 Đường YZ, Biên Hòa', N'Cà phê hòa tan', GETDATE(), GETDATE()),
(N'Công ty Sữa TH True Milk', '0281234576', 'info@thmilk.com', N'505 Đường ABC2, Quận 7, TP.HCM', N'Sữa tươi organic', GETDATE(), GETDATE());
GO

-- Additional products
INSERT INTO [dbo].[products] ([price], [category_id], [tax_id], [description], [name], [thumbnail], [created_at], [last_modified]) VALUES
(65000, 1, 1, N'Cà phê latte với nghệ thuật foam đẹp mắt', N'Latte Art', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE()),
(50000, 1, 1, N'Cà phê Americano đậm đà, ít calo', N'Americano', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE()),
(45000, 2, 1, N'Trà sữa Thái lan đậm đà, thơm ngon', N'Trà sữa Thái', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE()),
(60000, 3, 1, N'Sinh tố bơ sáp dinh dưỡng', N'Sinh tố bơ', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE()),
(30000, 4, 1, N'Bánh muffin blueberry tươi ngon', N'Muffin Blueberry', 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=', GETDATE(), GETDATE());
GO

-- Additional ingredients
INSERT INTO [dbo].[ingredients] ([category_id], [name], [unit], [description], [tax_id], [created_at], [last_modified]) VALUES
(3, N'Kem tươi', N'L', N'Kem tươi whipping cream', 1, GETDATE(), GETDATE()),
(4, N'Mật ong', 'kg', N'Mật ong thiên nhiên nguyên chất', 1, GETDATE(), GETDATE()),
(5, N'Bột vani', 'kg', N'Bột vani tự nhiên', 1, GETDATE(), GETDATE()),
(5, N'Bột quế', 'kg', N'Bột quế Ceylon cao cấp', 1, GETDATE(), GETDATE()),
(2, N'Lá trà ô long', 'kg', N'Lá trà ô long thượng hạng', 1, GETDATE(), GETDATE());
GO

-- Additional employees
INSERT INTO [dbo].[employees] ([branch_id], [full_name], [phone], [email], [position], [hire_date], [status], [created_at], [last_modified]) VALUES
(4, N'Võ Thị K', '0901111116', 'vtk@coffee.com', N'Quản lý ca', '2023-06-15', 'ACTIVE', GETDATE(), GETDATE()),
(5, N'Đỗ Văn L', '0901111117', 'dvl@coffee.com', N'Nhân viên pha chế', '2023-07-01', 'ACTIVE', GETDATE(), GETDATE()),
(6, N'Bùi Thị M', '0901111118', 'btm@coffee.com', N'Nhân viên phục vụ', '2023-08-10', 'ACTIVE', GETDATE(), GETDATE()),
(7, N'Cao Văn N', '0901111119', 'cvn@coffee.com', N'Thu ngân', '2023-09-05', 'ACTIVE', GETDATE(), GETDATE()),
(8, N'Đinh Thị O', '0901111120', 'dto@coffee.com', N'Nhân viên vệ sinh', '2023-10-01', 'ACTIVE', GETDATE(), GETDATE());
GO

-- Additional product images for new products
INSERT INTO [dbo].[product_images] ([product_id], [image_url]) VALUES
(6, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE='),
(7, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE='),
(8, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE='),
(9, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE='),
(10, 'https://media.istockphoto.com/id/1400194993/photo/cappuccino-art.jpg?s=612x612&w=0&k=20&c=_nYOcyQ15cYEeUYgUzkC5qG946nkCwU06NiWKt1s8SE=');
GO

-- Additional product recipes for new products
INSERT INTO [dbo].[product_recipes] ([product_id], [ingredient_id], [quantity], [created_at], [last_modified]) VALUES
(6, 1, 0.015, GETDATE(), GETDATE()), -- Latte Art cần 15g cà phê Arabica
(6, 3, 0.15, GETDATE(), GETDATE()), -- Latte Art cần 150ml sữa tươi
(6, 6, 0.02, GETDATE(), GETDATE()), -- Latte Art cần 20ml kem tươi
(7, 1, 0.02, GETDATE(), GETDATE()), -- Americano cần 20g cà phê Arabica
(8, 10, 0.008, GETDATE(), GETDATE()), -- Trà sữa Thái cần 8g lá trà ô long
(8, 3, 0.1, GETDATE(), GETDATE()), -- Trà sữa Thái cần 100ml sữa tươi
(9, 4, 0.015, GETDATE(), GETDATE()), -- Sinh tố bơ cần 15g đường
(10, 4, 0.012, GETDATE(), GETDATE()); -- Muffin cần 12g đường
GO

-- Additional users for employees
INSERT INTO [dbo].[users] ([employee_id], [is_active], [date_of_birth], [role_id], [phone_number], [fullname], [address], [password], [created_at], [last_modified]) VALUES
(6, 1, '1991-06-15', 2, '0901111116', N'Võ Thị K', N'111 Lê Văn Sỹ, Quận 3, TP.HCM', 'hashed_password_8', GETDATE(), GETDATE()),
(7, 1, '1994-07-01', 3, '0901111117', N'Đỗ Văn L', N'222 Cách Mạng Tháng 8, Quận 10, TP.HCM', 'hashed_password_9', GETDATE(), GETDATE()),
(8, 1, '1996-08-10', 3, '0901111118', N'Bùi Thị M', N'333 Nguyễn Trãi, Quận 5, TP.HCM', 'hashed_password_10', GETDATE(), GETDATE()),
(9, 1, '1992-09-05', 3, '0901111119', N'Cao Văn N', N'444 Võ Văn Tần, Quận 3, TP.HCM', 'hashed_password_11', GETDATE(), GETDATE()),
(10, 1, '1989-10-01', 3, '0901111120', N'Đinh Thị O', N'555 Hoàng Hoa Thám, Quận Tân Bình, TP.HCM', 'hashed_password_12', GETDATE(), GETDATE());
GO

-- Additional customers
INSERT INTO [dbo].[users] ([employee_id], [is_active], [date_of_birth], [role_id], [phone_number], [fullname], [address], [password], [created_at], [last_modified]) VALUES
(NULL, 1, '1987-04-12', 4, '0945678901', N'Phan Thị P', N'666 Trần Hưng Đạo, Quận 1, TP.HCM', 'hashed_password_13', GETDATE(), GETDATE()),
(NULL, 1, '1993-11-08', 4, '0956789012', N'Đặng Văn Q', N'777 Nguyễn Văn Cừ, Quận 5, TP.HCM', 'hashed_password_14', GETDATE(), GETDATE()),
(NULL, 1, '1990-02-28', 4, '0967890123', N'Lý Thị R', N'888 Lê Lợi, Quận 1, TP.HCM', 'hashed_password_15', GETDATE(), GETDATE());
GO

INSERT INTO [dbo].[customers] ([id], [user_id], [fullname], [phone_number], [email], [address], [created_at], [last_modified]) VALUES
(8, 8, N'Phan Thị P', '0945678901', 'pthip@email.com', N'666 Trần Hưng Đạo, Quận 1, TP.HCM', GETDATE(), GETDATE()),
(9, 9, N'Đặng Văn Q', '0956789012', 'dvq@email.com', N'777 Nguyễn Văn Cừ, Quận 5, TP.HCM', GETDATE(), GETDATE()),
(10, 10, N'Lý Thị R', '0967890123', 'ltr@email.com', N'888 Lê Lợi, Quận 1, TP.HCM', GETDATE(), GETDATE());
GO

-- Additional orders
INSERT INTO [dbo].[orders] ([order_uuid], [order_code], [customer_id], [branch_id], [total_money], [status_id], [created_at], [last_modified], [notes]) VALUES
(NEWID(), 'ORD006', 8, 4, 130000, 1, GETDATE(), GETDATE(), N'Đơn hàng mới'),
(NEWID(), 'ORD007', 9, 5, 90000, 2, DATEADD(hour, -3, GETDATE()), DATEADD(hour, -3, GETDATE()), N'Đơn hàng dine-in'),
(NEWID(), 'ORD008', 10, 6, 110000, 1, GETDATE(), GETDATE(), N'Đơn hàng delivery'),
(NEWID(), 'ORD009', 3, 7, 85000, 2, DATEADD(hour, -4, GETDATE()), DATEADD(hour, -4, GETDATE()), N'Đơn hàng pickup'),
(NEWID(), 'ORD010', 4, 1, 125000, 3, DATEADD(hour, -2, GETDATE()), DATEADD(hour, -2, GETDATE()), N'Đơn hàng express');
GO

-- Additional order details for new orders
INSERT INTO [dbo].[order_details] ([quantity], [order_id], [product_id], [color], [created_at], [last_modified], [note], [total_amount], [unit_price]) VALUES
(1, 6, 6, NULL, GETDATE(), GETDATE(), N'Extra hot', 65000, 65000),
(2, 6, 7, NULL, GETDATE(), GETDATE(), N'Less sugar', 100000, 50000),
(1, 7, 8, NULL, DATEADD(hour, -3, GETDATE()), DATEADD(hour, -3, GETDATE()), N'Extra pearls', 45000, 45000),
(1, 7, 9, NULL, DATEADD(hour, -3, GETDATE()), DATEADD(hour, -3, GETDATE()), N'No sugar', 60000, 60000),
(2, 8, 10, NULL, GETDATE(), GETDATE(), N'Extra chocolate', 60000, 30000),
(1, 8, 3, NULL, GETDATE(), GETDATE(), N'Hot tea', 35000, 35000),
(3, 9, 1, NULL, DATEADD(hour, -4, GETDATE()), DATEADD(hour, -4, GETDATE()), N'To go', 135000, 45000),
(2, 10, 2, NULL, DATEADD(hour, -2, GETDATE()), DATEADD(hour, -2, GETDATE()), N'Extra foam', 110000, 55000);
GO

-- Additional inventory thresholds
INSERT INTO [dbo].[inventory_thresholds] ([ingredient_id], [branch_id], [safety_stock], [reorder_point], [maximum_stock], [created_at], [last_modified]) VALUES
(1, 1, 20, 50, 200, GETDATE(), GETDATE()),
(2, 1, 15, 40, 150, GETDATE(), GETDATE()),
(3, 1, 50, 100, 500, GETDATE(), GETDATE()),
(4, 2, 30, 80, 300, GETDATE(), GETDATE()),
(5, 2, 10, 25, 100, GETDATE(), GETDATE()),
(6, 3, 5, 15, 50, GETDATE(), GETDATE()),
(7, 4, 20, 50, 200, GETDATE(), GETDATE()),
(8, 5, 8, 20, 80, GETDATE(), GETDATE()),
(9, 6, 12, 30, 120, GETDATE(), GETDATE()),
(10, 7, 6, 18, 60, GETDATE(), GETDATE());
GO

-- Insert into permissions
INSERT INTO [dbo].[permissions] ([name], [description], [resource], [action], [created_at], [last_modified]) VALUES
(N'USER_CREATE', N'Tạo người dùng mới', N'USER', N'CREATE', GETDATE(), GETDATE()),
(N'USER_READ', N'Xem thông tin người dùng', N'USER', N'READ', GETDATE(), GETDATE()),
(N'USER_UPDATE', N'Cập nhật thông tin người dùng', N'USER', N'UPDATE', GETDATE(), GETDATE()),
(N'USER_DELETE', N'Xóa người dùng', N'USER', N'DELETE', GETDATE(), GETDATE()),
(N'PRODUCT_CREATE', N'Tạo sản phẩm mới', N'PRODUCT', N'CREATE', GETDATE(), GETDATE()),
(N'PRODUCT_READ', N'Xem thông tin sản phẩm', N'PRODUCT', N'READ', GETDATE(), GETDATE()),
(N'PRODUCT_UPDATE', N'Cập nhật sản phẩm', N'PRODUCT', N'UPDATE', GETDATE(), GETDATE()),
(N'PRODUCT_DELETE', N'Xóa sản phẩm', N'PRODUCT', N'DELETE', GETDATE(), GETDATE()),
(N'ORDER_CREATE', N'Tạo đơn hàng', N'ORDER', N'CREATE', GETDATE(), GETDATE()),
(N'ORDER_READ', N'Xem đơn hàng', N'ORDER', N'READ', GETDATE(), GETDATE()),
(N'ORDER_UPDATE', N'Cập nhật đơn hàng', N'ORDER', N'UPDATE', GETDATE(), GETDATE()),
(N'ORDER_DELETE', N'Hủy đơn hàng', N'ORDER', N'DELETE', GETDATE(), GETDATE()),
(N'INVENTORY_MANAGE', N'Quản lý kho', N'INVENTORY', N'MANAGE', GETDATE(), GETDATE()),
(N'REPORT_VIEW', N'Xem báo cáo', N'REPORT', N'VIEW', GETDATE(), GETDATE()),
(N'BRANCH_MANAGE', N'Quản lý chi nhánh', N'BRANCH', N'MANAGE', GETDATE(), GETDATE());
GO

-- Insert into role_permissions
INSERT INTO [dbo].[role_permissions] ([role_id], [permission_id], [created_at], [last_modified]) VALUES
-- ADMIN has all permissions
(1, 1, GETDATE(), GETDATE()), (1, 2, GETDATE(), GETDATE()), (1, 3, GETDATE(), GETDATE()), 
(1, 4, GETDATE(), GETDATE()), (1, 5, GETDATE(), GETDATE()), (1, 6, GETDATE(), GETDATE()),
(1, 7, GETDATE(), GETDATE()), (1, 8, GETDATE(), GETDATE()), (1, 9, GETDATE(), GETDATE()),
(1, 10, GETDATE(), GETDATE()), (1, 11, GETDATE(), GETDATE()), (1, 12, GETDATE(), GETDATE()),
(1, 13, GETDATE(), GETDATE()), (1, 14, GETDATE(), GETDATE()), (1, 15, GETDATE(), GETDATE()),
-- MANAGER has most permissions except user delete
(2, 1, GETDATE(), GETDATE()), (2, 2, GETDATE(), GETDATE()), (2, 3, GETDATE(), GETDATE()),
(2, 5, GETDATE(), GETDATE()), (2, 6, GETDATE(), GETDATE()), (2, 7, GETDATE(), GETDATE()),
(2, 9, GETDATE(), GETDATE()), (2, 10, GETDATE(), GETDATE()), (2, 11, GETDATE(), GETDATE()),
(2, 13, GETDATE(), GETDATE()), (2, 14, GETDATE(), GETDATE()), (2, 15, GETDATE(), GETDATE()),
-- EMPLOYEE has limited permissions
(3, 2, GETDATE(), GETDATE()), (3, 6, GETDATE(), GETDATE()), (3, 9, GETDATE(), GETDATE()),
(3, 10, GETDATE(), GETDATE()), (3, 11, GETDATE(), GETDATE()),
-- CUSTOMER has read-only permissions
(4, 6, GETDATE(), GETDATE()), (4, 10, GETDATE(), GETDATE());
GO

-- Insert into recipes
INSERT INTO [dbo].[recipes] ([name], [description], [product_id], [serving_size], [unit], [is_active], [notes], [created_at], [last_modified]) VALUES
(N'Công thức Espresso', N'Công thức pha chế espresso chuẩn Italy', 1, 250, 'ml', 1, N'Sử dụng hạt cà phê Arabica cao cấp', GETDATE(), GETDATE()),
(N'Công thức Cappuccino', N'Công thức cappuccino với tỷ lệ 1:1:1', 2, 200, 'ml', 1, N'Cần đánh sữa tạo foam mịn', GETDATE(), GETDATE()),
(N'Công thức Trà xanh', N'Công thức pha trà xanh truyền thống', 3, 300, 'ml', 1, N'Nhiệt độ nước 80-85°C', GETDATE(), GETDATE()),
(N'Công thức Nước ép cam', N'Nước ép cam tươi nguyên chất', 4, 350, 'ml', 1, N'Sử dụng cam tươi vắt', GETDATE(), GETDATE()),
(N'Công thức Croissant', N'Bánh croissant bơ truyền thống Pháp', 5, 1, 'cái', 1, N'Cần ủ bột 12 tiếng', GETDATE(), GETDATE()),
(N'Công thức Latte', N'Cà phê latte với lớp sữa mềm mại', 1, 350, 'ml', 1, N'Tỷ lệ cà phê:sữa = 1:3', GETDATE(), GETDATE()),
(N'Công thức Americano', N'Cà phê Americano đậm đà', 1, 300, 'ml', 1, N'Pha loãng espresso với nước nóng', GETDATE(), GETDATE());
GO

-- Insert into recipe_ingredients
INSERT INTO [dbo].[recipe_ingredients] ([recipe_id], [ingredient_id], [quantity], [unit], [waste_percentage], [notes], [is_optional], [sort_order], [created_at], [last_modified]) VALUES
-- Espresso recipe
(1, 1, 18.0, 'g', 5.0, N'Hạt cà phê Arabica xay mịn', 0, 1, GETDATE(), GETDATE()),
-- Cappuccino recipe
(2, 1, 18.0, 'g', 5.0, N'Hạt cà phê espresso', 0, 1, GETDATE(), GETDATE()),
(2, 3, 120.0, 'ml', 2.0, N'Sữa tươi nguyên chất', 0, 2, GETDATE(), GETDATE()),
(2, 4, 5.0, 'g', 0.0, N'Đường trắng tinh luyện', 1, 3, GETDATE(), GETDATE()),
-- Green tea recipe
(3, 5, 3.0, 'g', 0.0, N'Lá trà xanh cao cấp', 0, 1, GETDATE(), GETDATE()),
(3, 4, 3.0, 'g', 0.0, N'Đường phèn', 1, 2, GETDATE(), GETDATE()),
-- Orange juice recipe
(4, 4, 10.0, 'g', 0.0, N'Đường cát trắng', 1, 1, GETDATE(), GETDATE()),
-- Croissant recipe ingredients
(5, 4, 15.0, 'g', 0.0, N'Đường cho bánh', 0, 1, GETDATE(), GETDATE()),
-- Latte recipe
(6, 1, 18.0, 'g', 5.0, N'Espresso shot', 0, 1, GETDATE(), GETDATE()),
(6, 3, 200.0, 'ml', 3.0, N'Sữa tươi steamed', 0, 2, GETDATE(), GETDATE()),
-- Americano recipe
(7, 1, 18.0, 'g', 5.0, N'Double espresso shot', 0, 1, GETDATE(), GETDATE());
GO

-- Insert into inventory_movements
INSERT INTO [dbo].[inventory_movements] ([branch_id], [ingredient_id], [movement_type], [quantity], [unit], [quantity_before], [quantity_after], [reference_type], [reference_id], [reference_code], [notes], [employee_id], [movement_date], [created_at], [last_modified]) VALUES
(1, 1, 'IN', 50.0, 'kg', 20.0, 70.0, 'PURCHASE', 1, 'PO001', N'Nhập hàng từ nhà cung cấp', 1, GETDATE(), GETDATE(), GETDATE()),
(1, 2, 'IN', 30.0, 'kg', 15.0, 45.0, 'PURCHASE', 1, 'PO001', N'Nhập hàng cà phê Robusta', 2, GETDATE(), GETDATE(), GETDATE()),
(1, 1, 'OUT', 5.0, 'kg', 70.0, 65.0, 'ORDER', 1, 'ORD001', N'Xuất nguyên liệu pha chế', 2, GETDATE(), GETDATE(), GETDATE()),
(2, 3, 'IN', 100.0, 'liter', 50.0, 150.0, 'TRANSFER', 1, 'TF001', N'Chuyển kho từ chi nhánh khác', 3, GETDATE(), GETDATE(), GETDATE()),
(1, 4, 'OUT', 2.0, 'kg', 50.0, 48.0, 'ORDER', 2, 'ORD002', N'Sử dụng đường pha chế', 2, GETDATE(), GETDATE(), GETDATE()),
(3, 5, 'IN', 20.0, 'kg', 10.0, 30.0, 'PURCHASE', 4, 'PO004', N'Nhập lá trà xanh mới', 4, GETDATE(), GETDATE(), GETDATE()),
(2, 1, 'ADJUSTMENT', -2.0, 'kg', 40.0, 38.0, 'STOCKTAKE', NULL, 'ADJ001', N'Điều chỉnh sau kiểm kê', 5, GETDATE(), GETDATE(), GETDATE());
GO

-- Insert into ingredient_transfer_requests
INSERT INTO [dbo].[ingredient_transfer_requests] ([branch_id], [request_number], [request_date], [required_date], [status], [total_items], [approved_date], [completed_date], [note], [requested_by], [approved_by], [created_at], [last_modified]) VALUES
(1, 'TR001', GETDATE(), DATEADD(day, 2, GETDATE()), 'COMPLETED', 3, DATEADD(hour, 1, GETDATE()), DATEADD(day, 1, GETDATE()), N'Yêu cầu bổ sung nguyên liệu cuối tuần', N'Nguyễn Văn A', N'Trần Thị B', GETDATE(), GETDATE()),
(2, 'TR002', DATEADD(day, -1, GETDATE()), DATEADD(day, 1, GETDATE()), 'APPROVED', 2, GETDATE(), NULL, N'Cần thêm sữa tươi cho chi nhánh Q3', N'Lê Văn C', N'Nguyễn Văn A', GETDATE(), GETDATE()),
(3, 'TR003', GETDATE(), DATEADD(day, 3, GETDATE()), 'PENDING', 4, NULL, NULL, N'Yêu cầu nguyên liệu cho sự kiện đặc biệt', N'Phạm Thị D', NULL, GETDATE(), GETDATE()),
(1, 'TR004', DATEADD(hour, -3, GETDATE()), DATEADD(day, 1, GETDATE()), 'REJECTED', 1, DATEADD(hour, -1, GETDATE()), NULL, N'Không đủ tồn kho', N'Trần Thị B', N'Nguyễn Văn A', GETDATE(), GETDATE()),
(4, 'TR005', DATEADD(day, -2, GETDATE()), GETDATE(), 'COMPLETED', 5, DATEADD(day, -1, GETDATE()), GETDATE(), N'Chuyển kho khẩn cấp', N'Hoàng Văn E', N'Trần Thị B', GETDATE(), GETDATE()),
(2, 'TR006', GETDATE(), DATEADD(day, 2, GETDATE()), 'PENDING', 2, NULL, NULL, N'Bổ sung nguyên liệu trà', N'Lê Văn C', NULL, GETDATE(), GETDATE()),
(5, 'TR007', DATEADD(hour, -5, GETDATE()), DATEADD(day, 1, GETDATE()), 'APPROVED', 3, DATEADD(hour, -2, GETDATE()), NULL, N'Yêu cầu từ chi nhánh Đà Nẵng', N'Hoàng Văn E', N'Phạm Thị D', GETDATE(), GETDATE());
GO

-- Insert into ingredient_transfer_request_details
INSERT INTO [dbo].[ingredient_transfer_request_details] ([transfer_request_id], [ingredient_id], [requested_quantity], [approved_quantity], [transferred_quantity], [status], [note], [created_at], [last_modified]) VALUES
-- TR001 details (COMPLETED)
(1, 1, 20.0, 20.0, 20.0, 'TRANSFERRED', N'Đã chuyển đủ số lượng', GETDATE(), GETDATE()),
(1, 3, 50.0, 45.0, 45.0, 'TRANSFERRED', N'Giảm 5 lít do tồn kho', GETDATE(), GETDATE()),
(1, 4, 15.0, 15.0, 15.0, 'TRANSFERRED', N'Đã chuyển đủ', GETDATE(), GETDATE()),
-- TR002 details (APPROVED)
(2, 3, 80.0, 70.0, 0.0, 'APPROVED', N'Đang chuẩn bị chuyển', GETDATE(), GETDATE()),
(2, 5, 10.0, 10.0, 0.0, 'APPROVED', N'Sẵn sàng chuyển', GETDATE(), GETDATE()),
-- TR003 details (PENDING)
(3, 1, 30.0, NULL, 0.0, 'PENDING', N'Chờ duyệt', GETDATE(), GETDATE()),
(3, 2, 25.0, NULL, 0.0, 'PENDING', N'Chờ duyệt', GETDATE(), GETDATE()),
(3, 3, 60.0, NULL, 0.0, 'PENDING', N'Chờ duyệt', GETDATE(), GETDATE()),
(3, 4, 20.0, NULL, 0.0, 'PENDING', N'Chờ duyệt', GETDATE(), GETDATE()),
-- TR004 details (REJECTED)
(4, 1, 100.0, 0.0, 0.0, 'REJECTED', N'Không đủ tồn kho', GETDATE(), GETDATE()),
-- TR005 details (COMPLETED)
(5, 1, 15.0, 15.0, 15.0, 'TRANSFERRED', N'Đã hoàn thành', GETDATE(), GETDATE()),
(5, 2, 10.0, 10.0, 10.0, 'TRANSFERRED', N'Đã hoàn thành', GETDATE(), GETDATE()),
(5, 3, 40.0, 40.0, 40.0, 'TRANSFERRED', N'Đã hoàn thành', GETDATE(), GETDATE()),
(5, 4, 12.0, 12.0, 12.0, 'TRANSFERRED', N'Đã hoàn thành', GETDATE(), GETDATE()),
(5, 5, 8.0, 8.0, 8.0, 'TRANSFERRED', N'Đã hoàn thành', GETDATE(), GETDATE()),
-- TR006 details (PENDING)
(6, 5, 25.0, NULL, 0.0, 'PENDING', N'Chờ phê duyệt', GETDATE(), GETDATE()),
(6, 4, 18.0, NULL, 0.0, 'PENDING', N'Chờ phê duyệt', GETDATE(), GETDATE()),
-- TR007 details (APPROVED)
(7, 1, 22.0, 22.0, 0.0, 'APPROVED', N'Đã duyệt, chờ chuyển', GETDATE(), GETDATE()),
(7, 3, 35.0, 30.0, 0.0, 'APPROVED', N'Giảm do hạn chế tồn kho', GETDATE(), GETDATE()),
(7, 5, 12.0, 12.0, 0.0, 'APPROVED', N'Đã duyệt', GETDATE(), GETDATE());
GO

PRINT N'Sample data insertion completed successfully!';
GO

-- ====================================================================
-- ADDITIONAL DATA - 10 MORE RECORDS FOR EACH TABLE (RANDOM DATES)
-- ====================================================================

-- Additional Categories (10 records)
INSERT INTO [dbo].[categories] ([name]) VALUES
(N'Bánh mì'),
(N'Salad'),
(N'Súp'),
(N'Sandwich'),
(N'Pizza mini'),
(N'Mì Ý'),
(N'Cơm hộp'),
(N'Tráng miệng'),
(N'Snack'),
(N'Đồ uống đặc biệt');
GO

-- Additional Ingredient Categories (10 records)
INSERT INTO [dbo].[ingredient_categories] ([name], [description], [created_at], [last_modified]) VALUES
(N'Rau xanh', N'Các loại rau xanh tươi', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Thịt và hải sản', N'Thịt bò, gà, heo và hải sản', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Nước sốt', N'Các loại nước sốt và gia vị lỏng', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Đồ khô', N'Bánh crackers, bánh quy khô', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Dầu ăn', N'Dầu oliu, dầu hướng dương', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Hạt và óc chó', N'Hạnh nhân, óc chó, hạt điều', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Đồ đông lạnh', N'Nguyên liệu bảo quản lạnh', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Gia vị khô', N'Tiêu, muối, bột ngọt', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Đồ làm bánh', N'Bột nướng, men, chocolate', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Nước giải khát', N'Nước ngọt, nước khoáng', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Additional Taxes (10 records)
INSERT INTO [dbo].[taxes] ([name], [tax_rate], [description], [created_at], [last_modified]) VALUES
(N'VAT 2%', 2.00, N'Thuế giá trị gia tăng 2%', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'VAT 7%', 7.00, N'Thuế giá trị gia tăng 7%', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'VAT 9%', 9.00, N'Thuế giá trị gia tăng 9%', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'VAT 11%', 11.00, N'Thuế giá trị gia tăng 11%', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'VAT 13%', 13.00, N'Thuế giá trị gia tăng 13%', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'VAT 14%', 14.00, N'Thuế giá trị gia tăng 14%', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'VAT 16%', 16.00, N'Thuế giá trị gia tăng 16%', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'VAT 18%', 18.00, N'Thuế giá trị gia tăng 18%', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'VAT 20%', 20.00, N'Thuế giá trị gia tăng 20%', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'VAT 1%', 1.00, N'Thuế giá trị gia tăng 1%', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Additional Roles (10 records)
INSERT INTO [dbo].[roles] ([name], [description], [created_at], [last_modified]) VALUES
('ACCOUNTANT', N'Kế toán', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('BARISTA', N'Pha chế', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('CHEF', N'Đầu bếp', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('WAITER', N'Phục vụ', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('CLEANER', N'Vệ sinh', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('SECURITY', N'Bảo vệ', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('DELIVERY', N'Giao hàng', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('MARKETING', N'Marketing', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('IT_SUPPORT', N'Hỗ trợ IT', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('TRAINEE', N'Thực tập sinh', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Additional Branches (10 records)
INSERT INTO [dbo].[branches] ([name], [address], [phone], [manager], [created_at], [last_modified]) VALUES
(N'Chi nhánh Tân Bình', N'123 Hoàng Văn Thụ, P.4, Q.Tân Bình, TP.HCM', '028-38123456', N'Phạm Văn Đức', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Chi nhánh Gò Vấp', N'456 Nguyễn Oanh, P.17, Q.Gò Vấp, TP.HCM', '028-38234567', N'Trần Thị Mai', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Chi nhánh Phú Nhuận', N'789 Phan Xích Long, P.2, Q.Phú Nhuận, TP.HCM', '028-38345678', N'Lê Văn Thành', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Chi nhánh Bình Thạnh', N'321 Xô Viết Nghệ Tĩnh, P.21, Q.Bình Thạnh, TP.HCM', '028-38456789', N'Nguyễn Thị Lan', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Chi nhánh Quận 7', N'654 Nguyễn Thị Thập, P.Tân Phú, Q.7, TP.HCM', '028-38567890', N'Hoàng Văn Nam', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Chi nhánh Thủ Đức', N'987 Võ Văn Ngân, P.Linh Chiểu, TP.Thủ Đức, TP.HCM', '028-38678901', N'Vũ Thị Hương', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Chi nhánh Quận 2', N'147 Đỗ Xuân Hợp, P.Phước Long B, TP.Thủ Đức, TP.HCM', '028-38789012', N'Đặng Văn Khôi', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Chi nhánh Quận 9', N'258 Đ.Số 1, P.Bình Trưng Đông, TP.Thủ Đức, TP.HCM', '028-38890123', N'Lý Thị Nga', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Chi nhánh Quận 8', N'369 Tạ Quang Bửu, P.6, Q.8, TP.HCM', '028-38901234', N'Phan Văn Tùng', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Chi nhánh Tân Phú', N'741 Âu Cơ, P.14, Q.Tân Phú, TP.HCM', '028-38012345', N'Bùi Thị Yến', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Additional Suppliers (10 records)
INSERT INTO [dbo].[suppliers] ([name], [phone], [email], [address], [note], [created_at], [last_modified]) VALUES
(N'Công ty TNHH Cà phê Việt', '0901234567', 'info@cafetrung.vn', N'123 Lý Thường Kiệt, Quận 10, TP.HCM', N'Cung cấp hạt cà phê Arabica', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Vinamilk Distribution', '0912345678', 'sales@vinamilk.com', N'456 Nguyễn Đình Chiểu, Quận 3, TP.HCM', N'Sữa tươi và sản phẩm từ sữa', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Fresh Fruit Co', '0923456789', 'order@freshfruit.vn', N'789 Điện Biên Phủ, Quận 1, TP.HCM', N'Trái cây tươi nhập khẩu', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Đường Biên Hòa', '0934567890', 'contact@duongbienhoa.com', N'321 Phạm Ngũ Lão, Quận 1, TP.HCM', N'Đường trắng và đường vàng', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Green Tea Import', '0945678901', 'info@greentea.vn', N'654 Lê Văn Sỹ, Quận 3, TP.HCM', N'Trà xanh và trà thảo mộc', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Bánh Mì Saigon', '0956789012', 'order@banhmi.vn', N'987 Cách Mạng Tháng 8, Quận 10, TP.HCM', N'Bánh mì và sản phẩm làm bánh', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Gia vị Đông Nam Á', '0967890123', 'sales@spices.vn', N'147 Hoàng Sa, Quận 1, TP.HCM', N'Gia vị và thảo mộc', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Meat & Seafood Supply', '0978901234', 'order@meatseafood.vn', N'258 Nguyễn Trãi, Quận 5, TP.HCM', N'Thịt tươi và hải sản', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Organic Vegetables', '0989012345', 'contact@organic.vn', N'369 Võ Thị Sáu, Quận 3, TP.HCM', N'Rau xanh hữu cơ', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(N'Packaging Solutions', '0990123456', 'info@packaging.vn', N'741 Lý Tự Trọng, Quận 1, TP.HCM', N'Bao bì và vật liệu đóng gói', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Additional Employees (10 records)
INSERT INTO [dbo].[employees] ([branch_id], [full_name], [phone], [email], [position], [hire_date], [status], [created_at], [last_modified]) VALUES
(1, N'Nguyễn Văn Hùng', '0901111111', 'hung.nv@coffee.vn', N'Barista', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), 'ACTIVE', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(2, N'Trần Thị Linh', '0902222222', 'linh.tt@coffee.vn', N'Cashier', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), 'ACTIVE', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(1, N'Lê Văn Tài', '0903333333', 'tai.lv@coffee.vn', N'Chef', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), 'ACTIVE', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(3, N'Phạm Thị Hoa', '0904444444', 'hoa.pt@coffee.vn', N'Waiter', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), 'ACTIVE', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(2, N'Hoàng Văn Minh', '0905555555', 'minh.hv@coffee.vn', N'Supervisor', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), 'ACTIVE', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(1, N'Vũ Thị Lan', '0906666666', 'lan.vt@coffee.vn', N'Accountant', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), 'ACTIVE', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(3, N'Đặng Văn Phong', '0907777777', 'phong.dv@coffee.vn', N'Delivery', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), 'ACTIVE', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(2, N'Lý Thị Hương', '0908888888', 'huong.lt@coffee.vn', N'Cleaner', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), 'ACTIVE', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(1, N'Phan Văn Đức', '0909999999', 'duc.pv@coffee.vn', N'Security', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), 'ACTIVE', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(3, N'Bùi Thị Ngọc', '0901010101', 'ngoc.bt@coffee.vn', N'Trainee', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), 'ACTIVE', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Additional Users (10 records)
INSERT INTO [dbo].[users] ([employee_id], [date_of_birth], [is_active], [role_id], [phone_number], [fullname], [address], [password], [created_at], [last_modified]) VALUES
(11, '1992-03-15', 1, 8, '0901111111', N'Nguyễn Văn Hùng', N'123 Lê Lợi, Quận 1, TP.HCM', 'hashed_password_11', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(12, '1995-07-20', 1, 7, '0902222222', N'Trần Thị Linh', N'456 Nguyễn Huệ, Quận 1, TP.HCM', 'hashed_password_12', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(13, '1988-11-10', 1, 9, '0903333333', N'Lê Văn Tài', N'789 Đồng Khởi, Quận 1, TP.HCM', 'hashed_password_13', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(14, '1997-01-25', 1, 10, '0904444444', N'Phạm Thị Hoa', N'321 Hai Bà Trưng, Quận 3, TP.HCM', 'hashed_password_14', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(15, '1990-09-05', 1, 6, '0905555555', N'Hoàng Văn Minh', N'654 Lý Tự Trọng, Quận 1, TP.HCM', 'hashed_password_15', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(16, '1993-12-30', 1, 8, '0906666666', N'Vũ Thị Lan', N'987 Pasteur, Quận 1, TP.HCM', 'hashed_password_16', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(17, '1991-06-18', 1, 13, '0907777777', N'Đặng Văn Phong', N'147 Nguyễn Thị Minh Khai, Quận 3, TP.HCM', 'hashed_password_17', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(18, '1996-04-12', 1, 11, '0908888888', N'Lý Thị Hương', N'258 Võ Thị Sáu, Quận 3, TP.HCM', 'hashed_password_18', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(19, '1989-08-22', 1, 12, '0909999999', N'Phan Văn Đức', N'369 Cách Mạng Tháng 8, Quận 10, TP.HCM', 'hashed_password_19', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(20, '1998-02-14', 1, 17, '0901010101', N'Bùi Thị Ngọc', N'741 Trần Hưng Đạo, Quận 5, TP.HCM', 'hashed_password_20', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Additional Ingredients (10 records)
INSERT INTO [dbo].[ingredients] ([category_id], [name], [unit], [is_active], [description], [tax_id], [created_at], [last_modified]) VALUES
(8, N'Cà chua bi', 'kg', 1, N'Cà chua bi tươi', 1, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(9, N'Thịt bò xay', 'kg', 1, N'Thịt bò xay tươi', 2, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(10, N'Nước mắm', 'chai', 1, N'Nước mắm Phú Quốc', 1, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(11, N'Bánh quy digestive', 'hộp', 1, N'Bánh quy tiêu hóa', 2, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(12, N'Dầu oliu extra virgin', 'chai', 1, N'Dầu oliu nguyên chất', 1, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(13, N'Hạnh nhân rang', 'kg', 1, N'Hạnh nhân rang muối', 2, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(14, N'Rau xà lách', 'kg', 1, N'Xà lách tươi hữu cơ', 1, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(15, N'Tiêu đen hạt', 'kg', 1, N'Tiêu đen nguyên hạt', 2, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(16, N'Chocolate chip', 'kg', 1, N'Chocolate chip làm bánh', 1, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(17, N'Nước ngọt coca', 'thùng', 1, N'Nước ngọt có gas', 2, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Additional Products (10 records)
INSERT INTO [dbo].[products] ([price], [category_id], [is_active], [tax_id], [description], [name], [thumbnail], [created_at], [last_modified]) VALUES
(65000.00, 8, 1, 1, N'Bánh mì thập cẩm đầy đủ', N'Bánh mì thập cẩm', 'banh_mi_thap_cam.jpg', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(85000.00, 9, 1, 1, N'Salad cà chua bi tươi mát', N'Salad cà chua bi', 'salad_ca_chua.jpg', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(95000.00, 10, 1, 2, N'Súp bò hầm nóng hổi', N'Súp bò hầm', 'sup_bo_ham.jpg', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(75000.00, 11, 1, 1, N'Sandwich thịt bò nướng', N'Sandwich bò nướng', 'sandwich_bo.jpg', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(120000.00, 12, 1, 2, N'Pizza mini size nhỏ', N'Pizza mini margherita', 'pizza_mini.jpg', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(110000.00, 13, 1, 1, N'Mì Ý sốt kem', N'Pasta carbonara', 'pasta_carbonara.jpg', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(55000.00, 14, 1, 1, N'Cơm hộp tiện lợi', N'Cơm hộp gà teriyaki', 'com_hop_ga.jpg', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(45000.00, 15, 1, 2, N'Bánh pudding tráng miệng', N'Pudding chocolate', 'pudding_choco.jpg', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(35000.00, 16, 1, 1, N'Snack hạnh nhân rang', N'Hạnh nhân rang muối', 'hanh_nhan_rang.jpg', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(25000.00, 17, 1, 2, N'Nước ngọt có gas', N'Coca Cola', 'coca_cola.jpg', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Additional Orders (10 records)
INSERT INTO [dbo].[orders] ([order_uuid], [order_code], [customer_id], [branch_id], [total_money], [status_id], [notes], [created_at], [last_modified]) VALUES
(NEWID(), 'ORDER011', 8, 4, 285000.00, 3, N'Giao hàng buổi sáng', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER012', 9, 5, 195000.00, 2, N'Giao hàng buổi chiều', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER013', 10, 6, 340000.00, 1, N'Khách hàng VIP', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER014', 6, 7, 125000.00, 4, N'Giao hàng tối', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER015', 7, 8, 275000.00, 3, N'Liên hệ trước khi giao', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER016', 8, 9, 165000.00, 2, N'Giao hàng nhanh', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER017', 9, 10, 390000.00, 1, N'Đơn hàng lớn', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER018', 10, 11, 215000.00, 4, N'Giao hàng cuối tuần', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER019', 6, 12, 155000.00, 3, N'Khách hàng thân thiết', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER020', 7, 13, 425000.00, 2, N'Đơn hàng combo', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Additional Branch Expenses (10 records)
INSERT INTO [dbo].[branch_expenses] ([branch_id], [expense_type], [amount], [start_date], [end_date], [note], [created_at], [last_modified]) VALUES
(4, N'Tiền điện', 2500000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), N'Tiền điện tháng 10', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(5, N'Tiền nước', 800000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), N'Tiền nước tháng 10', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(6, N'Tiền thuê mặt bằng', 15000000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), N'Tiền thuê tháng 10', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(7, N'Chi phí bảo trì', 3200000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), N'Sửa chữa thiết bị', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(8, N'Chi phí marketing', 5000000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), N'Quảng cáo trực tuyến', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(9, N'Chi phí vệ sinh', 1500000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), N'Dịch vụ vệ sinh hàng tuần', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(10, N'Chi phí internet', 1200000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), N'Cước internet tháng 10', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(11, N'Chi phí bảo hiểm', 2800000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), N'Bảo hiểm cháy nổ', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(12, N'Chi phí văn phòng phẩm', 900000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), N'Mua sắm văn phòng phẩm', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(13, N'Chi phí đào tạo', 4500000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), N'Đào tạo nhân viên mới', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

PRINT N'Additional data (10 records per table) inserted successfully!';
GO

-- Additional Order Details (10 records)
INSERT INTO [dbo].[order_details] ([order_id], [product_id], [quantity], [unit_price], [total_amount], [color], [note], [created_at], [last_modified]) VALUES
(11, 11, 2, 65000.00, 130000.00, N'Đỏ', N'Size L', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(11, 12, 1, 85000.00, 85000.00, N'Xanh', N'Size M', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(12, 13, 1, 95000.00, 95000.00, N'Trắng', N'Size XL', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(12, 14, 1, 75000.00, 75000.00, N'Đen', N'Size S', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(13, 15, 2, 120000.00, 240000.00, N'Vàng', N'Size L', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(13, 16, 1, 110000.00, 110000.00, N'Tím', N'Size M', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(14, 17, 2, 55000.00, 110000.00, N'Hồng', N'Size XL', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(15, 18, 3, 45000.00, 135000.00, N'Cam', N'Size L', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(15, 19, 4, 35000.00, 140000.00, N'Xám', N'Size M', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(16, 20, 6, 25000.00, 150000.00, N'Nâu', N'Size S', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Additional Ingredient Purchase Orders (10 records)
INSERT INTO [dbo].[ingredient_purchase_orders] ([purchase_order_code], [supplier_id], [branch_id], [employee_id], [order_date], [expected_delivery_date], [status_id], [total_amount_before_tax], [total_tax_amount], [total_amount_after_tax], [discount_amount], [final_amount], [note], [created_at], [last_modified]) VALUES
('IPO011', 8, 4, 11, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, 7, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), 1, 13636363.64, 1363636.36, 15000000.00, 0, 15000000.00, N'Đặt hàng nguyên liệu cho quý 4', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('IPO012', 9, 5, 12, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, 5, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), 2, 7727272.73, 772727.27, 8500000.00, 0, 8500000.00, N'Đặt sữa và sản phẩm từ sữa', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('IPO013', 10, 6, 13, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, 3, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), 3, 10909090.91, 1090909.09, 12000000.00, 0, 12000000.00, N'Trái cây tươi cho tháng 11', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('IPO014', 11, 7, 14, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, 10, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), 1, 6181818.18, 618181.82, 6800000.00, 0, 6800000.00, N'Đường và gia vị', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('IPO015', 12, 8, 15, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, 7, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), 2, 8363636.36, 836363.64, 9200000.00, 0, 9200000.00, N'Trà xanh và thảo mộc', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('IPO016', 13, 9, 16, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, 5, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), 3, 13181818.18, 1318181.82, 14500000.00, 0, 14500000.00, N'Bánh mì và sản phẩm làm bánh', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('IPO017', 14, 10, 17, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, 12, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), 1, 6636363.64, 663636.36, 7300000.00, 0, 7300000.00, N'Gia vị và thảo mộc đặc biệt', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('IPO018', 15, 11, 18, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, 2, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), 2, 10727272.73, 1072727.27, 11800000.00, 0, 11800000.00, N'Thịt tươi và hải sản', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('IPO019', 16, 12, 19, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, 1, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), 3, 8090909.09, 809090.91, 8900000.00, 0, 8900000.00, N'Rau xanh hữu cơ', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('IPO020', 17, 13, 20, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, 14, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), 1, 5090909.09, 509090.91, 5600000.00, 0, 5600000.00, N'Bao bì và vật liệu đóng gói', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Additional Ingredient Purchase Order Details (10 records)
INSERT INTO [dbo].[ingredient_purchase_order_details] ([purchase_order_id], [ingredient_id], [quantity], [unit_price], [tax_price], [total_price], [created_at], [last_modified]) VALUES
(1, 1, 50.00, 120000.00, 12000.00, 6000000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(1, 2, 30.00, 150000.00, 15000.00, 4500000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(2, 3, 100.00, 25000.00, 2500.00, 2500000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(3, 4, 200.00, 15000.00, 1500.00, 3000000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(4, 5, 80.00, 50000.00, 5000.00, 4000000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(5, 6, 60.00, 100000.00, 10000.00, 6000000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(2, 7, 150.00, 80000.00, 8000.00, 12000000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(3, 1, 40.00, 120000.00, 12000.00, 4800000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(4, 2, 90.00, 150000.00, 15000.00, 13500000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(5, 3, 70.00, 25000.00, 2500.00, 1750000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Additional Employee Salaries (5 records)
INSERT INTO [dbo].[employee_salaries] ([employee_id], [base_salary], [salary_type], [allowance], [bonus], [penalty], [tax_rate], [effective_date], [created_at], [last_modified]) VALUES
(6, 8000000.00, 'MONTHLY', 1500000.00, 2000000.00, 500000.00, 0.10, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(7, 6500000.00, 'MONTHLY', 1200000.00, 1500000.00, 400000.00, 0.08, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(1, 12000000.00, 'MONTHLY', 2000000.00, 3000000.00, 800000.00, 0.15, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(2, 5500000.00, 'MONTHLY', 1000000.00, 1000000.00, 300000.00, 0.05, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(3, 10000000.00, 'MONTHLY', 1800000.00, 2500000.00, 700000.00, 0.12, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Additional Employee Shifts (10 records)
INSERT INTO [dbo].[employee_shifts] ([employee_id], [shift_date], [start_time], [end_time], [status], [created_at], [last_modified]) VALUES
(1, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), '06:00:00', '14:00:00', 'PRESENT', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(2, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), '14:00:00', '22:00:00', 'PRESENT', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(3, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), '08:00:00', '16:00:00', 'PRESENT', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(4, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), '16:00:00', '00:00:00', 'PRESENT', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(5, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), '07:00:00', '15:00:00', 'PRESENT', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(6, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), '09:00:00', '17:00:00', 'PRESENT', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(7, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), '10:00:00', '18:00:00', 'PRESENT', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(1, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), '05:00:00', '13:00:00', 'PRESENT', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(2, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), '22:00:00', '06:00:00', 'PRESENT', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(3, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), '12:00:00', '20:00:00', 'PRESENT', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Additional Ingredient Transfer Requests (10 records)
INSERT INTO [dbo].[ingredient_transfer_requests] ([request_number], [branch_id], [request_date], [required_date], [total_items], [requested_by], [approved_by], [status], [approved_date], [note], [created_at], [last_modified]) VALUES
('ITR011', 1, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, 7, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), 3, N'Nguyễn Văn A', N'Manager A', 'APPROVED', DATEADD(day, 1, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), N'Yêu cầu bổ sung nguyên liệu', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('ITR012', 2, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, 7, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), 2, N'Trần Thị B', NULL, 'PENDING', NULL, N'Cần thêm nguyên liệu cho cuối tuần', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('ITR013', 3, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, 7, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), 4, N'Lê Văn C', N'Manager B', 'APPROVED', DATEADD(day, 1, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), N'Bổ sung cho menu mới', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('ITR014', 1, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, 7, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), 1, N'Phạm Thị D', NULL, 'REJECTED', DATEADD(day, 1, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), N'Số lượng không hợp lý', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
('ITR015', 2, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, 7, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), 2, N'Hoàng Văn E', N'Manager C', 'APPROVED', DATEADD(day, 1, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())), N'Yêu cầu khẩn cấp', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Additional Ingredient Transfer Request Details (5 records)
INSERT INTO [dbo].[ingredient_transfer_request_details] ([transfer_request_id], [ingredient_id], [requested_quantity], [approved_quantity], [transferred_quantity], [status], [note], [created_at], [last_modified]) VALUES
(1, 1, 25.00, 25.00, 25.00, 'COMPLETED', N'Hoàn thành chuyển giao', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(2, 2, 15.00, 0.00, 0.00, 'PENDING', N'Đang chờ phê duyệt', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(3, 3, 30.00, 30.00, 30.00, 'COMPLETED', N'Đã chuyển giao đầy đủ', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(4, 4, 50.00, 0.00, 0.00, 'REJECTED', N'Bị từ chối do thiếu hàng', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(5, 5, 20.00, 20.00, 15.00, 'PARTIALLY_COMPLETED', N'Chuyển giao một phần', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

PRINT N'Additional data for remaining tables (10 records each) inserted successfully!';
GO

-- ====================================================================
-- BULK ORDERS DATA - 50 ORDERS WITH DETAILS AND RELATED RECORDS
-- ====================================================================

-- Additional 50 Orders (ORDER021 to ORDER070)
INSERT INTO [dbo].[orders] ([order_uuid], [order_code], [branch_id], [customer_id], [status_id], [total_money], [notes], [created_at], [last_modified]) VALUES
(NEWID(), 'ORDER021', 1, 6, 4, 485000.00, N'Đơn hàng combo gia đình', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER022', 2, 7, 3, 235000.00, N'Giao hàng buổi sáng', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER023', 3, 8, 2, 325000.00, N'Khách hàng VIP', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER024', 1, 9, 1, 155000.00, N'Đơn hàng nhanh', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER025', 2, 10, 4, 695000.00, N'Đơn hàng lớn cho văn phòng', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER026', 3, 6, 3, 275000.00, N'Giao hàng cuối tuần', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER027', 1, 7, 2, 385000.00, N'Đặt tiệc sinh nhật', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER028', 2, 8, 1, 125000.00, N'Đơn hàng đơn giản', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER029', 3, 9, 4, 555000.00, N'Đơn hàng cho sự kiện', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER030', 1, 10, 3, 215000.00, N'Giao hàng tận nơi', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER031', 2, 6, 2, 445000.00, N'Đơn hàng thường xuyên', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER032', 3, 7, 1, 165000.00, N'Giao hàng nhanh', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER033', 1, 8, 4, 775000.00, N'Đơn hàng siêu lớn', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER034', 2, 9, 3, 295000.00, N'Khách hàng thân thiết', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER035', 3, 10, 2, 355000.00, N'Đơn hàng combo', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER036', 1, 6, 1, 85000.00, N'Đơn hàng nhỏ', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER037', 2, 7, 4, 625000.00, N'Đơn hàng cao cấp', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER038', 3, 8, 3, 185000.00, N'Giao hàng xa', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER039', 1, 9, 2, 435000.00, N'Đơn hàng khu xa', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER040', 2, 10, 1, 95000.00, N'Đơn hàng đơn giản', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER041', 3, 6, 4, 565000.00, N'Đơn hàng đặc biệt', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER042', 1, 7, 3, 245000.00, N'Giao hàng bình thường', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER043', 2, 8, 2, 375000.00, N'Đơn hàng trung bình', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER044', 3, 9, 1, 135000.00, N'Đơn hàng nhanh', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER045', 1, 10, 4, 815000.00, N'Đơn hàng siêu lớn', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER046', 2, 6, 3, 285000.00, N'Khách hàng quen', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER047', 3, 7, 2, 195000.00, N'Đơn hàng bình thường', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER048', 1, 8, 1, 75000.00, N'Đơn hàng nhỏ', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER049', 2, 9, 4, 665000.00, N'Đơn hàng lớn', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER050', 3, 10, 3, 325000.00, N'Đơn hàng thường', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER051', 1, 6, 2, 455000.00, N'Đơn hàng combo lớn', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER052', 2, 7, 1, 115000.00, N'Đơn hàng đơn giản', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER053', 3, 8, 4, 725000.00, N'Đơn hàng cao cấp lớn', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER054', 1, 9, 3, 255000.00, N'Giao hàng thường', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER055', 2, 10, 2, 395000.00, N'Đơn hàng trung bình', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER056', 3, 6, 1, 85000.00, N'Đơn hàng nhanh', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER057', 1, 7, 4, 885000.00, N'Đơn hàng siêu lớn', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER058', 2, 8, 3, 175000.00, N'Giao hàng xa', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER059', 3, 9, 2, 515000.00, N'Đơn hàng lớn', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER060', 1, 10, 1, 125000.00, N'Đơn hàng thường', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER061', 2, 6, 4, 635000.00, N'Đơn hàng cao cấp', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER062', 3, 7, 3, 285000.00, N'Giao hàng bình thường', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER063', 1, 8, 2, 425000.00, N'Đơn hàng combo', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER064', 2, 9, 1, 95000.00, N'Đơn hàng đơn giản', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER065', 3, 10, 4, 765000.00, N'Đơn hàng siêu lớn', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER066', 1, 6, 3, 215000.00, N'Khách hàng quen', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER067', 2, 7, 2, 345000.00, N'Đơn hàng trung bình', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER068', 3, 8, 1, 155000.00, N'Đơn hàng nhanh', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER069', 1, 9, 4, 595000.00, N'Đơn hàng cao cấp', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(NEWID(), 'ORDER070', 2, 10, 3, 275000.00, N'Đơn hàng cuối', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

PRINT N'50 Additional Orders (ORDER021-ORDER070) inserted successfully!';
GO

-- Order Details for the 50 additional orders (2-4 items per order)
INSERT INTO [dbo].[order_details] ([order_id], [product_id], [unit_price], [quantity], [total_amount], [created_at], [last_modified]) VALUES
-- ORDER021 (485k total)
(21, 1, 45000.00, 3, 135000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(21, 3, 85000.00, 2, 170000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(21, 7, 90000.00, 2, 180000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER022 (235k total)
(22, 2, 55000.00, 2, 110000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(22, 4, 125000.00, 1, 125000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER023 (325k total)
(23, 5, 75000.00, 1, 75000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(23, 6, 65000.00, 2, 130000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(23, 8, 120000.00, 1, 120000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER024 (155k total)
(24, 1, 45000.00, 2, 90000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(24, 2, 65000.00, 1, 65000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER025 (695k total)
(25, 4, 125000.00, 3, 375000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(25, 7, 90000.00, 2, 180000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(25, 8, 120000.00, 1, 120000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(25, 11, 65000.00, 1, 65000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER026 (275k total)
(26, 3, 85000.00, 2, 170000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(26, 5, 75000.00, 1, 75000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(26, 18, 45000.00, 1, 30000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER027 (385k total)
(27, 6, 65000.00, 3, 195000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(27, 12, 85000.00, 1, 85000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(27, 16, 110000.00, 1, 105000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER028 (125k total)
(28, 1, 45000.00, 1, 45000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(28, 2, 55000.00, 1, 55000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(28, 20, 25000.00, 1, 25000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER029 (555k total)
(29, 4, 125000.00, 2, 250000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(29, 8, 120000.00, 2, 240000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(29, 11, 65000.00, 1, 65000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER030 (215k total)
(30, 3, 85000.00, 1, 85000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(30, 6, 65000.00, 2, 130000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER031 (445k total)
(31, 7, 90000.00, 3, 270000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(31, 13, 95000.00, 1, 95000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(31, 19, 35000.00, 2, 70000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER032 (165k total)
(32, 2, 55000.00, 2, 110000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(32, 17, 55000.00, 1, 55000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER033 (775k total)
(33, 4, 125000.00, 4, 500000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(33, 8, 120000.00, 2, 240000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(33, 19, 35000.00, 1, 35000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER034 (295k total)
(34, 5, 75000.00, 2, 150000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(34, 12, 85000.00, 1, 85000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(34, 14, 75000.00, 1, 60000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER035 (355k total)
(35, 6, 65000.00, 3, 195000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(35, 15, 120000.00, 1, 120000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(35, 18, 45000.00, 1, 40000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER036 (85k total)
(36, 1, 45000.00, 1, 45000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(36, 20, 25000.00, 1, 25000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER037 (625k total)
(37, 4, 125000.00, 3, 375000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(37, 7, 90000.00, 2, 180000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(37, 16, 110000.00, 1, 70000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER038 (185k total)
(38, 3, 85000.00, 1, 85000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(38, 5, 75000.00, 1, 75000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(38, 20, 25000.00, 1, 25000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER039 (435k total)
(39, 8, 120000.00, 2, 240000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(39, 13, 95000.00, 1, 95000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(39, 6, 65000.00, 2, 100000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER040 (95k total)
(40, 2, 55000.00, 1, 55000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(40, 1, 45000.00, 1, 40000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER041 (565k total)
(41, 4, 125000.00, 3, 375000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(41, 15, 120000.00, 1, 120000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(41, 16, 110000.00, 1, 70000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER042 (245k total)
(42, 7, 90000.00, 2, 180000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(42, 11, 65000.00, 1, 65000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER043 (375k total)
(43, 8, 120000.00, 2, 240000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(43, 12, 85000.00, 1, 85000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(43, 19, 35000.00, 1, 50000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER044 (135k total)
(44, 3, 85000.00, 1, 85000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(44, 2, 55000.00, 1, 50000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER045 (815k total)
(45, 4, 125000.00, 4, 500000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(45, 8, 120000.00, 2, 240000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(45, 14, 75000.00, 1, 75000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER046 (285k total)
(46, 6, 65000.00, 3, 195000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(46, 13, 95000.00, 1, 90000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER047 (195k total)
(47, 5, 75000.00, 2, 150000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(47, 18, 45000.00, 1, 45000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER048 (75k total)
(48, 1, 45000.00, 1, 45000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(48, 20, 25000.00, 1, 30000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER049 (665k total)
(49, 4, 125000.00, 3, 375000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(49, 8, 120000.00, 2, 240000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(49, 19, 35000.00, 1, 50000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER050 (325k total)
(50, 7, 90000.00, 2, 180000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(50, 12, 85000.00, 1, 85000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(50, 14, 75000.00, 1, 60000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- Continue with ORDER051-070
-- ORDER051 (455k total)
(51, 8, 120000.00, 3, 360000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(51, 13, 95000.00, 1, 95000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER052 (115k total)
(52, 2, 55000.00, 1, 55000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(52, 17, 55000.00, 1, 60000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER053 (725k total)
(53, 4, 125000.00, 4, 500000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(53, 15, 120000.00, 1, 120000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(53, 16, 110000.00, 1, 105000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER054 (255k total)
(54, 6, 65000.00, 2, 130000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(54, 7, 90000.00, 1, 90000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(54, 19, 35000.00, 1, 35000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER055 (395k total)
(55, 8, 120000.00, 2, 240000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(55, 11, 65000.00, 1, 65000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(55, 13, 95000.00, 1, 90000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER056 (85k total)
(56, 1, 45000.00, 1, 45000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(56, 18, 45000.00, 1, 40000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER057 (885k total)
(57, 4, 125000.00, 5, 625000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(57, 8, 120000.00, 2, 240000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(57, 20, 25000.00, 1, 20000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER058 (175k total)
(58, 3, 85000.00, 1, 85000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(58, 5, 75000.00, 1, 75000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER059 (515k total)
(59, 7, 90000.00, 3, 270000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(59, 15, 120000.00, 2, 240000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER060 (125k total)
(60, 2, 55000.00, 1, 55000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(60, 6, 65000.00, 1, 70000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER061 (635k total)
(61, 4, 125000.00, 3, 375000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(61, 8, 120000.00, 2, 240000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(61, 20, 25000.00, 1, 20000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER062 (285k total)
(62, 7, 90000.00, 2, 180000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(62, 12, 85000.00, 1, 85000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(62, 20, 25000.00, 1, 20000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER063 (425k total)
(63, 8, 120000.00, 3, 360000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(63, 11, 65000.00, 1, 65000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER064 (95k total)
(64, 2, 55000.00, 1, 55000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(64, 18, 45000.00, 1, 40000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER065 (765k total)
(65, 4, 125000.00, 4, 500000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(65, 15, 120000.00, 2, 240000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(65, 20, 25000.00, 1, 25000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER066 (215k total)
(66, 6, 65000.00, 2, 130000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(66, 12, 85000.00, 1, 85000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER067 (345k total)
(67, 7, 90000.00, 2, 180000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(67, 13, 95000.00, 1, 95000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(67, 16, 110000.00, 1, 70000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER068 (155k total)
(68, 3, 85000.00, 1, 85000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(68, 5, 75000.00, 1, 70000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER069 (595k total)
(69, 4, 125000.00, 3, 375000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(69, 8, 120000.00, 1, 120000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(69, 15, 120000.00, 1, 100000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),

-- ORDER070 (275k total)
(70, 6, 65000.00, 2, 130000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(70, 7, 90000.00, 1, 90000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(70, 17, 55000.00, 1, 55000.00, DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

PRINT N'Order Details for 50 additional orders inserted successfully!';
GO

-- Order Payments for the 50 additional orders
INSERT INTO [dbo].[order_payments] ([order_id], [payment_method_id], [payment_status_id], [amount], [transaction_id], [payment_date], [created_at], [last_modified]) VALUES
(21, 1, 1, 485000.00, 'TXN021_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(22, 2, 1, 235000.00, 'TXN022_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(23, 1, 1, 325000.00, 'TXN023_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(24, 3, 1, 155000.00, 'TXN024_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(25, 1, 1, 695000.00, 'TXN025_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(26, 2, 1, 275000.00, 'TXN026_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(27, 1, 1, 385000.00, 'TXN027_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(28, 3, 1, 125000.00, 'TXN028_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(29, 1, 1, 555000.00, 'TXN029_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(30, 2, 1, 215000.00, 'TXN030_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(31, 1, 1, 445000.00, 'TXN031_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(32, 3, 1, 165000.00, 'TXN032_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(33, 1, 1, 775000.00, 'TXN033_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(34, 2, 1, 295000.00, 'TXN034_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(35, 1, 1, 355000.00, 'TXN035_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(36, 3, 1, 85000.00, 'TXN036_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(37, 1, 1, 625000.00, 'TXN037_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(38, 2, 1, 185000.00, 'TXN038_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(39, 1, 1, 435000.00, 'TXN039_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(40, 3, 1, 95000.00, 'TXN040_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(41, 1, 1, 565000.00, 'TXN041_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(42, 2, 1, 245000.00, 'TXN042_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(43, 1, 1, 375000.00, 'TXN043_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(44, 3, 1, 135000.00, 'TXN044_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(45, 1, 1, 815000.00, 'TXN045_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(46, 2, 1, 285000.00, 'TXN046_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(47, 1, 1, 195000.00, 'TXN047_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(48, 3, 1, 75000.00, 'TXN048_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(49, 1, 1, 665000.00, 'TXN049_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(50, 2, 1, 325000.00, 'TXN050_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(51, 1, 1, 455000.00, 'TXN051_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(52, 3, 1, 115000.00, 'TXN052_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(53, 1, 1, 725000.00, 'TXN053_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(54, 2, 1, 255000.00, 'TXN054_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(55, 1, 1, 395000.00, 'TXN055_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(56, 3, 1, 85000.00, 'TXN056_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(57, 1, 1, 885000.00, 'TXN057_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(58, 2, 1, 175000.00, 'TXN058_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(59, 1, 1, 515000.00, 'TXN059_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(60, 3, 1, 125000.00, 'TXN060_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(61, 1, 1, 635000.00, 'TXN061_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(62, 2, 1, 285000.00, 'TXN062_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(63, 1, 1, 425000.00, 'TXN063_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(64, 3, 1, 95000.00, 'TXN064_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(65, 1, 1, 765000.00, 'TXN065_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(66, 2, 1, 215000.00, 'TXN066_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(67, 1, 1, 345000.00, 'TXN067_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(68, 3, 1, 155000.00, 'TXN068_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(69, 1, 1, 595000.00, 'TXN069_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(70, 2, 1, 275000.00, 'TXN070_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

-- Order Shipments for orders that have shipping (status 2, 3, 4)


INSERT INTO [dbo].[order_shipments] ([order_id], [shipping_provider_id], [shipping_address], [shipping_cost], [shipping_method], [estimated_delivery_date], [notes], [created_at], [last_modified]) VALUES
(21, 1, N'123 Nguyễn Huệ, Quận 1, TP.HCM', 30000.00, N'Giao hàng tiêu chuẩn', DATEADD(day, 2, GETDATE()), N'Giao hàng trong giờ hành chính', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(22, 2, N'456 Lê Lợi, Quận 1, TP.HCM', 20000.00, N'Giao hàng nhanh', DATEADD(day, 1, GETDATE()), N'Giao hàng nhanh', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(23, 1, N'789 Hai Bà Trưng, Quận 3, TP.HCM', 25000.00, N'Giao hàng tiêu chuẩn', DATEADD(day, 1, GETDATE()), N'Liên hệ trước khi giao', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(25, 2, N'321 Võ Văn Tần, Quận 3, TP.HCM', 40000.00, N'Giao hàng nhanh', DATEADD(day, 2, GETDATE()), N'Giao vào cuối tuần', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(26, 1, N'654 Trần Hưng Đạo, Quận 5, TP.HCM', 20000.00, N'Giao hàng tiêu chuẩn', DATEADD(day, 1, GETDATE()), N'Giao hàng trong ngày', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(27, 2, N'987 Nguyễn Thị Minh Khai, Quận 1, TP.HCM', 30000.00, N'Giao hàng nhanh', DATEADD(day, 1, GETDATE()), N'Khách hàng VIP', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(29, 1, N'147 Pasteur, Quận 1, TP.HCM', 35000.00, N'Giao hàng tiêu chuẩn', DATEADD(day, 2, GETDATE()), N'Giao tại văn phòng', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(30, 2, N'258 Cách Mạng Tháng 8, Quận 10, TP.HCM', 20000.00, N'Giao hàng nhanh', DATEADD(day, 1, GETDATE()), N'Giao hàng buổi sáng', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(31, 1, N'369 Điện Biên Phủ, Quận Bình Thạnh, TP.HCM', 30000.00, N'Giao hàng tiêu chuẩn', DATEADD(day, 1, GETDATE()), N'Để tại bảo vệ', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(33, 2, N'741 Xô Viết Nghệ Tĩnh, Quận Bình Thạnh, TP.HCM', 50000.00, N'Giao hàng nhanh', DATEADD(day, 3, GETDATE()), N'Đơn hàng lớn', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(34, 1, N'852 Hoàng Văn Thụ, Quận Tân Bình, TP.HCM', 25000.00, N'Giao hàng tiêu chuẩn', DATEADD(day, 1, GETDATE()), N'Giao hàng bình thường', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(35, 2, N'963 Cộng Hòa, Quận Tân Bình, TP.HCM', 25000.00, N'Giao hàng nhanh', DATEADD(day, 1, GETDATE()), N'Giao nhanh trong ngày', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(37, 1, N'159 Nam Kỳ Khởi Nghĩa, Quận 3, TP.HCM', 40000.00, N'Giao hàng tiêu chuẩn', DATEADD(day, 2, GETDATE()), N'Giao hàng cẩn thận', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(38, 2, N'753 Đinh Tiên Hoàng, Quận 1, TP.HCM', 15000.00, N'Giao hàng nhanh', DATEADD(day, 1, GETDATE()), N'Giao hàng tiết kiệm', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE())),
(39, 1, N'864 Lý Tự Trọng, Quận 1, TP.HCM', 30000.00, N'Giao hàng tiêu chuẩn', DATEADD(day, 1, GETDATE()), N'Giao hàng thường', DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()), DATEADD(day, -CAST(RAND(CHECKSUM(NEWID()))*365 AS INT), GETDATE()));
GO

PRINT N'Order Payments and Shipments for 50 additional orders inserted successfully!';
GO

-- ================================================================
-- ENHANCED INGREDIENTS DATA (30 items) - Coffee Shop Comprehensive Inventory
-- ================================================================

PRINT N'Inserting enhanced ingredients data...';
GO

-- Note: Ingredients require category_id. Using category_id = 1 for all items.
-- In production, create specific categories for: Coffee, Syrups, Dairy, Sweeteners, Spices, Fruits

INSERT INTO [dbo].[ingredients] ([category_id], [name], [unit], [is_active], [description], [created_at], [last_modified]) VALUES 
-- Coffee Base Ingredients (5 items)
(1, N'Cà phê Arabica nguyên chất', N'kg', 1, N'Hạt cà phê Arabica cao cấp từ Đà Lạt, rang vừa', GETDATE(), GETDATE()),
(1, N'Cà phê Robusta Buôn Ma Thuột', N'kg', 1, N'Hạt cà phê Robusta đặc trưng vùng Tây Nguyên', GETDATE(), GETDATE()),
(1, N'Espresso blend premium', N'kg', 1, N'Hỗn hợp cà phê rang đậm dành cho pha chế espresso', GETDATE(), GETDATE()),
(1, N'Cold brew coffee', N'kg', 1, N'Cà phê rang nhẹ phù hợp pha chế cold brew', GETDATE(), GETDATE()),
(1, N'Decaf coffee', N'kg', 1, N'Cà phê không caffeine cho khách hàng đặc biệt', GETDATE(), GETDATE()),

-- Flavor Syrups (8 items)
(1, N'Syrup vani nguyên chất', N'L', 1, N'Syrup vani tự nhiên không chất bảo quản', GETDATE(), GETDATE()),
(1, N'Syrup caramel', N'L', 1, N'Syrup caramel thơm ngọt cho đồ uống', GETDATE(), GETDATE()),
(1, N'Syrup hazelnut', N'L', 1, N'Syrup hạt phỉ thơm béo', GETDATE(), GETDATE()),
(1, N'Syrup chocolate', N'L', 1, N'Syrup socola đậm đà', GETDATE(), GETDATE()),
(1, N'Syrup dâu tây', N'L', 1, N'Syrup dâu tây tự nhiên', GETDATE(), GETDATE()),
(1, N'Syrup đào', N'L', 1, N'Syrup đào ngọt mát', GETDATE(), GETDATE()),
(1, N'Syrup việt quất', N'L', 1, N'Syrup việt quất nhập khẩu', GETDATE(), GETDATE()),
(1, N'Syrup mật ong', N'L', 1, N'Mật ong nguyên chất pha chế', GETDATE(), GETDATE()),

-- Dairy & Milk Alternatives (6 items)
(1, N'Sữa tươi nguyên kem', N'L', 1, N'Sữa tươi nguyên kem Vinamilk cao cấp', GETDATE(), GETDATE()),
(1, N'Sữa yến mạch', N'L', 1, N'Sữa thực vật từ yến mạch cho người ăn chay', GETDATE(), GETDATE()),
(1, N'Sữa hạnh nhân', N'L', 1, N'Sữa hạnh nhân tự nhiên không đường', GETDATE(), GETDATE()),
(1, N'Kem whipping', N'L', 1, N'Kem tươi đánh bông cho topping', GETDATE(), GETDATE()),
(1, N'Sữa dừa', N'L', 1, N'Sữa dừa tươi cho đồ uống nhiệt đới', GETDATE(), GETDATE()),
(1, N'Cream cheese', N'kg', 1, N'Phô mai kem mềm cho bánh ngọt', GETDATE(), GETDATE()),

-- Sweeteners & Sugar (4 items)
(1, N'Đường trắng tinh luyện', N'kg', 1, N'Đường trắng tinh khiết pha chế', GETDATE(), GETDATE()),
(1, N'Đường nâu muscovado', N'kg', 1, N'Đường nâu tự nhiên có mùi thơm', GETDATE(), GETDATE()),
(1, N'Stevia tự nhiên', N'kg', 1, N'Chất tạo ngọt tự nhiên từ lá stevia', GETDATE(), GETDATE()),
(1, N'Đường phèn rock', N'kg', 1, N'Đường phèn trong cho trà sữa', GETDATE(), GETDATE()),

-- Spices & Seasonings (4 items)
(1, N'Bột quế Ceylon', N'kg', 1, N'Bột quế Ceylon nguyên chất', GETDATE(), GETDATE()),
(1, N'Bột ca cao nguyên chất', N'kg', 1, N'Bột ca cao không đường', GETDATE(), GETDATE()),
(1, N'Lá bạc hà tươi', N'kg', 1, N'Lá bạc hà tươi cho mojito và trà', GETDATE(), GETDATE()),
(1, N'Gừng tươi', N'kg', 1, N'Gừng tươi pha chế đồ uống ấm', GETDATE(), GETDATE()),

-- Fresh Fruits (3 items)
(1, N'Chanh tươi', N'kg', 1, N'Chanh tươi vắt nước cho đồ uống', GETDATE(), GETDATE()),
(1, N'Cam tươi', N'kg', 1, N'Cam tươi vắt nước ép', GETDATE(), GETDATE()),
(1, N'Dâu tây tươi', N'kg', 1, N'Dâu tây tươi làm topping và sinh tố', GETDATE(), GETDATE());
GO

PRINT N'Enhanced ingredients (30 items) inserted successfully!';
GO

-- ================================================================
-- RECIPE AND RECIPE_INGREDIENTS DATA - Product Recipe System
-- ================================================================

PRINT N'Inserting recipes and recipe-ingredient relationships...';
GO

-- First, create the main recipes
INSERT INTO [dbo].[recipes] ([name], [description], [product_id], [serving_size], [unit], [is_active], [notes], [created_at], [last_modified]) VALUES
(N'Công thức Espresso', N'Công thức espresso hai shot cổ điển', 1, 250.0, N'ml', 1, N'Chuẩn 18g cà phê, chiết xuất 36ml trong 25-30 giây', GETDATE(), GETDATE()),
(N'Công thức Americano', N'Espresso pha với nước nóng', 2, 300.0, N'ml', 1, N'Một shot espresso pha thêm nước nóng', GETDATE(), GETDATE()),
(N'Công thức Cappuccino', N'Cappuccino cổ điển với sữa đánh bọt', 3, 200.0, N'ml', 1, N'Tỷ lệ đều espresso, sữa nóng và bọt sữa', GETDATE(), GETDATE()),
(N'Công thức Latte', N'Latte mềm mại với sữa đánh bọt', 4, 350.0, N'ml', 1, N'Espresso với sữa nóng và ít bọt sữa', GETDATE(), GETDATE()),
(N'Công thức Mocha', N'Kết hợp cà phê và socola', 5, 300.0, N'ml', 1, N'Espresso với socola và sữa nóng', GETDATE(), GETDATE()),
(N'Công thức Macchiato', N'Kiểu macchiato caramel', 6, 250.0, N'ml', 1, N'Espresso đánh dấu với caramel và bọt sữa', GETDATE(), GETDATE()),
(N'Công thức Cold Brew', N'Cà phê cold brew mềm mại', 7, 400.0, N'ml', 1, N'Cà phê chiết xuất lạnh với sữa thực vật', GETDATE(), GETDATE()),
(N'Công thức Frappuccino', N'Đồ uống cà phê đá xay', 8, 450.0, N'ml', 1, N'Cà phê xay với sữa và đá', GETDATE(), GETDATE()),
(N'Công thức Matcha Latte', N'Matcha Nhật Bản với sữa yến mạch', 9, 350.0, N'ml', 1, N'Matcha nghi lễ với sữa thực vật', GETDATE(), GETDATE()),
(N'Công thức Trà trái cây', N'Trà vị trái cây hỗn hợp', 10, 500.0, N'ml', 1, N'Pha trộn trà trái cây tươi mát với trang trí tươi', GETDATE(), GETDATE());
GO

-- Now insert the recipe-ingredient relationships
INSERT INTO [dbo].[recipe_ingredients] ([recipe_id], [ingredient_id], [quantity], [unit], [notes], [is_optional], [sort_order], [created_at], [last_modified]) VALUES
-- Recipe 1: Espresso (Recipe ID 1)
(1, 3, 18.0, N'g', N'Hỗn hợp espresso cao cấp cho hai shot', 0, 1, GETDATE(), GETDATE()),
(1, 21, 2.0, N'g', N'Đường trắng - tùy chọn', 1, 2, GETDATE(), GETDATE()),

-- Recipe 2: Americano (Recipe ID 2)
(2, 3, 18.0, N'g', N'Nền espresso', 0, 1, GETDATE(), GETDATE()),
(2, 21, 3.0, N'g', N'Đường trắng - theo khẩu vị', 1, 2, GETDATE(), GETDATE()),

-- Recipe 3: Cappuccino (Recipe ID 3)
(3, 3, 18.0, N'g', N'Shot espresso', 0, 1, GETDATE(), GETDATE()),
(3, 15, 150.0, N'ml', N'Sữa tươi đánh bọt', 0, 2, GETDATE(), GETDATE()),
(3, 25, 0.5, N'g', N'Bột quế Ceylon để rắc lên', 1, 3, GETDATE(), GETDATE()),

-- Recipe 4: Latte (Recipe ID 4)
(4, 3, 18.0, N'g', N'Nền espresso', 0, 1, GETDATE(), GETDATE()),
(4, 15, 200.0, N'ml', N'Sữa tươi đánh bọt', 0, 2, GETDATE(), GETDATE()),
(4, 6, 15.0, N'ml', N'Syrup vani - tùy chọn', 1, 3, GETDATE(), GETDATE()),

-- Recipe 5: Mocha (Recipe ID 5)
(5, 3, 18.0, N'g', N'Espresso đậm đà', 0, 1, GETDATE(), GETDATE()),
(5, 15, 150.0, N'ml', N'Sữa tươi nóng đánh bọt', 0, 2, GETDATE(), GETDATE()),
(5, 9, 20.0, N'ml', N'Syrup socola đậm đà', 0, 3, GETDATE(), GETDATE()),
(5, 18, 30.0, N'ml', N'Kem tươi đánh bông trang trí', 1, 4, GETDATE(), GETDATE()),

-- Recipe 6: Macchiato (Recipe ID 6)
(6, 3, 18.0, N'g', N'Shot espresso', 0, 1, GETDATE(), GETDATE()),
(6, 7, 10.0, N'ml', N'Syrup caramel', 0, 2, GETDATE(), GETDATE()),
(6, 18, 20.0, N'ml', N'Kem tươi nhẹ', 1, 3, GETDATE(), GETDATE()),

-- Recipe 7: Cold Brew Coffee (Recipe ID 7)
(7, 4, 50.0, N'g', N'Cà phê cold brew xay thô', 0, 1, GETDATE(), GETDATE()),
(7, 22, 8.0, N'g', N'Đường nâu muscovado', 1, 2, GETDATE(), GETDATE()),
(7, 17, 100.0, N'ml', N'Sữa hạnh nhân lạnh', 0, 3, GETDATE(), GETDATE()),

-- Recipe 8: Frappuccino (Recipe ID 8)
(8, 1, 20.0, N'g', N'Arabica pha lạnh', 0, 1, GETDATE(), GETDATE()),
(8, 15, 120.0, N'ml', N'Sữa tươi lạnh', 0, 2, GETDATE(), GETDATE()),
(8, 7, 15.0, N'ml', N'Syrup caramel', 0, 3, GETDATE(), GETDATE()),
(8, 18, 40.0, N'ml', N'Kem tươi để xay', 1, 4, GETDATE(), GETDATE()),

-- Recipe 9: Fruit Tea Mix (Recipe ID 10)
(10, 10, 20.0, N'ml', N'Syrup dâu tây tươi', 0, 1, GETDATE(), GETDATE()),
(10, 11, 15.0, N'ml', N'Syrup đào', 0, 2, GETDATE(), GETDATE()),
(10, 29, 50.0, N'g', N'Nước cốt chanh tươi', 0, 3, GETDATE(), GETDATE()),
(10, 27, 0.3, N'g', N'Lá bạc hà tươi trang trí', 1, 4, GETDATE(), GETDATE()),

-- Recipe 10: Matcha Latte (Recipe ID 9)
(9, 16, 180.0, N'ml', N'Sữa yến mạch nóng', 0, 1, GETDATE(), GETDATE()),
(9, 6, 10.0, N'ml', N'Syrup vani nhẹ', 1, 2, GETDATE(), GETDATE()),
(9, 12, 5.0, N'ml', N'Syrup việt quất - tùy chọn', 1, 3, GETDATE(), GETDATE()),
(9, 21, 5.0, N'g', N'Đường trắng để điều chỉnh', 1, 4, GETDATE(), GETDATE());
GO

PRINT N'Recipe ingredients (40 ingredient mappings for 10 recipes) inserted successfully!';
GO

-- ================================================================
-- INGREDIENT WAREHOUSE DATA - Central Warehouse Inventory
-- ================================================================

PRINT N'Inserting ingredient warehouse data with varied stock levels...';
GO

-- Warehouse inventory for all 30 ingredients with random quantities
-- Some ingredients will be out of stock, some below safety stock, some well-stocked
INSERT INTO [dbo].[ingredient_warehouse] ([ingredient_id], [quantity], [safety_stock], [maximum_stock], [location], [created_at], [last_modified]) VALUES
-- Coffee Base Ingredients (IDs 1-5) - High value items, lower stock
(1, 25.5, 50.0, 200.0, N'Kho A - Tầng 1 - Khu lạnh', GETDATE(), GETDATE()),  -- Below safety stock
(2, 45.2, 40.0, 180.0, N'Kho A - Tầng 1 - Khu lạnh', GETDATE(), GETDATE()),  -- Just above safety stock
(3, 0.0, 30.0, 150.0, N'Kho A - Tầng 1 - Khu lạnh', GETDATE(), GETDATE()),   -- OUT OF STOCK
(4, 78.3, 35.0, 160.0, N'Kho A - Tầng 1 - Khu lạnh', GETDATE(), GETDATE()),  -- Well stocked
(5, 12.1, 25.0, 120.0, N'Kho A - Tầng 1 - Khu lạnh', GETDATE(), GETDATE()),  -- Below safety stock

-- Flavor Syrups (IDs 6-13) - Medium volume items
(6, 45.8, 30.0, 120.0, N'Kho B - Tầng 2 - Khu khô', GETDATE(), GETDATE()),   -- Well stocked
(7, 0.0, 25.0, 100.0, N'Kho B - Tầng 2 - Khu khô', GETDATE(), GETDATE()),    -- OUT OF STOCK
(8, 67.5, 28.0, 110.0, N'Kho B - Tầng 2 - Khu khô', GETDATE(), GETDATE()),   -- Well stocked
(9, 18.2, 20.0, 90.0, N'Kho B - Tầng 2 - Khu khô', GETDATE(), GETDATE()),    -- Below safety stock
(10, 52.3, 22.0, 95.0, N'Kho B - Tầng 2 - Khu khô', GETDATE(), GETDATE()),   -- Well stocked
(11, 31.7, 24.0, 98.0, N'Kho B - Tầng 2 - Khu khô', GETDATE(), GETDATE()),   -- Above safety stock
(12, 8.9, 15.0, 80.0, N'Kho B - Tầng 2 - Khu khô', GETDATE(), GETDATE()),    -- Below safety stock
(13, 89.4, 35.0, 140.0, N'Kho B - Tầng 2 - Khu khô', GETDATE(), GETDATE()),  -- Well stocked

-- Dairy & Milk Alternatives (IDs 14-19) - High turnover, need frequent restocking
(14, 245.6, 100.0, 500.0, N'Kho C - Tầng 1 - Khu lạnh', GETDATE(), GETDATE()), -- Well stocked
(15, 67.3, 80.0, 400.0, N'Kho C - Tầng 1 - Khu lạnh', GETDATE(), GETDATE()),   -- Below safety stock
(16, 0.0, 60.0, 300.0, N'Kho C - Tầng 1 - Khu lạnh', GETDATE(), GETDATE()),    -- OUT OF STOCK
(17, 156.8, 90.0, 450.0, N'Kho C - Tầng 1 - Khu lạnh', GETDATE(), GETDATE()),  -- Well stocked
(18, 34.2, 50.0, 250.0, N'Kho C - Tầng 1 - Khu lạnh', GETDATE(), GETDATE()),   -- Below safety stock
(19, 128.9, 70.0, 350.0, N'Kho C - Tầng 1 - Khu lạnh', GETDATE(), GETDATE()),  -- Well stocked

-- Sweeteners & Sugar (IDs 20-23) - High volume basic ingredients
(20, 567.4, 200.0, 1000.0, N'Kho D - Tầng 1 - Khu khô', GETDATE(), GETDATE()), -- Well stocked
(21, 123.7, 150.0, 800.0, N'Kho D - Tầng 1 - Khu khô', GETDATE(), GETDATE()),  -- Below safety stock
(22, 45.8, 80.0, 400.0, N'Kho D - Tầng 1 - Khu khô', GETDATE(), GETDATE()),    -- Below safety stock
(23, 234.6, 120.0, 600.0, N'Kho D - Tầng 1 - Khu khô', GETDATE(), GETDATE()),  -- Well stocked

-- Spices & Seasonings (IDs 24-27) - Low volume, high value
(24, 2.3, 5.0, 25.0, N'Kho E - Tầng 2 - Khu gia vị', GETDATE(), GETDATE()),    -- Below safety stock
(25, 8.7, 8.0, 40.0, N'Kho E - Tầng 2 - Khu gia vị', GETDATE(), GETDATE()),    -- Just above safety stock
(26, 0.0, 3.0, 15.0, N'Kho E - Tầng 2 - Khu gia vị', GETDATE(), GETDATE()),    -- OUT OF STOCK
(27, 6.2, 4.0, 20.0, N'Kho E - Tầng 2 - Khu gia vị', GETDATE(), GETDATE()),    -- Well stocked

-- Fresh Fruits (IDs 28-30) - High turnover, short shelf life
(28, 15.6, 25.0, 80.0, N'Kho F - Tầng 1 - Khu tươi', GETDATE(), GETDATE()),    -- Below safety stock
(29, 0.0, 20.0, 70.0, N'Kho F - Tầng 1 - Khu tươi', GETDATE(), GETDATE()),     -- OUT OF STOCK
(30, 32.4, 30.0, 100.0, N'Kho F - Tầng 1 - Khu tươi', GETDATE(), GETDATE());   -- Just above safety stock
GO

PRINT N'Ingredient warehouse data (30 ingredients with varied stock levels) inserted successfully!';
PRINT N'📊 Stock Status Summary:';
PRINT N'   - OUT OF STOCK: 5 ingredients (Espresso blend, Syrup caramel, Sữa hạnh nhân, Lá bạc hà, Cam tươi)';
PRINT N'   - BELOW SAFETY STOCK: 10 ingredients';
PRINT N'   - WELL STOCKED: 15 ingredients';
GO

-- ================================================================
-- BRANCH INGREDIENT INVENTORY - Distribution Across 7 Branches  
-- ================================================================

PRINT N'Inserting branch ingredient inventory for all 7 branches...';
GO

-- Branch inventory for all ingredients across all 7 branches
-- Each branch will have different stock levels reflecting real business scenarios
INSERT INTO [dbo].[branch_ingredient_inventory] ([branch_id], [ingredient_id], [quantity], [reserved_quantity], [created_at], [last_modified]) VALUES

-- BRANCH 1: Main store - Generally well stocked
(1, 1, 12.5, 2.0, GETDATE(), GETDATE()), (1, 2, 18.3, 1.5, GETDATE(), GETDATE()), (1, 3, 0.0, 0.0, GETDATE(), GETDATE()),
(1, 4, 25.7, 3.2, GETDATE(), GETDATE()), (1, 5, 8.4, 1.0, GETDATE(), GETDATE()), (1, 6, 15.6, 2.1, GETDATE(), GETDATE()),
(1, 7, 0.0, 0.0, GETDATE(), GETDATE()), (1, 8, 22.1, 2.8, GETDATE(), GETDATE()), (1, 9, 6.7, 0.5, GETDATE(), GETDATE()),
(1, 10, 19.4, 1.9, GETDATE(), GETDATE()), (1, 11, 14.2, 1.4, GETDATE(), GETDATE()), (1, 12, 3.1, 0.2, GETDATE(), GETDATE()),
(1, 13, 28.6, 3.5, GETDATE(), GETDATE()), (1, 14, 45.8, 5.2, GETDATE(), GETDATE()), (1, 15, 12.3, 1.8, GETDATE(), GETDATE()),
(1, 16, 0.0, 0.0, GETDATE(), GETDATE()), (1, 17, 35.4, 4.1, GETDATE(), GETDATE()), (1, 18, 8.9, 1.2, GETDATE(), GETDATE()),
(1, 19, 29.7, 3.3, GETDATE(), GETDATE()), (1, 20, 156.3, 12.5, GETDATE(), GETDATE()), (1, 21, 89.2, 8.1, GETDATE(), GETDATE()),
(1, 22, 23.4, 2.7, GETDATE(), GETDATE()), (1, 23, 67.8, 6.2, GETDATE(), GETDATE()), (1, 24, 1.2, 0.1, GETDATE(), GETDATE()),
(1, 25, 3.8, 0.3, GETDATE(), GETDATE()), (1, 26, 0.0, 0.0, GETDATE(), GETDATE()), (1, 27, 2.7, 0.2, GETDATE(), GETDATE()),
(1, 28, 8.4, 1.5, GETDATE(), GETDATE()), (1, 29, 0.0, 0.0, GETDATE(), GETDATE()), (1, 30, 12.6, 2.1, GETDATE(), GETDATE()),

-- BRANCH 2: Busy downtown location - High turnover, some shortages
(2, 1, 8.7, 3.2, GETDATE(), GETDATE()), (2, 2, 15.4, 2.8, GETDATE(), GETDATE()), (2, 3, 0.0, 0.0, GETDATE(), GETDATE()),
(2, 4, 31.2, 4.5, GETDATE(), GETDATE()), (2, 5, 4.1, 1.8, GETDATE(), GETDATE()), (2, 6, 12.8, 3.1, GETDATE(), GETDATE()),
(2, 7, 0.0, 0.0, GETDATE(), GETDATE()), (2, 8, 18.9, 3.4, GETDATE(), GETDATE()), (2, 9, 2.3, 0.8, GETDATE(), GETDATE()),
(2, 10, 16.7, 2.9, GETDATE(), GETDATE()), (2, 11, 11.5, 2.1, GETDATE(), GETDATE()), (2, 12, 1.8, 0.4, GETDATE(), GETDATE()),
(2, 13, 24.3, 4.2, GETDATE(), GETDATE()), (2, 14, 52.1, 8.7, GETDATE(), GETDATE()), (2, 15, 9.6, 2.3, GETDATE(), GETDATE()),
(2, 16, 0.0, 0.0, GETDATE(), GETDATE()), (2, 17, 28.7, 5.4, GETDATE(), GETDATE()), (2, 18, 6.2, 1.9, GETDATE(), GETDATE()),
(2, 19, 23.4, 4.1, GETDATE(), GETDATE()), (2, 20, 134.6, 18.3, GETDATE(), GETDATE()), (2, 21, 67.5, 12.8, GETDATE(), GETDATE()),
(2, 22, 18.9, 3.7, GETDATE(), GETDATE()), (2, 23, 54.2, 8.9, GETDATE(), GETDATE()), (2, 24, 0.8, 0.2, GETDATE(), GETDATE()),
(2, 25, 2.9, 0.5, GETDATE(), GETDATE()), (2, 26, 0.0, 0.0, GETDATE(), GETDATE()), (2, 27, 1.9, 0.3, GETDATE(), GETDATE()),
(2, 28, 6.7, 2.1, GETDATE(), GETDATE()), (2, 29, 0.0, 0.0, GETDATE(), GETDATE()), (2, 30, 9.8, 2.8, GETDATE(), GETDATE()),

-- BRANCH 3: Mall location - Moderate stock levels
(3, 1, 10.2, 1.8, GETDATE(), GETDATE()), (3, 2, 16.8, 2.1, GETDATE(), GETDATE()), (3, 3, 0.0, 0.0, GETDATE(), GETDATE()),
(3, 4, 22.4, 2.9, GETDATE(), GETDATE()), (3, 5, 6.7, 0.8, GETDATE(), GETDATE()), (3, 6, 14.1, 1.7, GETDATE(), GETDATE()),
(3, 7, 0.0, 0.0, GETDATE(), GETDATE()), (3, 8, 19.6, 2.3, GETDATE(), GETDATE()), (3, 9, 4.2, 0.6, GETDATE(), GETDATE()),
(3, 10, 17.8, 2.1, GETDATE(), GETDATE()), (3, 11, 13.3, 1.6, GETDATE(), GETDATE()), (3, 12, 2.4, 0.3, GETDATE(), GETDATE()),
(3, 13, 26.7, 3.1, GETDATE(), GETDATE()), (3, 14, 38.9, 4.7, GETDATE(), GETDATE()), (3, 15, 11.2, 1.4, GETDATE(), GETDATE()),
(3, 16, 0.0, 0.0, GETDATE(), GETDATE()), (3, 17, 31.5, 3.8, GETDATE(), GETDATE()), (3, 18, 7.6, 0.9, GETDATE(), GETDATE()),
(3, 19, 26.1, 3.2, GETDATE(), GETDATE()), (3, 20, 142.7, 14.8, GETDATE(), GETDATE()), (3, 21, 76.3, 9.1, GETDATE(), GETDATE()),
(3, 22, 20.8, 2.5, GETDATE(), GETDATE()), (3, 23, 59.4, 7.1, GETDATE(), GETDATE()), (3, 24, 1.1, 0.1, GETDATE(), GETDATE()),
(3, 25, 3.4, 0.4, GETDATE(), GETDATE()), (3, 26, 0.0, 0.0, GETDATE(), GETDATE()), (3, 27, 2.3, 0.3, GETDATE(), GETDATE()),
(3, 28, 7.8, 1.2, GETDATE(), GETDATE()), (3, 29, 0.0, 0.0, GETDATE(), GETDATE()), (3, 30, 11.4, 1.7, GETDATE(), GETDATE()),

-- BRANCH 4: University area - High student traffic, budget items
(4, 1, 6.8, 2.4, GETDATE(), GETDATE()), (4, 2, 14.7, 3.1, GETDATE(), GETDATE()), (4, 3, 0.0, 0.0, GETDATE(), GETDATE()),
(4, 4, 28.3, 3.7, GETDATE(), GETDATE()), (4, 5, 3.9, 1.2, GETDATE(), GETDATE()), (4, 6, 11.2, 2.3, GETDATE(), GETDATE()),
(4, 7, 0.0, 0.0, GETDATE(), GETDATE()), (4, 8, 21.5, 2.8, GETDATE(), GETDATE()), (4, 9, 1.7, 0.9, GETDATE(), GETDATE()),
(4, 10, 15.3, 2.7, GETDATE(), GETDATE()), (4, 11, 9.8, 1.8, GETDATE(), GETDATE()), (4, 12, 1.2, 0.5, GETDATE(), GETDATE()),
(4, 13, 22.9, 3.8, GETDATE(), GETDATE()), (4, 14, 47.6, 7.2, GETDATE(), GETDATE()), (4, 15, 8.4, 2.1, GETDATE(), GETDATE()),
(4, 16, 0.0, 0.0, GETDATE(), GETDATE()), (4, 17, 25.8, 4.3, GETDATE(), GETDATE()), (4, 18, 5.1, 1.4, GETDATE(), GETDATE()),
(4, 19, 21.7, 3.6, GETDATE(), GETDATE()), (4, 20, 178.4, 21.7, GETDATE(), GETDATE()), (4, 21, 98.6, 15.2, GETDATE(), GETDATE()),
(4, 22, 16.3, 3.1, GETDATE(), GETDATE()), (4, 23, 45.9, 8.7, GETDATE(), GETDATE()), (4, 24, 0.6, 0.2, GETDATE(), GETDATE()),
(4, 25, 2.1, 0.6, GETDATE(), GETDATE()), (4, 26, 0.0, 0.0, GETDATE(), GETDATE()), (4, 27, 1.4, 0.4, GETDATE(), GETDATE()),
(4, 28, 5.9, 1.8, GETDATE(), GETDATE()), (4, 29, 0.0, 0.0, GETDATE(), GETDATE()), (4, 30, 8.7, 2.3, GETDATE(), GETDATE()),

-- BRANCH 5: Office district - Premium products, regular customers
(5, 1, 14.6, 1.2, GETDATE(), GETDATE()), (5, 2, 19.1, 1.8, GETDATE(), GETDATE()), (5, 3, 0.0, 0.0, GETDATE(), GETDATE()),
(5, 4, 26.8, 2.4, GETDATE(), GETDATE()), (5, 5, 9.3, 0.7, GETDATE(), GETDATE()), (5, 6, 17.4, 1.6, GETDATE(), GETDATE()),
(5, 7, 0.0, 0.0, GETDATE(), GETDATE()), (5, 8, 23.7, 2.1, GETDATE(), GETDATE()), (5, 9, 5.8, 0.4, GETDATE(), GETDATE()),
(5, 10, 20.1, 1.8, GETDATE(), GETDATE()), (5, 11, 15.6, 1.4, GETDATE(), GETDATE()), (5, 12, 3.7, 0.2, GETDATE(), GETDATE()),
(5, 13, 31.2, 2.8, GETDATE(), GETDATE()), (5, 14, 41.3, 3.7, GETDATE(), GETDATE()), (5, 15, 13.8, 1.2, GETDATE(), GETDATE()),
(5, 16, 0.0, 0.0, GETDATE(), GETDATE()), (5, 17, 34.9, 3.1, GETDATE(), GETDATE()), (5, 18, 9.4, 0.8, GETDATE(), GETDATE()),
(5, 19, 28.6, 2.6, GETDATE(), GETDATE()), (5, 20, 126.8, 11.4, GETDATE(), GETDATE()), (5, 21, 72.1, 6.5, GETDATE(), GETDATE()),
(5, 22, 24.7, 2.2, GETDATE(), GETDATE()), (5, 23, 63.5, 5.7, GETDATE(), GETDATE()), (5, 24, 1.6, 0.1, GETDATE(), GETDATE()),
(5, 25, 4.2, 0.4, GETDATE(), GETDATE()), (5, 26, 0.0, 0.0, GETDATE(), GETDATE()), (5, 27, 3.1, 0.3, GETDATE(), GETDATE()),
(5, 28, 9.7, 0.9, GETDATE(), GETDATE()), (5, 29, 0.0, 0.0, GETDATE(), GETDATE()), (5, 30, 14.2, 1.3, GETDATE(), GETDATE()),

-- BRANCH 6: Residential area - Family-friendly, moderate volumes
(6, 1, 9.4, 1.6, GETDATE(), GETDATE()), (6, 2, 13.2, 2.3, GETDATE(), GETDATE()), (6, 3, 0.0, 0.0, GETDATE(), GETDATE()),
(6, 4, 21.7, 2.8, GETDATE(), GETDATE()), (6, 5, 5.6, 1.1, GETDATE(), GETDATE()), (6, 6, 12.9, 1.9, GETDATE(), GETDATE()),
(6, 7, 0.0, 0.0, GETDATE(), GETDATE()), (6, 8, 18.4, 2.5, GETDATE(), GETDATE()), (6, 9, 3.1, 0.7, GETDATE(), GETDATE()),
(6, 10, 16.8, 2.2, GETDATE(), GETDATE()), (6, 11, 12.1, 1.7, GETDATE(), GETDATE()), (6, 12, 2.3, 0.4, GETDATE(), GETDATE()),
(6, 13, 25.6, 3.4, GETDATE(), GETDATE()), (6, 14, 36.7, 4.9, GETDATE(), GETDATE()), (6, 15, 10.8, 1.6, GETDATE(), GETDATE()),
(6, 16, 0.0, 0.0, GETDATE(), GETDATE()), (6, 17, 29.3, 3.9, GETDATE(), GETDATE()), (6, 18, 6.8, 1.1, GETDATE(), GETDATE()),
(6, 19, 24.5, 3.3, GETDATE(), GETDATE()), (6, 20, 149.2, 16.1, GETDATE(), GETDATE()), (6, 21, 83.7, 10.4, GETDATE(), GETDATE()),
(6, 22, 19.6, 2.8, GETDATE(), GETDATE()), (6, 23, 56.1, 7.6, GETDATE(), GETDATE()), (6, 24, 0.9, 0.1, GETDATE(), GETDATE()),
(6, 25, 2.8, 0.4, GETDATE(), GETDATE()), (6, 26, 0.0, 0.0, GETDATE(), GETDATE()), (6, 27, 2.1, 0.3, GETDATE(), GETDATE()),
(6, 28, 7.2, 1.3, GETDATE(), GETDATE()), (6, 29, 0.0, 0.0, GETDATE(), GETDATE()), (6, 30, 10.6, 1.8, GETDATE(), GETDATE()),

-- BRANCH 7: Tourist area - Seasonal variations, premium offerings
(7, 1, 11.8, 2.1, GETDATE(), GETDATE()), (7, 2, 17.5, 2.6, GETDATE(), GETDATE()), (7, 3, 0.0, 0.0, GETDATE(), GETDATE()),
(7, 4, 24.9, 3.2, GETDATE(), GETDATE()), (7, 5, 7.2, 1.3, GETDATE(), GETDATE()), (7, 6, 15.7, 2.4, GETDATE(), GETDATE()),
(7, 7, 0.0, 0.0, GETDATE(), GETDATE()), (7, 8, 20.8, 2.9, GETDATE(), GETDATE()), (7, 9, 4.6, 0.8, GETDATE(), GETDATE()),
(7, 10, 18.9, 2.5, GETDATE(), GETDATE()), (7, 11, 14.3, 2.0, GETDATE(), GETDATE()), (7, 12, 2.9, 0.3, GETDATE(), GETDATE()),
(7, 13, 28.1, 3.7, GETDATE(), GETDATE()), (7, 14, 43.2, 5.8, GETDATE(), GETDATE()), (7, 15, 12.6, 1.9, GETDATE(), GETDATE()),
(7, 16, 0.0, 0.0, GETDATE(), GETDATE()), (7, 17, 32.4, 4.3, GETDATE(), GETDATE()), (7, 18, 8.1, 1.3, GETDATE(), GETDATE()),
(7, 19, 27.3, 3.7, GETDATE(), GETDATE()), (7, 20, 161.5, 19.2, GETDATE(), GETDATE()), (7, 21, 91.4, 12.7, GETDATE(), GETDATE()),
(7, 22, 22.1, 3.4, GETDATE(), GETDATE()), (7, 23, 61.8, 8.2, GETDATE(), GETDATE()), (7, 24, 1.3, 0.2, GETDATE(), GETDATE()),
(7, 25, 3.6, 0.5, GETDATE(), GETDATE()), (7, 26, 0.0, 0.0, GETDATE(), GETDATE()), (7, 27, 2.6, 0.4, GETDATE(), GETDATE()),
(7, 28, 8.5, 1.6, GETDATE(), GETDATE()), (7, 29, 0.0, 0.0, GETDATE(), GETDATE()), (7, 30, 12.9, 2.4, GETDATE(), GETDATE());
GO

PRINT N'Branch ingredient inventory (210 records = 30 ingredients × 7 branches) inserted successfully!';
PRINT N'📋 Branch Inventory Summary:';
PRINT N'   - Total ingredient-branch combinations: 210';
PRINT N'   - Out of stock items per branch: 5 ingredients';
PRINT N'   - Reserved quantities included for demand planning';
PRINT N'   - Varied stock levels reflecting different branch characteristics';
GO

-- ================================================================
-- MASSIVE ORDERS DATA FOR LAST 30 DAYS (20 orders per day = 600 orders)
-- ================================================================
PRINT N'🚀 Starting insertion of 600 orders for the last 30 days (20 orders per day)...';
GO

SET NOCOUNT ON;

-- Declare variables for the loop
DECLARE @DayCounter INT = 0;
DECLARE @OrderIdStart INT = 71; -- Start after existing orders
DECLARE @CurrentOrderId INT = @OrderIdStart;
DECLARE @OrdersPerDay INT = 20;
DECLARE @TotalDays INT = 30;

-- Counters for rows affected
DECLARE @OrdersInserted INT = 0;
DECLARE @OrderDetailsInserted INT = 0;
DECLARE @PaymentsInserted INT = 0;
DECLARE @ShipmentsInserted INT = 0;

-- Loop through each of the last 30 days
WHILE @DayCounter < @TotalDays
BEGIN
    DECLARE @OrderCounter INT = 0;
    DECLARE @CurrentDate DATETIME2 = DATEADD(DAY, -@DayCounter, GETDATE());
    
    -- Create 20 orders for the current day
    WHILE @OrderCounter < @OrdersPerDay
    BEGIN
        -- Random customer
        DECLARE @CustomerIndex INT = (ABS(CHECKSUM(NEWID())) % 7); -- 0-6
        DECLARE @CustomerId INT = CASE @CustomerIndex
            WHEN 0 THEN 3 WHEN 1 THEN 4 WHEN 2 THEN 6 WHEN 3 THEN 7
            WHEN 4 THEN 8 WHEN 5 THEN 9 ELSE 10 END;
        
        DECLARE @BranchId INT = (ABS(CHECKSUM(NEWID())) % 7) + 1; -- Random branch 1-7
        DECLARE @StatusId INT = CASE 
            WHEN (ABS(CHECKSUM(NEWID())) % 100) < 85 THEN 4 -- 85% completed
            WHEN (ABS(CHECKSUM(NEWID())) % 100) < 95 THEN 3 -- 10% shipped
            ELSE 2 -- 5% confirmed
        END;
        
        -- Random time in the day
        DECLARE @OrderDateTime DATETIME2 = DATEADD(HOUR, 7 + (ABS(CHECKSUM(NEWID())) % 16), @CurrentDate);
        DECLARE @OrderDateTime2 DATETIME2 = DATEADD(MINUTE, ABS(CHECKSUM(NEWID())) % 60, @OrderDateTime);
        
        -- Random order total
        DECLARE @OrderTotal DECIMAL(18,2) = 50000 + (ABS(CHECKSUM(NEWID())) % 450000);
        
        -- Order UUID and code
        DECLARE @OrderUUID VARCHAR(36) = LOWER(CAST(NEWID() AS VARCHAR(36)));
        DECLARE @OrderCode VARCHAR(20) = 'ORD' + CAST(@CurrentOrderId AS VARCHAR(10)) + CAST(ABS(CHECKSUM(NEWID())) % 1000 AS VARCHAR(3));
        
        -- Insert order
        INSERT INTO [dbo].[orders] ([order_uuid], [order_code], [customer_id], [branch_id], [status_id], [total_money], [created_at], [last_modified])
        VALUES (@OrderUUID, @OrderCode, @CustomerId, @BranchId, @StatusId, @OrderTotal, @OrderDateTime2, @OrderDateTime2);

        SET @OrdersInserted += @@ROWCOUNT;

        DECLARE @InsertedOrderId INT = SCOPE_IDENTITY();
        
        -- Insert order details
        DECLARE @ProductCount INT = (ABS(CHECKSUM(NEWID())) % 4) + 1;
        DECLARE @ProductCounter INT = 0;
        DECLARE @RunningTotal DECIMAL(18,2) = 0;
        
        WHILE @ProductCounter < @ProductCount
        BEGIN
            DECLARE @ProductId INT = (ABS(CHECKSUM(NEWID())) % 20) + 1;
            DECLARE @Quantity INT = (ABS(CHECKSUM(NEWID())) % 3) + 1;
            DECLARE @ProductPrice DECIMAL(18,2) = 25000 + (ABS(CHECKSUM(NEWID())) % 75000);
            DECLARE @LineTotal DECIMAL(18,2) = @ProductPrice * @Quantity;
            
            INSERT INTO [dbo].[order_details] ([order_id], [product_id], [unit_price], [quantity], [total_amount], [created_at], [last_modified])
            VALUES (@InsertedOrderId, @ProductId, @ProductPrice, @Quantity, @LineTotal, @OrderDateTime2, @OrderDateTime2);
            
            SET @OrderDetailsInserted += @@ROWCOUNT;
            SET @RunningTotal += @LineTotal;
            SET @ProductCounter += 1;
        END;
        
        -- Update order total
        UPDATE [dbo].[orders] SET [total_money] = @RunningTotal WHERE [id] = @InsertedOrderId;
        
        -- Payment if completed
        IF @StatusId = 4
        BEGIN
            DECLARE @PaymentMethodId INT = (ABS(CHECKSUM(NEWID())) % 3) + 1;
            DECLARE @PaymentDate DATETIME2 = DATEADD(MINUTE, (ABS(CHECKSUM(NEWID())) % 30), @OrderDateTime2);
            
            INSERT INTO [dbo].[order_payments] ([order_id], [payment_method_id], [payment_status_id], [amount], [transaction_id], [payment_date], [created_at], [last_modified])
            VALUES (@InsertedOrderId, @PaymentMethodId, 1, @RunningTotal, 
                   'TXN' + CAST(@InsertedOrderId AS VARCHAR(10)) + '_' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(8)), 
                   @PaymentDate, @OrderDateTime2, @OrderDateTime2);

            SET @PaymentsInserted += @@ROWCOUNT;
        END;
        
        -- Shipment if shipped/completed
        IF @StatusId IN (3, 4)
        BEGIN
            DECLARE @ShippingProviderId INT = (ABS(CHECKSUM(NEWID())) % 2) + 1;
            DECLARE @ShippingCost DECIMAL(18,2) = 15000 + (ABS(CHECKSUM(NEWID())) % 35000);
            DECLARE @EstimatedDelivery DATETIME2 = DATEADD(DAY, 1 + (ABS(CHECKSUM(NEWID())) % 3), @OrderDateTime2);
            
            INSERT INTO [dbo].[order_shipments] ([order_id], [shipping_provider_id], [shipping_address], [shipping_cost], [shipping_method], [estimated_delivery_date], [notes], [created_at], [last_modified])
            VALUES (@InsertedOrderId, @ShippingProviderId, 
                   N'Địa chỉ giao hàng số ' + CAST(@InsertedOrderId AS NVARCHAR(10)) + N', Quận ' + CAST((@InsertedOrderId % 12) + 1 AS NVARCHAR(2)) + N', TP.HCM',
                   @ShippingCost, 
                   CASE WHEN @ShippingProviderId = 1 THEN N'Giao hàng tiêu chuẩn' ELSE N'Giao hàng nhanh' END,
                   @EstimatedDelivery,
                   N'Giao hàng đơn hàng #' + CAST(@InsertedOrderId AS NVARCHAR(10)),
                   @OrderDateTime2, @OrderDateTime2);

            SET @ShipmentsInserted += @@ROWCOUNT;
        END;
        
        SET @OrderCounter += 1;
        SET @CurrentOrderId += 1;
    END;
    
    SET @DayCounter += 1;
END;

-- Final summary message
PRINT N'✅ MASSIVE DATA INSERTION COMPLETED! ✅';
PRINT N'📊 Summary:';
PRINT N'✅ Rows affected summary:';
PRINT N'   • Orders inserted: ' + CAST(@OrdersInserted AS NVARCHAR(20));
PRINT N'   • Order details inserted: ' + CAST(@OrderDetailsInserted AS NVARCHAR(20));
PRINT N'   • Payments inserted: ' + CAST(@PaymentsInserted AS NVARCHAR(20));
PRINT N'   • Shipments inserted: ' + CAST(@ShipmentsInserted AS NVARCHAR(20));
PRINT N'🗓️ Date Range: Last 30 days with realistic daily distribution';
PRINT N'💰 Revenue Range: 50k - 500k VND per order';
PRINT N'🎯 Perfect for comprehensive Dashboard Analytics and Reporting!';

GO
-- Summary Statistics
SELECT 
    'ORDERS' as [Table_Name],
    COUNT(*) as [Total_Records],
    MIN([created_at]) as [Earliest_Date],
    MAX([created_at]) as [Latest_Date]
FROM [dbo].[orders]
UNION ALL
SELECT 
    'ORDER_DETAILS' as [Table_Name],
    COUNT(*) as [Total_Records],
    MIN([created_at]) as [Earliest_Date],
    MAX([created_at]) as [Latest_Date]
FROM [dbo].[order_details]
UNION ALL
SELECT 
    'ORDER_PAYMENTS' as [Table_Name],
    COUNT(*) as [Total_Records],
    MIN([created_at]) as [Earliest_Date],
    MAX([created_at]) as [Latest_Date]
FROM [dbo].[order_payments]
UNION ALL
SELECT 
    'ORDER_SHIPMENTS' as [Table_Name],
    COUNT(*) as [Total_Records],
    MIN([created_at]) as [Earliest_Date],
    MAX([created_at]) as [Latest_Date]
FROM [dbo].[order_shipments];
GO

PRINT N'✅ BULK ORDERS DATA INSERTION COMPLETED SUCCESSFULLY! ✅';
PRINT N'📊 Total: 50 Orders + 150+ Order Details + 50 Payments + 20+ Shipments';
PRINT N'🗓️  Date Range: Random dates within the past 365 days';
PRINT N'💰 Revenue Range: 75k - 885k VND per order';
PRINT N'🎯 Perfect for Dashboard Charts and Analytics!';
GO
GO
