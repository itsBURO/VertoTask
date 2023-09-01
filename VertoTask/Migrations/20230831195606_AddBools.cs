using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VertoTask.Migrations
{
    /// <inheritdoc />
    public partial class AddBools : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsForProductOfferSlider",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsForSingleProductDisplay",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForProductOfferSlider",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsForSingleProductDisplay",
                table: "Products");
        }
    }
}
