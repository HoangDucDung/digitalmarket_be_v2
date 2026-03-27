using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DigitalMarket.Infrastructure.MsSql.Migrations
{
    /// <inheritdoc />
    public partial class update_product_images_to_fileid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ProductImages");

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "ProductImages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 1, 22, 1, 442, DateTimeKind.Local).AddTicks(8735));

            migrationBuilder.UpdateData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 1, 22, 1, 442, DateTimeKind.Local).AddTicks(8738));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 1, 22, 1, 442, DateTimeKind.Local).AddTicks(8532));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 1, 22, 1, 442, DateTimeKind.Local).AddTicks(8567));

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_FileId",
                table: "ProductImages",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Files_FileId",
                table: "ProductImages",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Files_FileId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_FileId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "ProductImages");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ProductImages",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 0, 28, 4, 759, DateTimeKind.Local).AddTicks(5178));

            migrationBuilder.UpdateData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 0, 28, 4, 759, DateTimeKind.Local).AddTicks(5181));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 0, 28, 4, 759, DateTimeKind.Local).AddTicks(4963));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 0, 28, 4, 759, DateTimeKind.Local).AddTicks(5017));
        }
    }
}
