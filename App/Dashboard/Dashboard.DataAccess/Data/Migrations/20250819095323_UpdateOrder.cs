using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ingredients_IngredientCategories_category_id",
                table: "ingredients");

            migrationBuilder.DropColumn(
                name: "note",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "payment_method_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "payment_status_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "shipping_address",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "shipping_date",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "shipping_method",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "price",
                table: "order_details");

            migrationBuilder.DropColumn(
                name: "total_money",
                table: "order_details");

            migrationBuilder.RenameTable(
                name: "IngredientCategories",
                newName: "ingredient_categories");

            migrationBuilder.RenameColumn(
                name: "number_of_products",
                table: "order_details",
                newName: "quantity");

            migrationBuilder.AddColumn<string>(
                name: "notes",
                table: "orders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "product_id",
                table: "order_details",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "order_id",
                table: "order_details",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "order_details",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_amount",
                table: "order_details",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "unit_price",
                table: "order_details",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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
                    table.PrimaryKey("PK__order_pa__3213E83F8B2AD3E1", x => x.id);
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
                    table.PrimaryKey("PK__order_sh__3213E83F9A3C5D2F", x => x.id);
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

            migrationBuilder.CreateIndex(
                name: "IX_sales_summary_branch_id",
                table: "sales_summary",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_profit_summary_branch_id",
                table: "profit_summary",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_status_id",
                table: "orders",
                column: "status_id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ingredients_ingredient_categories",
                table: "ingredients",
                column: "category_id",
                principalTable: "ingredient_categories",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_order_status",
                table: "orders",
                column: "status_id",
                principalTable: "order_statuses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_profit_summary_branches",
                table: "profit_summary",
                column: "branch_id",
                principalTable: "branches",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_sales_summary_branches",
                table: "sales_summary",
                column: "branch_id",
                principalTable: "branches",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ingredients_ingredient_categories",
                table: "ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_order_status",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_profit_summary_branches",
                table: "profit_summary");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_summary_branches",
                table: "sales_summary");

            migrationBuilder.DropTable(
                name: "order_payments");

            migrationBuilder.DropTable(
                name: "order_shipments");

            migrationBuilder.DropIndex(
                name: "IX_sales_summary_branch_id",
                table: "sales_summary");

            migrationBuilder.DropIndex(
                name: "IX_profit_summary_branch_id",
                table: "profit_summary");

            migrationBuilder.DropIndex(
                name: "IX_orders_status_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "notes",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "note",
                table: "order_details");

            migrationBuilder.DropColumn(
                name: "total_amount",
                table: "order_details");

            migrationBuilder.DropColumn(
                name: "unit_price",
                table: "order_details");

            migrationBuilder.RenameTable(
                name: "ingredient_categories",
                newName: "IngredientCategories");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "order_details",
                newName: "number_of_products");

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "orders",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "payment_method_id",
                table: "orders",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "payment_status_id",
                table: "orders",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shipping_address",
                table: "orders",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "shipping_date",
                table: "orders",
                type: "datetime2(6)",
                precision: 6,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shipping_method",
                table: "orders",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "product_id",
                table: "order_details",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "order_id",
                table: "order_details",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<float>(
                name: "price",
                table: "order_details",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "total_money",
                table: "order_details",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddForeignKey(
                name: "FK_ingredients_IngredientCategories_category_id",
                table: "ingredients",
                column: "category_id",
                principalTable: "IngredientCategories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
