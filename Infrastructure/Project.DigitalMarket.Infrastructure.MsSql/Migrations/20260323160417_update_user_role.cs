using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DigitalMarket.Infrastructure.MsSql.Migrations
{
    /// <inheritdoc />
    public partial class update_user_role : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserRoles",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRoles",
                table: "Users");
        }
    }
}
