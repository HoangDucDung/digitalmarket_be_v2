using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DigitalMarket.Infrastructure.MsSql.Migrations
{
    /// <inheritdoc />
    public partial class add_category_tree_contract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_ParentId",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 2, 40, 44, 419, DateTimeKind.Local).AddTicks(7823));

            migrationBuilder.UpdateData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 2, 40, 44, 419, DateTimeKind.Local).AddTicks(7825));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Level", "SortOrder" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "Level", "SortOrder" },
                values: new object[] { 2, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId_SortOrder",
                table: "Categories",
                columns: new[] { "ParentId", "SortOrder" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_ParentId_SortOrder",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");
        }
    }
}
