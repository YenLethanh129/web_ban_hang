using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateViewSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    minimum_stock = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                name: "v_products_with_prices",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    sku = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    category_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    current_price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    price_type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    tax_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    tax_rate = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    unit_of_measure = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    weight = table.Column<decimal>(type: "decimal(10,3)", nullable: true),
                    dimensions = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_v_products_with_prices_products_id",
                        column: x => x.id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_expenses_summary_branch_id",
                table: "expenses_summary",
                column: "branch_id");

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
                name: "IX_v_products_with_prices_id",
                table: "v_products_with_prices",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_v_profit_summary_branch_id",
                table: "v_profit_summary",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_v_sales_summary_branch_id",
                table: "v_sales_summary",
                column: "branch_id");

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_summary_branches",
                table: "expenses_summary",
                column: "branch_id",
                principalTable: "branches",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_expenses_summary_branches",
                table: "expenses_summary");

            migrationBuilder.DropTable(
                name: "v_employee_payroll");

            migrationBuilder.DropTable(
                name: "v_expenses_summary");

            migrationBuilder.DropTable(
                name: "v_inventory_status");

            migrationBuilder.DropTable(
                name: "v_products_with_prices");

            migrationBuilder.DropTable(
                name: "v_profit_summary");

            migrationBuilder.DropTable(
                name: "v_sales_summary");

            migrationBuilder.DropIndex(
                name: "IX_expenses_summary_branch_id",
                table: "expenses_summary");
        }
    }
}
