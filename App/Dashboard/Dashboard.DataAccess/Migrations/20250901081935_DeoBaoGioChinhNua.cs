using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DeoBaoGioChinhNua : Migration
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
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    manager = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__branches__3213E83F4514DD9B", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__categori__3213E83F081D2F09", x => x.id);
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
                    table.PrimaryKey("PK__delivery__3213E83F906BA1F6", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "goods_received_statuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__goods_re__3213E83F8F12AE54", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ingredient_categories",
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
                    table.PrimaryKey("PK__ingredie__3213E83F71C1F796", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "invoice_statuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__invoice___3213E83F06395854", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_statuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__order_st__3213E83FB26D3DF0", x => x.id);
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
                    table.PrimaryKey("PK__payment___3213E83F9BB41367", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment_statuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__payment___3213E83F2EAECF1C", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "purchase_order_statuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__purchase__3213E83FBD9D52EE", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__roles__3213E83FDA1EB870", x => x.id);
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
                    table.PrimaryKey("PK__shipping__3213E83F7B4A3DDF", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    note = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__supplier__3213E83FD72856BA", x => x.id);
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
                    table.PrimaryKey("PK__taxes__3213E83F92587024", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "branch_expenses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    branch_id = table.Column<long>(type: "bigint", nullable: false),
                    expense_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    payment_cycle = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, defaultValue: "MONTHLY"),
                    note = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__branch_e__3213E83FC6B11CF5", x => x.id);
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
                    position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    hire_date = table.Column<DateOnly>(type: "date", nullable: false),
                    resign_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true, defaultValue: "ACTIVE"),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(sysdatetime())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__employee__3213E83F2B6C67A0", x => x.id);
                    table.ForeignKey(
                        name: "FK_employees_branch",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id");
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
                    table.PrimaryKey("PK__expenses__3213E83F27D3985C", x => x.id);
                    table.ForeignKey(
                        name: "FK_expenses_summary_branches",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ingredient_transfer_requests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    branch_id = table.Column<long>(type: "bigint", nullable: false),
                    request_number = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    request_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    required_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    total_items = table.Column<int>(type: "int", nullable: false),
                    approved_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    completed_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    requested_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    approved_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredient_transfer_requests", x => x.id);
                    table.ForeignKey(
                        name: "FK_ingredient_transfer_requests_branches_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.PrimaryKey("PK__profit_s__3213E83FBBCC3CFF", x => x.id);
                    table.ForeignKey(
                        name: "FK_profit_summary_branches",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id");
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
                    table.PrimaryKey("PK__sales_su__3213E83FCC378A81", x => x.id);
                    table.ForeignKey(
                        name: "FK_sales_summary_branches",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "v_expenses_summary",
                columns: table => new
                {
                    branch_id = table.Column<long>(type: "bigint", nullable: true),
                    year = table.Column<int>(type: "int", nullable: false),
                    month = table.Column<int>(type: "int", nullable: false),
                    period = table.Column<string>(type: "varchar(7)", unicode: false, maxLength: 7, nullable: false),
                    total_purchase_orders = table.Column<int>(type: "int", nullable: false),
                    total_ingredients = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    expense_before_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    expense_after_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tax_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_v_expenses_summary_branches_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "v_profit_summary",
                columns: table => new
                {
                    branch_id = table.Column<long>(type: "bigint", nullable: true),
                    year = table.Column<int>(type: "int", nullable: false),
                    month = table.Column<int>(type: "int", nullable: false),
                    period = table.Column<string>(type: "varchar(7)", unicode: false, maxLength: 7, nullable: false),
                    revenue_before_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    revenue_after_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    expense_before_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    expense_after_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    output_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    input_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    vat_to_pay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    profit_before_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    profit_after_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_v_profit_summary_branches_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "v_sales_summary",
                columns: table => new
                {
                    branch_id = table.Column<long>(type: "bigint", nullable: true),
                    year = table.Column<int>(type: "int", nullable: false),
                    month = table.Column<int>(type: "int", nullable: false),
                    period = table.Column<string>(type: "varchar(7)", unicode: false, maxLength: 7, nullable: false),
                    total_orders = table.Column<int>(type: "int", nullable: false),
                    total_products = table.Column<int>(type: "int", nullable: false),
                    revenue_before_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    revenue_after_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tax_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_v_sales_summary_branches_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "supplier_performance",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplier_id = table.Column<long>(type: "bigint", nullable: false),
                    evaluation_period = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    period_value = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    total_orders = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    on_time_deliveries = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    late_deliveries = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    quality_score = table.Column<decimal>(type: "decimal(3,2)", nullable: true, defaultValue: 0m),
                    service_score = table.Column<decimal>(type: "decimal(3,2)", nullable: true, defaultValue: 0m),
                    overall_rating = table.Column<decimal>(type: "decimal(3,2)", nullable: true, defaultValue: 0m),
                    total_returns = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    return_value = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    comments = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__supplier__3213E83F241CD2A2", x => x.id);
                    table.ForeignKey(
                        name: "FK_performance_supplier",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ingredients",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    tax_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ingredie__3213E83FBDC4B752", x => x.id);
                    table.ForeignKey(
                        name: "FK_ingredients_ingredient_categories",
                        column: x => x.category_id,
                        principalTable: "ingredient_categories",
                        principalColumn: "id");
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
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    tax_id = table.Column<long>(type: "bigint", nullable: true),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    thumbnail = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__products__3213E83FAB8FAA1E", x => x.id);
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
                    allowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0.0m),
                    bonus = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0.0m),
                    penalty = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0.0m),
                    tax_rate = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0.1m),
                    effective_date = table.Column<DateOnly>(type: "date", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__employee__3213E83FD6680FE2", x => x.id);
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
                    table.PrimaryKey("PK__employee__3213E83FD6C7B96C", x => x.id);
                    table.ForeignKey(
                        name: "FK_shifts_employee",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ingredient_purchase_orders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    purchase_order_code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    supplier_id = table.Column<long>(type: "bigint", nullable: true),
                    branch_id = table.Column<long>(type: "bigint", nullable: true),
                    employee_id = table.Column<long>(type: "bigint", nullable: true),
                    order_date = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true, defaultValueSql: "(getdate())"),
                    expected_delivery_date = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    status_id = table.Column<long>(type: "bigint", nullable: true, defaultValue: 1L),
                    total_amount_before_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    total_tax_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    total_amount_after_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    discount_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    final_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    note = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ingredie__3213E83F117A1FF1", x => x.id);
                    table.ForeignKey(
                        name: "FK_purchase_order_branch",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_purchase_order_employee",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_purchase_order_status",
                        column: x => x.status_id,
                        principalTable: "purchase_order_statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_purchase_order_supplier",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
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
                    table.PrimaryKey("PK__payrolls__3213E83F432AAB89", x => x.id);
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
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    facebook_account_id = table.Column<long>(type: "bigint", nullable: true),
                    google_account_id = table.Column<long>(type: "bigint", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK__users__3213E83FDE9A3EE9", x => x.id);
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
                name: "v_employee_payroll",
                columns: table => new
                {
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    branch_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    position_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    base_salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    salary_type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    total_allowances = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_bonus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_deductions = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    gross_salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    effective_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_v_employee_payroll_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    reserved_quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    safety_stock = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    last_transfer_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__branch_i__3213E83FCC79BFE6", x => x.id);
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
                name: "ingredient_transfer_request_details",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    transfer_request_id = table.Column<long>(type: "bigint", nullable: false),
                    ingredient_id = table.Column<long>(type: "bigint", nullable: false),
                    requested_quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    approved_quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    transferred_quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    note = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredient_transfer_request_details", x => x.id);
                    table.ForeignKey(
                        name: "FK_ingredient_transfer_request_details_ingredient_transfer_requests_transfer_request_id",
                        column: x => x.transfer_request_id,
                        principalTable: "ingredient_transfer_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ingredient_transfer_request_details_ingredients_ingredient_id",
                        column: x => x.ingredient_id,
                        principalTable: "ingredients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    transfer_type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    transfer_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    completed_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    requested_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    approved_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ingredie__3213E83F69D4D00C", x => x.id);
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
                    safety_stock = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    maximum_stock = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ingredie__3213E83FA666C6E1", x => x.id);
                    table.ForeignKey(
                        name: "FK_ingredient_warehouse",
                        column: x => x.ingredient_id,
                        principalTable: "ingredients",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "inventory_movements",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    IngredientId = table.Column<long>(type: "bigint", nullable: false),
                    MovementType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QuantityBefore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReferenceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReferenceId = table.Column<long>(type: "bigint", nullable: true),
                    ReferenceCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    MovementDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_movements", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventory_movements_branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventory_movements_employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_inventory_movements_ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "ingredients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_thresholds",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    IngredientId = table.Column<long>(type: "bigint", nullable: false),
                    MinimumStock = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReorderPoint = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumStock = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SafetyStock = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LeadTimeDays = table.Column<int>(type: "int", nullable: false),
                    AverageDailyConsumption = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastCalculatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_thresholds", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventory_thresholds_branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventory_thresholds_ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "ingredients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.PrimaryKey("PK__supplier__3213E83FCBEFFDFF", x => x.id);
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
                name: "v_inventory_status",
                columns: table => new
                {
                    ingredient_id = table.Column<long>(type: "bigint", nullable: false),
                    ingredient_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    location_id = table.Column<long>(type: "bigint", nullable: false),
                    location_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    branch_id = table.Column<long>(type: "bigint", nullable: false),
                    branch_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    quantity_on_hand = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    quantity_reserved = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    available_quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    safety_stock = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    stock_status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    unit_of_measure = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    last_updated = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_v_inventory_status_branches_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_v_inventory_status_ingredients_ingredient_id",
                        column: x => x.ingredient_id,
                        principalTable: "ingredients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.PrimaryKey("PK__product___3213E83FD8C6F7A9", x => x.id);
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
                    table.PrimaryKey("PK__product___3213E83FA96E90A9", x => x.id);
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
                name: "recipes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ServingSize = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipes", x => x.id);
                    table.ForeignKey(
                        name: "FK_recipes_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.PrimaryKey("PK__ingredie__3213E83F1DCD047F", x => x.id);
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
                name: "purchase_invoices",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    invoice_code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    purchase_order_id = table.Column<long>(type: "bigint", nullable: true),
                    supplier_id = table.Column<long>(type: "bigint", nullable: false),
                    branch_id = table.Column<long>(type: "bigint", nullable: true),
                    invoice_date = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    due_date = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    payment_date = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    status_id = table.Column<long>(type: "bigint", nullable: true, defaultValue: 1L),
                    total_amount_before_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    total_tax_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    total_amount_after_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    paid_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    remaining_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    discount_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    payment_method = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    payment_reference = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    note = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__purchase__3213E83F73ECDCDE", x => x.id);
                    table.ForeignKey(
                        name: "FK_invoice_branch",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_invoice_purchase_order",
                        column: x => x.purchase_order_id,
                        principalTable: "ingredient_purchase_orders",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_invoice_status",
                        column: x => x.status_id,
                        principalTable: "invoice_statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_invoice_supplier",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(sysdatetime())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__customer__3213E83F99FE37C6", x => x.id);
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
                    table.PrimaryKey("PK__social_a__3213E83F4D3B725E", x => x.id);
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
                    table.PrimaryKey("PK__tokens__3213E83F23FD3269", x => x.id);
                    table.ForeignKey(
                        name: "FK2dylsfo39lgjyqml2tbe0b0ss",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "recipe_ingredients",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<long>(type: "bigint", nullable: false),
                    IngredientId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WastePercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsOptional = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipe_ingredients", x => x.id);
                    table.ForeignKey(
                        name: "FK_recipe_ingredients_ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "ingredients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_recipe_ingredients_recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "recipes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "goods_received_notes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    grn_code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    purchase_order_id = table.Column<long>(type: "bigint", nullable: true),
                    invoice_id = table.Column<long>(type: "bigint", nullable: true),
                    supplier_id = table.Column<long>(type: "bigint", nullable: false),
                    branch_id = table.Column<long>(type: "bigint", nullable: false),
                    warehouse_staff_id = table.Column<long>(type: "bigint", nullable: true),
                    received_date = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true, defaultValueSql: "(getdate())"),
                    status_id = table.Column<long>(type: "bigint", nullable: true, defaultValue: 1L),
                    total_quantity_ordered = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    total_quantity_received = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    total_quantity_rejected = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    delivery_note_number = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    vehicle_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    driver_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    note = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__goods_re__3213E83FC2694A22", x => x.id);
                    table.ForeignKey(
                        name: "FK_grn_branch",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_grn_invoice",
                        column: x => x.invoice_id,
                        principalTable: "purchase_invoices",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_grn_purchase_order",
                        column: x => x.purchase_order_id,
                        principalTable: "ingredient_purchase_orders",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_grn_status",
                        column: x => x.status_id,
                        principalTable: "goods_received_statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_grn_supplier",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_grn_warehouse_staff",
                        column: x => x.warehouse_staff_id,
                        principalTable: "employees",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "purchase_invoice_details",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    invoice_id = table.Column<long>(type: "bigint", nullable: false),
                    ingredient_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    amount_before_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tax_rate = table.Column<decimal>(type: "decimal(5,2)", nullable: true, defaultValue: 0m),
                    tax_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    amount_after_tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    discount_rate = table.Column<decimal>(type: "decimal(5,2)", nullable: true, defaultValue: 0m),
                    discount_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    final_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    expiry_date = table.Column<DateOnly>(type: "date", nullable: true),
                    batch_number = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    note = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__purchase__3213E83F1FC51C06", x => x.id);
                    table.ForeignKey(
                        name: "FK_invoice_detail_ingredient",
                        column: x => x.ingredient_id,
                        principalTable: "ingredients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_invoice_detail_invoice",
                        column: x => x.invoice_id,
                        principalTable: "purchase_invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_uuid = table.Column<string>(type: "char(36)", unicode: false, fixedLength: true, maxLength: 36, nullable: false),
                    order_code = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    customer_id = table.Column<long>(type: "bigint", nullable: false),
                    branch_id = table.Column<long>(type: "bigint", nullable: true),
                    total_money = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    status_id = table.Column<long>(type: "bigint", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(sysdatetime())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__orders__3213E83F663C9EB0", x => x.id);
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
                    table.ForeignKey(
                        name: "FK_orders_order_status",
                        column: x => x.status_id,
                        principalTable: "order_statuses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "goods_received_details",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    grn_id = table.Column<long>(type: "bigint", nullable: false),
                    ingredient_id = table.Column<long>(type: "bigint", nullable: false),
                    ordered_quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    received_quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    rejected_quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    quality_status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true, defaultValue: "ACCEPTED"),
                    rejection_reason = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    expiry_date = table.Column<DateOnly>(type: "date", nullable: true),
                    batch_number = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    storage_location = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    note = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__goods_re__3213E83FB45A7D74", x => x.id);
                    table.ForeignKey(
                        name: "FK_grn_detail_grn",
                        column: x => x.grn_id,
                        principalTable: "goods_received_notes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_grn_detail_ingredient",
                        column: x => x.ingredient_id,
                        principalTable: "ingredients",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "purchase_returns",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    return_code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    grn_id = table.Column<long>(type: "bigint", nullable: true),
                    invoice_id = table.Column<long>(type: "bigint", nullable: true),
                    supplier_id = table.Column<long>(type: "bigint", nullable: false),
                    branch_id = table.Column<long>(type: "bigint", nullable: false),
                    return_date = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true, defaultValueSql: "(getdate())"),
                    return_reason = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    status_id = table.Column<long>(type: "bigint", nullable: true, defaultValue: 1L),
                    total_return_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    refund_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    credit_note_number = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    approved_by = table.Column<long>(type: "bigint", nullable: true),
                    approval_date = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    note = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__purchase__3213E83FD75D4F59", x => x.id);
                    table.ForeignKey(
                        name: "FK_return_approved_by",
                        column: x => x.approved_by,
                        principalTable: "employees",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_return_branch",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_return_grn",
                        column: x => x.grn_id,
                        principalTable: "goods_received_notes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_return_invoice",
                        column: x => x.invoice_id,
                        principalTable: "purchase_invoices",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_return_supplier",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
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
                    table.PrimaryKey("PK__order_de__3213E83F5863CFF7", x => x.id);
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
                    quantity = table.Column<int>(type: "int", nullable: false),
                    order_id = table.Column<long>(type: "bigint", nullable: false),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    color = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    note = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__order_de__3213E83F4CE42D1D", x => x.id);
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

            migrationBuilder.CreateTable(
                name: "order_payments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<long>(type: "bigint", nullable: false),
                    payment_method_id = table.Column<long>(type: "bigint", nullable: false),
                    payment_status_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    payment_date = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    transaction_id = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    notes = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(sysdatetime())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__order_pa__3213E83F1BEAD382", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_payments_orders",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_order_payments_payment_methods",
                        column: x => x.payment_method_id,
                        principalTable: "payment_methods",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_order_payments_payment_statuses",
                        column: x => x.payment_status_id,
                        principalTable: "payment_statuses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order_shipments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<long>(type: "bigint", nullable: false),
                    shipping_provider_id = table.Column<long>(type: "bigint", nullable: true),
                    shipping_address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    shipping_cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    shipping_method = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    estimated_delivery_date = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    notes = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(sysdatetime())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__order_sh__3213E83F6D88CAE2", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_shipments_orders",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_order_shipments_shipping_providers",
                        column: x => x.shipping_provider_id,
                        principalTable: "shipping_providers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "purchase_return_details",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    return_id = table.Column<long>(type: "bigint", nullable: false),
                    ingredient_id = table.Column<long>(type: "bigint", nullable: false),
                    return_quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    return_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    return_reason = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    batch_number = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    expiry_date = table.Column<DateOnly>(type: "date", nullable: true),
                    quality_issue = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    note = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false, defaultValueSql: "(getdate())"),
                    last_modified = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__purchase__3213E83FA0FB62AC", x => x.id);
                    table.ForeignKey(
                        name: "FK_return_detail_ingredient",
                        column: x => x.ingredient_id,
                        principalTable: "ingredients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_return_detail_return",
                        column: x => x.return_id,
                        principalTable: "purchase_returns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_branch_expenses_branch_id",
                table: "branch_expenses",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_branch_ingredient_inventory_branch_id_ingredient_id",
                table: "branch_ingredient_inventory",
                columns: new[] { "branch_id", "ingredient_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_branch_ingredient_inventory_ingredient_id",
                table: "branch_ingredient_inventory",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "UQ__categori__72E12F1BFB7E3EEA",
                table: "categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__delivery__72E12F1BA335DE00",
                table: "delivery_statuses",
                column: "name",
                unique: true);

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
                name: "IX_expenses_summary_branch_id",
                table: "expenses_summary",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_goods_received_details_grn_id",
                table: "goods_received_details",
                column: "grn_id");

            migrationBuilder.CreateIndex(
                name: "IX_goods_received_details_ingredient_id",
                table: "goods_received_details",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_goods_received_notes_branch_id",
                table: "goods_received_notes",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_goods_received_notes_invoice_id",
                table: "goods_received_notes",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "IX_goods_received_notes_purchase_order_id",
                table: "goods_received_notes",
                column: "purchase_order_id");

            migrationBuilder.CreateIndex(
                name: "IX_goods_received_notes_status_id",
                table: "goods_received_notes",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_goods_received_notes_supplier_id",
                table: "goods_received_notes",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_goods_received_notes_warehouse_staff_id",
                table: "goods_received_notes",
                column: "warehouse_staff_id");

            migrationBuilder.CreateIndex(
                name: "UQ__goods_re__D27DBA9E53D946EE",
                table: "goods_received_notes",
                column: "grn_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__goods_re__72E12F1B5B4629D9",
                table: "goods_received_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_purchase_order_details_ingredient_id",
                table: "ingredient_purchase_order_details",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_purchase_order_details_purchase_order_id",
                table: "ingredient_purchase_order_details",
                column: "purchase_order_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_purchase_orders_branch_id",
                table: "ingredient_purchase_orders",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_purchase_orders_employee_id",
                table: "ingredient_purchase_orders",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_purchase_orders_status_id",
                table: "ingredient_purchase_orders",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_purchase_orders_supplier_id",
                table: "ingredient_purchase_orders",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "UQ__ingredie__19DA46F1B121FE7C",
                table: "ingredient_purchase_orders",
                column: "purchase_order_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_transfer_request_details_ingredient_id",
                table: "ingredient_transfer_request_details",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_transfer_request_details_transfer_request_id",
                table: "ingredient_transfer_request_details",
                column: "transfer_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_transfer_requests_branch_id",
                table: "ingredient_transfer_requests",
                column: "branch_id");

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
                column: "ingredient_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ingredients_category_id",
                table: "ingredients",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredients_tax_id",
                table: "ingredients",
                column: "tax_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_movements_BranchId_IngredientId_MovementDate",
                table: "inventory_movements",
                columns: new[] { "BranchId", "IngredientId", "MovementDate" });

            migrationBuilder.CreateIndex(
                name: "IX_inventory_movements_EmployeeId",
                table: "inventory_movements",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_movements_IngredientId",
                table: "inventory_movements",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_movements_ReferenceType_ReferenceId",
                table: "inventory_movements",
                columns: new[] { "ReferenceType", "ReferenceId" });

            migrationBuilder.CreateIndex(
                name: "IX_inventory_thresholds_BranchId_IngredientId",
                table: "inventory_thresholds",
                columns: new[] { "BranchId", "IngredientId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_inventory_thresholds_IngredientId",
                table: "inventory_thresholds",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "UQ__invoice___72E12F1B28814C66",
                table: "invoice_statuses",
                column: "name",
                unique: true);

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
                name: "IX_order_payments_order_id",
                table: "order_payments",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_payments_payment_method_id",
                table: "order_payments",
                column: "payment_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_payments_payment_status_id",
                table: "order_payments",
                column: "payment_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_shipments_order_id",
                table: "order_shipments",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_shipments_shipping_provider_id",
                table: "order_shipments",
                column: "shipping_provider_id");

            migrationBuilder.CreateIndex(
                name: "UQ__order_st__72E12F1B05345EFE",
                table: "order_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_branch_id",
                table: "orders",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_customer_id",
                table: "orders",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_status_id",
                table: "orders",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "UQ__orders__3DE398663640EAE7",
                table: "orders",
                column: "order_uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__orders__99D12D3FB3E1E0BA",
                table: "orders",
                column: "order_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__payment___72E12F1BEC94A5AC",
                table: "payment_methods",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__payment___72E12F1B7D0643E4",
                table: "payment_statuses",
                column: "name",
                unique: true);

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
                name: "IX_profit_summary_branch_id",
                table: "profit_summary",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_invoice_details_ingredient_id",
                table: "purchase_invoice_details",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_invoice_details_invoice_id",
                table: "purchase_invoice_details",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_invoices_branch_id",
                table: "purchase_invoices",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_invoices_purchase_order_id",
                table: "purchase_invoices",
                column: "purchase_order_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_invoices_status_id",
                table: "purchase_invoices",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_invoices_supplier_id",
                table: "purchase_invoices",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "UQ__purchase__5ED70A355181E3E1",
                table: "purchase_invoices",
                column: "invoice_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__purchase__72E12F1B763CAB51",
                table: "purchase_order_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_purchase_return_details_ingredient_id",
                table: "purchase_return_details",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_return_details_return_id",
                table: "purchase_return_details",
                column: "return_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_returns_approved_by",
                table: "purchase_returns",
                column: "approved_by");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_returns_branch_id",
                table: "purchase_returns",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_returns_grn_id",
                table: "purchase_returns",
                column: "grn_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_returns_invoice_id",
                table: "purchase_returns",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_returns_supplier_id",
                table: "purchase_returns",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "UQ__purchase__51FB33A06DB976E7",
                table: "purchase_returns",
                column: "return_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_recipe_ingredients_IngredientId",
                table: "recipe_ingredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_recipe_ingredients_RecipeId_IngredientId",
                table: "recipe_ingredients",
                columns: new[] { "RecipeId", "IngredientId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_recipes_ProductId_Name",
                table: "recipes",
                columns: new[] { "ProductId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__roles__72E12F1B8563DF69",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sales_summary_branch_id",
                table: "sales_summary",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "UQ__shipping__72E12F1B8D36D97F",
                table: "shipping_providers",
                column: "name",
                unique: true);

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
                name: "IX_supplier_performance_supplier_id",
                table: "supplier_performance",
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

            migrationBuilder.CreateIndex(
                name: "IX_v_employee_payroll_employee_id",
                table: "v_employee_payroll",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_v_expenses_summary_branch_id",
                table: "v_expenses_summary",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_v_inventory_status_branch_id",
                table: "v_inventory_status",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_v_inventory_status_ingredient_id",
                table: "v_inventory_status",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_v_profit_summary_branch_id",
                table: "v_profit_summary",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_v_sales_summary_branch_id",
                table: "v_sales_summary",
                column: "branch_id");
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
                name: "goods_received_details");

            migrationBuilder.DropTable(
                name: "ingredient_purchase_order_details");

            migrationBuilder.DropTable(
                name: "ingredient_transfer_request_details");

            migrationBuilder.DropTable(
                name: "ingredient_transfers");

            migrationBuilder.DropTable(
                name: "ingredient_warehouse");

            migrationBuilder.DropTable(
                name: "inventory_movements");

            migrationBuilder.DropTable(
                name: "inventory_thresholds");

            migrationBuilder.DropTable(
                name: "order_delivery_tracking");

            migrationBuilder.DropTable(
                name: "order_details");

            migrationBuilder.DropTable(
                name: "order_payments");

            migrationBuilder.DropTable(
                name: "order_shipments");

            migrationBuilder.DropTable(
                name: "payrolls");

            migrationBuilder.DropTable(
                name: "product_images");

            migrationBuilder.DropTable(
                name: "product_recipes");

            migrationBuilder.DropTable(
                name: "profit_summary");

            migrationBuilder.DropTable(
                name: "purchase_invoice_details");

            migrationBuilder.DropTable(
                name: "purchase_return_details");

            migrationBuilder.DropTable(
                name: "recipe_ingredients");

            migrationBuilder.DropTable(
                name: "sales_summary");

            migrationBuilder.DropTable(
                name: "social_accounts");

            migrationBuilder.DropTable(
                name: "supplier_ingredient_prices");

            migrationBuilder.DropTable(
                name: "supplier_performance");

            migrationBuilder.DropTable(
                name: "tokens");

            migrationBuilder.DropTable(
                name: "v_employee_payroll");

            migrationBuilder.DropTable(
                name: "v_expenses_summary");

            migrationBuilder.DropTable(
                name: "v_inventory_status");

            migrationBuilder.DropTable(
                name: "v_profit_summary");

            migrationBuilder.DropTable(
                name: "v_sales_summary");

            migrationBuilder.DropTable(
                name: "ingredient_transfer_requests");

            migrationBuilder.DropTable(
                name: "delivery_statuses");

            migrationBuilder.DropTable(
                name: "payment_methods");

            migrationBuilder.DropTable(
                name: "payment_statuses");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "shipping_providers");

            migrationBuilder.DropTable(
                name: "purchase_returns");

            migrationBuilder.DropTable(
                name: "recipes");

            migrationBuilder.DropTable(
                name: "ingredients");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "order_statuses");

            migrationBuilder.DropTable(
                name: "goods_received_notes");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "ingredient_categories");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "purchase_invoices");

            migrationBuilder.DropTable(
                name: "goods_received_statuses");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "taxes");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "ingredient_purchase_orders");

            migrationBuilder.DropTable(
                name: "invoice_statuses");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "purchase_order_statuses");

            migrationBuilder.DropTable(
                name: "suppliers");

            migrationBuilder.DropTable(
                name: "branches");
        }
    }
}
