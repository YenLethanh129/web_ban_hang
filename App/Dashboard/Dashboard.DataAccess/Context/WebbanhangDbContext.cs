using System;
using System.Collections.Generic;
using Dashboard.DataAccess.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Context;

public partial class WebbanhangDbContext : DbContext
{
    public WebbanhangDbContext(DbContextOptions<WebbanhangDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<BranchExpense> BranchExpenses { get; set; }

    public virtual DbSet<BranchIngredientInventory> BranchIngredientInventories { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DeliveryStatus> DeliveryStatuses { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeSalary> EmployeeSalaries { get; set; }

    public virtual DbSet<EmployeeShift> EmployeeShifts { get; set; }

    public virtual DbSet<ExpensesSummary> ExpensesSummaries { get; set; }

    public virtual DbSet<GoodsReceivedDetail> GoodsReceivedDetails { get; set; }

    public virtual DbSet<GoodsReceivedNote> GoodsReceivedNotes { get; set; }

    public virtual DbSet<GoodsReceivedStatus> GoodsReceivedStatuses { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<IngredientCategory> IngredientCategories { get; set; }

    public virtual DbSet<IngredientPurchaseOrder> IngredientPurchaseOrders { get; set; }

    public virtual DbSet<IngredientPurchaseOrderDetail> IngredientPurchaseOrderDetails { get; set; }

    public virtual DbSet<IngredientTransfer> IngredientTransfers { get; set; }

    public virtual DbSet<IngredientWarehouse> IngredientWarehouses { get; set; }

    public virtual DbSet<InvoiceStatus> InvoiceStatuses { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDeliveryTracking> OrderDeliveryTrackings { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderPayment> OrderPayments { get; set; }

    public virtual DbSet<OrderShipment> OrderShipments { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; }

    public virtual DbSet<Payroll> Payrolls { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductRecipe> ProductRecipes { get; set; }

    public virtual DbSet<ProfitSummary> ProfitSummaries { get; set; }

    public virtual DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }

    public virtual DbSet<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; set; }

    public virtual DbSet<PurchaseOrderStatus> PurchaseOrderStatuses { get; set; }

    public virtual DbSet<PurchaseReturn> PurchaseReturns { get; set; }

    public virtual DbSet<PurchaseReturnDetail> PurchaseReturnDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SalesSummary> SalesSummaries { get; set; }

    public virtual DbSet<ShippingProvider> ShippingProviders { get; set; }

    public virtual DbSet<SocialAccount> SocialAccounts { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierIngredientPrice> SupplierIngredientPrices { get; set; }

    public virtual DbSet<SupplierPerformance> SupplierPerformances { get; set; }

    public virtual DbSet<Taxis> Taxes { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VEmployeePayroll> VEmployeePayrolls { get; set; }

    public virtual DbSet<VExpensesSummary> VExpensesSummaries { get; set; }

    public virtual DbSet<VInventoryStatus> VInventoryStatuses { get; set; }

    public virtual DbSet<VProductsWithPrice> VProductsWithPrices { get; set; }

    public virtual DbSet<VProfitSummary> VProfitSummaries { get; set; }

    public virtual DbSet<VSalesSummary> VSalesSummaries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__branches__3213E83F4514DD9B");
        });

        modelBuilder.Entity<BranchExpense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__branch_e__3213E83FC6B11CF5");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PaymentCycle).HasDefaultValue("MONTHLY");

            entity.HasOne(d => d.Branch).WithMany(p => p.BranchExpenses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_branch_expenses_branches");
        });

        modelBuilder.Entity<BranchIngredientInventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__branch_i__3213E83FCC79BFE6");

            entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Branch).WithMany(p => p.BranchIngredientInventories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_bii_branch");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.BranchIngredientInventories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_bii_ingredient");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__categori__3213E83F081D2F09");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__customer__3213E83F99FE37C6");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Customer).HasConstraintName("FK_customers_users");
        });

        modelBuilder.Entity<DeliveryStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__delivery__3213E83F906BA1F6");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__employee__3213E83F2B6C67A0");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("ACTIVE");

            entity.HasOne(d => d.Branch).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employees_branch");
        });

        modelBuilder.Entity<EmployeeSalary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__employee__3213E83FD6680FE2");

            entity.Property(e => e.Allowance).HasDefaultValue(0.0m);
            entity.Property(e => e.Bonus).HasDefaultValue(0.0m);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Penalty).HasDefaultValue(0.0m);
            entity.Property(e => e.SalaryType).HasDefaultValue("MONTHLY");
            entity.Property(e => e.TaxRate).HasDefaultValue(0.1m);

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeSalaries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_salaries_employee");
        });

        modelBuilder.Entity<EmployeeShift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__employee__3213E83FD6C7B96C");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("PRESENT");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeShifts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_shifts_employee");
        });

        modelBuilder.Entity<ExpensesSummary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__expenses__3213E83F27D3985C");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Branch).WithMany(p => p.ExpensesSummaries).HasConstraintName("FK_expenses_summary_branches");
        });

        modelBuilder.Entity<GoodsReceivedDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__goods_re__3213E83FB45A7D74");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.QualityStatus).HasDefaultValue("ACCEPTED");
            entity.Property(e => e.RejectedQuantity).HasDefaultValue(0m);

            entity.HasOne(d => d.Grn).WithMany(p => p.GoodsReceivedDetails).HasConstraintName("FK_grn_detail_grn");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.GoodsReceivedDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_grn_detail_ingredient");
        });

        modelBuilder.Entity<GoodsReceivedNote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__goods_re__3213E83FC2694A22");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ReceivedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusId).HasDefaultValue(1L);
            entity.Property(e => e.TotalQuantityOrdered).HasDefaultValue(0m);
            entity.Property(e => e.TotalQuantityReceived).HasDefaultValue(0m);
            entity.Property(e => e.TotalQuantityRejected).HasDefaultValue(0m);

            entity.HasOne(d => d.Branch).WithMany(p => p.GoodsReceivedNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_grn_branch");

            entity.HasOne(d => d.Invoice).WithMany(p => p.GoodsReceivedNotes).HasConstraintName("FK_grn_invoice");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.GoodsReceivedNotes).HasConstraintName("FK_grn_purchase_order");

            entity.HasOne(d => d.Status).WithMany(p => p.GoodsReceivedNotes).HasConstraintName("FK_grn_status");

            entity.HasOne(d => d.Supplier).WithMany(p => p.GoodsReceivedNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_grn_supplier");

            entity.HasOne(d => d.WarehouseStaff).WithMany(p => p.GoodsReceivedNotes).HasConstraintName("FK_grn_warehouse_staff");
        });

        modelBuilder.Entity<GoodsReceivedStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__goods_re__3213E83F8F12AE54");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ingredie__3213E83FBDC4B752");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Category).WithMany(p => p.Ingredients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ingredients_ingredient_categories");

            entity.HasOne(d => d.Tax).WithMany(p => p.Ingredients).HasConstraintName("FK_ingredients_taxes");
        });

        modelBuilder.Entity<IngredientCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ingredie__3213E83F71C1F796");
        });

        modelBuilder.Entity<IngredientPurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ingredie__3213E83F117A1FF1");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DiscountAmount).HasDefaultValue(0m);
            entity.Property(e => e.FinalAmount).HasDefaultValue(0m);
            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusId).HasDefaultValue(1L);
            entity.Property(e => e.TotalAmountAfterTax).HasDefaultValue(0m);
            entity.Property(e => e.TotalAmountBeforeTax).HasDefaultValue(0m);
            entity.Property(e => e.TotalTaxAmount).HasDefaultValue(0m);

            entity.HasOne(d => d.Branch).WithMany(p => p.IngredientPurchaseOrders).HasConstraintName("FK_purchase_order_branch");

            entity.HasOne(d => d.Employee).WithMany(p => p.IngredientPurchaseOrders).HasConstraintName("FK_purchase_order_employee");

            entity.HasOne(d => d.Status).WithMany(p => p.IngredientPurchaseOrders).HasConstraintName("FK_purchase_order_status");

            entity.HasOne(d => d.Supplier).WithMany(p => p.IngredientPurchaseOrders).HasConstraintName("FK_purchase_order_supplier");
        });

        modelBuilder.Entity<IngredientPurchaseOrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ingredie__3213E83F1DCD047F");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.IngredientPurchaseOrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ipod_ingredients");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.IngredientPurchaseOrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ipod_purchase_orders");
        });

        modelBuilder.Entity<IngredientTransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ingredie__3213E83F69D4D00C");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Branch).WithMany(p => p.IngredientTransfers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transfer_branch");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.IngredientTransfers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transfer_ingredient");
        });

        modelBuilder.Entity<IngredientWarehouse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ingredie__3213E83FA666C6E1");

            entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.IngredientWarehouses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ingredient_warehouse");
        });

        modelBuilder.Entity<InvoiceStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__invoice___3213E83F06395854");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__orders__3213E83F663C9EB0");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.OrderUuid).IsFixedLength();

            entity.HasOne(d => d.Branch).WithMany(p => p.Orders).HasConstraintName("FK_orders_branches");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_orders_customers");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders).HasConstraintName("FK_orders_order_status");
        });

        modelBuilder.Entity<OrderDeliveryTracking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order_de__3213E83F5863CFF7");

            entity.Property(e => e.LastModified).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.DeliveryPerson).WithMany(p => p.OrderDeliveryTrackings).HasConstraintName("FK_tracking_employees");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDeliveryTrackings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tracking_orders");

            entity.HasOne(d => d.ShippingProvider).WithMany(p => p.OrderDeliveryTrackings).HasConstraintName("FK_tracking_providers");

            entity.HasOne(d => d.Status).WithMany(p => p.OrderDeliveryTrackings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tracking_status");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order_de__3213E83F4CE42D1D");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKjyu2qbqt8gnvno9oe9j2s2ldk");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK4q98utpd73imf4yhttm3w0eax");
        });

        modelBuilder.Entity<OrderPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order_pa__3213E83F1BEAD382");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderPayments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_order_payments_orders");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.OrderPayments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_order_payments_payment_methods");

            entity.HasOne(d => d.PaymentStatus).WithMany(p => p.OrderPayments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_order_payments_payment_statuses");
        });

        modelBuilder.Entity<OrderShipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order_sh__3213E83F6D88CAE2");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderShipments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_order_shipments_orders");

            entity.HasOne(d => d.ShippingProvider).WithMany(p => p.OrderShipments).HasConstraintName("FK_order_shipments_shipping_providers");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order_st__3213E83FB26D3DF0");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__payment___3213E83F9BB41367");
        });

        modelBuilder.Entity<PaymentStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__payment___3213E83F2EAECF1C");
        });

        modelBuilder.Entity<Payroll>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__payrolls__3213E83F432AAB89");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Employee).WithMany(p => p.Payrolls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_payroll_employee");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__products__3213E83FAB8FAA1E");

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("FK_products_categories");

            entity.HasOne(d => d.Tax).WithMany(p => p.Products).HasConstraintName("FK_products_taxes");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__product___3213E83FD8C6F7A9");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages).HasConstraintName("FKqnq71xsohugpqwf3c9gxmsuy");
        });

        modelBuilder.Entity<ProductRecipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__product___3213E83FA96E90A9");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.ProductRecipes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_recipe_ingredient");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductRecipes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_recipe_product");
        });

        modelBuilder.Entity<ProfitSummary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__profit_s__3213E83FBBCC3CFF");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Branch).WithMany(p => p.ProfitSummaries).HasConstraintName("FK_profit_summary_branches");
        });

        modelBuilder.Entity<PurchaseInvoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__purchase__3213E83F73ECDCDE");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DiscountAmount).HasDefaultValue(0m);
            entity.Property(e => e.PaidAmount).HasDefaultValue(0m);
            entity.Property(e => e.RemainingAmount).HasDefaultValue(0m);
            entity.Property(e => e.StatusId).HasDefaultValue(1L);
            entity.Property(e => e.TotalAmountAfterTax).HasDefaultValue(0m);
            entity.Property(e => e.TotalAmountBeforeTax).HasDefaultValue(0m);
            entity.Property(e => e.TotalTaxAmount).HasDefaultValue(0m);

            entity.HasOne(d => d.Branch).WithMany(p => p.PurchaseInvoices).HasConstraintName("FK_invoice_branch");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseInvoices).HasConstraintName("FK_invoice_purchase_order");

            entity.HasOne(d => d.Status).WithMany(p => p.PurchaseInvoices).HasConstraintName("FK_invoice_status");

            entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseInvoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_invoice_supplier");
        });

        modelBuilder.Entity<PurchaseInvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__purchase__3213E83F1FC51C06");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DiscountAmount).HasDefaultValue(0m);
            entity.Property(e => e.DiscountRate).HasDefaultValue(0m);
            entity.Property(e => e.TaxAmount).HasDefaultValue(0m);
            entity.Property(e => e.TaxRate).HasDefaultValue(0m);

            entity.HasOne(d => d.Ingredient).WithMany(p => p.PurchaseInvoiceDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_invoice_detail_ingredient");

            entity.HasOne(d => d.Invoice).WithMany(p => p.PurchaseInvoiceDetails).HasConstraintName("FK_invoice_detail_invoice");
        });

        modelBuilder.Entity<PurchaseOrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__purchase__3213E83FBD9D52EE");
        });

        modelBuilder.Entity<PurchaseReturn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__purchase__3213E83FD75D4F59");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.RefundAmount).HasDefaultValue(0m);
            entity.Property(e => e.ReturnDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusId).HasDefaultValue(1L);
            entity.Property(e => e.TotalReturnAmount).HasDefaultValue(0m);

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.PurchaseReturns).HasConstraintName("FK_return_approved_by");

            entity.HasOne(d => d.Branch).WithMany(p => p.PurchaseReturns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_return_branch");

            entity.HasOne(d => d.Grn).WithMany(p => p.PurchaseReturns).HasConstraintName("FK_return_grn");

            entity.HasOne(d => d.Invoice).WithMany(p => p.PurchaseReturns).HasConstraintName("FK_return_invoice");

            entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseReturns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_return_supplier");
        });

        modelBuilder.Entity<PurchaseReturnDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__purchase__3213E83FA0FB62AC");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.PurchaseReturnDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_return_detail_ingredient");

            entity.HasOne(d => d.Return).WithMany(p => p.PurchaseReturnDetails).HasConstraintName("FK_return_detail_return");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles__3213E83FDA1EB870");
        });

        modelBuilder.Entity<SalesSummary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sales_su__3213E83FCC378A81");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Branch).WithMany(p => p.SalesSummaries).HasConstraintName("FK_sales_summary_branches");
        });

        modelBuilder.Entity<ShippingProvider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__shipping__3213E83F7B4A3DDF");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<SocialAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__social_a__3213E83F4D3B725E");

            entity.HasOne(d => d.User).WithMany(p => p.SocialAccounts).HasConstraintName("FK6rmxxiton5yuvu7ph2hcq2xn7");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__supplier__3213E83FD72856BA");
        });

        modelBuilder.Entity<SupplierIngredientPrice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__supplier__3213E83FCBEFFDFF");

            entity.Property(e => e.EffectiveDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.SupplierIngredientPrices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_sip_ingredient");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierIngredientPrices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_sip_supplier");
        });

        modelBuilder.Entity<SupplierPerformance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__supplier__3213E83F241CD2A2");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LateDeliveries).HasDefaultValue(0);
            entity.Property(e => e.OnTimeDeliveries).HasDefaultValue(0);
            entity.Property(e => e.OverallRating).HasDefaultValue(0m);
            entity.Property(e => e.QualityScore).HasDefaultValue(0m);
            entity.Property(e => e.ReturnValue).HasDefaultValue(0m);
            entity.Property(e => e.ServiceScore).HasDefaultValue(0m);
            entity.Property(e => e.TotalAmount).HasDefaultValue(0m);
            entity.Property(e => e.TotalOrders).HasDefaultValue(0);
            entity.Property(e => e.TotalReturns).HasDefaultValue(0);

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierPerformances).HasConstraintName("FK_performance_supplier");
        });

        modelBuilder.Entity<Taxis>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__taxes__3213E83F92587024");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tokens__3213E83F23FD3269");

            entity.HasOne(d => d.User).WithMany(p => p.Tokens).HasConstraintName("FK2dylsfo39lgjyqml2tbe0b0ss");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83FDE9A3EE9");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Employee).WithMany(p => p.Users).HasConstraintName("FK_users_employees");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_users_roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
