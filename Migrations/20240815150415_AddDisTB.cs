using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITProductECommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddDisTB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscountPrograms",
                columns: table => new
                {
                    DiscountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CouponCode = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    DiscountPercent = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BannerImg = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountPrograms", x => x.DiscountId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_DiscountId",
                table: "Customers",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountPrograms_CouponCode",
                table: "DiscountPrograms",
                column: "CouponCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_DiscountPrograms_DiscountId",
                table: "Customers",
                column: "DiscountId",
                principalTable: "DiscountPrograms",
                principalColumn: "DiscountId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_DiscountPrograms_DiscountId",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "DiscountPrograms");

            migrationBuilder.DropIndex(
                name: "IX_Customers_DiscountId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Customers");
        }
    }
}
