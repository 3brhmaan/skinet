using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "97e326b1-7727-4012-bfd1-fdf40628faf7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3dfb58c-0c08-411b-ba81-e5c8d31df048");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6185b5bc-30f5-4da1-a581-d079e6f0b1b5", null, "Admin", "ADMIN" },
                    { "89fa3379-8719-49fe-a5e1-b7ea6d4176e9", null, "Customer", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6185b5bc-30f5-4da1-a581-d079e6f0b1b5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89fa3379-8719-49fe-a5e1-b7ea6d4176e9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "97e326b1-7727-4012-bfd1-fdf40628faf7", null, "Admin", "ADMIN" },
                    { "c3dfb58c-0c08-411b-ba81-e5c8d31df048", null, "Customer", "CUSTOMER" }
                });
        }
    }
}
