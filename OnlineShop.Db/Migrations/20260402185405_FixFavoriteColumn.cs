using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Db.Migrations
{
    /// <inheritdoc />
    public partial class FixFavoriteColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FavoriteId",
                table: "Products",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Favorites",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FavoriteId",
                table: "Products",
                column: "FavoriteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Favorites_FavoriteId",
                table: "Products",
                column: "FavoriteId",
                principalTable: "Favorites",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Favorites_FavoriteId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FavoriteId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FavoriteId",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Favorites",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
