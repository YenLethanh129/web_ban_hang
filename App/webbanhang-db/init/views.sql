-- Views for reporting (replacing summary tables that violate 3NF)
-- These views calculate aggregated data on-the-fly instead of storing pre-calculated values

-- Sales Summary View
GO
CREATE VIEW [dbo].[v_sales_summary] AS
SELECT 
    o.branch_id,
    YEAR(o.order_date) as year,
    MONTH(o.order_date) as month,
    CONCAT(YEAR(o.order_date), '-', FORMAT(MONTH(o.order_date), '00')) as period,
    COUNT(o.id) as total_orders,
    SUM(od.quantity) as total_products,
    SUM(o.subtotal) as revenue_before_tax,
    SUM(o.total_amount) as revenue_after_tax,
    SUM(o.tax_amount) as tax_amount
FROM [dbo].[orders] o
INNER JOIN [dbo].[order_details] od ON o.id = od.order_id
INNER JOIN [dbo].[order_statuses] os ON o.status_id = os.id
WHERE os.name NOT IN ('CANCELLED', 'REJECTED')
GROUP BY o.branch_id, YEAR(o.order_date), MONTH(o.order_date);

-- Expenses Summary View
GO
CREATE VIEW [dbo].[v_expenses_summary] AS
SELECT 
    po.branch_id,
    YEAR(po.order_date) as year,
    MONTH(po.order_date) as month,
    CONCAT(YEAR(po.order_date), '-', FORMAT(MONTH(po.order_date), '00')) as period,
    COUNT(po.id) as total_purchase_orders,
    SUM(pod.quantity_ordered) as total_ingredients,
    SUM(po.subtotal) as expense_before_tax,
    SUM(po.total_amount) as expense_after_tax,
    SUM(po.tax_amount) as tax_amount
FROM [dbo].[ingredient_purchase_orders] po
INNER JOIN [dbo].[ingredient_purchase_order_details] pod ON po.id = pod.purchase_order_id
WHERE po.status NOT IN ('CANCELLED', 'REJECTED')
GROUP BY po.branch_id, YEAR(po.order_date), MONTH(po.order_date);

-- Profit Summary View
GO
CREATE VIEW [dbo].[v_profit_summary] AS
SELECT 
    COALESCE(s.branch_id, e.branch_id) as branch_id,
    COALESCE(s.year, e.year) as year,
    COALESCE(s.month, e.month) as month,
    COALESCE(s.period, e.period) as period,
    COALESCE(s.revenue_before_tax, 0) as revenue_before_tax,
    COALESCE(s.revenue_after_tax, 0) as revenue_after_tax,
    COALESCE(e.expense_before_tax, 0) as expense_before_tax,
    COALESCE(e.expense_after_tax, 0) as expense_after_tax,
    COALESCE(s.tax_amount, 0) as output_tax,
    COALESCE(e.tax_amount, 0) as input_tax,
    (COALESCE(s.tax_amount, 0) - COALESCE(e.tax_amount, 0)) as vat_to_pay,
    (COALESCE(s.revenue_before_tax, 0) - COALESCE(e.expense_before_tax, 0)) as profit_before_tax,
    (COALESCE(s.revenue_after_tax, 0) - COALESCE(e.expense_after_tax, 0)) as profit_after_tax
FROM [dbo].[v_sales_summary] s
FULL OUTER JOIN [dbo].[v_expenses_summary] e 
    ON s.branch_id = e.branch_id AND s.year = e.year AND s.month = e.month;

-- Inventory Status View
GO
CREATE VIEW [dbo].[v_inventory_status] AS
SELECT 
    i.id as ingredient_id,
    i.name as ingredient_name,
    ii.location_id,
    il.location_name,
    il.branch_id,
    b.name as branch_name,
    ii.quantity_on_hand,
    ii.quantity_reserved,
    (ii.quantity_on_hand - ii.quantity_reserved) as available_quantity,
    i.minimum_stock,
    CASE 
        WHEN (ii.quantity_on_hand - ii.quantity_reserved) <= i.minimum_stock THEN 'LOW_STOCK'
        WHEN (ii.quantity_on_hand - ii.quantity_reserved) = 0 THEN 'OUT_OF_STOCK'
        ELSE 'IN_STOCK'
    END as stock_status,
    uom.name as unit_of_measure,
    ii.last_updated
FROM [dbo].[ingredients] i
INNER JOIN [dbo].[ingredient_inventory] ii ON i.id = ii.ingredient_id
INNER JOIN [dbo].[inventory_locations] il ON ii.location_id = il.id
INNER JOIN [dbo].[branches] b ON il.branch_id = b.id
INNER JOIN [dbo].[units_of_measure] uom ON i.unit_of_measure_id = uom.id;

-- Employee Payroll View
GO
CREATE VIEW [dbo].[v_employee_payroll] AS
SELECT 
    e.id as employee_id,
    e.full_name,
    b.name as branch_name,
    ep.name as position_name,
    es.base_salary,
    es.salary_type,
    SUM(CASE WHEN sc.component_type = 'ALLOWANCE' THEN esc.amount ELSE 0 END) as total_allowances,
    SUM(CASE WHEN sc.component_type = 'BONUS' THEN esc.amount ELSE 0 END) as total_bonus,
    SUM(CASE WHEN sc.component_type = 'DEDUCTION' THEN esc.amount ELSE 0 END) as total_deductions,
    (es.base_salary + 
     SUM(CASE WHEN sc.component_type = 'ALLOWANCE' THEN esc.amount ELSE 0 END) + 
     SUM(CASE WHEN sc.component_type = 'BONUS' THEN esc.amount ELSE 0 END) - 
     SUM(CASE WHEN sc.component_type = 'DEDUCTION' THEN esc.amount ELSE 0 END)) as gross_salary,
    es.effective_date,
    es.end_date
FROM [dbo].[employees] e
INNER JOIN [dbo].[branches] b ON e.branch_id = b.id
LEFT JOIN [dbo].[employee_salaries] es ON e.id = es.employee_id 
    AND (es.end_date IS NULL OR es.end_date >= GETDATE())
LEFT JOIN [dbo].[employee_positions] ep ON es.position_id = ep.id
LEFT JOIN [dbo].[employee_salary_components] esc ON es.id = esc.employee_salary_id
LEFT JOIN [dbo].[salary_components] sc ON esc.component_id = sc.id
WHERE e.status = 'ACTIVE'
GROUP BY e.id, e.full_name, b.name, ep.name, es.base_salary, es.salary_type, es.effective_date, es.end_date;

-- Product with Current Price View
GO
CREATE VIEW [dbo].[v_products_with_prices] AS
SELECT 
    p.id,
    p.name,
    p.description,
    p.sku,
    c.name as category_name,
    pp.price as current_price,
    pp.price_type,
    t.name as tax_name,
    t.tax_rate,
    uom.name as unit_of_measure,
    p.weight,
    p.dimensions,
    p.is_active,
    p.created_at,
    p.updated_at
FROM [dbo].[products] p
LEFT JOIN [dbo].[categories] c ON p.category_id = c.id
LEFT JOIN [dbo].[taxes] t ON p.tax_id = t.id
LEFT JOIN [dbo].[units_of_measure] uom ON p.unit_of_measure_id = uom.id
LEFT JOIN [dbo].[product_prices] pp ON p.id = pp.product_id 
    AND pp.is_active = 1 
    AND pp.effective_from <= GETDATE() 
    AND (pp.effective_to IS NULL OR pp.effective_to >= GETDATE())
    AND pp.price_type = 'BASE'
WHERE p.is_active = 1;
