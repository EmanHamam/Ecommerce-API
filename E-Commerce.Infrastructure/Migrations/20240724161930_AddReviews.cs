using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
    name: "Reviews",
    columns: table => new
    {
        ReviewID = table.Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
        ProductID = table.Column<int>(type: "int", nullable: false),
        Rating = table.Column<int>(type: "int", nullable: false),
        Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
        ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        AppUserID = table.Column<string>(type: "nvarchar(450)", nullable: true)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Reviews", x => x.ReviewID);
        table.ForeignKey(
            name: "FK_Reviews_AspNetUsers_AppUserId",
            column: x => x.AppUserID,
            principalTable: "AspNetUsers",
            principalColumn: "Id");
        table.ForeignKey(
            name: "FK_Reviews_Products_ProductID",
            column: x => x.ProductID,
            principalTable: "Products",
            principalColumn: "ProductID",
            onDelete: ReferentialAction.Cascade);
    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
