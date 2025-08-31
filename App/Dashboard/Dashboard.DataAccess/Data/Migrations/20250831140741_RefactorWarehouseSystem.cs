using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorWarehouseSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ingredient_warehouse_ingredient_id",
                table: "ingredient_warehouse");

            migrationBuilder.DropIndex(
                name: "IX_branch_ingredient_inventory_branch_id",
                table: "branch_ingredient_inventory");

            migrationBuilder.RenameColumn(
                name: "SafetyStock",
                table: "inventory_thresholds",
                newName: "MinimumThreshold");

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "ingredient_warehouse",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "maximum_stock",
                table: "ingredient_warehouse",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "minimum_stock",
                table: "ingredient_warehouse",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "ingredient_transfers",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "approved_by",
                table: "ingredient_transfers",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "completed_date",
                table: "ingredient_transfers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "requested_by",
                table: "ingredient_transfers",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "ingredient_transfers",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "transfer_date",
                table: "ingredient_transfers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "transfer_type",
                table: "ingredient_transfers",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_transfer_date",
                table: "branch_ingredient_inventory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "branch_ingredient_inventory",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "minimum_stock",
                table: "branch_ingredient_inventory",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "reserved_quantity",
                table: "branch_ingredient_inventory",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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
                    note = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    requested_by = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    approved_by = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
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
                    note = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_warehouse_ingredient_id",
                table: "ingredient_warehouse",
                column: "ingredient_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_branch_ingredient_inventory_branch_id_ingredient_id",
                table: "branch_ingredient_inventory",
                columns: new[] { "branch_id", "ingredient_id" },
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ingredient_transfer_request_details");

            migrationBuilder.DropTable(
                name: "ingredient_transfer_requests");

            migrationBuilder.DropIndex(
                name: "IX_ingredient_warehouse_ingredient_id",
                table: "ingredient_warehouse");

            migrationBuilder.DropIndex(
                name: "IX_branch_ingredient_inventory_branch_id_ingredient_id",
                table: "branch_ingredient_inventory");

            migrationBuilder.DropColumn(
                name: "location",
                table: "ingredient_warehouse");

            migrationBuilder.DropColumn(
                name: "maximum_stock",
                table: "ingredient_warehouse");

            migrationBuilder.DropColumn(
                name: "minimum_stock",
                table: "ingredient_warehouse");

            migrationBuilder.DropColumn(
                name: "approved_by",
                table: "ingredient_transfers");

            migrationBuilder.DropColumn(
                name: "completed_date",
                table: "ingredient_transfers");

            migrationBuilder.DropColumn(
                name: "requested_by",
                table: "ingredient_transfers");

            migrationBuilder.DropColumn(
                name: "status",
                table: "ingredient_transfers");

            migrationBuilder.DropColumn(
                name: "transfer_date",
                table: "ingredient_transfers");

            migrationBuilder.DropColumn(
                name: "transfer_type",
                table: "ingredient_transfers");

            migrationBuilder.DropColumn(
                name: "last_transfer_date",
                table: "branch_ingredient_inventory");

            migrationBuilder.DropColumn(
                name: "location",
                table: "branch_ingredient_inventory");

            migrationBuilder.DropColumn(
                name: "minimum_stock",
                table: "branch_ingredient_inventory");

            migrationBuilder.DropColumn(
                name: "reserved_quantity",
                table: "branch_ingredient_inventory");

            migrationBuilder.RenameColumn(
                name: "MinimumThreshold",
                table: "inventory_thresholds",
                newName: "SafetyStock");

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "ingredient_transfers",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_warehouse_ingredient_id",
                table: "ingredient_warehouse",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_branch_ingredient_inventory_branch_id",
                table: "branch_ingredient_inventory",
                column: "branch_id");
        }
    }
}
