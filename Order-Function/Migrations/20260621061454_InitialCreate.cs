using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order_Function.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "orders");

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    merchantId = table.Column<Guid>(type: "uuid", nullable: false),
                    itemId = table.Column<Guid>(type: "uuid", nullable: false),
                    qty = table.Column<decimal>(type: "decimal", nullable: false),
                    cost = table.Column<decimal>(type: "numeric", nullable: false),
                    notes = table.Column<string>(type: "varchar(256)", nullable: true),
                    orderTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    orderStatus = table.Column<string>(type: "varchar(256)", nullable: false),
                    orderStatusReason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orders",
                schema: "orders");
        }
    }
}
