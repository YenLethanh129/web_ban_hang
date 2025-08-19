using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "branches",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    address = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    manager = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__branches__3213E83F228C5BCB", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__categori__3213E83F650E2080", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "delivery_statuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__delivery__3213E83FA692EDBC", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "expenses_summary",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    branch_id = table.Column<long>(type: "bigint", nullable: true),
                    period_type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    period_value = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    total_purchase_orders = table.Column<int>(type: "int", nullable: false),
                    total_ingredients = table.Column<int>(type: "int", nullable: false),
                    expense_before_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    expense_after_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tax_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__expenses__3213E83F847BC26B", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "IngredientCategories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ingredie__3213E83F6A754175", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_statuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__order_st__3213E83F5808452C", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment_methods",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__payment___3213E83F30F0438C", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment_statuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__payment___3213E83F1777CE46", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "profit_summary",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    branch_id = table.Column<long>(type: "bigint", nullable: true),
                    period_type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    period_value = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    revenue_before_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    revenue_after_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    expense_before_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    expense_after_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    output_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    input_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    vat_to_pay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    profit_before_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    profit_after_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__profit_s__3213E83FB3B36FC9", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__roles__3213E83F1579A6F1", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sales_summary",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    branch_id = table.Column<long>(type: "bigint", nullable: true),
                    period_type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    period_value = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    total_orders = table.Column<int>(type: "int", nullable: false),
                    total_products = table.Column<int>(type: "int", nullable: false),
                    revenue_before_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    revenue_after_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tax_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__sales_su__3213E83F44A784A9", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shipping_providers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    contact_info = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    api_endpoint = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(sysdatetime())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__shipping__3213E83FA8D871A4", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    note = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__supplier__3213E83FC084036E", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "taxes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    tax_rate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__taxes__3213E83FACB47953", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "branch_expenses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    branch_id = table.Column<long>(type: "bigint", nullable: false),
                    expense_type = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    payment_cycle = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, defaultValue: "MONTHLY"),
                    note = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__branch_e__3213E83F9C2D1119", x => x.id);
                    table.ForeignKey(
                        name: "FK_branch_expenses_branches",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    branch_id = table.Column<long>(type: "bigint", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    position = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    hire_date = table.Column<DateOnly>(type: "date", nullable: false),
                    resign_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true, defaultValue: "ACTIVE"),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(sysdatetime())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__employee__3213E83F62A64C81", x => x.id);
                    table.ForeignKey(
                        name: "FK_employees_branch",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ingredient_purchase_orders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplier_id = table.Column<long>(type: "bigint", nullable: true),
                    order_date = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true, defaultValueSql: "(getdate())"),
                    total_money = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    note = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ingredie__3213E83F762D28A7", x => x.id);
                    table.ForeignKey(
                        name: "FK_purchase_order_supplier",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ingredients",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    unit = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    tax_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ingredie__3213E83F3CE86B2E", x => x.id);
                    table.ForeignKey(
                        name: "FK_ingredients_IngredientCategories_category_id",
                        column: x => x.category_id,
                        principalTable: "IngredientCategories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ingredients_taxes",
                        column: x => x.tax_id,
                        principalTable: "taxes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    category_id = table.Column<long>(type: "bigint", nullable: true),
                    tax_id = table.Column<long>(type: "bigint", nullable: true),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    thumbnail = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__products__3213E83F6A754175", x => x.id);
                    table.ForeignKey(
                        name: "FK_products_categories",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_products_taxes",
                        column: x => x.tax_id,
                        principalTable: "taxes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "employee_salaries",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    base_salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    salary_type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true, defaultValue: "MONTHLY"),
                    allowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    bonus = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    penalty = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    tax_rate = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0.1m),
                    effective_date = table.Column<DateOnly>(type: "date", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__employee__3213E83F55211C0A", x => x.id);
                    table.ForeignKey(
                        name: "FK_salaries_employee",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "employee_shifts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    shift_date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true, defaultValue: "PRESENT"),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__employee__3213E83FADEFFA2B", x => x.id);
                    table.ForeignKey(
                        name: "FK_shifts_employee",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "payrolls",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    month = table.Column<int>(type: "int", nullable: false),
                    year = table.Column<int>(type: "int", nullable: false),
                    total_working_hours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    base_salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    allowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    bonus = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    penalty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    gross_salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    tax_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    net_salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__payrolls__3213E83FF57EF7CF", x => x.id);
                    table.ForeignKey(
                        name: "FK_payroll_employee",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employee_id = table.Column<long>(type: "bigint", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    facebook_account_id = table.Column<long>(type: "bigint", nullable: true),
                    google_account_id = table.Column<long>(type: "bigint", nullable: true),
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    password = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(sysdatetime())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__3213E83F62990E3B", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_employees",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_users_roles",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "branch_ingredient_inventory",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    branch_id = table.Column<long>(type: "bigint", nullable: false),
                    ingredient_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__branch_i__3213E83F4191B3B5", x => x.id);
                    table.ForeignKey(
                        name: "FK_bii_branch",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_bii_ingredient",
                        column: x => x.ingredient_id,
                        principalTable: "ingredients",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ingredient_purchase_order_details",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    purchase_order_id = table.Column<long>(type: "bigint", nullable: false),
                    ingredient_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tax_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ingredie__3213E83FD6072415", x => x.id);
                    table.ForeignKey(
                        name: "FK_ipod_ingredients",
                        column: x => x.ingredient_id,
                        principalTable: "ingredients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ipod_purchase_orders",
                        column: x => x.purchase_order_id,
                        principalTable: "ingredient_purchase_orders",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ingredient_transfers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ingredient_id = table.Column<long>(type: "bigint", nullable: false),
                    branch_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    note = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ingredie__3213E83F4CF0E1E6", x => x.id);
                    table.ForeignKey(
                        name: "FK_transfer_branch",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_transfer_ingredient",
                        column: x => x.ingredient_id,
                        principalTable: "ingredients",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ingredient_warehouse",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ingredient_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ingredie__3213E83F724F0268", x => x.id);
                    table.ForeignKey(
                        name: "FK_ingredient_warehouse",
                        column: x => x.ingredient_id,
                        principalTable: "ingredients",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "supplier_ingredient_prices",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplier_id = table.Column<long>(type: "bigint", nullable: false),
                    ingredient_id = table.Column<long>(type: "bigint", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    unit = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    effective_date = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true, defaultValueSql: "(getdate())"),
                    expired_date = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__supplier__3213E83F441DA26C", x => x.id);
                    table.ForeignKey(
                        name: "FK_sip_ingredient",
                        column: x => x.ingredient_id,
                        principalTable: "ingredients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_sip_supplier",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product_images",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<long>(type: "bigint", nullable: true),
                    image_url = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__product___3213E83F711F91E7", x => x.id);
                    table.ForeignKey(
                        name: "FKqnq71xsohugpqwf3c9gxmsuy",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product_recipes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    ingredient_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__product___3213E83FBB161C8F", x => x.id);
                    table.ForeignKey(
                        name: "FK_recipe_ingredient",
                        column: x => x.ingredient_id,
                        principalTable: "ingredients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_recipe_product",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    fullname = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(sysdatetime())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__customer__3213E83F7FAC9DFD", x => x.id);
                    table.ForeignKey(
                        name: "FK_customers_users",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "social_accounts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    provider_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    provider = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__social_a__3213E83F2A02110D", x => x.id);
                    table.ForeignKey(
                        name: "FK6rmxxiton5yuvu7ph2hcq2xn7",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tokens",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    expired = table.Column<bool>(type: "bit", nullable: false),
                    revoked = table.Column<bool>(type: "bit", nullable: false),
                    expiration_date = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    token_type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    token = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tokens__3213E83F593BCD93", x => x.id);
                    table.ForeignKey(
                        name: "FK2dylsfo39lgjyqml2tbe0b0ss",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<long>(type: "bigint", nullable: false),
                    branch_id = table.Column<long>(type: "bigint", nullable: true),
                    total_money = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    shipping_date = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    note = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    payment_method_id = table.Column<long>(type: "bigint", nullable: true),
                    payment_status_id = table.Column<long>(type: "bigint", nullable: true),
                    status_id = table.Column<long>(type: "bigint", nullable: true),
                    shipping_address = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    shipping_method = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(sysdatetime())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__orders__3213E83F9CC372E1", x => x.id);
                    table.ForeignKey(
                        name: "FK_orders_branches",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_orders_customers",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order_delivery_tracking",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<long>(type: "bigint", nullable: false),
                    tracking_number = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    status_id = table.Column<long>(type: "bigint", nullable: false),
                    location = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    estimated_delivery = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    delivery_person_id = table.Column<long>(type: "bigint", nullable: true),
                    shipping_provider_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__order_de__3213E83FE5070075", x => x.id);
                    table.ForeignKey(
                        name: "FK_tracking_employees",
                        column: x => x.delivery_person_id,
                        principalTable: "employees",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tracking_orders",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tracking_providers",
                        column: x => x.shipping_provider_id,
                        principalTable: "shipping_providers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tracking_status",
                        column: x => x.status_id,
                        principalTable: "delivery_statuses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order_details",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    number_of_products = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<float>(type: "real", nullable: false),
                    total_money = table.Column<float>(type: "real", nullable: false),
                    order_id = table.Column<long>(type: "bigint", nullable: true),
                    product_id = table.Column<long>(type: "bigint", nullable: true),
                    color = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__order_de__3213E83F619D82EA", x => x.id);
                    table.ForeignKey(
                        name: "FK4q98utpd73imf4yhttm3w0eax",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FKjyu2qbqt8gnvno9oe9j2s2ldk",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_branch_expenses_branch_id",
                table: "branch_expenses",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_branch_ingredient_inventory_branch_id",
                table: "branch_ingredient_inventory",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_branch_ingredient_inventory_ingredient_id",
                table: "branch_ingredient_inventory",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_salaries_employee_id",
                table: "employee_salaries",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_shifts_employee_id",
                table: "employee_shifts",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_branch_id",
                table: "employees",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_purchase_order_details_ingredient_id",
                table: "ingredient_purchase_order_details",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_purchase_order_details_purchase_order_id",
                table: "ingredient_purchase_order_details",
                column: "purchase_order_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_purchase_orders_supplier_id",
                table: "ingredient_purchase_orders",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_transfers_branch_id",
                table: "ingredient_transfers",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_transfers_ingredient_id",
                table: "ingredient_transfers",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_warehouse_ingredient_id",
                table: "ingredient_warehouse",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredients_category_id",
                table: "ingredients",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredients_tax_id",
                table: "ingredients",
                column: "tax_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_delivery_tracking_delivery_person_id",
                table: "order_delivery_tracking",
                column: "delivery_person_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_delivery_tracking_order_id",
                table: "order_delivery_tracking",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_delivery_tracking_shipping_provider_id",
                table: "order_delivery_tracking",
                column: "shipping_provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_delivery_tracking_status_id",
                table: "order_delivery_tracking",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_details_order_id",
                table: "order_details",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_details_product_id",
                table: "order_details",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_branch_id",
                table: "orders",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_customer_id",
                table: "orders",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_payrolls_employee_id",
                table: "payrolls",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_images_product_id",
                table: "product_images",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_recipes_ingredient_id",
                table: "product_recipes",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_recipes_product_id",
                table: "product_recipes",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_category_id",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_tax_id",
                table: "products",
                column: "tax_id");

            migrationBuilder.CreateIndex(
                name: "IX_social_accounts_user_id",
                table: "social_accounts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_supplier_ingredient_prices_ingredient_id",
                table: "supplier_ingredient_prices",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_supplier_ingredient_prices_supplier_id",
                table: "supplier_ingredient_prices",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_tokens_user_id",
                table: "tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_employee_id",
                table: "users",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "branch_expenses");

            migrationBuilder.DropTable(
                name: "branch_ingredient_inventory");

            migrationBuilder.DropTable(
                name: "employee_salaries");

            migrationBuilder.DropTable(
                name: "employee_shifts");

            migrationBuilder.DropTable(
                name: "expenses_summary");

            migrationBuilder.DropTable(
                name: "ingredient_purchase_order_details");

            migrationBuilder.DropTable(
                name: "ingredient_transfers");

            migrationBuilder.DropTable(
                name: "ingredient_warehouse");

            migrationBuilder.DropTable(
                name: "order_delivery_tracking");

            migrationBuilder.DropTable(
                name: "order_details");

            migrationBuilder.DropTable(
                name: "order_statuses");

            migrationBuilder.DropTable(
                name: "payment_methods");

            migrationBuilder.DropTable(
                name: "payment_statuses");

            migrationBuilder.DropTable(
                name: "payrolls");

            migrationBuilder.DropTable(
                name: "product_images");

            migrationBuilder.DropTable(
                name: "product_recipes");

            migrationBuilder.DropTable(
                name: "profit_summary");

            migrationBuilder.DropTable(
                name: "sales_summary");

            migrationBuilder.DropTable(
                name: "social_accounts");

            migrationBuilder.DropTable(
                name: "supplier_ingredient_prices");

            migrationBuilder.DropTable(
                name: "tokens");

            migrationBuilder.DropTable(
                name: "ingredient_purchase_orders");

            migrationBuilder.DropTable(
                name: "shipping_providers");

            migrationBuilder.DropTable(
                name: "delivery_statuses");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "ingredients");

            migrationBuilder.DropTable(
                name: "suppliers");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "IngredientCategories");

            migrationBuilder.DropTable(
                name: "taxes");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "branches");
        }
    }
}
