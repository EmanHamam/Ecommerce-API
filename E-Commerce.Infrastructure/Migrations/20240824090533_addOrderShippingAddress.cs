using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addOrderShippingAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Addresses_ShippingAddressID",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippingAddressID",
                table: "Orders",
                newName: "ShippingAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ShippingAddressID",
                table: "Orders",
                newName: "IX_Orders_ShippingAddressId");

            migrationBuilder.CreateTable(
                name: "OrderShippingAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderShippingAddress", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderShippingAddress_ShippingAddressId",
                table: "Orders",
                column: "ShippingAddressId",
                principalTable: "OrderShippingAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderShippingAddress_ShippingAddressId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "OrderShippingAddress");

            migrationBuilder.RenameColumn(
                name: "ShippingAddressId",
                table: "Orders",
                newName: "ShippingAddressID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ShippingAddressId",
                table: "Orders",
                newName: "IX_Orders_ShippingAddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Addresses_ShippingAddressID",
                table: "Orders",
                column: "ShippingAddressID",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
