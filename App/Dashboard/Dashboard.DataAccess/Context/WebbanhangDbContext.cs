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

    public virtual DbSet<Ingredient> Ingredients { get; set; }
    public virtual DbSet<IngredientCategory> IngredientCategories { get; set; }

    public virtual DbSet<IngredientPurchaseOrder> IngredientPurchaseOrders { get; set; }

    public virtual DbSet<IngredientPurchaseOrderDetail> IngredientPurchaseOrderDetails { get; set; }

    public virtual DbSet<IngredientTransfer> IngredientTransfers { get; set; }

    public virtual DbSet<IngredientWarehouse> IngredientWarehouses { get; set; }

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

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SalesSummary> SalesSummaries { get; set; }

    public virtual DbSet<ShippingProvider> ShippingProviders { get; set; }

    public virtual DbSet<SocialAccount> SocialAccounts { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierIngredientPrice> SupplierIngredientPrices { get; set; }

    public virtual DbSet<Taxes> Taxes { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    // Views
    public virtual DbSet<SalesSummaryView> SalesSummaryViews { get; set; }
    
    public virtual DbSet<ExpensesSummaryView> ExpensesSummaryViews { get; set; }
    
    public virtual DbSet<ProfitSummaryView> ProfitSummaryViews { get; set; }
    
    public virtual DbSet<InventoryStatusView> InventoryStatusViews { get; set; }
    
    public virtual DbSet<EmployeePayrollView> EmployeePayrollViews { get; set; }
    
    public virtual DbSet<ProductWithPricesView> ProductWithPricesViews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__branches__3213E83F228C5BCB");
        });

        modelBuilder.Entity<BranchExpense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__branch_e__3213E83F9C2D1119");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PaymentCycle).HasDefaultValue("MONTHLY");

            entity.HasOne(d => d.Branch).WithMany(p => p.BranchExpenses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_branch_expenses_branches");
        });

        modelBuilder.Entity<BranchIngredientInventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__branch_i__3213E83F4191B3B5");

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
            entity.HasKey(e => e.Id).HasName("PK__categori__3213E83F650E2080");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__customer__3213E83F7FAC9DFD");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.User).WithMany(p => p.Customers).HasConstraintName("FK_customers_users");
        });

        modelBuilder.Entity<DeliveryStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__delivery__3213E83FA692EDBC");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__employee__3213E83F62A64C81");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("ACTIVE");

            entity.HasOne(d => d.Branch).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employees_branch");
        });

        modelBuilder.Entity<EmployeeSalary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__employee__3213E83F55211C0A");

            entity.Property(e => e.Allowance).HasDefaultValue(0m);
            entity.Property(e => e.Bonus).HasDefaultValue(0m);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Penalty).HasDefaultValue(0m);
            entity.Property(e => e.SalaryType).HasDefaultValue("MONTHLY");
            entity.Property(e => e.TaxRate).HasDefaultValue(0.1m);

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeSalaries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_salaries_employee");
        });

        modelBuilder.Entity<EmployeeShift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__employee__3213E83FADEFFA2B");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("PRESENT");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeShifts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_shifts_employee");
        });

        modelBuilder.Entity<ExpensesSummary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__expenses__3213E83F847BC26B");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Branch).WithMany(p => p.ExpensesSummaries)
                .HasConstraintName("FK_expenses_summary_branches");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ingredie__3213E83F3CE86B2E");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Tax).WithMany(p => p.Ingredients).HasConstraintName("FK_ingredients_taxes");

            entity.HasOne(d => d.Category).WithMany(p => p.Ingredients)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ingredients_ingredient_categories");
        });

        modelBuilder.Entity<IngredientCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ingredie__3213E83F6A754175");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50).IsUnicode(true);
            entity.Property(e => e.Description).HasMaxLength(255).IsUnicode(true);
        });

        modelBuilder.Entity<IngredientPurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ingredie__3213E83F762D28A7");

            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Supplier).WithMany(p => p.IngredientPurchaseOrders).HasConstraintName("FK_purchase_order_supplier");
        });

        modelBuilder.Entity<IngredientPurchaseOrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ingredie__3213E83FD6072415");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.IngredientPurchaseOrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ipod_ingredients");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.IngredientPurchaseOrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ipod_purchase_orders");
        });

        modelBuilder.Entity<IngredientTransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ingredie__3213E83F4CF0E1E6");

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
            entity.HasKey(e => e.Id).HasName("PK__ingredie__3213E83F724F0268");

            entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.IngredientWarehouses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ingredient_warehouse");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__orders__3213E83F9CC372E1");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Branch).WithMany(p => p.Orders).HasConstraintName("FK_orders_branches");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_orders_customers");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasConstraintName("FK_orders_order_status");
        });

        modelBuilder.Entity<OrderDeliveryTracking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order_de__3213E83FE5070075");

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
            entity.HasKey(e => e.Id).HasName("PK__order_de__3213E83F619D82EA");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKjyu2qbqt8gnvno9oe9j2s2ldk");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK4q98utpd73imf4yhttm3w0eax");
        });

        modelBuilder.Entity<OrderPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order_pa__3213E83F8B2AD3E1");

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
            entity.HasKey(e => e.Id).HasName("PK__order_sh__3213E83F9A3C5D2F");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderShipments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_order_shipments_orders");

            entity.HasOne(d => d.ShippingProvider).WithMany(p => p.OrderShipments)
                .HasConstraintName("FK_order_shipments_shipping_providers");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order_st__3213E83F5808452C");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__payment___3213E83F30F0438C");
        });

        modelBuilder.Entity<PaymentStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__payment___3213E83F1777CE46");
        });

        modelBuilder.Entity<Payroll>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__payrolls__3213E83FF57EF7CF");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Employee).WithMany(p => p.Payrolls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_payroll_employee");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__products__3213E83F6A754175");

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("FK_products_categories");

            entity.HasOne(d => d.Tax).WithMany(p => p.Products).HasConstraintName("FK_products_taxes");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__product___3213E83F711F91E7");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages).HasConstraintName("FKqnq71xsohugpqwf3c9gxmsuy");
        });

        modelBuilder.Entity<ProductRecipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__product___3213E83FBB161C8F");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.ProductRecipes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_recipe_ingredient");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductRecipes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_recipe_product");
        });

        modelBuilder.Entity<ProfitSummary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__profit_s__3213E83FB3B36FC9");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Branch).WithMany(p => p.ProfitSummaries)
                .HasConstraintName("FK_profit_summary_branches");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles__3213E83F1579A6F1");
        });

        modelBuilder.Entity<SalesSummary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sales_su__3213E83F44A784A9");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Branch).WithMany(p => p.SalesSummaries)
                .HasConstraintName("FK_sales_summary_branches");
        });

        modelBuilder.Entity<ShippingProvider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__shipping__3213E83FA8D871A4");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<SocialAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__social_a__3213E83F2A02110D");

            entity.HasOne(d => d.User).WithMany(p => p.SocialAccounts).HasConstraintName("FK6rmxxiton5yuvu7ph2hcq2xn7");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__supplier__3213E83FC084036E");
        });

        modelBuilder.Entity<SupplierIngredientPrice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__supplier__3213E83F441DA26C");

            entity.Property(e => e.EffectiveDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.SupplierIngredientPrices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_sip_ingredient");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierIngredientPrices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_sip_supplier");
        });

        modelBuilder.Entity<Taxes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__taxes__3213E83FACB47953");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tokens__3213E83F593BCD93");

            entity.HasOne(d => d.User).WithMany(p => p.Tokens).HasConstraintName("FK2dylsfo39lgjyqml2tbe0b0ss");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F62990E3B");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Employee).WithMany(p => p.Users).HasConstraintName("FK_users_employees");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_users_roles");
        });

        // Configure Views (no primary keys, read-only)
        modelBuilder.Entity<SalesSummaryView>(entity =>
        {
            entity.ToView("v_sales_summary");
            entity.HasNoKey();
        });

        modelBuilder.Entity<ExpensesSummaryView>(entity =>
        {
            entity.ToView("v_expenses_summary");
            entity.HasNoKey();
        });

        modelBuilder.Entity<ProfitSummaryView>(entity =>
        {
            entity.ToView("v_profit_summary");
            entity.HasNoKey();
        });

        modelBuilder.Entity<InventoryStatusView>(entity =>
        {
            entity.ToView("v_inventory_status");
            entity.HasNoKey();
        });

        modelBuilder.Entity<EmployeePayrollView>(entity =>
        {
            entity.ToView("v_employee_payroll");
            entity.HasNoKey();
        });

        modelBuilder.Entity<ProductWithPricesView>(entity =>
        {
            entity.ToView("v_products_with_prices");
            entity.HasNoKey();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
