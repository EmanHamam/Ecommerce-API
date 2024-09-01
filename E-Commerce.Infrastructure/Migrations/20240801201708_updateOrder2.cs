using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateOrder2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryMethod_DeliveryMethodId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "DeliveryMethodId",
                table: "Orders",
                newName: "DeliveryMethodID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DeliveryMethodId",
                table: "Orders",
                newName: "IX_Orders_DeliveryMethodID");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryMethodID",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryMethod_DeliveryMethodID",
                table: "Orders",
                column: "DeliveryMethodID",
                principalTable: "DeliveryMethod",
                principalColumn: "DeliveryMethodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryMethod_DeliveryMethodID",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "DeliveryMethodID",
                table: "Orders",
                newName: "DeliveryMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DeliveryMethodID",
                table: "Orders",
                newName: "IX_Orders_DeliveryMethodId");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryMethodId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryMethod_DeliveryMethodId",
                table: "Orders",
                column: "DeliveryMethodId",
                principalTable: "DeliveryMethod",
                principalColumn: "DeliveryMethodId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
