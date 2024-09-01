using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePrdDetailsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetails_Products_ProductID",
                table: "ProductDetails");

            migrationBuilder.DropIndex(
                name: "IX_ProductDetails_ProductID",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "ProductDetails");

            migrationBuilder.RenameColumn(
                name: "smallImg3",
                table: "ProductDetails",
                newName: "SmallImg3");

            migrationBuilder.RenameColumn(
                name: "smallImg2",
                table: "ProductDetails",
                newName: "SmallImg2");

            migrationBuilder.RenameColumn(
                name: "smallImg1",
                table: "ProductDetails",
                newName: "SmallImg1");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_ProducID",
                table: "ProductDetails",
                column: "ProducID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetails_Products_ProducID",
                table: "ProductDetails",
                column: "ProducID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetails_Products_ProducID",
                table: "ProductDetails");

            migrationBuilder.DropIndex(
                name: "IX_ProductDetails_ProducID",
                table: "ProductDetails");

            migrationBuilder.RenameColumn(
                name: "SmallImg3",
                table: "ProductDetails",
                newName: "smallImg3");

            migrationBuilder.RenameColumn(
                name: "SmallImg2",
                table: "ProductDetails",
                newName: "smallImg2");

            migrationBuilder.RenameColumn(
                name: "SmallImg1",
                table: "ProductDetails",
                newName: "smallImg1");

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "ProductDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_ProductID",
                table: "ProductDetails",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetails_Products_ProductID",
                table: "ProductDetails",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID");
        }
    }
}
