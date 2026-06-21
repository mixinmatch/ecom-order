using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order_Function.Migrations
{
    /// <inheritdoc />
    public partial class schemaChange1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_orders",
                schema: "orders",
                table: "orders");

            migrationBuilder.RenameTable(
                name: "orders",
                schema: "orders",
                newName: "ORDERS",
                newSchema: "orders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ORDERS",
                schema: "orders",
                table: "ORDERS",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ORDERS",
                schema: "orders",
                table: "ORDERS");

            migrationBuilder.RenameTable(
                name: "ORDERS",
                schema: "orders",
                newName: "orders",
                newSchema: "orders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_orders",
                schema: "orders",
                table: "orders",
                column: "id");
        }
    }
}
