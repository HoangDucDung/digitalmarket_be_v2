using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DigitalMarket.Infrastructure.MsSql.Migrations
{
    /// <inheritdoc />
    public partial class RemoveJsonUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRoles",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserRoles",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
