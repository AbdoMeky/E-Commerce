using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECO.EF.Migrations
{
    /// <inheritdoc />
    public partial class addMoreAdditions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReady",
                table: "OrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReady",
                table: "OrderItems");
        }
    }
}
